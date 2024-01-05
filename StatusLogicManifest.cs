using CobaltCoreModding.Definitions.ExternalItems;
using CobaltCoreModding.Definitions.ModContactPoints;
using CobaltCoreModding.Definitions.ModManifests;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.Extensions.Logging;
using FMOD;
using FSPRO;
using Microsoft.Xna.Framework.Input;

using HarmonyLib;

namespace Eddie
{
    public partial class Manifest : IStatusManifest
    {
        public static ExternalStatus? CircuitStatus { get; private set; }
        public static ExternalStatus? ClosedCircuitStatus { get; private set; }
        public static ExternalStatus? LoseEnergyEveryTurnStatus { get; private set; }
        public static ExternalStatus? HealNextTurnStatus { get; private set; }

        public void LoadManifest(IStatusRegistry statusRegistry)
        {

            //create status objects
            CircuitStatus = new ExternalStatus("Eddie.Status.Circuit", true, Eddie_PrimaryColor, null, CircuitIcon ?? throw new Exception("Missing Circuit Icon for status"), true);
            CircuitStatus.AddLocalisation("Circuit", "Gain {0} <c=status>CLOSED CIRCUIT</c> at the start of your turn.");
            statusRegistry.RegisterStatus(CircuitStatus);

            ClosedCircuitStatus = new ExternalStatus("Eddie.Status.ClosedCircuit", true, Eddie_PrimaryColor, null, ClosedCircuitIcon ?? throw new Exception("Missing Closed Circuit Icon for status"), true);
            ClosedCircuitStatus.AddLocalisation("Closed Circuit", "When a card would be discarded, lose 1 <c=status>closed circuit</c> instead.");
            statusRegistry.RegisterStatus(ClosedCircuitStatus);

            LoseEnergyEveryTurnStatus = new ExternalStatus("Eddie.Status.LoseEnergyEveryTurn", true, Eddie_PrimaryColor, null, LoseEnergyEveryTurnIcon ?? throw new Exception("Missing Lose Energy Icon for status"), true);
            LoseEnergyEveryTurnStatus.AddLocalisation("Lose Energy Every Turn", "Lose {0} <c=energy>ENERGY</c> at the start of every turn.");
            statusRegistry.RegisterStatus(LoseEnergyEveryTurnStatus);

            HealNextTurnStatus = new ExternalStatus("Eddie.Status.HealNextTurn", true, Eddie_PrimaryColor, null, HealNextTurnIcon ?? throw new Exception("Missing Heal Next Turn Icon for status"), true);
            HealNextTurnStatus.AddLocalisation("Regain Hull Next Turn", "Regain {0} hull at the start of your next turn or at the end of combat. <c=downside>This does not count as healing</c>");
            statusRegistry.RegisterStatus(HealNextTurnStatus);

            // Patching status logic
            var harmony = new Harmony("Eddie.Status");
            CircuitStatusLogic(harmony);
            ClosedCircuitStatusLogic(harmony);
            LoseEnergyEveryTurnStatusLogic(harmony);
            HealNextTurnStatusLogic(harmony);
        }

        private void LoseEnergyEveryTurnStatusLogic(Harmony harmony)
        {
            if (LoseEnergyEveryTurnStatus?.Id == null)
                return;
            var start_turn_method = typeof(Ship).GetMethod("OnBeginTurn") ?? throw new Exception("Couldn't find Ship.OnBeginTurn method");
            var start_turn_post = typeof(Manifest).GetMethod("LoseEnergyEveryTurn", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic) ?? throw new Exception("Couldnt find Manifest.LoseEnergyEveryTurn method");
            harmony.Patch(start_turn_method, postfix: new HarmonyMethod(start_turn_post));
        }

