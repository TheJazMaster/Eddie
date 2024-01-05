using Eddie.Artifacts;
using CobaltCoreModding.Definitions.ExternalItems;
using CobaltCoreModding.Definitions.ModContactPoints;
using CobaltCoreModding.Definitions.ModManifests;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using Nanoray.Shrike.Harmony;
using Nanoray.Shrike;
using FMOD;
using FSPRO;
using Microsoft.Xna.Framework.Input;
using static System.Reflection.BindingFlags;

using HarmonyLib;

namespace Eddie;

public partial class Manifest : IArtifactManifest, ICustomEventManifest
{
    private static ICustomEventHub? _eventHub;
    internal static ICustomEventHub EventHub { get => _eventHub ?? throw new Exception(); set => _eventHub = value; }

    // Common
    public static ExternalArtifact? FrazzledWiresArtifact { get; private set; }
    public static ExternalArtifact? SunLampArtifact { get; private set; }
    public static ExternalArtifact? ElectromagneticCoilArtifact { get; private set; }
    public static ExternalArtifact? FissionChamberArtifact { get; private set; }
    // Boss
    public static ExternalArtifact? SolarPanelsArtifact { get; private set; }
    public static ExternalArtifact? DeconstructionGogglesArtifact { get; private set; }

    // Duos
    public static ExternalArtifact? PerfectInsulationArtifact { get; private set; }
    public static ExternalArtifact? UltraLightBatteriesArtifact { get; private set; }
    public static ExternalArtifact? OverdriveFeedbackArtifact { get; private set; }
    public static ExternalArtifact? ThunderstrikeArtifact { get; private set; }
    public static ExternalArtifact? EmergencyVentilatorArtifact { get; private set; }
    public static ExternalArtifact? VersionControlArtifact { get; private set; }
    public static ExternalArtifact? SpellboardArtifact { get; private set; }
    public static ExternalArtifact? VirtualTreadmillArtifact { get; private set; }
    public static ExternalArtifact? WaxWingsArtifact { get; private set; }

    private ExternalArtifact RegisterArtifact(IArtifactRegistry registry, Type type, ExternalSprite sprite, string name, string desc, List<ExternalGlossary>? extraGlossary = null, ExternalDeck? deck = null)
    {
        if (deck == null)
            deck = EddieDeck ?? throw new Exception("No default deck available");
        var artifact = new ExternalArtifact("Eddie.Artifacts." + type.Name, type, sprite, extraGlossary, deck);
        artifact.AddLocalisation(name, desc);
        registry.RegisterArtifact(artifact);
        return artifact!;
    }

    public void LoadManifest(IArtifactRegistry registry)
    {
        SunLampArtifact = RegisterArtifact(registry, typeof(SunLamp), SunLampOnSprite ?? throw new Exception("missing SunLamp sprite"),
            "SUN LAMP", "Each turn, if you didn't use any <c=status>EVADE</c> last turn, gain 1 evade. <c=downside>Cannot activate again until you spend <c=status>EVADE</c>.</c>");

        SolarPanelsArtifact = RegisterArtifact(registry, typeof(SolarPanels), SolarPanelsOnSprite ?? throw new Exception("missing SolarPanels sprite"),
            "SOLAR PANELS", "Gain 1 extra <c=energy>ENERGY</c> on the first turn. Each turn, if you didn't use any <c=status>EVADE</c> last turn, gain 1 extra <c=energy>ENERGY</c>.");

        ElectromagneticCoilArtifact = RegisterArtifact(registry, typeof(ElectromagneticCoil), ElectromagneticCoilSprite ?? throw new Exception("missing ElectromagneticCoil sprite"),
            "ELECTROMAGNETIC COIL", "If you end your turn with more than 0 <c=energy>ENERGY</c>, gain 1 <c=status>EVADE</c>.");

        FrazzledWiresArtifact = RegisterArtifact(registry, typeof(FrazzledWires), FrazzledWiresSprite ?? throw new Exception("missing FrazzledWires sprite"),
            "FRAZZLED WIRES", "Choose a card in your deck that costs 1 <c=energy>ENERGY</c>. Its cost is reduced to 0. <c=downside>It gains <c=cardtrait>short-circuit</c>.");

        FissionChamberArtifact = RegisterArtifact(registry, typeof(FissionChamber), FissionChamberSprite ?? throw new Exception("missing FissionChamber sprite"),
            "FISSION CHAMBER", "All <c=keyword>X</c> values on cards are increased by 1.");

        DeconstructionGogglesArtifact = RegisterArtifact(registry, typeof(DeconstructionGoggles), DeconstructionGogglesSprite ?? throw new Exception("missing DeconstructionGoggles sprite"),
            "DECONSTRUCTION GOGGLES", "At the start of combat, gain 2 <c=card>Reverse-Engineer</c>s.");


        // Patching status logic
        var harmony = new Harmony("Eddie.Status");

        MoveEventLogic(harmony);
        FissionChamberLogic(harmony);


        if (DuoArtifactsApi != null) RegisterDuoArtifacts(registry, harmony);
        /* Duos:
            Dizzy: +1 energy first time you overshield.
            Riggs: once per turn, when you reach 0 energy, gain 1 hermes.
            Peri: overdrive doesn't tick down if you end your turn with at least 1 energy.
            Isaac (Thunderstrike): whenever you the enemy moves during your turn, they take 1 hull damage.
            Drake (Emergency Ventilators): -1 heat if you end your turn with energy.
            Max (Version Control): Every 5th time you exhaust a card, gain 1 ENERGY.
            Books (Spellboard): Cards with enabled shard costs are infinite
            Cat (Virtual Treadmill): On pickup, your Basic cards become infinite.
            Soggins (Wax Wings): Gain 1 energy when you oversmug
        */
    }

