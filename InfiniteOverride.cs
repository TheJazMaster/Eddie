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

namespace Eddie;

public static class InfiniteOverride
{
	private static IKokoroApi KokoroApi => Manifest.Instance.KokoroApi;

	internal const string InfiniteOverrideKey = "InfiniteOverride";
	internal const string InfiniteOverrideIsPermanentKey = "InfiniteOverrideIsPermanent";

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
			if (!KokoroApi.TryGetExtensionData<bool>(card, InfiniteOverrideIsPermanentKey, out var permanent) || !permanent) {
				KokoroApi.RemoveExtensionData(card, InfiniteOverrideKey);
			}
		}
	}

	public static bool HasInfiniteOverride(Card card)
	{
		return KokoroApi.TryGetExtensionData<bool>(card, InfiniteOverrideIsPermanentKey, out var value) && value;
	}

	public static void SetInfiniteOverride(Card card, bool? overrideValue = null, bool? permanentValue = null)
	{
		if (overrideValue != null) {
			KokoroApi.SetExtensionData<bool>(card, InfiniteOverrideKey, overrideValue.Value);
		}
		if (permanentValue != null) {
			KokoroApi.SetExtensionData<bool>(card, InfiniteOverrideIsPermanentKey, permanentValue.Value);
		}
	}

	public static void RemoveInfiniteOverride(Card card)
	{
		KokoroApi.RemoveExtensionData(card, InfiniteOverrideKey);
		KokoroApi.RemoveExtensionData(card, InfiniteOverrideIsPermanentKey);
	}
}