        private void HealNextTurnStatusLogic(Harmony harmony)
        {
            if (HealNextTurnStatus?.Id == null)
                return;
            var start_turn_method = typeof(Ship).GetMethod("OnBeginTurn") ?? throw new Exception("Couldn't find Ship.OnBeginTurn method");
            var start_turn_post = typeof(Manifest).GetMethod("HealOnStartOfTurn", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic) ?? throw new Exception("Couldnt find Manifest.HealOnStartOfTurn method");
            harmony.Patch(start_turn_method, postfix: new HarmonyMethod(start_turn_post));

            var updateMethod = typeof(Combat).GetMethod("Update") ?? throw new Exception("Couldn't find Combat.Update method");
            var patch = typeof(Manifest).GetMethod("HealOnEnemyDie", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic) ?? throw new Exception("Couldnt find Manifest.HealOnStartOfTurn method");
            harmony.Patch(updateMethod, postfix: new HarmonyMethod(patch));
        }

        private void ClosedCircuitStatusLogic(Harmony harmony)
        {
            if (ClosedCircuitStatus?.Id == null)
                return;
            {
                var try_play_method = typeof(Combat).GetMethod("TryPlayCard") ?? throw new Exception("Couldn't find Combat.TryPlayCard method");
                var patch = typeof(Manifest).GetMethod("ClosedCircuitPreventDiscard", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic) ?? throw new Exception("Couldnt find Manifest.ClosedCircuitPreventDiscard method");
                harmony.Patch(try_play_method, transpiler: new HarmonyMethod(patch));
            } {
                var discard_hand_method = typeof(Combat).GetMethod("DiscardHand") ?? throw new Exception("Couldn't find Combat.DiscardHand method");
                var patch = typeof(Manifest).GetMethod("ClosedCircuitPreventHandDiscard", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic) ?? throw new Exception("Couldnt find Manifest.ClosedCircuitPreventHandDiscard method");
                harmony.Patch(discard_hand_method, transpiler: new HarmonyMethod(patch));
            }
        }

        private void CircuitStatusLogic(Harmony harmony)
        {
            if (CircuitStatus?.Id == null)
                return;
            var start_turn_method = typeof(Ship).GetMethod("OnBeginTurn") ?? throw new Exception("Couldn't find Ship.OnBeginTurn method");
            var start_turn_post = typeof(Manifest).GetMethod("GiveClosedCircuit", BindingFlags.Static | BindingFlags.NonPublic) ?? throw new Exception("Couldnt find Manifest.GiveClosedCircuit method");
            harmony.Patch(start_turn_method, postfix: new HarmonyMethod(start_turn_post));

            var getTooltipsMethod = typeof(StatusMeta).GetMethod("GetTooltips", BindingFlags.Static | BindingFlags.Public) ?? throw new Exception("Couldn't find StatusMeta.GetTooltips");
            var patch = typeof(Manifest).GetMethod("CircuitTooltipPatch", BindingFlags.Static | BindingFlags.Public) ?? throw new Exception("Couldnt find Manifest.CircuitTooltipPatch method");
            harmony.Patch(getTooltipsMethod, postfix: new HarmonyMethod(patch));
        }

        private static void HealOnStartOfTurn(Ship __instance, State s, Combat c)
        {
            var status = (Status)HealNextTurnStatus!.Id!;

            if (__instance.Get(status) > 0) {
                __instance.Heal(__instance.Get(status));
                __instance.Set(status, 0);
            }
        }

        private static void HealOnEnemyDie(Combat __instance, G g)
        {
            var status = (Status)HealNextTurnStatus!.Id!;
            var ship = g.state.ship;

            if (ship.Get(status) > 0 && __instance.otherShip.deathProgress.HasValue && __instance.EitherShipIsDead(g.state) && !ship.deathProgress.HasValue)
            {
                ship.Heal(ship.Get(status));
                ship.Set(status, 0);
            }
        }

        private static void LoseEnergyEveryTurn(Ship __instance, State s, Combat c)
        {
            if (LoseEnergyEveryTurnStatus?.Id == null)
                return;
            var status = (Status)LoseEnergyEveryTurnStatus.Id;
            
            c.energy -= __instance.Get(status);
        }

        private static void GiveClosedCircuit(Ship __instance, State s, Combat c)
        {
            if (CircuitStatus?.Id == null || ClosedCircuitStatus?.Id == null)
                return;
            var status = (Status)CircuitStatus.Id;
            var ccStatus = (Status)ClosedCircuitStatus.Id;
            
            __instance.Set(ccStatus, __instance.Get(status) + __instance.Get(ccStatus));
        }

