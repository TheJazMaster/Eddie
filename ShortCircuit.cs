using Eddie.Actions;
using CobaltCoreModding.Definitions;
using CobaltCoreModding.Definitions.ExternalItems;
using CobaltCoreModding.Definitions.ModContactPoints;
using CobaltCoreModding.Definitions.ModManifests;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using HarmonyLib;
using FMOD;
using FSPRO;
using Microsoft.Xna.Framework.Input;
using ILInstruction = Mono.Cecil.Cil.Instruction;

namespace Eddie
{
	public static class ShortCircuit
	{

		public static ConditionalWeakTable<Card, StructRef<bool>> short_circuit = new ConditionalWeakTable<Card, StructRef<bool>>();
		public static ConditionalWeakTable<Card, StructRef<bool>> short_circuit_override = new ConditionalWeakTable<Card, StructRef<bool>>();
		public static ConditionalWeakTable<Card, StructRef<bool>> short_circuit_override_is_permanent = new ConditionalWeakTable<Card, StructRef<bool>>();

		private static void ShortCircuitIncreaseCost(Card __instance, ref CardData __result, State state)
		{
			if (DoesShortCircuit(__instance))
			{
				__result.cost += 1;
			}
		}

		private static void ShortCircuitRemoveOverride(Combat __instance, State state)
		{
			foreach (Card card in state.deck)
			{
				StructRef<bool>? value;
				short_circuit_override_is_permanent.TryGetValue(card, out value);
				if (value == null || !value)
				{
					short_circuit_override.Remove(card);
				}
			}
		}

		private static IEnumerable<CodeInstruction> ShortCircuitPlayTwice(IEnumerable<CodeInstruction> iseq, ILGenerator il, MethodBase originalMethod)
		{
			int? local_index = null;
            foreach (LocalVariableInfo info in originalMethod.GetMethodBody()!.LocalVariables)
            {
                if (info.LocalType == typeof(List<CardAction>))
                {
                    local_index = info.LocalIndex;
                    break;
                }
            }
			if (local_index == null)
				throw new Exception("CardAction list not found");

			using IEnumerator<CodeInstruction> iter = iseq.GetEnumerator();

			while(iter.MoveNext()) {
				List<CodeInstruction> candidates = new List<CodeInstruction>();
				yield return iter.Current;
				if(iter.Current.opcode != OpCodes.Ldarg_0) {
					continue;
				}

				candidates.Add(iter.Current);
				if(!iter.MoveNext()) {
					break;
				}
				yield return iter.Current;
				
				if(!TranspilerUtils.IsLocalLoad(iter.Current) || TranspilerUtils.ExtractLocalIndex(iter.Current) != local_index) {
					continue;
				}

				candidates.Add(iter.Current);
				if(!iter.MoveNext()) {
					break;
				}
				yield return iter.Current;

				if(iter.Current.opcode != OpCodes.Call || (MethodInfo) iter.Current.operand != typeof(Combat).GetMethod("Queue", 0, new Type[] {typeof(IEnumerable<CardAction>)})) {
					continue;
				}

				candidates.Add(iter.Current);
				
				Label end_label = il.DefineLabel();

				yield return new CodeInstruction(OpCodes.Ldarg_2);
				yield return new CodeInstruction(OpCodes.Ldc_I4_1);
				yield return new CodeInstruction(OpCodes.Call, typeof(ShortCircuit).GetMethod("DoesShortCircuit"));
				yield return new CodeInstruction(OpCodes.Brfalse, end_label);

				yield return new CodeInstruction(candidates[0].opcode, candidates[0].operand);
				yield return new CodeInstruction(candidates[1].opcode, candidates[1].operand);
				yield return new CodeInstruction(OpCodes.Call, typeof(Mutil).GetMethod("DeepCopy")!.MakeGenericMethod(new Type[] {typeof(List<CardAction>)}));
				yield return new CodeInstruction(candidates[2].opcode, candidates[2].operand);

				iter.MoveNext();
				iter.Current.labels.Add(end_label);
				yield return iter.Current;
				break;
			}
			while(iter.MoveNext())
				yield return iter.Current;
		}

		public static bool DoesShortCircuit(Card card, bool with_overrides = true)
		{
			StructRef<bool>? is_innate;
			StructRef<bool>? is_overridden;
			return (short_circuit.TryGetValue(card, out is_innate) && is_innate) || (with_overrides && short_circuit_override.TryGetValue(card, out is_overridden) && is_overridden);
		}
	}
}