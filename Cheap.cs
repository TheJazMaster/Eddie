using Eddie.Actions;
using Eddie.Cards;
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
	public static class Cheap
	{	
		public static ConditionalWeakTable<Card, StructRef<bool>> free = new ConditionalWeakTable<Card, StructRef<bool>>();

		private static void SetCheapDiscount(ref Combat __result, State s, AI ai, bool doForReal)
		{
			if (doForReal) {
				foreach (Card item in s.deck) {
					if (item is CheapCard cheapCard) {
						item.discount += cheapCard.GetCheapDiscount();
					}
				}
			}
		}
		
		private static void SetFree(Card __instance, ref CardData __result, State state)
		{
			StructRef<bool>? value;
			if (free.TryGetValue(__instance, out value) && value)
			{
				__result.cost = 0;
			}
		}

		private static void RemoveFree(Combat __instance, State state)
		{
			foreach (Card card in state.deck)
			{
				free.Remove(card);
			}
		}

		// private static IEnumerable<CodeInstruction> SetDefaultDiscount(IEnumerable<CodeInstruction> iseq, ILGenerator il, MethodBase originalMethod)
		// {
		// 	using IEnumerator<CodeInstruction> iter = iseq.GetEnumerator();
		// 	while(iter.MoveNext()) {
		// 		if(iter.Current.opcode != OpCodes.Ldc_I4_0) {
		// 			yield return iter.Current;
		// 			continue;
		// 		}

		// 		var candidate1 = iter.Current;
		// 		if(!iter.MoveNext()) {
		// 			yield return candidate1;
		// 			yield return iter.Current;
		// 			break;
		// 		}
				
		// 		if(iter.Current.opcode != OpCodes.Stfld || (FieldInfo)iter.Current.operand != typeof(Card).GetField("discount")) {
		// 			yield return candidate1;
		// 			yield return iter.Current;
		// 			continue;
		// 		}

		// 		yield return new CodeInstruction(OpCodes.Dup);
		// 		yield return new CodeInstruction(OpCodes.Ldloc_0);
		// 		yield return new CodeInstruction(OpCodes.Ldfld, typeof(Combat).GetField("s"));
		// 		yield return new CodeInstruction(OpCodes.Call, typeof(Cheap).GetMethod("GetDefaultDiscount", 0, new Type[] {typeof(Card)}));
		// 		yield return iter.Current;
				
		// 		break;
		// 	}

		// 	while(iter.MoveNext())
		// 		yield return iter.Current;
		// }
	}
}