using Eddie.Actions;
using CobaltCoreModding.Definitions;
using CobaltCoreModding.Definitions.ExternalItems;
using CobaltCoreModding.Definitions.ModContactPoints;
using CobaltCoreModding.Definitions.ModManifests;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using Microsoft.Xna.Framework.Input;
using ILInstruction = Mono.Cecil.Cil.Instruction;

namespace Eddie;

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

    // [HarmonyDebug]
    // [HarmonyTranspiler]
    // private static IEnumerable<CodeInstruction> EqualsXPatch(IEnumerable<CodeInstruction> iseq, ILGenerator il)
    // {
	// 	using IEnumerator<CodeInstruction> iter = iseq.GetEnumerator();
    //     // if (action2 is AStatus aStatus2 && aStatus2.mode == AStatusMode.Set)
	// 	// IL_01e3: ldloc.0
	// 	// IL_01e4: ldfld class CardAction Card/'<>c__DisplayClass56_0'::action
	// 	// IL_01e9: isinst AStatus
	// 	// IL_01ee: stloc.s 7
	// 	// // (no C# code)
	// 	// IL_01f0: ldloc.s 7
	// 	// IL_01f2: brfalse.s IL_0207

	// 	while(iter.MoveNext()) {
    //         var candidates = new List<CodeInstruction>();

    //         candidates.Add(iter.Current);
    //         if(iter.Current.opcode != OpCodes.Ldloc_0) {
    //             foreach (CodeInstruction instr in candidates)
    //                 yield return instr;
	// 			continue;
	// 		}

    //         if(!iter.MoveNext()) {
    //             foreach (CodeInstruction instr in candidates)
    //                 yield return instr;
	// 			break;
	// 		}
    //         candidates.Add(iter.Current);

	// 		if(iter.Current.opcode != OpCodes.Ldfld) {
    //             foreach (CodeInstruction instr in candidates)
    //                 yield return instr;
	// 			continue;
	// 		}

    //         if(!iter.MoveNext()) {
    //             foreach (CodeInstruction instr in candidates)
    //                 yield return instr;
	// 			break;
	// 		}
    //         candidates.Add(iter.Current);

	// 		if(iter.Current.opcode != OpCodes.Isinst || (Type) iter.Current.operand != typeof(AStatus)) {
    //             foreach (CodeInstruction instr in candidates)
    //                 yield return instr;
	// 			continue;
	// 		}

    //         if(!iter.MoveNext()) {
    //             foreach (CodeInstruction instr in candidates)
    //                 yield return instr;
	// 			break;
	// 		}
    //         candidates.Add(iter.Current);

	// 		if(iter.Current.opcode != OpCodes.Stloc_S) {
    //             foreach (CodeInstruction instr in candidates)
    //                 yield return instr;
	// 			continue;
	// 		}
    //         int local_index = ((LocalBuilder) iter.Current.operand).LocalIndex;

    //         if(!iter.MoveNext()) {
    //             foreach (CodeInstruction instr in candidates)
    //                 yield return instr;
	// 			break;
	// 		}
    //         candidates.Add(iter.Current);

	// 		if(iter.Current.opcode != OpCodes.Ldloc_S || ((LocalBuilder) iter.Current.operand).LocalIndex != local_index) {
    //             foreach (CodeInstruction instr in candidates)
    //                 yield return instr;
	// 			continue;
	// 		}

    //         if(!iter.MoveNext()) {
    //             foreach (CodeInstruction instr in candidates)
    //                 yield return instr;
	// 			break;
	// 		}
    //         candidates.Add(iter.Current);

	// 		if(iter.Current.opcode != OpCodes.Brfalse_S) {
    //             foreach (CodeInstruction instr in candidates)
    //                 yield return instr;
	// 			continue;
	// 		}
    //         Label old_label = (Label) iter.Current.operand;

    //         if(!iter.MoveNext()) {
    //             foreach (CodeInstruction instr in candidates)
    //                 yield return instr;
	// 			break;
	// 		}
    //         candidates.Add(iter.Current);

	// 		if(iter.Current.opcode != OpCodes.Ldloc_S || ((LocalBuilder) iter.Current.operand).LocalIndex != local_index) {
    //             foreach (CodeInstruction instr in candidates)
    //                 yield return instr;
	// 			continue;
	// 		}

    //         if(!iter.MoveNext()) {
    //             foreach (CodeInstruction instr in candidates)
    //                 yield return instr;
	// 			break;
	// 		}
    //         candidates.Add(iter.Current);

	// 		if(iter.Current.opcode != OpCodes.Ldfld || ((FieldInfo) iter.Current.operand).Name != "mode") {
    //             foreach (CodeInstruction instr in candidates)
    //                 yield return instr;
	// 			continue;
	// 		}

    //         if(!iter.MoveNext()) {
    //             foreach (CodeInstruction instr in candidates)
    //                 yield return instr;
	// 			break;
	// 		}
    //         candidates.Add(iter.Current);

	// 		if(iter.Current.opcode != OpCodes.Ldc_I4_1) {
    //             foreach (CodeInstruction instr in candidates)
    //                 yield return instr;
	// 			continue;
	// 		}

    //         if(!iter.MoveNext()) {
    //             foreach (CodeInstruction instr in candidates)
    //                 yield return instr;
	// 			break;
	// 		}
    //         candidates.Add(iter.Current);

	// 		if(iter.Current.opcode != OpCodes.Bne_Un_S) {
    //             foreach (CodeInstruction instr in candidates)
    //                 yield return instr;
	// 			continue;
	// 		}

    //         if(!iter.MoveNext()) {
    //             foreach (CodeInstruction instr in candidates)
    //                 yield return instr;
	// 			break;
	// 		}
    //         candidates.Add(iter.Current);

	// 		if(iter.Current.opcode != OpCodes.Ldloc_S || ((LocalBuilder) iter.Current.operand).LocalIndex != local_index) {
    //             foreach (CodeInstruction instr in candidates)
    //                 yield return instr;
	// 			continue;
	// 		}

    //         if(!iter.MoveNext()) {
    //             foreach (CodeInstruction instr in candidates)
    //                 yield return instr;
	// 			break;
	// 		}
    //         candidates.Add(iter.Current);

	// 		if(iter.Current.opcode != OpCodes.Ldloca_S) {
    //             foreach (CodeInstruction instr in candidates)
    //                 yield return instr;
	// 			continue;
	// 		}

    //         if(!iter.MoveNext()) {
    //             foreach (CodeInstruction instr in candidates)
    //                 yield return instr;
	// 			break;
	// 		}
    //         candidates.Add(iter.Current);

	// 		if(iter.Current.opcode != OpCodes.Call) {
    //             foreach (CodeInstruction instr in candidates)
    //                 yield return instr;
	// 			continue;
	// 		}
    //         // MethodInfo method = (MethodInfo) iter.Current.operand;
            
    //         if(!iter.MoveNext()) {
    //             foreach (CodeInstruction instr in candidates)
    //                 yield return instr;
	// 			break;
	// 		}

    //         var new_label = il.DefineLabel();
            
    //         CodeInstruction end_of_if = CopyWithoutLabels(candidates[0]);
    //         end_of_if.labels.Add(old_label);
    //         iter.Current.labels.Remove(old_label);
    //         iter.Current.labels.Add(new_label);

    //         foreach (CodeInstruction instr in candidates)
    //                 yield return instr;

    //         yield return end_of_if;
    //         yield return CopyWithoutLabels(candidates[1]);
    //         yield return new CodeInstruction(OpCodes.Isinst, typeof(AEnergySet));
    //         yield return CopyWithoutLabels(candidates[3]); //stloc
    //         yield return CopyWithoutLabels(candidates[4]); //ldloc
    //         // yield return CopyWithoutLabels(candidates[5]); //brfalse
    //         yield return new CodeInstruction(OpCodes.Brfalse_S, new_label);
    //         yield return CopyWithoutLabels(candidates[candidates.Count - 3]); //ldloc
    //         yield return CopyWithoutLabels(candidates[candidates.Count - 2]); //ldloca
    //         yield return CopyWithoutLabels(candidates[candidates.Count - 1]); //call
    //         yield return iter.Current;


    //         break;

    //         // IL_012c: ldloc.0
    //         // IL_012d: ldfld class CardAction Card/'<>c__DisplayClass56_0'::action
    //         // IL_0132: isinst AVariableHint
    //         // IL_0137: stloc.s 4
    //         // // (no C# code)
    //         // IL_0139: ldloc.s 4
    //         // IL_013b: brfalse.s IL_015b

    //         // IL_013d: ldloc.s 4
    //         // IL_013f: ldfld bool AVariableHint::hand
    //         // IL_0144: brtrue.s IL_0154
	// 	}
	// 	while(iter.MoveNext())
	// 		yield return iter.Current;
	// }

    // public static CodeInstruction CopyWithoutLabels(CodeInstruction instr)
    // {
    //     return new CodeInstruction(instr.opcode, instr.operand);
    // }
}