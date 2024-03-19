using Microsoft.Extensions.Logging;

namespace TheJazMaster.Eddie.Dialogue;

public static class StoryVarsAdditions
{
	internal static Manifest Instance => Manifest.Instance;

	private static IKokoroApi KokoroApi => Instance.KokoroApi;


	internal const string DiscountOfCardJustPlayedKey = "DiscountOfCardJustPlayed";
	internal const string LastCardPlayedWasInfiniteKey = "LastCardPlayedWasInfinite";


	internal const string CardJustPlayedWasInfiniteKey = "CardJustPlayedWasInfinite";
	internal const string MinDiscountOfCardJustPlayedKey = "MinDiscountOfCardJustPlayed";
	internal const string MaxDiscountOfCardJustPlayedKey = "MaxDiscountOfCardJustPlayed";


	private static void Combat_TryPlayCard_Prefix(Combat __instance, Card card, State s, bool exhaustNoMatterWhat)
	{	
		CardData dataWithOverrides = card.GetDataWithOverrides(s);
		KokoroApi.SetExtensionData(s.storyVars, LastCardPlayedWasInfiniteKey,
			!(dataWithOverrides.exhaust || exhaustNoMatterWhat) && (dataWithOverrides.infinite || s.ship.Get((Status)Manifest.ClosedCircuitStatus!.Id!) > 0));
		KokoroApi.SetExtensionData(s.storyVars, DiscountOfCardJustPlayedKey, -card.discount);
	}

	private static void StoryVars_ResetAfterCombatLine_Postfix(StoryVars __instance)
	{	
		KokoroApi.RemoveExtensionData(__instance, LastCardPlayedWasInfiniteKey);
		KokoroApi.RemoveExtensionData(__instance, DiscountOfCardJustPlayedKey);
	}

	private static void StoryNode_Filter_Postfix(ref bool __result, string key, StoryNode n, State s, StorySearch ctx) {
		if (!__result) return;

		if (KokoroApi.TryGetExtensionData<bool>(n, CardJustPlayedWasInfiniteKey, out var needs) &&
			(!KokoroApi.TryGetExtensionData<bool>(s.storyVars, LastCardPlayedWasInfiniteKey, out var was) || was != needs)) {
			__result = false;
			return;
		}

		bool discountExists = KokoroApi.TryGetExtensionData<int>(s.storyVars, DiscountOfCardJustPlayedKey, out var discount);
		if (KokoroApi.TryGetExtensionData<int>(n, MinDiscountOfCardJustPlayedKey, out var minDiscount) && (!discountExists || discount < minDiscount)) {
			__result = false;
			return;
		}
		if (KokoroApi.TryGetExtensionData<int>(n, MaxDiscountOfCardJustPlayedKey, out var maxDiscount) && (!discountExists || discount > maxDiscount)) {
			__result = false;
			return;
		}
	}

	internal static void DrawLoadingScreen_Prefix(MG __instance, ref int __state)
		=> __state = __instance.loadingQueue?.Count ?? 0;

	internal static void DrawLoadingScreen_Postfix(MG __instance, ref int __state)
	{
		if (__state <= 0)
			return;
		if ((__instance.loadingQueue?.Count ?? 0) > 0)
			return;
		
        ArtifactDialogue.Inject();
        CombatDialogue.Inject();
        EventDialogue.Inject();
	}
}