    void RegisterDuoArtifacts(IArtifactRegistry registry, Harmony harmony)
    {
        var eddieDeck = (Deck)EddieDeck!.Id!.Value;
        var duoDeck = DuoArtifactsApi!.DuoArtifactDeck;

        PerfectInsulationArtifact = RegisterArtifact(registry, typeof(PerfectInsulation), PerfectInsulationSprite ?? throw new Exception("missing PerfectInsulation sprite"),
            "PERFECT INSULATION", "The first time you would gain shield above your maximum each combat, gain 2 <c=energy>ENERGY</c>.", deck: duoDeck);
        Instance.DuoArtifactsApi!.RegisterDuoArtifact(typeof(PerfectInsulation), new[] { eddieDeck, Enum.Parse<Deck>("dizzy") });

        UltraLightBatteriesArtifact = RegisterArtifact(registry, typeof(UltraLightBatteries), UltraLightBatteriesSprite ?? throw new Exception("missing UltraLightBatteries sprite"),
            "ULTRA-LIGHT BATTERIES", "When you run out of energy after playing a card, gain a <c=card>Lightweight</c>.", deck: duoDeck);
        Instance.DuoArtifactsApi!.RegisterDuoArtifact(typeof(UltraLightBatteries), new[] { eddieDeck, Enum.Parse<Deck>("riggs") });

        OverdriveFeedbackArtifact = RegisterArtifact(registry, typeof(OverdriveFeedback), OverdriveFeedbackSprite ?? throw new Exception("missing OverdriveFeedback sprite"),
            "OVERDRIVE FEEDBACK", "If you end your turn with more than 0 <c=energy>ENERGY</c>, your <c=status>OVERDRIVE</c> doesn't decrease.", deck: duoDeck);
        Instance.DuoArtifactsApi!.RegisterDuoArtifact(typeof(OverdriveFeedback), new[] { eddieDeck, Enum.Parse<Deck>("peri") });

        ThunderstrikeArtifact = RegisterArtifact(registry, typeof(Thunderstrike), ThunderstrikeSprite ?? throw new Exception("missing Thunderstrike sprite"),
            "THUNDERSTRIKE", "Whenever the enemy moves during your turn, they lose 1 hull.", deck: duoDeck);
        Instance.DuoArtifactsApi!.RegisterDuoArtifact(typeof(Thunderstrike), new[] { eddieDeck, Enum.Parse<Deck>("goat") });

        EmergencyVentilatorArtifact = RegisterArtifact(registry, typeof(EmergencyVentilator), EmergencyVentilatorSprite ?? throw new Exception("missing EmergencyVentilator sprite"),
            "EMERGENCY VENTILATOR", "If you end your turn with more than 0 <c=energy>ENERGY</c>, lose 1 <c=status>HEAT</c>.", deck: duoDeck);
        Instance.DuoArtifactsApi!.RegisterDuoArtifact(typeof(EmergencyVentilator), new[] { eddieDeck, Enum.Parse<Deck>("eunice") });

        VersionControlArtifact = RegisterArtifact(registry, typeof(VersionControl), VersionControlSprite ?? throw new Exception("missing VersionControl sprite"),
            "VERSION CONTROL", "Every 4th time you exhaust a card, gain 1 <c=energy>ENERGY</c>.", deck: duoDeck);
        Instance.DuoArtifactsApi!.RegisterDuoArtifact(typeof(VersionControl), new[] { eddieDeck, Enum.Parse<Deck>("hacker") });

        SpellboardArtifact = RegisterArtifact(registry, typeof(Spellboard), SpellboardSprite ?? throw new Exception("missing Spellboard sprite"),
            "SPELLBOARD", "All cards with active shard costs you can pay are <c=cardtrait>infinite</c>.", deck: duoDeck);
        Instance.DuoArtifactsApi!.RegisterDuoArtifact(typeof(Spellboard), new[] { eddieDeck, Enum.Parse<Deck>("shard") });

        VirtualTreadmillArtifact = RegisterArtifact(registry, typeof(VirtualTreadmill), VirtualTreadmillSprite ?? throw new Exception("missing VirtualTreadmill sprite"),
            "VIRTUAL TREADMILL", "All Basic cards are <c=cardtrait>infinite</c>.", deck: duoDeck);
        Instance.DuoArtifactsApi!.RegisterDuoArtifact(typeof(VirtualTreadmill), new[] { eddieDeck, Enum.Parse<Deck>("catartifact") });

        if (SogginsApi != null) {
            WaxWingsArtifact = RegisterArtifact(registry, typeof(WaxWings), WaxWingsSprite ?? throw new Exception("missing WaxWings sprite"),
                "WAX WINGS", "Whenever you <c=downside>botch</c> from oversmugging, gain 1 <c=energy>ENERGY</c>.", deck: duoDeck);
            Instance.DuoArtifactsApi!.RegisterDuoArtifact(typeof(WaxWings), new[] { eddieDeck, (Deck)SogginsApi.SogginsDeck.Id!.Value });
        }

        InsulationLogic(harmony);
        OverdriveLogic(harmony);
        VersionControlLogic(harmony);
        SpellboardLogic(harmony);
    }