        public static void CircuitTooltipPatch(List<Tooltip> __result, Status status, int amt)
        {
            if (status == (Status)(CircuitStatus?.Id ?? throw new Exception())) {
                __result.Add(new TTGlossary("status." + (ClosedCircuitStatus?.Id ?? throw new Exception())));
            }
        }

        private static IEnumerable<CodeInstruction> ClosedCircuitPreventDiscard(IEnumerable<CodeInstruction> iseq, ILGenerator il, MethodBase originalMethod)
        {
            int? local_index = null;
            foreach (LocalVariableInfo info in originalMethod.GetMethodBody()!.LocalVariables)
            {
                if (info.LocalType == typeof(CardData))
                {
                    local_index = info.LocalIndex;
                    break;
                }
            }
            
            
            using IEnumerator<CodeInstruction> iter = iseq.GetEnumerator();
            if (local_index != null && CircuitStatus != null && CircuitStatus.Id != null && ClosedCircuitStatus != null && ClosedCircuitStatus.Id != null)
            {
                while(iter.MoveNext()) {
                    yield return iter.Current;
                    if(!TranspilerUtils.IsLocalLoad(iter.Current) || TranspilerUtils.ExtractLocalIndex(iter.Current) != local_index) {
                        continue;
                    }
                    var card_data_local_load_opcode = iter.Current.opcode;
                    var card_data_local_operand = iter.Current.operand;

                    if(!iter.MoveNext()) {
                        break;
                    }
                    yield return iter.Current;

                    if(iter.Current.opcode != OpCodes.Ldfld || ((FieldInfo) iter.Current.operand).Name != "infinite") {
                        continue;
                    }
                    var infinite_field_info = iter.Current.operand;

                    if(!iter.MoveNext()) {
                        break;
                    }
                    yield return iter.Current;

                    if(iter.Current.opcode != OpCodes.Brfalse_S) {
                        continue;
                    }

                    if(!iter.MoveNext()) {
                        break;
                    }
                    yield return iter.Current;

                    if(!TranspilerUtils.IsLocalLoad(iter.Current)) {
                        continue;
                    }
                    var exhaust_flag_local_load_opcode = iter.Current.opcode;
                    var exhaust_flag_local_operand = iter.Current.operand;

                    if(!iter.MoveNext()) {
                        break;
                    }
                    yield return iter.Current;

                    if(iter.Current.opcode != OpCodes.Ldc_I4_0) {
                        continue;
                    }

                    if(!iter.MoveNext()) {
                        break;
                    }
                    yield return iter.Current;

                    if(iter.Current.opcode != OpCodes.Ceq) {
                        continue;
                    }

                    if(!iter.MoveNext()) {
                        break;
                    }
                    yield return iter.Current;

                    if(iter.Current.opcode != OpCodes.Br_S) {
                        continue;
                    }

                    if(!iter.MoveNext()) {
                        break;
                    }
                    yield return iter.Current;

                    if(iter.Current.opcode != OpCodes.Ldc_I4_0) {
                        continue;
                    }

                    if(!iter.MoveNext()) {
                        break;
                    }
                    yield return iter.Current;

                    if(!TranspilerUtils.IsLocalStore(iter.Current)) {
                        continue;
                    }
                    var discard_flag_local_store_opcode = iter.Current.opcode;
                    var discard_flag_local_load_opcode = TranspilerUtils.StoreToLoad(discard_flag_local_store_opcode);
                    var discard_flag_local_operand = iter.Current.operand;
                    Label end_label = il.DefineLabel();

                    // The if statement: if(!exhaust && !stay && !cardData.infinite && s.ship.Get(ClosedCirtuit) > 0)
                    yield return new CodeInstruction(exhaust_flag_local_load_opcode, exhaust_flag_local_operand);
                    yield return new CodeInstruction(OpCodes.Brtrue, end_label);
                    yield return new CodeInstruction(discard_flag_local_load_opcode, discard_flag_local_operand);
                    yield return new CodeInstruction(OpCodes.Brtrue, end_label);
                    yield return new CodeInstruction(card_data_local_load_opcode, card_data_local_operand);
                    yield return new CodeInstruction(OpCodes.Ldfld, infinite_field_info);
                    yield return new CodeInstruction(OpCodes.Brtrue, end_label);
                    yield return new CodeInstruction(OpCodes.Ldarg_1);
                    yield return new CodeInstruction(OpCodes.Ldfld, typeof(State).GetField("ship"));
                    yield return new CodeInstruction(OpCodes.Ldc_I4, Manifest.ClosedCircuitStatus.Id);
                    yield return new CodeInstruction(OpCodes.Callvirt, typeof(Ship).GetMethod("Get"));
                    yield return new CodeInstruction(OpCodes.Ldc_I4_0);
                    yield return new CodeInstruction(OpCodes.Ble, end_label);

                    // s.ship.Set(ClosedCircuit, s.ship.Get(ClosedCircuit) - 1);
                    yield return new CodeInstruction(OpCodes.Ldarg_1);
                    yield return new CodeInstruction(OpCodes.Ldfld, typeof(State).GetField("ship"));
                    yield return new CodeInstruction(OpCodes.Ldc_I4, Manifest.ClosedCircuitStatus.Id);
                    yield return new CodeInstruction(OpCodes.Ldarg_1);
                    yield return new CodeInstruction(OpCodes.Ldfld, typeof(State).GetField("ship"));
                    yield return new CodeInstruction(OpCodes.Ldc_I4, Manifest.ClosedCircuitStatus.Id);
                    yield return new CodeInstruction(OpCodes.Callvirt, typeof(Ship).GetMethod("Get"));
                    yield return new CodeInstruction(OpCodes.Ldc_I4_1);
                    yield return new CodeInstruction(OpCodes.Sub);
                    yield return new CodeInstruction(OpCodes.Callvirt, typeof(Ship).GetMethod("Set"));

                    // // stay = true
                    yield return new CodeInstruction(OpCodes.Ldc_I4_1);
                    yield return new CodeInstruction(discard_flag_local_store_opcode, discard_flag_local_operand);
                    

                    if(!iter.MoveNext()) {
                        break;
                    }
                    iter.Current.labels.Add(end_label);
                    yield return iter.Current;
                    break;
                }
            }
            while(iter.MoveNext())
                yield return iter.Current;
        }

