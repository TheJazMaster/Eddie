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

namespace TheJazMaster.Eddie;

public partial class Manifest : IStatusManifest
{
    public static ExternalStatus CircuitStatus { get; private set; } = null!;
    public static ExternalStatus ClosedCircuitStatus { get; private set; } = null!;
    public static ExternalStatus LoseEnergyEveryTurnStatus { get; private set; } = null!;
    public static ExternalStatus GainEnergyEveryTurnStatus { get; private set; } = null!;
    public static ExternalStatus HealNextTurnStatus { get; private set; } = null!;
    // public static ExternalStatus OverchargeStatus { get; private set; } = null!;

    internal static readonly string NoHealThisTurnKey = "NoHealThisTurn";

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
        LoseEnergyEveryTurnStatus.AddLocalisation("Less Energy", "Lose {0} <c=energy>ENERGY</c> at the start of every turn.");
        statusRegistry.RegisterStatus(LoseEnergyEveryTurnStatus);

        GainEnergyEveryTurnStatus = new ExternalStatus("Eddie.Status.GainEnergyEveryTurn", true, Eddie_PrimaryColor, null, GainEnergyEveryTurnIcon ?? throw new Exception("Missing Lose Energy Icon for status"), true);
        GainEnergyEveryTurnStatus.AddLocalisation("More Energy", "Gain {0} <c=energy>ENERGY</c> at the start of every turn.");
        statusRegistry.RegisterStatus(GainEnergyEveryTurnStatus);

        HealNextTurnStatus = new ExternalStatus("Eddie.Status.HealNextTurn", true, Eddie_PrimaryColor, null, HealNextTurnIcon ?? throw new Exception("Missing Heal Next Turn Icon for status"), true);
        HealNextTurnStatus.AddLocalisation("Regain Hull Later", "Regain {0} hull at the end of combat, or at the end of a turn where you didn't suffer temporary hull loss. <c=downside>This does not count as healing.</c>");
        statusRegistry.RegisterStatus(HealNextTurnStatus);

        // OverchargeStatus = new ExternalStatus("Eddie.Status.Overcharge", true, System.Drawing.Color.FromArgb(255, 56, 56), null, OverchargeIcon ?? throw new Exception("Missing Overcharge Icon for status"), true);
        // OverchargeStatus.AddLocalisation("Overcharge", "At the beginning of your turn, lose 1 <c=status>OVERCHARGE</c> and gain 1 <c=status>OVERDRIVE</c>.");
        // statusRegistry.RegisterStatus(OverchargeStatus);

        // Patching status logic
        var harmony = new Harmony("Eddie.Status");
        CircuitStatusLogic(harmony);
        ClosedCircuitStatusLogic(harmony);
        EnergyStatusLogic(harmony);
        HealNextTurnStatusLogic(harmony);

