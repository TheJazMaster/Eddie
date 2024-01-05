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

namespace Eddie
{
	public static class ShortCircuit
	{

		public static ConditionalWeakTable<Card, StructRef<bool>> short_circuit = new ConditionalWeakTable<Card, StructRef<bool>>();
		public static ConditionalWeakTable<Card, StructRef<bool>> short_circuit_override = new ConditionalWeakTable<Card, StructRef<bool>>();
		public static ConditionalWeakTable<Card, StructRef<bool>> short_circuit_override_is_permanent = new ConditionalWeakTable<Card, StructRef<bool>>();

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

		[HarmonyBefore(new string[] { "Shockah.Soggins" })]
		private static IEnumerable<CodeInstruction> ShortCircuitDiscardMaybe(IEnumerable<CodeInstruction> iseq, ILGenerator il, MethodBase originalMethod)
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

			int? cost_local_index = null;
            foreach (LocalVariableInfo info in originalMethod.GetMethodBody()!.LocalVariables)
            {
                if (info.LocalType == typeof(int))
                {
                    cost_local_index = info.LocalIndex;
                    break;
                }
            }
			if (cost_local_index == null)
				throw new Exception("cost int not found");

			using IEnumerator<CodeInstruction> iter = iseq.GetEnumerator();

			while(iter.MoveNext()) {
				yield return iter.Current;
				if(iter.Current.opcode != OpCodes.Ldarg_0) {
					continue;
				}

				if(!iter.MoveNext()) {
					break;
				}
				yield return iter.Current;
				
				if(!TranspilerUtils.IsLocalLoad(iter.Current) || TranspilerUtils.ExtractLocalIndex(iter.Current) != local_index) {
					continue;
				}

				while (iter.MoveNext() && (iter.Current.opcode != OpCodes.Call || ((MethodInfo)iter.Current.operand).Name != "Queue")) {
					yield return iter.Current;
				}
				yield return iter.Current;


				if(iter.Current.opcode != OpCodes.Call || (MethodInfo) iter.Current.operand != typeof(Combat).GetMethod("Queue", 0, new Type[] {typeof(IEnumerable<CardAction>)})) {
					continue;
				}
				
				Label end_label = il.DefineLabel();

				yield return new CodeInstruction(OpCodes.Ldarg_2);
				yield return new CodeInstruction(OpCodes.Ldc_I4_1);
				yield return new CodeInstruction(OpCodes.Call, typeof(ShortCircuit).GetMethod("DoesShortCircuit"));
				yield return new CodeInstruction(OpCodes.Brfalse, end_label);


				yield return new CodeInstruction(OpCodes.Ldarg_2);
				yield return new CodeInstruction(OpCodes.Ldarg_1);
				yield return new CodeInstruction(OpCodes.Ldarg_0);
				yield return new CodeInstruction(OpCodes.Ldloc, cost_local_index);
				yield return new CodeInstruction(OpCodes.Call, typeof(ShortCircuit).GetMethod("DiscardMaybe", BindingFlags.Static | BindingFlags.NonPublic));

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

		private static void DiscardMaybe(Card card, State s, Combat c, int cost) {
			if (cost == 0) {
				c.Queue(new ADiscardLeftmost {
					count = 2
				});
			}
		}
	}
}