        private static IEnumerable<CodeInstruction> ClosedCircuitPreventHandDiscard(IEnumerable<CodeInstruction> iseq, ILGenerator il, MethodBase originalMethod)
        {
            using IEnumerator<CodeInstruction> iter = iseq.GetEnumerator();

            while(iter.MoveNext()) {
                yield return iter.Current;
                if(iter.Current.opcode != OpCodes.Ldarga_S) {
                    continue;
                }
                byte countLocal = (byte)iter.Current.operand;

                if(!iter.MoveNext()) {
                    break;
                }
                yield return iter.Current;

                if(iter.Current.opcode != OpCodes.Call || ((MethodInfo)iter.Current.operand).Name != "get_HasValue") {
                    continue;
                }

                if(!iter.MoveNext()) {
                    break;
                }
                yield return iter.Current;

                if(iter.Current.opcode != OpCodes.Brfalse_S) {
                    continue;
                }
                Label endLabel = (Label)iter.Current.operand;
                Label midLabel = il.DefineLabel();
                iter.Current.operand = midLabel;

                yield return new CodeInstruction(OpCodes.Ldarg_0);
                yield return new CodeInstruction(OpCodes.Ldarg_1);
                yield return new CodeInstruction(OpCodes.Ldarga, countLocal);
                yield return new CodeInstruction(OpCodes.Call, typeof(Manifest).GetMethod("NewCount", BindingFlags.Static | BindingFlags.NonPublic));
                yield return new CodeInstruction(OpCodes.Starg, countLocal);

                while (iter.MoveNext() && !iter.Current.labels.Contains(endLabel)) {
                    yield return iter.Current;
                }

                if (!iter.Current.labels.Contains(endLabel)) {
                    throw new Exception("That shouldn't happen");
                }

                yield return new CodeInstruction(OpCodes.Br, endLabel);
                yield return new CodeInstruction(OpCodes.Ldarg_0).WithLabels(midLabel);
                yield return new CodeInstruction(OpCodes.Ldarg_1);
                yield return new CodeInstruction(OpCodes.Ldarg_2);
                yield return new CodeInstruction(OpCodes.Call, typeof(Manifest).GetMethod("ClosedCircuitRetain", BindingFlags.Static | BindingFlags.NonPublic));
                yield return new CodeInstruction(OpCodes.Stloc_1);

                yield return iter.Current.WithLabels(endLabel);
                break;
            }

            while(iter.MoveNext())
                yield return iter.Current;
        }

