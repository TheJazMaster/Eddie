using TheJazMaster.Eddie.Actions;
using CobaltCoreModding.Definitions;
using CobaltCoreModding.Definitions.ExternalItems;
using CobaltCoreModding.Definitions.ModContactPoints;
using CobaltCoreModding.Definitions.ModManifests;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using Microsoft.Xna.Framework.Input;

namespace TheJazMaster.Eddie;

// [HarmonyPatch]
[HarmonyDebug]
public static class RenderingPatch {

	// [HarmonyPatch(typeof(Card), nameof(Card.RenderAction))]
	[HarmonyTranspiler]
	private static IEnumerable<CodeInstruction> XEqualsPatch(IEnumerable<CodeInstruction> iseq)
    {
		using IEnumerator<CodeInstruction> iter = iseq.GetEnumerator();
        // if (action2 is AVariableHint aVariableHint && (aVariableHint.hand || aVariableHint.status.HasValue))
		while(iter.MoveNext()) {
            yield return iter.Current;
            if(iter.Current.opcode != OpCodes.Ldloc_0) {
				continue;
			}

            if(!iter.MoveNext()) {
				break;
			}
            yield return iter.Current;

			if(iter.Current.opcode != OpCodes.Ldfld) {
				continue;
			}

            if(!iter.MoveNext()) {
				break;
			}
            yield return iter.Current;

			if(iter.Current.opcode != OpCodes.Isinst || (Type) iter.Current.operand != typeof(AVariableHint)) {
				continue;
			}

            if(!iter.MoveNext()) {
				break;
			}
            yield return iter.Current;

			if(iter.Current.opcode != OpCodes.Stloc_S) {
				continue;
			}
            int local_index = ((LocalBuilder) iter.Current.operand).LocalIndex;

            if(!iter.MoveNext()) {
				break;
			}
            yield return iter.Current;

			if(iter.Current.opcode != OpCodes.Ldloc_S || ((LocalBuilder) iter.Current.operand).LocalIndex != local_index) {
				continue;
			}

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

			if(iter.Current.opcode != OpCodes.Ldloc_S || ((LocalBuilder) iter.Current.operand).LocalIndex != local_index) {
				continue;
			}

            if(!iter.MoveNext()) {
				break;
			}
            yield return iter.Current;

			if(iter.Current.opcode != OpCodes.Ldfld) {
				continue;
			}

            if(!iter.MoveNext()) {
				break;
			}
            yield return iter.Current;

			if(iter.Current.opcode != OpCodes.Brtrue_S) {
				continue;
			}
            object label = iter.Current.operand;

            // yield return new CodeInstruction(OpCodes.Ldc_I4_1);
            yield return new CodeInstruction(OpCodes.Ldloc_S, local_index);
            yield return new CodeInstruction(OpCodes.Isinst, typeof(AVariableHintEnergy));
            yield return new CodeInstruction(OpCodes.Brtrue_S, label);
            break;

            // IL_012c: ldloc.0
            // IL_012d: ldfld class CardAction Card/'<>c__DisplayClass56_0'::action
            // IL_0132: isinst AVariableHint
            // IL_0137: stloc.s 4
            // // (no C# code)
            // IL_0139: ldloc.s 4
            // IL_013b: brfalse.s IL_015b

            // IL_013d: ldloc.s 4
            // IL_013f: ldfld bool AVariableHint::hand
            // IL_0144: brtrue.s IL_0154
		}
		while(iter.MoveNext())
			yield return iter.Current;
	}
    
    private static IEnumerable<CodeInstruction> DiscountHandPatch(IEnumerable<CodeInstruction> iseq, ILGenerator il, MethodBase originalMethod)
    {
		using IEnumerator<CodeInstruction> iter = iseq.GetEnumerator();
        // if (action2 is AVariableHint aVariableHint && (aVariableHint.hand || aVariableHint.status.HasValue))
		while(iter.MoveNext()) {
            yield return iter.Current;
            if(iter.Current.opcode != OpCodes.Ldloc_0) {
				continue;
			}

            if(!iter.MoveNext()) {
				break;
			}
            yield return iter.Current;

			if(iter.Current.opcode != OpCodes.Ldfld) {
				continue;
			}
            var ldfldInstruction = iter.Current;

            if(!iter.MoveNext()) {
				break;
			}
            yield return iter.Current;

			if(iter.Current.opcode != OpCodes.Isinst || (Type) iter.Current.operand != typeof(AExhaustEntireHand)) {
				continue;
			}

            if(!iter.MoveNext()) {
				break;
			}

			if(iter.Current.opcode != OpCodes.Brfalse_S) {
                yield return iter.Current;
				continue;
			}

            Label endLabel = (Label) iter.Current.operand;
            Label trueLabel = il.DefineLabel();

            yield return new CodeInstruction(OpCodes.Brtrue, trueLabel);
            yield return new CodeInstruction(OpCodes.Ldloc_0);
            yield return new CodeInstruction(ldfldInstruction.opcode, ldfldInstruction.operand);
            yield return new CodeInstruction(OpCodes.Isinst, typeof(ADiscountHand));
            yield return new CodeInstruction(OpCodes.Brtrue, trueLabel);
            yield return new CodeInstruction(OpCodes.Br, endLabel);
            
            iter.MoveNext();
            yield return iter.Current.WithLabels(trueLabel);
            break;
		}
		while(iter.MoveNext())
			yield return iter.Current;
	}
}