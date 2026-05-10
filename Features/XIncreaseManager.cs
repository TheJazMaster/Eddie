
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using Microsoft.Extensions.Logging;
using Nanoray.Shrike;
using Nanoray.Shrike.Harmony;
using TheJazMaster.Eddie.Artifacts;
using TheJazMaster.TyAndSasha;

namespace TheJazMaster.Eddie;

[HarmonyPatch]
public class XIncreaseManager {
    static ModEntry Instance => ModEntry.Instance;
    private bool isTyHere = false;

    internal const string IncreasedHintsKey = "IncreasedHints";

    public XIncreaseManager() {
        Instance.Helper.ModRegistry.AwaitApiOrNull<ITyAndSashaApi>("TheJazMaster.TyAndSasha", api => {
            if (api != null || isTyHere) {
                isTyHere = true;
                return;
            }
            
            Instance.Harmony.TryPatch(
                logger: Instance.Logger,
                original: AccessTools.DeclaredMethod(typeof(Card), nameof(Card.GetActionsOverridden)),
                postfix: AccessTools.DeclaredMethod(GetType(), nameof(Card_GetActionsOverridden_Postfix))
            );
            Instance.Harmony.TryPatch(
                logger: Instance.Logger,
                original: AccessTools.DeclaredMethod(typeof(Card), nameof(Card.RenderAction)),
                transpiler: AccessTools.DeclaredMethod(GetType(), nameof(Card_RenderAction_Transpiler))
            );
            Instance.Harmony.TryPatch(
                logger: Instance.Logger,
                original: AccessTools.DeclaredMethod(typeof(AVariableHint), nameof(AVariableHint.GetTooltips)),
                postfix: AccessTools.DeclaredMethod(GetType(), nameof(AVariableHint_GetTooltips_Postfix))
            );
        });
    }


    public static int GetXBonus(State s) {
        int baseXBonus = 0;
        foreach (Artifact item in s.EnumerateAllArtifacts()) {
            if (item is IXAffectorArtifact xAffector) 
                baseXBonus += xAffector.AffectX(baseXBonus);
        }
        return baseXBonus;
    }

    public static void ImproveActionX(CardAction action, int baseXBonus) {
        if (action is AVariableHint) {
            Instance.Helper.ModData.SetModData(action, IncreasedHintsKey, baseXBonus);
        }
        var xHint = action.xHint;
        if (xHint == null)
            return;

        int xBonus = xHint.Value * baseXBonus;

        if (action is AAttack attack) {
            attack.damage += xBonus;
        } else if (action is AStatus status) {
            status.statusAmount += xBonus;
        } else if (action is ADrawCard draw) {
            draw.count += xBonus;
        } else if (action is AAddCard add) {
            add.amount += xBonus;
        } else if (action is ADiscard discard) {
            discard.count += xBonus;
        } else if (action is AHeal heal) {
            heal.healAmount += xBonus;
        } else if (action is AHurt hurt) {
            hurt.hurtAmount += xBonus;
        } else if (action is AMove move) {
            if (move.dir > 0) {
                move.dir += xBonus;
            } else {
                move.dir -= xBonus;
            }
        }
    }

    private static void Card_GetActionsOverridden_Postfix(Card __instance, ref List<CardAction> __result, State s, Combat c) {
        int baseXBonus = GetXBonus(s);
        if (baseXBonus == 0) return;

        foreach (CardAction wrappedAction in __result) {
            foreach (CardAction action in Instance.KokoroApi.WrappedActions.GetWrappedCardActionsRecursively(wrappedAction, false)) {
                ImproveActionX(action, baseXBonus);
            }
        }
    }

