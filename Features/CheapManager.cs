using Microsoft.Extensions.Logging;
using Nanoray.Shrike;
using Nanoray.Shrike.Harmony;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using Nickel;

namespace TheJazMaster.Eddie;

[HarmonyPatch]
public class CheapManager
{	
    static ModEntry Instance => ModEntry.Instance;
	static IModHelper Helper => Instance.Helper;

	internal static ICardTraitEntry CheapTrait = null!;
	
	internal const string FreeKey = "Free";
	internal const string FreeOncePerTurnKey = "FreeOncePerTurn";
	internal const string CostsLessPermanentKey = "FreeIsPermanent";

	public CheapManager() {
		Spr CheapIcon = Helper.Content.Sprites.RegisterSprite(Instance.Package.PackageRoot.GetRelativeFile("Sprites/icons/cheap.png")).Sprite;

        CheapTrait = Helper.Content.Cards.RegisterTrait("Cheap", new()
        {
            Icon = (state, card) => CheapIcon,
            Name = (_) =>  Instance.Localizations.Localize(["trait", "Cheap", "name"]),
            Tooltips = (state, card) => [
				new GlossaryTooltip($"trait.{GetType().Namespace!}::Cheap") {
					Icon = CheapIcon,
					TitleColor = Colors.cardtrait,
					Title = Instance.Localizations.Localize(["trait", "Cheap", "name"]),
                    Description = Instance.Localizations.Localize(["trait", "Cheap", "description"]),
				}
            ]
        });

		Helper.Events.RegisterAfterArtifactsHook(nameof(Artifact.OnTurnEnd), (State state, Combat combat) => {
			foreach (Card card in combat.discard.Concat(state.deck).Concat(combat.hand).Concat(combat.exhausted)) {
				if (IsFreeOncePerTurn(card))
					Helper.ModData.SetModData(card, FreeOncePerTurnKey, true);
			}
		});
	}

	public static void SetFree(Card card, bool? overrideValue = null, bool? oncePerTurnOnlyValue = null, int? cheaper = null) {
		if (overrideValue != null) {
			Helper.ModData.SetModData(card, FreeKey, overrideValue.Value);
		}
		if (oncePerTurnOnlyValue != null) {
			Helper.ModData.SetModData(card, FreeOncePerTurnKey, oncePerTurnOnlyValue.Value);
		}
		if (cheaper != null) {
			Helper.ModData.SetModData(card, CostsLessPermanentKey, cheaper.Value);
		}
	}

	public static bool UsedFreeOncePerTurn(Card card) {
		return Helper.ModData.TryGetModData<bool>(card, FreeOncePerTurnKey, out var available) && !available;
	}

	public static bool IsFree(Card card, bool withOncePerTurnLimit = true) {
		return Helper.ModData.TryGetModData<bool>(card, FreeKey, out var free) && free && !(withOncePerTurnLimit && UsedFreeOncePerTurn(card));
	}

	public static bool IsFreeOncePerTurn(Card card) {
		return Helper.ModData.TryGetModData(card, FreeOncePerTurnKey, out bool _);
	}

	public static int CostsLessPermanent(Card card) {
		return Helper.ModData.TryGetModData(card, CostsLessPermanentKey, out int howMuch) ? howMuch : 0;
	}


	[HarmonyPostfix]
	[HarmonyPatch(typeof(Combat), nameof(Combat.ReturnCardsToDeck))]
	private static void RemoveFree(Combat __instance, State state)
	{
		foreach (Card card in state.deck)
		{
			Helper.ModData.RemoveModData(card, FreeKey);
			Helper.ModData.RemoveModData(card, FreeOncePerTurnKey);
		}
	}

	[HarmonyPostfix]
	[HarmonyPatch(typeof(Combat), nameof(Combat.Make))]
	private static void Combat_Make_Postfix(ref Combat __result, State s, AI ai, bool doForReal)
	{
		if (doForReal) {
			foreach (Card item in s.deck) {
				if (Helper.Content.Cards.IsCardTraitActive(s, item, CheapTrait)) {
					item.discount -= 1;
				}
			}
		}
	}

	[HarmonyPostfix]
	[HarmonyPatch(typeof(Card), nameof(Card.GetDataWithOverrides))]
	private static void Card_GetDataWithOverrides_Postfix(Card __instance, ref CardData __result, State state)
	{
		int costLess = CostsLessPermanent(__instance);
		if (costLess > 0) {
			__result.cost = Math.Max(0, __result.cost - costLess);
		}
		if (IsFree(__instance)) {
			__result.cost = 0;
		}
	}

