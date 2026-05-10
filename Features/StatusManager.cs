using System.Reflection;
using System.Reflection.Emit;

using HarmonyLib;
using Nickel;
using Nanoray.PluginManager;
using Shockah.Kokoro;

namespace TheJazMaster.Eddie;

[HarmonyPatch]
public class StatusManager : IKokoroApi.IV2.IStatusLogicApi.IHook, IKokoroApi.IV2.IStatusRenderingApi.IHook
{
    static ModEntry Instance => ModEntry.Instance;
    static IModHelper Helper => Instance.Helper;
    static IPluginPackage<IModManifest> Package => Instance.Package;

	internal static Status CircuitStatus;
	internal static Status ClosedCircuitStatus;
    internal static Status MoreEnergyStatus;


    public StatusManager() {
        Instance.KokoroApi.StatusLogic.RegisterHook(this);
        Instance.KokoroApi.StatusRendering.RegisterHook(this);

        CircuitStatus = Helper.Content.Statuses.RegisterStatus("circuit", new()
		{
			Definition = new()
			{
				icon = Helper.Content.Sprites.RegisterSprite(Package.PackageRoot.GetRelativeFile("Sprites/icons/circuit.png")).Sprite,
				color = new("00802d"),
				isGood = true
			},	
			Name = Instance.AnyLocalizations.Bind(["status", "Circuit", "name"]).Localize,
			Description = Instance.AnyLocalizations.Bind(["status", "Circuit", "description"]).Localize
		}).Status;

        ClosedCircuitStatus = Helper.Content.Statuses.RegisterStatus("ClosedCircuit", new()
		{
			Definition = new()
			{
				icon = Helper.Content.Sprites.RegisterSprite(Package.PackageRoot.GetRelativeFile("Sprites/icons/closed_circuit.png")).Sprite,
				color = new("f2ca00"),
				isGood = true
			},
			Name = Instance.AnyLocalizations.Bind(["status", "ClosedCircuit", "name"]).Localize,
			Description = Instance.AnyLocalizations.Bind(["status", "ClosedCircuit", "description"]).Localize
		}).Status;

        MoreEnergyStatus = Helper.Content.Statuses.RegisterStatus("MoreEnergy", new()
		{
			Definition = new()
			{
				icon = Helper.Content.Sprites.RegisterSprite(Package.PackageRoot.GetRelativeFile("Sprites/icons/energy_more_every_turn.png")).Sprite,
				color = new("18529c")
			},
			Name = Instance.AnyLocalizations.Bind(["status", "MoreEnergy", "name"]).Localize,
			Description = Instance.AnyLocalizations.Bind(["status", "MoreEnergy", "description"]).Localize
		}).Status;

        Instance.DraculaApi?.RegisterBloodTapOptionProvider(CircuitStatus, (_, _, status) => [
            new AHurt { targetPlayer = true, hurtAmount = 2 },
            new AStatus { targetPlayer = true, status = status, statusAmount = 1 },
            new AStatus { targetPlayer = true, status = ClosedCircuitStatus, statusAmount = 1 },
        ]);
        Instance.DraculaApi?.RegisterBloodTapOptionProvider(ClosedCircuitStatus, (_, _, status) => [
            new AHurt { targetPlayer = true, hurtAmount = 1 },
            new AStatus { targetPlayer = true, status = status, statusAmount = 4 }
        ]);
    }
	public void OnStatusTurnTrigger(IKokoroApi.IV2.IStatusLogicApi.IHook.IOnStatusTurnTriggerArgs args) {
        if (args.Timing != IKokoroApi.IV2.IStatusLogicApi.StatusTurnTriggerTiming.TurnStart) return;

        if (args.Status == CircuitStatus) args.Ship.Add(ClosedCircuitStatus, args.OldAmount);
        if (args.Status == MoreEnergyStatus) args.Combat.energy += args.OldAmount;
    }

    public IReadOnlyList<Tooltip> OverrideStatusTooltips(IKokoroApi.IV2.IStatusRenderingApi.IHook.IOverrideStatusTooltipsArgs args) {
        if (args.Status == CircuitStatus) 
            return [.. args.Tooltips, .. StatusMeta.GetTooltips(ClosedCircuitStatus, args.Amount)];
        return args.Tooltips;
    }