    public void LoadManifest(ICustomEventHub eventHub)
    {
        _eventHub = eventHub;

        eventHub.MakeEvent<Tuple<Combat, AMove>>("Eddie.OnMoveEvent");
    }

    private void SpellboardLogic(Harmony harmony)
    {
        harmony.TryPatch(
            logger: Instance.Logger!,
            original: typeof(Card).GetMethod("GetDataWithOverrides"),
            postfix: new HarmonyMethod(typeof(Manifest).GetMethod("AffectCardData", BindingFlags.Static | BindingFlags.NonPublic))
        );
    }

    private void VersionControlLogic(Harmony harmony)
    {
        harmony.TryPatch(
            logger: Instance.Logger!,
            original: typeof(Combat).GetMethod("SendCardToExhaust"),
            prefix: new HarmonyMethod(typeof(Manifest).GetMethod("TriggerExhaustArtifacts", BindingFlags.Static | BindingFlags.NonPublic))
        );
    }

    private void InsulationLogic(Harmony harmony)
    {
        harmony.TryPatch(
            logger: Instance.Logger!,
            original: typeof(AStatus).GetMethod("Begin"),
            transpiler: new HarmonyMethod(typeof(Manifest).GetMethod("EnergyOnOvershield", BindingFlags.Static | BindingFlags.NonPublic))
        );
    }

    private void OverdriveLogic(Harmony harmony)
    {
        harmony.TryPatch(
            logger: Instance.Logger!,
            original: typeof(Ship).GetMethod("OnAfterTurn"),
            transpiler: new HarmonyMethod(typeof(Manifest).GetMethod("SkipReduceOverdrive", BindingFlags.Static | BindingFlags.NonPublic))
        );
    }