	private static void RenderFreeIcon(Card card, State state, Vec vec, bool playable) {
		if (IsFreeOncePerTurn(card)) {
			var deckDef = DB.decks[card.GetMeta().deck];
			var color = playable ? Color.Lerp(deckDef.color, Colors.white, 0.6) : Color.Lerp(Colors.textMain.fadeAlpha(0.55), Colors.redd, card.shakeNoAnim);
			Draw.Sprite(ModEntry.Instance.FreeMarkerSprite, vec.x + 11, vec.y + 18, flipX: false, flipY: false, color: color);
		}
	}

	[HarmonyTranspiler]
	[HarmonyPatch(typeof(Card), nameof(Card.Render))]
	private static IEnumerable<CodeInstruction> Card_Render_Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il, MethodBase originalMethod)
	{
		new SequenceBlockMatcher<CodeInstruction>(instructions)
			.Find(
				ILMatches.Ldarg(0),
				ILMatches.Ldloc(0),
				ILMatches.Call("GetCurrentCost"),
				ILMatches.Ldloc<Vec>(originalMethod.GetMethodBody()!.LocalVariables)
			)
			.PointerMatcher(SequenceMatcherRelativeElement.Last)
			.CreateLdlocInstruction(out var ldLoc);

		new SequenceBlockMatcher<CodeInstruction>(instructions)
			.Find(
				ILMatches.Ldloc<bool>(originalMethod.GetMethodBody()!.LocalVariables),
				ILMatches.Brtrue,
				ILMatches.Instruction(OpCodes.Ldsflda, typeof(Colors).GetField("textMain")),
				ILMatches.Instruction(OpCodes.Ldc_R8),
				ILMatches.Call("fadeAlpha")
			)
			.PointerMatcher(SequenceMatcherRelativeElement.First)
			.CreateLdlocInstruction(out var ldLoc2);

		return new SequenceBlockMatcher<CodeInstruction>(instructions)
			.Find(
				ILMatches.Call("Render"),
				ILMatches.Ldloc(1),
				ILMatches.Ldfld("unplayable")
			)
			.PointerMatcher(SequenceMatcherRelativeElement.First)
			.Insert(SequenceMatcherPastBoundsDirection.After, SequenceMatcherInsertionResultingBounds.IncludingInsertion, new List<CodeInstruction> {
				new(OpCodes.Ldarg_0),
				new(OpCodes.Ldloc_0),
				ldLoc,
				ldLoc2,
				new(OpCodes.Call, AccessTools.DeclaredMethod(typeof(CheapManager), nameof(RenderFreeIcon)))
			})
			.AllElements();
	}

	private static void RemoveOncePerTurn(State s, Combat c, Card card)
	{
		if (IsFree(card) && IsFreeOncePerTurn(card))
			Helper.ModData.SetModData(card, FreeOncePerTurnKey, false);
	}

	[HarmonyTranspiler]
	[HarmonyPatch(typeof(Combat), nameof(Combat.TryPlayCard))]
	private static IEnumerable<CodeInstruction> RemoveFreeForTurn(IEnumerable<CodeInstruction> instructions, ILGenerator il, MethodBase originalMethod)
	{
		var sequence = new SequenceBlockMatcher<CodeInstruction>(instructions)
			.Find(
				ILMatches.Ldloc(0),
				ILMatches.Ldfld("card").Anchor(out var fieldAnchor),
				ILMatches.Ldarg(1),
				ILMatches.Ldarg(0),
				ILMatches.Call("AfterWasPlayed")
			);
		sequence.PointerMatcher(SequenceMatcherRelativeElement.First).CreateLdlocInstruction(out var ldLoc);
		sequence.Anchors().PointerMatcher(fieldAnchor).Element(out var fieldInstruction);
		return sequence
			.PointerMatcher(SequenceMatcherRelativeElement.Last)
			.Insert(SequenceMatcherPastBoundsDirection.After, SequenceMatcherInsertionResultingBounds.IncludingInsertion, new List<CodeInstruction> {
				new(OpCodes.Ldarg_1),
				new(OpCodes.Ldarg_0),
				ldLoc,
				new(fieldInstruction.opcode, fieldInstruction.operand),
				new(OpCodes.Call, AccessTools.DeclaredMethod(typeof(CheapManager), nameof(RemoveOncePerTurn))),
			})
			.AllElements();
	}
}