    private static List<int> previousHandUuids = [];

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Combat), nameof(Combat.Update))]
    private static void Combat_Update_Prefix(G g, Combat __instance) {
        previousHandUuids = [.. __instance.hand.Select(card => card.uuid)];
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Combat), nameof(Combat.SendCardToDiscard))]
    private static bool Combat_SendCardToDiscard_Prefix(State s, Combat __instance, Card card) {
        if (s.ship.Get(ClosedCircuitStatus) > 0 && previousHandUuids.Contains(card.uuid)) {
            List<Card> hand = __instance.hand;
            int j = 0;
            for (int i = 0; i < previousHandUuids.Count && j < hand.Count; i++) {
                if (previousHandUuids[i] == card.uuid) break;
                if (previousHandUuids[i] == hand[j].uuid) j++;
            }
            __instance.hand.Insert(j, card);
            s.ship.Add(ClosedCircuitStatus, -1);
            s.ship.PulseStatus(ClosedCircuitStatus);
            card.flipAnim = 1;
            return false;
        }
        return true;
    }

    // [HarmonyTranspiler]
    // [HarmonyPatch(typeof(Combat), nameof(Combat.TryPlayCard))]
    // private static void Combat_TryPlayCard_Transpiler() {
        
    // }

    // [HarmonyTranspiler]
    // [HarmonyPatch(typeof(Combat), nameof(Combat.DiscardHand))]
    // private static void Combat_TryPlayCard_Transpiler() {
        
    // }

    //Remember hand on Combat.Update prefix, then when discarding, go over the list. If remember[i] matches hand[j], incremenent ij, otherwise increment i. If remember[i] == card, j is its position

    // private static IEnumerable<CodeInstruction> ClosedCircuitPreventDiscard(IEnumerable<CodeInstruction> iseq, ILGenerator il, MethodBase originalMethod)
    // {
    //     int? local_index = null;
    //     foreach (LocalVariableInfo info in originalMethod.GetMethodBody()!.LocalVariables)
    //     {
    //         if (info.LocalType == typeof(CardData))
    //         {
    //             local_index = info.LocalIndex;
    //             break;
    //         }
    //     }
        
        
    //     using IEnumerator<CodeInstruction> iter = iseq.GetEnumerator();
    //     if (local_index != null && CircuitStatus != null && CircuitStatus.Id != null && ClosedCircuitStatus != null && ClosedCircuitStatus.Id != null)
    //     {
    //         while(iter.MoveNext()) {
    //             yield return iter.Current;
    //             if(!TranspilerUtils.IsLocalLoad(iter.Current) || TranspilerUtils.ExtractLocalIndex(iter.Current) != local_index) {
    //                 continue;
    //             }
    //             var card_data_local_load_opcode = iter.Current.opcode;
    //             var card_data_local_operand = iter.Current.operand;

    //             if(!iter.MoveNext()) {
    //                 break;
    //             }
    //             yield return iter.Current;

    //             if(iter.Current.opcode != OpCodes.Ldfld || ((FieldInfo) iter.Current.operand).Name != "infinite") {
    //                 continue;
    //             }
    //             var infinite_field_info = iter.Current.operand;

    //             if(!iter.MoveNext()) {
    //                 break;
    //             }
    //             yield return iter.Current;

    //             if(iter.Current.opcode != OpCodes.Brfalse_S) {
    //                 continue;
    //             }

    //             if(!iter.MoveNext()) {
    //                 break;
    //             }
    //             yield return iter.Current;

    //             if(!TranspilerUtils.IsLocalLoad(iter.Current)) {
    //                 continue;
    //             }
    //             var exhaust_flag_local_load_opcode = iter.Current.opcode;
    //             var exhaust_flag_local_operand = iter.Current.operand;

    //             if(!iter.MoveNext()) {
    //                 break;
    //             }
    //             yield return iter.Current;

    //             if(iter.Current.opcode != OpCodes.Ldc_I4_0) {
    //                 continue;
    //             }

    //             if(!iter.MoveNext()) {
    //                 break;
    //             }
    //             yield return iter.Current;

    //             if(iter.Current.opcode != OpCodes.Ceq) {
    //                 continue;
    //             }

    //             if(!iter.MoveNext()) {
    //                 break;
    //             }
    //             yield return iter.Current;

    //             if(iter.Current.opcode != OpCodes.Br_S) {
    //                 continue;
    //             }

    //             if(!iter.MoveNext()) {
    //                 break;
    //             }
    //             yield return iter.Current;

    //             if(iter.Current.opcode != OpCodes.Ldc_I4_0) {
    //                 continue;
    //             }

    //             if(!iter.MoveNext()) {
    //                 break;
    //             }
    //             yield return iter.Current;

    //             if(!TranspilerUtils.IsLocalStore(iter.Current)) {
    //                 continue;
    //             }
    //             var discard_flag_local_store_opcode = iter.Current.opcode;
    //             var discard_flag_local_load_opcode = TranspilerUtils.StoreToLoad(discard_flag_local_store_opcode);
    //             var discard_flag_local_operand = iter.Current.operand;
    //             Label end_label = il.DefineLabel();
    //             Label end_infinite_fix_label = il.DefineLabel();


    //             yield return new CodeInstruction(discard_flag_local_load_opcode, discard_flag_local_operand);
    //             yield return new CodeInstruction(OpCodes.Brfalse, end_infinite_fix_label);
    //             yield return new CodeInstruction(card_data_local_load_opcode, card_data_local_operand);
    //             yield return new CodeInstruction(OpCodes.Ldfld, typeof(CardData).GetField("singleUse"));
    //             yield return new CodeInstruction(OpCodes.Brfalse, end_infinite_fix_label);
    //             yield return new CodeInstruction(OpCodes.Ldc_I4_0);
    //             yield return new CodeInstruction(discard_flag_local_store_opcode, discard_flag_local_operand);

    //             // The if statement: if(!exhaust && !stay && !cardData.infinite && s.ship.Get(ClosedCirtuit) > 0)
    //             yield return new CodeInstruction(exhaust_flag_local_load_opcode, exhaust_flag_local_operand).WithLabels(end_infinite_fix_label);
    //             yield return new CodeInstruction(OpCodes.Brtrue, end_label);
    //             yield return new CodeInstruction(discard_flag_local_load_opcode, discard_flag_local_operand);
    //             yield return new CodeInstruction(OpCodes.Brtrue, end_label);
    //             yield return new CodeInstruction(card_data_local_load_opcode, card_data_local_operand);
    //             yield return new CodeInstruction(OpCodes.Ldfld, infinite_field_info);
    //             yield return new CodeInstruction(OpCodes.Brtrue, end_label);
    //             yield return new CodeInstruction(OpCodes.Ldarg_1);
    //             yield return new CodeInstruction(OpCodes.Ldfld, typeof(State).GetField("ship"));
    //             yield return new CodeInstruction(OpCodes.Ldc_I4, ClosedCircuitStatus.Id);
    //             yield return new CodeInstruction(OpCodes.Callvirt, typeof(Ship).GetMethod("Get"));
    //             yield return new CodeInstruction(OpCodes.Ldc_I4_0);
    //             yield return new CodeInstruction(OpCodes.Ble, end_label);
    //             yield return new CodeInstruction(card_data_local_load_opcode, card_data_local_operand);
    //             yield return new CodeInstruction(OpCodes.Ldfld, typeof(CardData).GetField("singleUse"));
    //             yield return new CodeInstruction(OpCodes.Brtrue, end_label);

    //             // s.ship.Set(ClosedCircuit, s.ship.Get(ClosedCircuit) - 1);
    //             yield return new CodeInstruction(OpCodes.Ldarg_1);
    //             yield return new CodeInstruction(OpCodes.Ldfld, typeof(State).GetField("ship"));
    //             yield return new CodeInstruction(OpCodes.Ldc_I4, Manifest.ClosedCircuitStatus.Id);
    //             yield return new CodeInstruction(OpCodes.Ldarg_1);
    //             yield return new CodeInstruction(OpCodes.Ldfld, typeof(State).GetField("ship"));
    //             yield return new CodeInstruction(OpCodes.Ldc_I4, Manifest.ClosedCircuitStatus.Id);
    //             yield return new CodeInstruction(OpCodes.Callvirt, typeof(Ship).GetMethod("Get"));
    //             yield return new CodeInstruction(OpCodes.Ldc_I4_1);
    //             yield return new CodeInstruction(OpCodes.Sub);
    //             yield return new CodeInstruction(OpCodes.Callvirt, typeof(Ship).GetMethod("Set"));

    //             // // stay = true
    //             yield return new CodeInstruction(OpCodes.Ldc_I4_1);
    //             yield return new CodeInstruction(discard_flag_local_store_opcode, discard_flag_local_operand);
                

    //             if(!iter.MoveNext()) {
    //                 break;
    //             }
    //             iter.Current.labels.Add(end_label);
    //             yield return iter.Current;
    //             break;
    //         }
    //     }
    //     while(iter.MoveNext())
    //         yield return iter.Current;
    // }

    // private static IEnumerable<CodeInstruction> ClosedCircuitPreventHandDiscard(IEnumerable<CodeInstruction> iseq, ILGenerator il, MethodBase originalMethod)
    // {
    //     using IEnumerator<CodeInstruction> iter = iseq.GetEnumerator();

    //     while(iter.MoveNext()) {
    //         yield return iter.Current;
    //         if(iter.Current.opcode != OpCodes.Ldarga_S) {
    //             continue;
    //         }
    //         byte countLocal = (byte)iter.Current.operand;

    //         if(!iter.MoveNext()) {
    //             break;
    //         }
    //         yield return iter.Current;

    //         if(iter.Current.opcode != OpCodes.Call || ((MethodInfo)iter.Current.operand).Name != "get_HasValue") {
    //             continue;
    //         }

    //         if(!iter.MoveNext()) {
    //             break;
    //         }
    //         yield return iter.Current;

    //         if(iter.Current.opcode != OpCodes.Brfalse_S) {
    //             continue;
    //         }
    //         Label endLabel = (Label)iter.Current.operand;
    //         Label midLabel = il.DefineLabel();
    //         iter.Current.operand = midLabel;

    //         yield return new CodeInstruction(OpCodes.Ldarg_0);
    //         yield return new CodeInstruction(OpCodes.Ldarg_1);
    //         yield return new CodeInstruction(OpCodes.Ldarga, countLocal);
    //         yield return new CodeInstruction(OpCodes.Call, typeof(Manifest).GetMethod("NewCount", BindingFlags.Static | BindingFlags.NonPublic));
    //         yield return new CodeInstruction(OpCodes.Starg, countLocal);

    //         while (iter.MoveNext() && !iter.Current.labels.Contains(endLabel)) {
    //             yield return iter.Current;
    //         }

    //         if (!iter.Current.labels.Contains(endLabel)) {
    //             throw new Exception("That shouldn't happen");
    //         }

    //         yield return new CodeInstruction(OpCodes.Br, endLabel);
    //         yield return new CodeInstruction(OpCodes.Ldarg_0).WithLabels(midLabel);
    //         yield return new CodeInstruction(OpCodes.Ldarg_1);
    //         yield return new CodeInstruction(OpCodes.Ldloc_1);
    //         yield return new CodeInstruction(OpCodes.Call, typeof(Manifest).GetMethod("ClosedCircuitRetain", BindingFlags.Static | BindingFlags.NonPublic));
    //         yield return new CodeInstruction(OpCodes.Stloc_1);

    //         yield return iter.Current.WithLabels(endLabel);
    //         break;
    //     }

    //     while(iter.MoveNext())
    //         yield return iter.Current;
    // }

    // private static int? NewCount(Combat c, State s, ref int? countMebbe) {
    //     if (countMebbe == null) return null;
    //     int count = countMebbe.Value;
    //     int handSize = c.hand.Count;
    //     int ccAmount = s.ship.Get((Status)ClosedCircuitStatus!.Id!);
    //     int toRemove = count - Math.Min(count, ccAmount);
    //     s.ship.Set((Status)ClosedCircuitStatus!.Id!, ccAmount - Math.Min(count, ccAmount));
    //     return toRemove;
    // }

    // private static List<Card> ClosedCircuitRetain(Combat c, State s, List<Card> list) {
    //     int ccAmount = s.ship.Get((Status)ClosedCircuitStatus!.Id!);
    //     int ccAmountAfter = Math.Max(0, ccAmount - list.Count);
    //     List<Card> cards = list.TakeLast(list.Count - ccAmount).ToList();
    //     s.ship.Set((Status)ClosedCircuitStatus!.Id!, ccAmountAfter);
    //     return cards;
    // }
}