    private static void AffectCardData(Card __instance, ref CardData __result, State state) {
        foreach (Artifact item in state.EnumerateAllArtifacts()) {
            if (item is CardDataAffectorArtifact artifact)  {
                artifact.AffectCardData(state, __instance, ref __result);
            }       
        }
    }

    private static IEnumerable<CodeInstruction> SkipReduceOverdrive(IEnumerable<CodeInstruction> instructions, MethodBase originalMethod) {
        try
		{
            return new SequenceBlockMatcher<CodeInstruction>(instructions)
				.Find(
                    ILMatches.Ldarg(0),
					ILMatches.LdcI4((int)Status.overdrive),
					ILMatches.Call("Get"),
					ILMatches.LdcI4(0),
                    ILMatches.Ble
				)
                .PointerMatcher(SequenceMatcherRelativeElement.Last)
				.ExtractBranchTarget(out var branchTarget)
                .Insert(SequenceMatcherPastBoundsDirection.After, SequenceMatcherInsertionResultingBounds.IncludingInsertion, new List<CodeInstruction> {
                    new CodeInstruction(OpCodes.Ldarg_1),
                    new CodeInstruction(OpCodes.Ldarg_2),
                    new CodeInstruction(OpCodes.Call, typeof(Manifest).GetMethod("ShouldReduceOverdrive", BindingFlags.NonPublic | BindingFlags.Static)),
                    new CodeInstruction(OpCodes.Brfalse, branchTarget)
                })
                .AllElements();
		}
		catch (Exception ex)
		{
			Instance.Logger!.LogError("Could not patch method {Method} - {Mod} probably won't work.\nReason: {Exception}", originalMethod, Instance.Name, ex);
			return instructions;
		}
    }

    private static void TriggerExhaustArtifacts(Combat __instance, State s, Card card) {
        foreach (Artifact item in s.EnumerateAllArtifacts()) {
            if (item is OnExhaustArtifact artifact)  {
                artifact.OnExhaustCard(s, __instance, card);
            }     
        }
    }

    private static bool ShouldReduceOverdrive(State s, Combat c) {
        foreach (Artifact item in s.EnumerateAllArtifacts()) {
            if (item is OverdriveReductionPreventerArtifact artifact && !artifact.ShouldReduceOverdrive(s, c))  {
                item.Pulse();
                return false;
            }
                
        }
        return true;
    }
    
