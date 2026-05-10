using HarmonyLib;
using Microsoft.Extensions.Logging;
using Shockah.Kokoro;

namespace TheJazMaster.Eddie.DialogueAdditions;

[HarmonyPatch]
public static class StoryVarsAdditions
{
	internal static ModEntry Instance => ModEntry.Instance;
	public static string? SogginsName => Instance.SogginsApi?.SogginsDeck.GlobalName;


	internal const string DiscountOfCardJustPlayedKey = "DiscountOfCardJustPlayed";
	internal const string LastCardPlayedWasInfiniteKey = "LastCardPlayedWasInfinite";


	internal const string CardJustPlayedWasInfiniteKey = "CardJustPlayedWasInfinite";
	internal const string MinDiscountOfCardJustPlayedKey = "MinDiscountOfCardJustPlayed";
	internal const string MaxDiscountOfCardJustPlayedKey = "MaxDiscountOfCardJustPlayed";


	[HarmonyPrefix]
	[HarmonyPatch(typeof(Combat), nameof(Combat.TryPlayCard))]
	private static void Combat_TryPlayCard_Prefix(Combat __instance, Card card, State s, bool exhaustNoMatterWhat)
	{	
		CardData dataWithOverrides = card.GetDataWithOverrides(s);
		Instance.Helper.ModData.SetModData(s.storyVars, LastCardPlayedWasInfiniteKey,
			!(dataWithOverrides.exhaust || exhaustNoMatterWhat) && (dataWithOverrides.infinite || s.ship.Get(StatusManager.ClosedCircuitStatus) > 0));
		Instance.Helper.ModData.SetModData(s.storyVars, DiscountOfCardJustPlayedKey, -card.discount);
	}

	[HarmonyPostfix]
	[HarmonyPatch(typeof(StoryVars), nameof(StoryVars.ResetAfterCombatLine))]
	private static void StoryVars_ResetAfterCombatLine_Postfix(StoryVars __instance)
	{	
		Instance.Helper.ModData.RemoveModData(__instance, LastCardPlayedWasInfiniteKey);
		Instance.Helper.ModData.RemoveModData(__instance, DiscountOfCardJustPlayedKey);
	}

	[HarmonyPostfix]
	[HarmonyPatch(typeof(StoryNode), nameof(StoryNode.Filter))]
	private static void StoryNode_Filter_Postfix(ref bool __result, string key, StoryNode n, State s, StorySearch ctx) {
		if (!__result) return;

		if (Instance.Helper.ModData.TryGetModData<bool>(n, CardJustPlayedWasInfiniteKey, out var needs) &&
			(!Instance.Helper.ModData.TryGetModData<bool>(s.storyVars, LastCardPlayedWasInfiniteKey, out var was) || was != needs)) {
			__result = false;
			return;
		}

		bool discountExists = Instance.Helper.ModData.TryGetModData<int>(s.storyVars, DiscountOfCardJustPlayedKey, out var discount);
		if (Instance.Helper.ModData.TryGetModData<int>(n, MinDiscountOfCardJustPlayedKey, out var minDiscount) && (!discountExists || discount < minDiscount)) {
			__result = false;
			return;
		}
		if (Instance.Helper.ModData.TryGetModData<int>(n, MaxDiscountOfCardJustPlayedKey, out var maxDiscount) && (!discountExists || discount > maxDiscount)) {
			__result = false;
			return;
		}
	}

	internal static SaySwitch MaybeAdd(this SaySwitch source, bool condition, CustomSay item)
	{
		if (condition) source.lines.Add(item);
		return source;
	}
}