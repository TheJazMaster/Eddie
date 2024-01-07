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

	public static ConditionalWeakTable<Card, StructRef<bool>> free = new ConditionalWeakTable<Card, StructRef<bool>>();
	public static ConditionalWeakTable<Card, StructRef<bool>> free_once_per_turn = new ConditionalWeakTable<Card, StructRef<bool>>();
	public static ConditionalWeakTable<Card, StructRef<bool>> free_permanent = new ConditionalWeakTable<Card, StructRef<bool>>();

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
	
	private static void SetFree(Card __instance, ref CardData __result, State state)
	{
		if (free.TryGetValue(__instance, out StructRef<bool>? value) && value && (!free_once_per_turn.TryGetValue(__instance, out StructRef<bool>? onceValue) || onceValue))
		{
			__result.cost = 0;
		}
	}

	private static void RemoveFree(Combat __instance, State state)
	{
		foreach (Card card in state.deck)
		{
			if (!free_permanent.TryGetValue(card, out StructRef<bool>? value) || !value)
				free.Remove(card);
			free_once_per_turn.Remove(card);
		}
	}

	private static void RemoveOncePerTurn(State s, Combat c, Card card)
	{
		if (free_once_per_turn.TryGetValue(card, out StructRef<bool>? value) && value)
			free_once_per_turn.AddOrUpdate(card, false);
	}

	private static void ResetFreeForTurn(Ship __instance, State s, Combat c)
	{
		if (!__instance.isPlayerShip) return;

		foreach (List<Card> list in new List<Card>[] {c.discard, s.deck, c.hand, c.exhausted}) {
			foreach (Card card in list) {
				if (free_once_per_turn.TryGetValue(card, out StructRef<bool>? value) && !value)
					free_once_per_turn.AddOrUpdate(card, true);
			}
		}
	}

	
	private static void FreeIcon(Card card, State state, Vec vec, bool playable) {
		if (free.TryGetValue(card, out StructRef<bool>? value) && value && free_once_per_turn.TryGetValue(card, out StructRef<bool>? value2) && value2) {
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