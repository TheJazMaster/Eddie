using Eddie.Actions;
using Eddie.Cards;
using Microsoft.Extensions.Logging;
using CobaltCoreModding.Definitions;
using CobaltCoreModding.Definitions.ExternalItems;
using CobaltCoreModding.Definitions.ModContactPoints;
using CobaltCoreModding.Definitions.ModManifests;
using Nanoray.Shrike;
using Nanoray.Shrike.Harmony;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using HarmonyLib;
using FMOD;
using FSPRO;
using Microsoft.Xna.Framework.Input;

namespace Eddie;

public static class Cheap
{	
	internal static Manifest Instance => Manifest.Instance;

	private static IKokoroApi KokoroApi => Instance.KokoroApi;

	internal const string FreeKey = "Free";
	internal const string FreeOncePerTurnKey = "FreeOncePerTurn";
	internal const string FreeIsPermanentKey = "FreeIsPermanent";

	// public static ConditionalWeakTable<Card, StructRef<bool>> free = new ConditionalWeakTable<Card, StructRef<bool>>();
	// public static ConditionalWeakTable<Card, StructRef<bool>> free_once_per_turn = new ConditionalWeakTable<Card, StructRef<bool>>();
	// public static ConditionalWeakTable<Card, StructRef<bool>> free_permanent = new ConditionalWeakTable<Card, StructRef<bool>>();


	private static void SetCheapDiscount(ref Combat __result, State s, AI ai, bool doForReal)
	{
		if (doForReal) {
			foreach (Card item in s.deck) {
				if (item is CheapCard cheapCard) {
					item.discount = cheapCard.GetCheapDiscount();
				}
			}
		}
	}


	public static void SetFree(Card card, bool? overrideValue = null, bool? oncePerTurnOnlyValue = null, bool? permanentValue = null) {
		if (overrideValue != null) {
			KokoroApi.SetExtensionData<bool>(card, FreeKey, overrideValue.Value);
		}
		if (oncePerTurnOnlyValue != null) {
			KokoroApi.SetExtensionData<bool>(card, FreeOncePerTurnKey, oncePerTurnOnlyValue.Value);
		}
		if (permanentValue != null) {
			KokoroApi.SetExtensionData<bool>(card, FreeIsPermanentKey, permanentValue.Value);
		}
	}

	public static bool UsedFreeOncePerTurn(Card card) {
		return KokoroApi.TryGetExtensionData<bool>(card, FreeOncePerTurnKey, out var available) && !available;
	}

	public static bool IsFree(Card card, bool withOncePerTurnLimit = true) {
		return KokoroApi.TryGetExtensionData<bool>(card, FreeKey, out var free) && free && !(withOncePerTurnLimit && UsedFreeOncePerTurn(card));
	}

	public static bool IsFreeOncePerTurn(Card card) {
		return KokoroApi.TryGetExtensionData<bool>(card, FreeOncePerTurnKey, out var _);
	}

	public static bool IsFreePermanent(Card card) {
		return KokoroApi.TryGetExtensionData<bool>(card, FreeIsPermanentKey, out var isPermanent) && isPermanent;
	}
	
	private static void SetFree(Card __instance, ref CardData __result, State state)
	{
		if (IsFree(__instance)) {
			__result.cost = 0;
		}
	}

	private static void RemoveFree(Combat __instance, State state)
	{
		foreach (Card card in state.deck)
		{
			if (!IsFreePermanent(card))
				KokoroApi.RemoveExtensionData(card, FreeKey);
			KokoroApi.RemoveExtensionData(card, FreeOncePerTurnKey);
		}
	}

	private static void RemoveOncePerTurn(State s, Combat c, Card card)
	{
		if (IsFree(card) && IsFreeOncePerTurn(card))
			KokoroApi.SetExtensionData<bool>(card, FreeOncePerTurnKey, false);
	}

	private static void ResetFreeForTurn(Ship __instance, State s, Combat c)
	{
		if (!__instance.isPlayerShip) return;

		foreach (List<Card> list in new List<Card>[] {c.discard, s.deck, c.hand, c.exhausted}) {
			foreach (Card card in list) {
				if (IsFreeOncePerTurn(card))
					KokoroApi.SetExtensionData<bool>(card, FreeOncePerTurnKey, true);
			}
		}
	}

	
	private static void FreeIcon(Card card, State state, Vec vec, bool playable) {
		if (IsFreeOncePerTurn(card)) {
			var deckDef = DB.decks[card.GetMeta().deck];
			var color = playable ? Color.Lerp(deckDef.color, Colors.white, 0.6) : Color.Lerp(Colors.textMain.fadeAlpha(0.55), Colors.redd, card.shakeNoAnim);
			Draw.Sprite((Spr)Manifest.FreeMarkerSprite!.Id!, vec.x + 11, vec.y + 18, flipX: false, flipY: false, color: color);
		}
	}

	private static IEnumerable<CodeInstruction> RenderFreeIcon(IEnumerable<CodeInstruction> instructions, ILGenerator il, MethodBase originalMethod)
	{
		try
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
                    new CodeInstruction(OpCodes.Ldarg_0),
					new CodeInstruction(OpCodes.Ldloc_0),
					ldLoc,
					ldLoc2,
                    new CodeInstruction(OpCodes.Call, typeof(Cheap).GetMethod("FreeIcon", BindingFlags.NonPublic | BindingFlags.Static))
                })
				.AllElements();
		}
		catch (Exception ex)
		{
			Instance.Logger!.LogError("Could not patch method {Method} - {Mod} probably won't work.\nReason: {Exception}", originalMethod, Instance.Name, ex);
			return instructions;
		}
	}

	private static IEnumerable<CodeInstruction> RemoveFreeForTurn(IEnumerable<CodeInstruction> instructions, ILGenerator il, MethodBase originalMethod)
	{
		try
		{
			var sequence = new SequenceBlockMatcher<CodeInstruction>(instructions)
				.AsGuidAnchorable()
				.Find(
					ILMatches.Ldloc(0),
					ILMatches.Ldfld("card").WithAutoAnchor(out Guid fieldAnchor),
					ILMatches.Ldarg(1),
					ILMatches.Ldarg(0),
					ILMatches.Call("AfterWasPlayed")
				);
			sequence.PointerMatcher(SequenceMatcherRelativeElement.First).CreateLdlocInstruction(out var ldLoc);
			sequence.PointerMatcher(fieldAnchor).Element(out var fieldInstruction);
			return sequence
				.PointerMatcher(SequenceMatcherRelativeElement.Last)
				.Insert(SequenceMatcherPastBoundsDirection.After, SequenceMatcherInsertionResultingBounds.IncludingInsertion, new List<CodeInstruction> {
					new CodeInstruction(OpCodes.Ldarg_1),
					new CodeInstruction(OpCodes.Ldarg_0),
                    ldLoc,
                    new CodeInstruction(fieldInstruction.opcode, fieldInstruction.operand),
                    new CodeInstruction(OpCodes.Call, typeof(Cheap).GetMethod("RemoveOncePerTurn", BindingFlags.NonPublic | BindingFlags.Static)),
                })
				.AllElements();
		}
		catch (Exception ex)
		{
			Instance.Logger!.LogError("Could not patch method {Method} - {Mod} probably won't work.\nReason: {Exception}", originalMethod, Instance.Name, ex);
			return instructions;
		}
	}
}