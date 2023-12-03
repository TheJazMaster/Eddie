using CobaltCoreModding.Definitions.ExternalItems;
using CobaltCoreModding.Definitions.ModContactPoints;
using CobaltCoreModding.Definitions.ModManifests;
using System.Reflection;
using System.Reflection.Emit;
using FMOD;
using FSPRO;
using Microsoft.Xna.Framework.Input;
using ILInstruction = Mono.Cecil.Cil.Instruction;

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
            CircuitStatus = new ExternalStatus("Eddie.Status.Circuit", true, Eddie_PrimaryColor, null, CircuitIcon ?? throw MakeInformativeException(Logger, "Missing Circuit Icon for status"), true);
            CircuitStatus.AddLocalisation("Circuit", "Gain {0} Closed Circuit at the start of your turn.");
            statusRegistry.RegisterStatus(CircuitStatus);

            ClosedCircuitStatus = new ExternalStatus("Eddie.Status.ClosedCircuit", true, Eddie_PrimaryColor, null, ClosedCircuitIcon ?? throw MakeInformativeException(Logger, "Missing Closed Circuit Icon for status"), true);
            ClosedCircuitStatus.AddLocalisation("Closed Circuit", "When a card would be discarded, lose 1 Closed Circuit instead.");
            statusRegistry.RegisterStatus(ClosedCircuitStatus);

            LoseEnergyEveryTurnStatus = new ExternalStatus("Eddie.Status.LoseEnergyEveryTurn", true, Eddie_PrimaryColor, null, LoseEnergyEveryTurnIcon ?? throw MakeInformativeException(Logger, "Missing Lose Energy Icon for status"), true);
            LoseEnergyEveryTurnStatus.AddLocalisation("Lose Energy Every Turn", "Lose {0} energy at the start of every turn.");
            statusRegistry.RegisterStatus(LoseEnergyEveryTurnStatus);

            HealNextTurnStatus = new ExternalStatus("Eddie.Status.HealNextTurn", true, Eddie_PrimaryColor, null, HealNextTurnIcon ?? throw MakeInformativeException(Logger, "Missing Heal Next Turn Icon for status"), true);
            HealNextTurnStatus.AddLocalisation("Regain Hull Next Turn", "Regain {0} hull at the start of your next turn. <c=drawback>This does not count as healing</c>");
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
            var start_turn_method = typeof(Ship).GetMethod("OnBeginTurn") ?? throw MakeInformativeException(Logger, "Couldn't find Ship.OnBeginTurn method");
            var start_turn_post = typeof(Manifest).GetMethod("LoseEnergyEveryTurn", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic) ?? throw MakeInformativeException(Logger, "Couldnt find Manifest.LoseEnergyEveryTurn method");
            harmony.Patch(start_turn_method, postfix: new HarmonyMethod(start_turn_post));
        }

        private void HealNextTurnStatusLogic(Harmony harmony)
        {
            if (HealNextTurnStatus?.Id == null)
                return;
            var start_turn_method = typeof(Ship).GetMethod("OnBeginTurn") ?? throw MakeInformativeException(Logger, "Couldn't find Ship.OnBeginTurn method");
            var start_turn_post = typeof(Manifest).GetMethod("HealOnStartOfTurn", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic) ?? throw MakeInformativeException(Logger, "Couldnt find Manifest.HealOnStartOfTurn method");
            harmony.Patch(start_turn_method, postfix: new HarmonyMethod(start_turn_post));
        }

        private void ClosedCircuitStatusLogic(Harmony harmony)
        {
            if (ClosedCircuitStatus?.Id == null)
                return;
            var try_play_method = typeof(Combat).GetMethod("TryPlayCard") ?? throw MakeInformativeException(Logger, "Couldn't find Combat.TryPlayCard method");
            var try_play_trans = typeof(Manifest).GetMethod("ClosedCircuitPreventDiscard", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic) ?? throw MakeInformativeException(Logger, "Couldnt find Manifest.ClosedCircuitPreventDiscard method");
            harmony.Patch(try_play_method, transpiler: new HarmonyMethod(try_play_trans));
        }

        private void CircuitStatusLogic(Harmony harmony)
        {
            if (CircuitStatus?.Id == null)
                return;
            var start_turn_method = typeof(Ship).GetMethod("OnBeginTurn") ?? throw MakeInformativeException(Logger, "Couldn't find Ship.OnBeginTurn method");
            var start_turn_post = typeof(Manifest).GetMethod("GiveClosedCircuit", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic) ?? throw MakeInformativeException(Logger, "Couldnt find Manifest.GiveClosedCircuit method");
            harmony.Patch(start_turn_method, postfix: new HarmonyMethod(start_turn_post));
        }

        private static void HealOnStartOfTurn(Ship __instance, State s, Combat c)
        {
            if (HealNextTurnStatus?.Id == null)
                return;
            var status = (Status)HealNextTurnStatus.Id;

            __instance.Heal(__instance.Get(status));
            __instance.Set(status, 0);
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


//         private void SmartExplosiveLogic(Harmony harmony)
//         {
//             //patch start turn to decrease status by 1.

//             var start_turn_method = typeof(Ship).GetMethod("OnBeginTurn") ?? throw MakeInformativeException(Logger, "Couldnt find Ship.OnBeginTurn method");
//             var start_turn_post = typeof(Manifest).GetMethod("SmartExplosiveTurnStart", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic) ?? throw MakeInformativeException(Logger, "Couldnt find Manifest.SmartExplosiveTurnStart method");
//             harmony.Patch(start_turn_method, postfix: new HarmonyMethod(start_turn_post));

//             //patch regular missile.begin to not fire if the owner has smart explosive

//             var a_missile_hit_update = typeof(AMissileHit).GetMethod("Update") ?? throw MakeInformativeException(Logger, "Couldnt find AMissileHit.Update method");

//             var a_missile_hit_prefix = typeof(Manifest).GetMethod("SmartExplosiveMissileLock", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic) ?? throw MakeInformativeException(Logger, "Couldnt find Manifest.SmartExplosiveMissileLock method");
//             harmony.Patch(a_missile_hit_update, prefix: new HarmonyMethod(a_missile_hit_prefix));
//         }

//         private static bool SmartExplosiveMissileLock(State s, Combat c, AMissileHit __instance)
//         {
//             if (SmartExplosiveStatus?.Id == null)
//                 return true;
//             if (!c.stuff.TryGetValue(__instance.worldX, out var stuffBase) || stuffBase is not Missile missile)
//                 return true;

//             var ship = missile.fromPlayer ? s.ship : c.otherShip;
//             if (ship.Get((Status)SmartExplosiveStatus.Id) <= 0)
//                 return true;
//             //check if seeker missile
//             if (missile.missileType == MissileType.seeker)
//                 return true;
//             //check if missile will hit
//             var target_ship = missile.targetPlayer ? s.ship : c.otherShip;
//             if (target_ship.HasNonEmptyPartAtWorldX(__instance.worldX))
//                 return true;
//             //if not kill action immediately and let update not run.
//             __instance.timer = -1;
//             return false;
//         }

//         private static void SmartExplosiveTurnStart(Ship __instance)
//         {
//             if (SmartExplosiveStatus?.Id == null)
//                 return;
//             __instance.Add((Status)SmartExplosiveStatus.Id, -1);
//         }

//         private void RocketSiloStatusLogic(Harmony harmony)
//         {
//             // patch turn start of player to generate card in hand.
//             var a_start_player_turn_begin_method = typeof(AStartPlayerTurn).GetMethod("Begin") ?? throw MakeInformativeException(Logger, "Couldnt find AStartPlayerTurn.Begin method");
//             var a_start_player_turn_begin_postfix = typeof(Manifest).GetMethod("RocketSiloTurnStart", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic) ?? throw MakeInformativeException(Logger, "Couldnt find Manifest.RocketSiloTurnStart method");
//             harmony.Patch(a_start_player_turn_begin_method, postfix: new HarmonyMethod(a_start_player_turn_begin_postfix));
//         }

//         private static void RocketSiloTurnStart(State s, Combat c)
//         {
//             if (RocketSiloStatus?.Id == null)
//                 return;
//             var status = (Status)RocketSiloStatus.Id;
//             var ammount = s.ship.Get(status);
//             if (ammount <= 0)
//                 return;
//             s.ship.PulseStatus(status);
//             c.QueueImmediate(new AAddCard()
//             {
//                 amount = ammount,
//                 card = new Cards.MicroMissiles(),
//                 handPosition = 0,
//                 destination = CardDestination.Hand
//             });
//         }

//         private void PopBubblesStatusLogic(Harmony harmony)
//         {
//             //patch turn start to decrease this value by 1 and destroy all midrow bubbles.
//             var start_turn_method = typeof(Ship).GetMethod("OnBeginTurn") ?? throw MakeInformativeException(Logger, "Couldnt find Ship.OnBeginTurn method");
//             var start_turn_post = typeof(Manifest).GetMethod("PopBubblesTurnStart", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic) ?? throw MakeInformativeException(Logger, "Couldnt find Manifest.PopBubblesTurnStart method");
//             harmony.Patch(start_turn_method, postfix: new HarmonyMethod(start_turn_post));
//         }

//         private static void PopBubblesTurnStart(Combat c, Ship __instance)
//         {
//             if (PopBubblesStatus?.Id == null)
//                 return;
//             var status = (Status)PopBubblesStatus.Id;
//             if (__instance.Get(status) <= 0)
//                 return;
//             Audio.Play(FSPRO.Event.Hits_ShieldPop);
//             foreach (var stuff in c.stuff.Values)
//             {
//                 stuff.bubbleShield = false;
//             }
//             // __instance.Add(status, -1);
//             __instance.Set(status, 0);
//         }

    }
}
