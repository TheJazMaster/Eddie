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
	public static class InfiniteOverride
	{
		public static ConditionalWeakTable<Card, StructRef<bool>> infinite_override = new ConditionalWeakTable<Card, StructRef<bool>>();
		public static ConditionalWeakTable<Card, StructRef<bool>> infinite_override_is_permanent = new ConditionalWeakTable<Card, StructRef<bool>>();


		private static void OverrideInfinite(Card __instance, ref CardData __result, State state)
		{
			if (HasInfiniteOverride(__instance))
			{
				__result.infinite = true;
			}
		}

		private static void InfiniteRemoveOverride(Combat __instance, State state)
		{
			foreach (Card card in state.deck)
			{
				StructRef<bool>? value;
				infinite_override_is_permanent.TryGetValue(card, out value);
				if (value == null || !value)
				{
					infinite_override.Remove(card);
				}
			}
		}

		public static bool HasInfiniteOverride(Card card)
		{
			StructRef<bool>? is_overridden;
			return infinite_override.TryGetValue(card, out is_overridden) && is_overridden;
		}
	}
}