        private static int? NewCount(Combat c, State s, ref int? countMebbe) {
            if (countMebbe == null) return null;
            int count = countMebbe.Value;
            int handSize = c.hand.Count;
            int ccAmount = s.ship.Get((Status)ClosedCircuitStatus!.Id!);
            int toRemove = count - Math.Min(count, ccAmount);
            s.ship.Set((Status)ClosedCircuitStatus!.Id!, ccAmount - Math.Min(count, ccAmount));
            return toRemove;
        }

        private static List<Card> ClosedCircuitRetain(Combat c, State s, bool ignoreRetain) {
            List<Card> cards = new List<Card>();
            foreach (Card card in c.hand) {
                if (!ignoreRetain || !card.GetDataWithOverrides(s).retain) {
                    int ccAmount = s.ship.Get((Status)ClosedCircuitStatus!.Id!);
                    if (ccAmount > 0) {
                        s.ship.Set((Status)ClosedCircuitStatus!.Id!, ccAmount - 1);
                    } else {
                        cards.Add(card);
                    }
                }
            }
            cards.Reverse();
            return cards;
        }

        // private static IEnumerable<CodeInstruction> ClosedCircuitMakesRetain(IEnumerable<CodeInstruction> iseq, ILGenerator il, MethodBase originalMethod)
        // {
        //     using IEnumerator<CodeInstruction> iter = iseq.GetEnumerator();

        //     while(iter.MoveNext()) {
        //         yield return iter.Current;
        //         if(iter.Current.opcode != OpCodes.Ldarg_1) {
        //             continue;
        //         }

        //         if(!iter.MoveNext()) break;
        //         yield return iter.Current;

        //         if(iter.Current.opcode != OpCodes.Ldarg_0) {
        //             continue;
        //         }

        //         if(!iter.MoveNext()) break;
        //         yield return iter.Current;

        //         if(iter.Current.opcode != OpCodes.Ldfld) {
        //             continue;
        //         }
        //         var sField = iter.Current.operand;

        //         while (iter.MoveNext() && iter.Current.opcode != OpCodes.Ret) {
        //             yield return iter.Current;
        //         }

        //         if(iter.Current.opcode != OpCodes.Ret) {
        //             yield return iter.Current;
        //             continue;
        //         }
        //         Label endLabel = il.DefineLabel();

        //         yield return new CodeInstruction(OpCodes.Dup);
        //         yield return new CodeInstruction(OpCodes.Brfalse, endLabel);
        //         yield return new CodeInstruction(OpCodes.Pop);
        //         yield return new CodeInstruction(OpCodes.Ldarg_0);
        //         yield return new CodeInstruction(OpCodes.Ldfld, sField);
        //         yield return new CodeInstruction(OpCodes.Ldarg_1);
        //         yield return new CodeInstruction(OpCodes.Ldarg_0);
        //         yield return new CodeInstruction(OpCodes.Ldfld, countField);
        //         yield return new CodeInstruction(OpCodes.Call, typeof(Manifest).GetMethod("ClosedCircuitRetain", BindingFlags.Static | BindingFlags.NonPublic));

        //         yield return iter.Current.WithLabels(endLabel);
        //         break;
        //     }

        //     while(iter.MoveNext())
        //         yield return iter.Current;

        // }
    }
}