        DraculaApi?.RegisterBloodTapOptionProvider((Status)CircuitStatus.Id!, (_, _, status) => new() {
            new AHurt { targetPlayer = true, hurtAmount = 2 },
            new AStatus { targetPlayer = true, status = status, statusAmount = 1 },
            new AStatus { targetPlayer = true, status = (Status)ClosedCircuitStatus.Id!, statusAmount = 1 },
        });
        DraculaApi?.RegisterBloodTapOptionProvider((Status)ClosedCircuitStatus.Id!, (_, _, status) => new() {
            new AHurt { targetPlayer = true, hurtAmount = 1 },
            new AStatus { targetPlayer = true, status = status, statusAmount = 4 }
        });
        DraculaApi?.RegisterBloodTapOptionProvider((Status)HealNextTurnStatus.Id!, (_, _, status) => new() {
            new AHurt { targetPlayer = true, hurtAmount = 1 },
            new AStatus { targetPlayer = true, status = status, statusAmount = 1 }
        });
    }

    private void EnergyStatusLogic(Harmony harmony)
    {
        var start_turn_method = typeof(Ship).GetMethod("OnBeginTurn") ?? throw new Exception("Couldn't find Ship.OnBeginTurn method");
        var start_turn_post = typeof(Manifest).GetMethod("ChangeEnergy", BindingFlags.Static | BindingFlags.NonPublic) ?? throw new Exception("Couldnt find Manifest.LoseEnergyEveryTurn method");
        harmony.Patch(start_turn_method, postfix: new HarmonyMethod(start_turn_post));
    }

    private void HealNextTurnStatusLogic(Harmony harmony)
    {
        if (HealNextTurnStatus?.Id == null)
            return;
        var start_turn_method = typeof(Ship).GetMethod("OnAfterTurn") ?? throw new Exception("Couldn't find Ship.OnAfterTurn method");
        var start_turn_post = typeof(Manifest).GetMethod("HealOnEndOfTurn", BindingFlags.Static | BindingFlags.NonPublic) ?? throw new Exception("Couldnt find Manifest.HealOnEndOfTurn method");
        harmony.Patch(start_turn_method, postfix: new HarmonyMethod(start_turn_post));

        var updateMethod = typeof(Combat).GetMethod("Update") ?? throw new Exception("Couldn't find Combat.Update method");
        var patch = typeof(Manifest).GetMethod("HealOnEnemyDie", BindingFlags.Static | BindingFlags.NonPublic) ?? throw new Exception("Couldnt find Manifest.HealOnEnemyDie method");
        harmony.Patch(updateMethod, postfix: new HarmonyMethod(patch));
    }

    private void ClosedCircuitStatusLogic(Harmony harmony)
    {
        if (ClosedCircuitStatus?.Id == null)
            return;
        {
            var try_play_method = typeof(Combat).GetMethod("TryPlayCard") ?? throw new Exception("Couldn't find Combat.TryPlayCard method");
            var patch = typeof(Manifest).GetMethod("ClosedCircuitPreventDiscard", BindingFlags.Static | BindingFlags.NonPublic) ?? throw new Exception("Couldnt find Manifest.ClosedCircuitPreventDiscard method");
            harmony.Patch(try_play_method, transpiler: new HarmonyMethod(patch));
        } {
            var discard_hand_method = typeof(Combat).GetMethod("DiscardHand") ?? throw new Exception("Couldn't find Combat.DiscardHand method");
            var patch = typeof(Manifest).GetMethod("ClosedCircuitPreventHandDiscard", BindingFlags.Static |  BindingFlags.NonPublic) ?? throw new Exception("Couldnt find Manifest.ClosedCircuitPreventHandDiscard method");
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

    private static void HealOnEndOfTurn(Ship __instance, State s, Combat c)
    {
        var status = (Status)HealNextTurnStatus!.Id!;


        if (__instance.Get(status) > 0) {
            if (!Instance.KokoroApi.TryGetExtensionData(__instance, NoHealThisTurnKey, out bool value) || !value) {
                __instance.Heal(__instance.Get(status));
                __instance.Set(status, 0);
            }
        }
        Instance.KokoroApi.RemoveExtensionData(__instance, NoHealThisTurnKey);
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

    private static void ChangeEnergy(Ship __instance, State s, Combat c)
    {
        var gain = (Status)GainEnergyEveryTurnStatus.Id!;
        var lose = (Status)LoseEnergyEveryTurnStatus.Id!;
        c.energy += __instance.Get(gain) - __instance.Get(lose);
        c.energy = Math.Max(0, c.energy);
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
                Label end_infinite_fix_label = il.DefineLabel();


                yield return new CodeInstruction(discard_flag_local_load_opcode, discard_flag_local_operand);
                yield return new CodeInstruction(OpCodes.Brfalse, end_infinite_fix_label);
                yield return new CodeInstruction(card_data_local_load_opcode, card_data_local_operand);
                yield return new CodeInstruction(OpCodes.Ldfld, typeof(CardData).GetField("singleUse"));
                yield return new CodeInstruction(OpCodes.Brfalse, end_infinite_fix_label);
                yield return new CodeInstruction(OpCodes.Ldc_I4_0);
                yield return new CodeInstruction(discard_flag_local_store_opcode, discard_flag_local_operand);

                // The if statement: if(!exhaust && !stay && !cardData.infinite && s.ship.Get(ClosedCirtuit) > 0)
                yield return new CodeInstruction(exhaust_flag_local_load_opcode, exhaust_flag_local_operand).WithLabels(end_infinite_fix_label);
                yield return new CodeInstruction(OpCodes.Brtrue, end_label);
                yield return new CodeInstruction(discard_flag_local_load_opcode, discard_flag_local_operand);
                yield return new CodeInstruction(OpCodes.Brtrue, end_label);
                yield return new CodeInstruction(card_data_local_load_opcode, card_data_local_operand);
                yield return new CodeInstruction(OpCodes.Ldfld, infinite_field_info);
                yield return new CodeInstruction(OpCodes.Brtrue, end_label);
                yield return new CodeInstruction(OpCodes.Ldarg_1);
                yield return new CodeInstruction(OpCodes.Ldfld, typeof(State).GetField("ship"));
                yield return new CodeInstruction(OpCodes.Ldc_I4, ClosedCircuitStatus.Id);
                yield return new CodeInstruction(OpCodes.Callvirt, typeof(Ship).GetMethod("Get"));
                yield return new CodeInstruction(OpCodes.Ldc_I4_0);
                yield return new CodeInstruction(OpCodes.Ble, end_label);
                yield return new CodeInstruction(card_data_local_load_opcode, card_data_local_operand);
                yield return new CodeInstruction(OpCodes.Ldfld, typeof(CardData).GetField("singleUse"));
                yield return new CodeInstruction(OpCodes.Brtrue, end_label);

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
            yield return new CodeInstruction(OpCodes.Ldloc_1);
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

    private static List<Card> ClosedCircuitRetain(Combat c, State s, List<Card> list) {
        int ccAmount = s.ship.Get((Status)ClosedCircuitStatus!.Id!);
        int ccAmountAfter = Math.Max(0, ccAmount - list.Count);
        List<Card> cards = list.TakeLast(list.Count - ccAmount).ToList();
        s.ship.Set((Status)ClosedCircuitStatus!.Id!, ccAmountAfter);
        return cards;
    }
}