    private static IEnumerable<CodeInstruction> Card_RenderAction_Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il, MethodBase originalMethod) {
        
        var actionField = new SequenceBlockMatcher<CodeInstruction>(instructions)
            .Find(SequenceBlockMatcherFindOccurence.First, SequenceMatcherRelativeBounds.WholeSequence, [
                ILMatches.Ldloc(0),
                ILMatches.Ldfld("action")
            ])
            .PointerMatcher(SequenceMatcherRelativeElement.Last)
            .Element().operand;

        var wField = new SequenceBlockMatcher<CodeInstruction>(instructions)
            .Find(SequenceBlockMatcherFindOccurence.First, SequenceMatcherRelativeBounds.WholeSequence, [
                ILMatches.Ldloc(0),
                ILMatches.Ldfld("w")
            ])
            .PointerMatcher(SequenceMatcherRelativeElement.Last)
            .Element().operand;

        var spriteColorField = new SequenceBlockMatcher<CodeInstruction>(instructions)
            .Find(SequenceBlockMatcherFindOccurence.First, SequenceMatcherRelativeBounds.WholeSequence, ILMatches.Stfld("spriteColor"))
            .Element().operand;

        var iconWidthField = new SequenceBlockMatcher<CodeInstruction>(instructions)
            .Find(SequenceBlockMatcherFindOccurence.First, SequenceMatcherRelativeBounds.WholeSequence, ILMatches.Stfld("iconWidth"))
            .Element().operand;
        
        var ret = new SequenceBlockMatcher<CodeInstruction>(instructions)
			.Find(SequenceBlockMatcherFindOccurence.Last, SequenceMatcherRelativeBounds.WholeSequence, [
				ILMatches.Ldloc(0).CreateLdlocInstruction(out var ldLoc).CreateLdlocaInstruction(out var ldLoca),
                ILMatches.Ldfld("action"),
                ILMatches.Ldflda("xHint"),
				ILMatches.Call("get_HasValue"),
                ILMatches.Brfalse
			])
			.PointerMatcher(SequenceMatcherRelativeElement.First).ExtractLabels(out var labels)
			.Insert(SequenceMatcherPastBoundsDirection.Before, SequenceMatcherInsertionResultingBounds.JustInsertion, [
                ldLoca.Value.WithLabels(labels),
                ldLoc.Value,
                new CodeInstruction(OpCodes.Ldfld, actionField),
                ldLoc.Value,
                new CodeInstruction(OpCodes.Ldfld, spriteColorField),
                new CodeInstruction(OpCodes.Ldarg_3),
                new CodeInstruction(OpCodes.Ldarg_0),
                ldLoc.Value,
                new CodeInstruction(OpCodes.Ldfld, iconWidthField),
                ldLoc.Value,
                new CodeInstruction(OpCodes.Ldfld, wField),
                new CodeInstruction(OpCodes.Call, AccessTools.DeclaredMethod(typeof(XIncreaseManager), nameof(RenderXIncrease))),
                new CodeInstruction(OpCodes.Stfld, wField)
            ])
            .AllElements();
        foreach (CodeInstruction ci in ret) ModEntry.Instance.Logger.LogInformation(ci.ToString());
        return ret;
    }
    // private static IEnumerable<CodeInstruction> RenderActionPatch(IEnumerable<CodeInstruction> iseq, ILGenerator il, MethodBase originalMethod) {
    //     List<CodeInstruction> spriteColorInstructions = [];
    //     List<CodeInstruction> iconWidthInstructions = [];
    //     List<CodeInstruction> wInstructions = [];
        
    //     using IEnumerator<CodeInstruction> iter = iseq.GetEnumerator();
    //     while (iter.MoveNext()) {
    //         yield return iter.Current;
    //         if(iter.Current.opcode != OpCodes.Ldstr || (string)(iter.Current.operand) != "ffffff") {
    //             continue;
    //         }

    //         if(!iter.MoveNext()) {
    //             break;
    //         }
            
    //         yield return iter.Current;
    //         if(iter.Current.opcode != OpCodes.Newobj || !((iter.Current.operand) is ConstructorInfo) || ((ConstructorInfo)iter.Current.operand).DeclaringType!.Name != "Color") {
    //             continue;
    //         }
            
    //         while (iter.MoveNext() && iter.Current.opcode != OpCodes.Stfld) {
    //             yield return iter.Current;
    //         }

    //         if (iter.Current.opcode != OpCodes.Stfld)
    //             break;
    //         yield return iter.Current;

    //         spriteColorInstructions.Add(iter.Current);
    //         break;
    //     }
    //     while (iter.MoveNext()) {
    //         yield return iter.Current;
    //         if(iter.Current.opcode != OpCodes.Ldloca_S || TranspilerUtils.ExtractLocalIndex(iter.Current) != 0) {
    //             continue;
    //         }

    //         List<CodeInstruction> candidates = new List<CodeInstruction>();
    //         candidates.Add(iter.Current);
    //         if(!iter.MoveNext()) {
    //             break;
    //         }
    //         yield return iter.Current;
            
    //         if(iter.Current.opcode != OpCodes.Ldc_I4_0) {
    //             continue;
    //         }

    //         if(!iter.MoveNext()) {
    //             break;
    //         }
    //         yield return iter.Current;
            
    //         if(iter.Current.opcode != OpCodes.Stfld) {
    //             continue;
    //         }

    //         candidates.Add(iter.Current);
    //         wInstructions = candidates;
    //         break;
    //         // IL_0082: ldloca.s 0
    //         // IL_0084: ldc.i4.0
    //         // IL_0085: stfld int32 Card/'<>c__DisplayClass57_0'::w
    //     }
    //     while (iter.MoveNext()) {
    //         yield return iter.Current;
    //         if(iter.Current.opcode != OpCodes.Ldloca_S || TranspilerUtils.ExtractLocalIndex(iter.Current) != 0) {
    //             continue;
    //         }

    //         List<CodeInstruction> candidates = new List<CodeInstruction>();
    //         candidates.Add(iter.Current);
    //         if(!iter.MoveNext()) {
    //             break;
    //         }
    //         yield return iter.Current;
            
    //         if(iter.Current.opcode != OpCodes.Ldc_I4_8) {
    //             continue;
    //         }

    //         if(!iter.MoveNext()) {
    //             break;
    //         }
    //         yield return iter.Current;
            
    //         if(iter.Current.opcode != OpCodes.Stfld) {
    //             continue;
    //         }

    //         candidates.Add(iter.Current);
    //         iconWidthInstructions = candidates;
    //         break;
    //     }

    //     bool second = false;
    //     while (iter.MoveNext()) {
    //         if (wInstructions.Count == 0 || spriteColorInstructions.Count == 0 || iconWidthInstructions.Count == 0)
    //             break; 
            
    //         yield return iter.Current;
    //         if(iter.Current.opcode != OpCodes.Ldloc_0) {
    //             continue;
    //         }

    //         List<CodeInstruction> candidates = new List<CodeInstruction>();
    //         candidates.Add(iter.Current);
    //         if(!iter.MoveNext()) {
    //             break;
    //         }
    //         yield return iter.Current;
            
    //         if(iter.Current.opcode != OpCodes.Ldfld) {
    //             continue;
    //         }

    //         candidates.Add(iter.Current);
    //         if(!iter.MoveNext()) {
    //             break;
    //         }
    //         yield return iter.Current;
            
    //         if(iter.Current.opcode != OpCodes.Isinst || (Type)iter.Current.operand != typeof(AVariableHint)) {
    //             continue;
    //         }

    //         candidates.Add(iter.Current);
    //         if(!iter.MoveNext()) {
    //             break;
    //         }
    //         yield return iter.Current;

    //         if(!TranspilerUtils.IsLocalStore(iter.Current)) {
    //             continue;
    //         }
    //         {
    //             var local = ((LocalBuilder)iter.Current.operand).LocalIndex;
    //             candidates.Add(iter.Current);
    //             if(!iter.MoveNext()) {
    //                 break;
    //             }
    //             yield return iter.Current;
                
    //             if(!TranspilerUtils.IsLocalLoad(iter.Current) || ((LocalBuilder)iter.Current.operand).LocalIndex != local) {
    //                 continue;
    //             }
    //         }

    //         candidates.Add(iter.Current);
    //         if(!iter.MoveNext()) {
    //             break;
    //         }
    //         yield return iter.Current;
            
    //         if(iter.Current.opcode != OpCodes.Brfalse_S) {
    //             continue;
    //         }

    //         if (!second) {
    //             second = true;
    //             continue;
    //         }

    //         candidates.Add(iter.Current);
    //         List<Label> labels = new List<Label> { (Label)iter.Current.operand };

    //         while (iter.MoveNext() && !(iter.Current.labels is List<Label> list && list.Contains(labels[0]))) {
    //             yield return iter.Current;
    //             if (iter.Current.operand is Label)
    //                 labels.Add((Label)iter.Current.operand);

    //         }

    //         if (!(iter.Current.labels is List<Label> && (iter.Current.labels as List<Label>).Contains(labels[0])))
    //             break;

    //         List<Label> labelsToAdd = new List<Label>();
    //         labelsToAdd.AddRange(labels.Where(label => iter.Current.labels.Remove(label)));
            
    //         yield return new CodeInstruction(wInstructions[0].opcode, wInstructions[0].operand).WithLabels(labelsToAdd); // prepare for w
    //         yield return new CodeInstruction(candidates[0].opcode, candidates[0].operand); //
    //         yield return new CodeInstruction(candidates[1].opcode, candidates[1].operand); // action
    //         yield return new CodeInstruction(wInstructions[0].opcode, wInstructions[0].operand); //
    //         yield return new CodeInstruction(OpCodes.Ldfld, spriteColorInstructions[0].operand); // color
    //         yield return new CodeInstruction(OpCodes.Ldarg_3); // dontDraw
    //         yield return new CodeInstruction(OpCodes.Ldarg_0); // g
    //         yield return new CodeInstruction(iconWidthInstructions[0].opcode, iconWidthInstructions[0].operand); //
    //         yield return new CodeInstruction(OpCodes.Ldfld, iconWidthInstructions[1].operand); // icon width
    //         yield return new CodeInstruction(wInstructions[0].opcode, wInstructions[0].operand); //
    //         yield return new CodeInstruction(OpCodes.Ldfld, wInstructions[1].operand); // w
    //         yield return new CodeInstruction(OpCodes.Call, typeof(Manifest).GetMethod("RenderXIncrease", BindingFlags.NonPublic | BindingFlags.Static));
    //         yield return new CodeInstruction(wInstructions[1].opcode, wInstructions[1].operand); // set w
            
    //         yield return iter.Current;
    //     }
    // }

    private static int RenderXIncrease(CardAction action, Color color, bool dontDraw, G g, int iconWidth, int w) {
        if (ModEntry.Instance.Helper.ModData.TryGetModData<int>(action, IncreasedHintsKey, out var value) && value != 0) {
            // Plus
            // w += 3;
            w--;
            if (!dontDraw)
            {
                Rect? rect = new Rect(w + 1);
                Vec xy = g.Push(null, rect).rect.xy;
                Spr? plus = StableSpr.icons_plus;
                // Spr? plus = StableSpr.icons_plus;
                Color? trueColor = (action.disabled ? color : Colors.textMain);
                Draw.Sprite(plus, xy.x, xy.y, flipX: false, flipY: false, 0.0, null, null, null, null, trueColor);
                g.Pop();
            }
            w += iconWidth - 1;

            // Number
            w += 4;
            if (!dontDraw)
            {
                Rect? rect = new Rect(w - 1);
                Vec xy = g.Push(null, rect).rect.xy;
                Color textColor = action.disabled ? Colors.disabledText : Colors.textMain;;
                Draw.Text(value + "", xy.x, xy.y + 2, null, textColor, null, null, null, null, dontDraw: false, null, color, null, null, null, dontSubstituteLocFont: true);
                g.Pop();
            }
            w += iconWidth - 5;
        }
        return w;
    }

    private static void AVariableHint_GetTooltips_Postfix(AVariableHint __instance, List<Tooltip> __result, State s) {
        // int baseXBonus = 0;
        // foreach (Artifact item in s.EnumerateAllArtifacts()) {
        //     if (item is XAffectorArtifact xAffector) 
        //         baseXBonus += xAffector.AffectX(baseXBonus);
        // }
        int baseXBonus = Instance.Helper.ModData.GetModDataOrDefault(__instance, IncreasedHintsKey, 0);
        if (baseXBonus == 0) return;

        foreach (Tooltip t in __result) {
            if (t is TTGlossary glossary && glossary.vals != null) {
                if (glossary.vals.Count() == 0) glossary.vals.AddItem(baseXBonus.ToString());
                else glossary.vals[^1] = glossary.vals[^1] + " + " + baseXBonus;
            }
        }
    }

    public interface IXAffectorArtifact {
        int AffectX(int value);
    }
}