    private static IEnumerable<CodeInstruction> EnergyOnOvershield(IEnumerable<CodeInstruction> instructions, MethodBase originalMethod) {
        try
		{
            int? index1 = null;
            int? index2 = null;
            foreach (LocalVariableInfo info in originalMethod.GetMethodBody()!.LocalVariables)
            {
                if (info.LocalType == typeof(int))
                {
                    if (index1 == null) {
                        index1 = info.LocalIndex;
                    } else {
                        index2 = info.LocalIndex;
                        break;
                    }
                }
            }
            if (index1 == null || index2 == null) throw new Exception("Two local ints not found");

            new SequenceBlockMatcher<CodeInstruction>(instructions)
            .Find(
                ILMatches.Ldloc(0),
                ILMatches.Ldarg(0),
                ILMatches.Ldfld("status"),
                ILMatches.Call("Get"),
                ILMatches.Stloc(index1.Value)
            )
            .PointerMatcher(SequenceMatcherRelativeElement.Last)
            .CreateLdlocInstruction(out var beforeLoc);

            return new SequenceBlockMatcher<CodeInstruction>(instructions)
				.Find(
					ILMatches.Ldloc(0),
                    ILMatches.Ldarg(0),
					ILMatches.Ldfld("status"),
					ILMatches.Call("Get"),
					ILMatches.Stloc(index2.Value)
				)
                .PointerMatcher(SequenceMatcherRelativeElement.Last)
                .CreateLdlocInstruction(out var afterLoc)
				.Encompass(SequenceMatcherEncompassDirection.After, 4)
                .Insert(SequenceMatcherPastBoundsDirection.After, SequenceMatcherInsertionResultingBounds.IncludingInsertion, new List<CodeInstruction> {
                    new CodeInstruction(OpCodes.Ldarg_2),
                    new CodeInstruction(OpCodes.Ldarg_3),
                    beforeLoc,
                    afterLoc,
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Call, typeof(Manifest).GetMethod("AddEnergyOnOvershield", BindingFlags.NonPublic | BindingFlags.Static)),
                })
                .AllElements();
		}
		catch (Exception ex)
		{
			Instance.Logger!.LogError("Could not patch method {Method} - {Mod} probably won't work.\nReason: {Exception}", originalMethod, Instance.Name, ex);
			return instructions;
		}
    }

    private static void AddEnergyOnOvershield(State s, Combat c, int before, int after, AStatus action)
    {
        if (action.status != Enum.Parse<Status>("shield"))
            return;

        int supposed = before;
        if (action.mode == AStatusMode.Add)
		{
			supposed += action.statusAmount;
		}
		else if (action.mode == AStatusMode.Set)
		{
			supposed = action.statusAmount;
		}
		else if (action.mode == AStatusMode.Mult)
		{
			supposed *= action.statusAmount;
		}

        var overshield = Math.Max(supposed - after, 0);
        foreach (Artifact item in s.EnumerateAllArtifacts()) {
            if (item is OvershieldArtifact artifact) 
                artifact.OnOvershield(s, c, overshield, action.targetPlayer);
        }
    }

    private void MoveEventLogic(Harmony harmony)
    {
        var a_move_begin_method = typeof(AMove).GetMethod("Begin") ?? throw new Exception("Couldn't find AMove.Begin method");
        var a_move_begin_post = typeof(Manifest).GetMethod("FireOnMoveEvent", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic) ?? throw new Exception("Couldnt find Manifest.FireOnMoveEvent method");
        harmony.Patch(a_move_begin_method, postfix: new HarmonyMethod(a_move_begin_post));
    }

    private static void FireOnMoveEvent(AMove __instance, Combat c, State s)
    {
        Manifest.EventHub.SignalEvent<Tuple<Combat, AMove>>("Eddie.OnMoveEvent", new(c, __instance));
    }

    private void FissionChamberLogic(Harmony harmony) {
        {
            var getActionsOverridden = typeof(Card).GetMethod("GetActionsOverridden", BindingFlags.Public | BindingFlags.Instance);
            var patch = typeof(Manifest).GetMethod("FissionChamberEffect", BindingFlags.NonPublic | BindingFlags.Static);
            harmony.Patch(getActionsOverridden, postfix: new HarmonyMethod(patch));
        } {
            var renderAction = typeof(Card).GetMethod("RenderAction", BindingFlags.Public | BindingFlags.Static);
            var patch = typeof(Manifest).GetMethod("RenderActionPatch", BindingFlags.NonPublic | BindingFlags.Static);
            harmony.Patch(renderAction, transpiler: new HarmonyMethod(patch));
        } {
            var getTooltips = typeof(AVariableHint).GetMethod("GetTooltips", BindingFlags.Public | BindingFlags.Instance) ?? throw new Exception("Couldn't find AVariableHint.GetTooltips method");
            var patch = typeof(Manifest).GetMethod("GetTooltipsPatch", BindingFlags.NonPublic | BindingFlags.Static) ?? throw new Exception("Couldn't find Manifest.GetTooltipsPatch method");
            harmony.Patch(getTooltips, postfix: new HarmonyMethod(patch));
        }
    }


    public static ConditionalWeakTable<CardAction, StructRef<int>> increasedHints = new ConditionalWeakTable<CardAction, StructRef<int>>();

    private static void FissionChamberEffect(Card __instance, List<CardAction> __result, State s, Combat c) {
        int baseXBonus = 0;
        foreach (Artifact item in s.EnumerateAllArtifacts()) {
            if (item is XAffectorArtifact xAffector) 
                baseXBonus += xAffector.AffectX(baseXBonus);
        }
        if (baseXBonus == 0) return;

        foreach (CardAction action in __result) {
            if (action is AVariableHint) {
                increasedHints.Add(action, baseXBonus);
            }
            var xHint = action.xHint;
            if (xHint == null)
                continue;

            int xBonus = (int)xHint * baseXBonus;

            if (action is AAttack attack) {
                attack.damage += xBonus;
            } else if (action is AStatus status) {
                status.statusAmount += xBonus;
            } else if (action is ADrawCard draw) {
                draw.count += xBonus;
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
    }

    [HarmonyDebug]
    private static IEnumerable<CodeInstruction> RenderActionPatch(IEnumerable<CodeInstruction> iseq, ILGenerator il, MethodBase originalMethod) {
        List<CodeInstruction> spriteColorInstructions = new List<CodeInstruction>();
        List<CodeInstruction> iconWidthInstructions = new List<CodeInstruction>();
        List<CodeInstruction> wInstructions = new List<CodeInstruction>();
        
        using IEnumerator<CodeInstruction> iter = iseq.GetEnumerator();
        while (iter.MoveNext()) {
            yield return iter.Current;
            if(iter.Current.opcode != OpCodes.Ldstr || (string)(iter.Current.operand) != "ffffff") {
                continue;
            }

            if(!iter.MoveNext()) {
                break;
            }
            
            yield return iter.Current;
            if(iter.Current.opcode != OpCodes.Newobj || !((iter.Current.operand) is ConstructorInfo) || ((ConstructorInfo)iter.Current.operand).DeclaringType!.Name != "Color") {
                continue;
            }
            
            while (iter.MoveNext() && iter.Current.opcode != OpCodes.Stfld) {
                yield return iter.Current;
            }

            if (iter.Current.opcode != OpCodes.Stfld)
                break;
            yield return iter.Current;

            spriteColorInstructions.Add(iter.Current);
            break;
        }
        while (iter.MoveNext()) {
            yield return iter.Current;
            if(iter.Current.opcode != OpCodes.Ldloca_S || TranspilerUtils.ExtractLocalIndex(iter.Current) != 0) {
                continue;
            }

            List<CodeInstruction> candidates = new List<CodeInstruction>();
            candidates.Add(iter.Current);
            if(!iter.MoveNext()) {
                break;
            }
            yield return iter.Current;
            
            if(iter.Current.opcode != OpCodes.Ldc_I4_0) {
                continue;
            }

            if(!iter.MoveNext()) {
                break;
            }
            yield return iter.Current;
            
            if(iter.Current.opcode != OpCodes.Stfld) {
                continue;
            }

            candidates.Add(iter.Current);
            wInstructions = candidates;
            break;
            // IL_0082: ldloca.s 0
            // IL_0084: ldc.i4.0
            // IL_0085: stfld int32 Card/'<>c__DisplayClass57_0'::w
        }
        while (iter.MoveNext()) {
            yield return iter.Current;
            if(iter.Current.opcode != OpCodes.Ldloca_S || TranspilerUtils.ExtractLocalIndex(iter.Current) != 0) {
                continue;
            }

            List<CodeInstruction> candidates = new List<CodeInstruction>();
            candidates.Add(iter.Current);
            if(!iter.MoveNext()) {
                break;
            }
            yield return iter.Current;
            
            if(iter.Current.opcode != OpCodes.Ldc_I4_8) {
                continue;
            }

            if(!iter.MoveNext()) {
                break;
            }
            yield return iter.Current;
            
            if(iter.Current.opcode != OpCodes.Stfld) {
                continue;
            }

            candidates.Add(iter.Current);
            iconWidthInstructions = candidates;
            break;
        }

        bool second = false;
        while (iter.MoveNext()) {
            if (wInstructions.Count == 0 || spriteColorInstructions.Count == 0 || iconWidthInstructions.Count == 0)
                break; 
            
            yield return iter.Current;
            if(iter.Current.opcode != OpCodes.Ldloc_0) {
                continue;
            }

            List<CodeInstruction> candidates = new List<CodeInstruction>();
            candidates.Add(iter.Current);
            if(!iter.MoveNext()) {
                break;
            }
            yield return iter.Current;
            
            if(iter.Current.opcode != OpCodes.Ldfld) {
                continue;
            }

            candidates.Add(iter.Current);
            if(!iter.MoveNext()) {
                break;
            }
            yield return iter.Current;
            
            if(iter.Current.opcode != OpCodes.Isinst || (Type)iter.Current.operand != typeof(AVariableHint)) {
                continue;
            }

            candidates.Add(iter.Current);
            if(!iter.MoveNext()) {
                break;
            }
            yield return iter.Current;

            if(!TranspilerUtils.IsLocalStore(iter.Current)) {
                continue;
            }
            {
                var local = ((LocalBuilder)iter.Current.operand).LocalIndex;
                candidates.Add(iter.Current);
                if(!iter.MoveNext()) {
                    break;
                }
                yield return iter.Current;
                
                if(!TranspilerUtils.IsLocalLoad(iter.Current) || ((LocalBuilder)iter.Current.operand).LocalIndex != local) {
                    continue;
                }
            }

            candidates.Add(iter.Current);
            if(!iter.MoveNext()) {
                break;
            }
            yield return iter.Current;
            
            if(iter.Current.opcode != OpCodes.Brfalse_S) {
                continue;
            }

            if (!second) {
                second = true;
                continue;
            }

            candidates.Add(iter.Current);
            List<Label> labels = new List<Label> { (Label)iter.Current.operand };

            while (iter.MoveNext() && !(iter.Current.labels is List<Label> list && list.Contains(labels[0]))) {
                yield return iter.Current;
                if (iter.Current.operand is Label)
                    labels.Add((Label)iter.Current.operand);

            }

            if (!(iter.Current.labels is List<Label> && (iter.Current.labels as List<Label>).Contains(labels[0])))
                break;

            List<Label> labelsToAdd = new List<Label>();
            labelsToAdd.AddRange(labels.Where(label => iter.Current.labels.Remove(label)));
            
            yield return new CodeInstruction(wInstructions[0].opcode, wInstructions[0].operand).WithLabels(labelsToAdd); // prepare for w
            yield return new CodeInstruction(candidates[0].opcode, candidates[0].operand); //
            yield return new CodeInstruction(candidates[1].opcode, candidates[1].operand); // action
            yield return new CodeInstruction(wInstructions[0].opcode, wInstructions[0].operand); //
            yield return new CodeInstruction(OpCodes.Ldfld, spriteColorInstructions[0].operand); // color
            yield return new CodeInstruction(OpCodes.Ldarg_3); // dontDraw
            yield return new CodeInstruction(OpCodes.Ldarg_0); // g
            yield return new CodeInstruction(iconWidthInstructions[0].opcode, iconWidthInstructions[0].operand); //
            yield return new CodeInstruction(OpCodes.Ldfld, iconWidthInstructions[1].operand); // icon width
            yield return new CodeInstruction(wInstructions[0].opcode, wInstructions[0].operand); //
            yield return new CodeInstruction(OpCodes.Ldfld, wInstructions[1].operand); // w
            yield return new CodeInstruction(OpCodes.Call, typeof(Manifest).GetMethod("RenderXIncrease", BindingFlags.NonPublic | BindingFlags.Static));
            yield return new CodeInstruction(wInstructions[1].opcode, wInstructions[1].operand); // set w
            
            yield return iter.Current;
        }
    }

    private static int RenderXIncrease(CardAction action, Color color, bool dontDraw, G g, int iconWidth, int w) {
        StructRef<int>? value;
        if (increasedHints.TryGetValue(action, out value) && value != 0) {
            // Plus
            // w += 3;
            w--;
            if (!dontDraw)
            {
                Rect? rect = new Rect(w + 1);
                Vec xy = g.Push(null, rect).rect.xy;
                Spr? plus = Enum.Parse<Spr>("icons_plus");
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
                Color textColor = (action.disabled ? Colors.disabledText : Colors.textMain);;
                Draw.Text(value.Value + "", xy.x, xy.y + 2, null, textColor, null, null, null, null, dontDraw: false, null, color, null, null, null, dontSubstituteLocFont: true);
                g.Pop();
            }
            w += iconWidth - 5;
        }
        return w;
    }

    private static void GetTooltipsPatch(CardAction __instance, List<Tooltip> __result, State s) {
        int baseXBonus = 0;
        foreach (Artifact item in s.EnumerateAllArtifacts()) {
            if (item is XAffectorArtifact xAffector) 
                baseXBonus += xAffector.AffectX(baseXBonus);
        }
        if (baseXBonus == 0) return;
        foreach (Tooltip t in __result) {
            if (t is TTGlossary glossary && glossary.vals != null) {
                string last = glossary.vals[glossary.vals.Length-1]?.ToString() ?? "";
                glossary.vals[glossary.vals.Length-1] = last + " + " + baseXBonus;
            }
        }
    }
}
