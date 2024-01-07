using CobaltCoreModding.Definitions;
using CobaltCoreModding.Definitions.ExternalItems;
using CobaltCoreModding.Definitions.ModContactPoints;
using CobaltCoreModding.Definitions.ModManifests;
using HarmonyLib;
using Eddie.Cards;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Reflection.Emit;
using static System.Reflection.BindingFlags;
using daisyowl.text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Eddie;

public partial class Manifest : ISpriteManifest, IDeckManifest, IGlossaryManifest, ICardManifest, ICharacterManifest, IAnimationManifest, IModManifest//, ICustomEventManifest, IArtifactManifest
{
    internal static Manifest Instance { get; private set; } = null!;
    internal IDuoArtifactsApi? DuoArtifactsApi { get; private set; } = null!;
    internal ISogginsApi? SogginsApi { get; private set; } = null!;

    public IEnumerable<DependencyEntry> Dependencies => new DependencyEntry[]
    {
        new DependencyEntry<IModManifest>("Shockah.DuoArtifacts", ignoreIfMissing: true),
        new DependencyEntry<IModManifest>("Shockah.Soggins", ignoreIfMissing: true)
    };

    public ILogger? Logger { get; set; }

    public static ExternalGlossary? AddShortCircuitGlossary { get; private set; }
    public static ExternalGlossary? ShortCircuitGlossary { get; private set; }
    public static ExternalGlossary? AddInfiniteGlossary { get; private set; }
    public static ExternalGlossary? DiscardLeftmostGlossary { get; private set; }
    public static ExternalGlossary? MakeFreeGlossary { get; private set; }
    public static ExternalGlossary? CheapGlossary { get; private set; }
    public static ExternalGlossary? PowerCellGlossary { get; private set; }
    // public static ExternalGlossary? LeftmostCardGlossary { get; private set; }
    public static ExternalGlossary? TemporaryHurtGlossary { get; private set; }
    public static ExternalGlossary? XIsEnergyGlossary { get; private set; }
    public static ExternalGlossary? DiscountHandGlossary { get; private set; }
    public static ExternalGlossary? ExpensiveHandGlossary { get; private set; }
    public static ExternalGlossary? MoveEnemyLeftGlossary { get; private set; }
    public static ExternalGlossary? MoveEnemyRightGlossary { get; private set; }

    public static ExternalSprite? ShortCircuitIcon { get; private set; }
    public static ExternalSprite? CheapIcon { get; private set; }
    public static ExternalSprite? CircuitIcon { get; private set; }
    public static ExternalSprite? ClosedCircuitIcon { get; private set; }
    public static ExternalSprite? LoseEnergyEveryTurnIcon { get; private set; }
    public static ExternalSprite? PowerCellSprite { get; private set; }
    public static ExternalSprite? PowerCellIcon { get; private set; }
    // public static ExternalSprite? LeftmostCardIcon { get; private set; }
    public static ExternalSprite? TemporaryHurtIcon { get; private set; }
    public static ExternalSprite? HealNextTurnIcon { get; private set; }
    // public static ExternalSprite? OverchargeIcon { get; private set; }
    public static ExternalSprite? EnergyIcon { get; private set; }
    public static ExternalSprite? ApplyShortCircuitIcon { get; private set; }
    public static ExternalSprite? ApplyInfiniteIcon { get; private set; }

    public static ExternalSprite? EddieCardFrame { get; private set; }
    public static ExternalSprite? EddieUncommonCardFrame { get; private set; }
    public static ExternalSprite? EddieRareCardFrame { get; private set; }
    public static ExternalSprite? EddiePanelFrame { get; private set; }

    public static ExternalSprite? ChannelCardArt { get; private set; }
    public static ExternalSprite? ChannelTopCardArt { get; private set; }
    public static ExternalSprite? ChannelBottomCardArt { get; private set; }
    public static ExternalSprite? GammaRayCardArt { get; private set; }
    public static ExternalSprite? CircuitCardArt { get; private set; }
    public static ExternalSprite? EnergyBoltCardArt { get; private set; }
    public static ExternalSprite? RummageCardArt { get; private set; }
    public static ExternalSprite? PowerNapBottomCardArt { get; private set; }
    public static ExternalSprite? PowerNapTopCardArt { get; private set; }

    public static ExternalSprite? FrazzledWiresSprite { get; private set; }
    public static ExternalSprite? SunLampOnSprite { get; private set; }
    public static ExternalSprite? SunLampOffSprite { get; private set; }
    public static ExternalSprite? SunLampUnchargedSprite { get; private set; }
    public static ExternalSprite? ElectromagneticCoilSprite { get; private set; }
    public static ExternalSprite? SolarPanelsOnSprite { get; private set; }
    public static ExternalSprite? SolarPanelsOffSprite { get; private set; }
    public static ExternalSprite? DeconstructionGogglesSprite { get; private set; }
    public static ExternalSprite? FissionChamberSprite { get; private set; }

    public static ExternalSprite? PerfectInsulationSprite { get; private set; }
    public static ExternalSprite? UltraLightBatteriesSprite { get; private set; }
    public static ExternalSprite? OverdriveFeedbackSprite { get; private set; }
    public static ExternalSprite? ThunderstrikeSprite { get; private set; }
    public static ExternalSprite? EmergencyVentilatorSprite { get; private set; }
    public static ExternalSprite? VersionControlSprite { get; private set; }
    public static ExternalSprite? SpellboardSprite { get; private set; }
    public static ExternalSprite? VirtualTreadmillSprite { get; private set; }
    public static ExternalSprite? WaxWingsSprite { get; private set; }

    public static ExternalSprite? FreeMarkerSprite { get; private set; }

    public static ExternalCharacter? EddieCharacter { get; private set; }
    public static ExternalDeck? EddieDeck { get; private set; }
    public static ExternalAnimation? EddieDefaultAnimation { get; private set; }
    public static ExternalSprite? EddiePortrait { get; private set; }
    public static ExternalSprite? EddieMini { get; private set; }
    public static ExternalAnimation? EddieMiniAnimation { get; private set; }
    public static ExternalAnimation? EddieGameoverAnimation { get; private set; }
    public static ExternalAnimation? EddieSquintAnimation { get; private set; }
    
    public static ExternalCard? ChannelCard { get; private set; }
    public static ExternalCard? PowerNapCard { get; private set; }
    public static ExternalCard? ReverseEngineerCard { get; private set; }
    public static ExternalCard? PowerCellCard { get; private set; }
    public static ExternalCard? RefundShotCard { get; private set; }
    public static ExternalCard? SolarSailingCard { get; private set; }
    public static ExternalCard? EnergyBoltCard { get; private set; }
    public static ExternalCard? PowerSinkCard { get; private set; }
    public static ExternalCard? RummageCard { get; private set; }
    public static ExternalCard? BorrowCard { get; private set; }
    public static ExternalCard? InterferenceCard { get; private set; }
    public static ExternalCard? ChargeCannonsCard { get; private set; }
    public static ExternalCard? ChargeShieldsCard { get; private set; }
    public static ExternalCard? ChargeThrustersCard { get; private set; }
    public static ExternalCard? GarageSaleCard { get; private set; }
    public static ExternalCard? ShortTermSolutionCard { get; private set; }
    public static ExternalCard? AmplifyCard { get; private set; }
    // public static ExternalCard? OrganizeCard { get; private set; }
    public static ExternalCard? InnovationCard { get; private set; }
    public static ExternalCard? CircuitCard { get; private set; }
    public static ExternalCard? RenewableResourceCard { get; private set; }
    public static ExternalCard? GammaRayCard { get; private set; }
    public static ExternalCard? JumpstartCard { get; private set; }

    public static ExternalCard? LightweightCard { get; private set; }
    public static ExternalCard? SurgeCard { get; private set; }
    

    public static List<ExternalSprite> TalkScaredSprites { get; private set; } = new List<ExternalSprite>();

    public DirectoryInfo? ModRootFolder { get; set; }
    public string Name { get; init; } = "TheJazMaster.Eddie";
    public DirectoryInfo? GameRootFolder { get; set; }


    private ExternalSprite RegisterSprite(ISpriteRegistry registry, string globalName, string path) {
        var sprite = new ExternalSprite("Eddie." + globalName, new FileInfo(path));
        registry.RegisterArt(sprite);
        return sprite;
    }


    void ISpriteManifest.LoadManifest(ISpriteRegistry registry)
    {
        if (ModRootFolder == null)
            throw new Exception("Root Folder not set");

        // Artifacts
        FrazzledWiresSprite = RegisterSprite(registry, "FrazzledWires", Path.Combine(ModRootFolder.FullName, "Sprites", "artifact_icons", Path.GetFileName("frazzled_wires.png")));
        SunLampOnSprite = RegisterSprite(registry, "SunLampOn", Path.Combine(ModRootFolder.FullName, "Sprites", "artifact_icons", Path.GetFileName("sun_lamp.png")));
        SunLampOffSprite = RegisterSprite(registry, "SunLampOff", Path.Combine(ModRootFolder.FullName, "Sprites", "artifact_icons", Path.GetFileName("sun_lamp_off.png")));
        SunLampUnchargedSprite = RegisterSprite(registry, "SunLampUncharged", Path.Combine(ModRootFolder.FullName, "Sprites", "artifact_icons", Path.GetFileName("sun_lamp_uncharged.png")));
        SolarPanelsOnSprite = RegisterSprite(registry, "SolarPanelsOn", Path.Combine(ModRootFolder.FullName, "Sprites", "artifact_icons", Path.GetFileName("solar_panels.png")));
        SolarPanelsOffSprite = RegisterSprite(registry, "SolarPanelsOff", Path.Combine(ModRootFolder.FullName, "Sprites", "artifact_icons", Path.GetFileName("solar_panels_off.png")));
        ElectromagneticCoilSprite = RegisterSprite(registry, "ElectromagneticCoil", Path.Combine(ModRootFolder.FullName, "Sprites", "artifact_icons", Path.GetFileName("electromagnetic_coil.png")));
        FissionChamberSprite = RegisterSprite(registry, "FissionChamber", Path.Combine(ModRootFolder.FullName, "Sprites", "artifact_icons", Path.GetFileName("fission_chamber.png")));
        DeconstructionGogglesSprite = RegisterSprite(registry, "DeconstructionGoggles", Path.Combine(ModRootFolder.FullName, "Sprites", "artifact_icons", Path.GetFileName("deconstruction_goggles.png")));

        PowerCellSprite = RegisterSprite(registry, "PowerCell", Path.Combine(ModRootFolder.FullName, "Sprites", Path.GetFileName("power_cell.png")));

        // Character sprites
        EddiePortrait = RegisterSprite(registry, "EddiePortrait", Path.Combine(ModRootFolder.FullName, "Sprites", Path.GetFileName("EddieNeutral.png")));
        EddieMini = RegisterSprite(registry, "EddieMini", Path.Combine(ModRootFolder.FullName, "Sprites", Path.GetFileName("EddieMini.png")));
        EddiePanelFrame = RegisterSprite(registry, "EddiePanelFrame", Path.Combine(ModRootFolder.FullName, "Sprites", Path.GetFileName("EddieFrame.png")));

        // Deck sprites
        EddieCardFrame = RegisterSprite(registry, "EddieCardFrame", Path.Combine(ModRootFolder.FullName, "Sprites", Path.GetFileName("EddieCardFrame.png")));

        // Icons
        LoseEnergyEveryTurnIcon = RegisterSprite(registry, "LoseEnergyEveryTurnIcon", Path.Combine(ModRootFolder.FullName, "Sprites/icons", Path.GetFileName("energy_less_every_turn.png")));
        CircuitIcon = RegisterSprite(registry, "CircuitIcon", Path.Combine(ModRootFolder.FullName, "Sprites/icons", Path.GetFileName("circuit.png")));
        ClosedCircuitIcon = RegisterSprite(registry, "ClosedCircuitIcon", Path.Combine(ModRootFolder.FullName, "Sprites/icons", Path.GetFileName("closed_circuit.png")));
        ShortCircuitIcon = RegisterSprite(registry, "ShortCircuitIcon", Path.Combine(ModRootFolder.FullName, "Sprites/icons", Path.GetFileName("short_circuit.png")));
        CheapIcon = RegisterSprite(registry, "CheapIcon", Path.Combine(ModRootFolder.FullName, "Sprites/icons", Path.GetFileName("cheap.png")));
        TemporaryHurtIcon = RegisterSprite(registry, "TemporaryHurtIcon", Path.Combine(ModRootFolder.FullName, "Sprites/icons", Path.GetFileName("temporary_hurt.png")));
        HealNextTurnIcon = RegisterSprite(registry, "HealNextTurnIcon", Path.Combine(ModRootFolder.FullName, "Sprites/icons", Path.GetFileName("heal_next_turn.png")));
        // OverchargeIcon = RegisterSprite(registry, "OverchargeIcon", Path.Combine(ModRootFolder.FullName, "Sprites/icons", Path.GetFileName("overcharge.png")));
        PowerCellIcon = RegisterSprite(registry, "PowerCellIcon", Path.Combine(ModRootFolder.FullName, "Sprites/icons", Path.GetFileName("power_cell.png")));
        EnergyIcon = RegisterSprite(registry, "EnergyIcon", Path.Combine(ModRootFolder.FullName, "Sprites/icons", Path.GetFileName("energy.png")));
        ApplyShortCircuitIcon = RegisterSprite(registry, "ApplyShortCircuitIcon", Path.Combine(ModRootFolder.FullName, "Sprites/icons", Path.GetFileName("apply_short_circuit.png")));
        ApplyInfiniteIcon = RegisterSprite(registry, "ApplyInfiniteIcon", Path.Combine(ModRootFolder.FullName, "Sprites/icons", Path.GetFileName("apply_infinite.png")));

        FreeMarkerSprite = RegisterSprite(registry, "FreeMarker", Path.Combine(ModRootFolder.FullName, "Sprites", Path.GetFileName("free_marker.png")));

        // Card art
        ChannelCardArt = RegisterSprite(registry, "ChannelCardArt", Path.Combine(ModRootFolder.FullName, "Sprites/card_art", Path.GetFileName("channel.png")));
        ChannelTopCardArt = RegisterSprite(registry, "ChannelTopCardArt", Path.Combine(ModRootFolder.FullName, "Sprites/card_art", Path.GetFileName("channel_upgraded_top.png")));
        ChannelBottomCardArt = RegisterSprite(registry, "ChannelBottomCardArt", Path.Combine(ModRootFolder.FullName, "Sprites/card_art", Path.GetFileName("channel_upgraded_bottom.png")));
        GammaRayCardArt = RegisterSprite(registry, "GammaRayCardArt", Path.Combine(ModRootFolder.FullName, "Sprites/card_art", Path.GetFileName("gamma_ray.png")));
        RummageCardArt = RegisterSprite(registry, "RummageCardArt", Path.Combine(ModRootFolder.FullName, "Sprites/card_art", Path.GetFileName("rummage.png")));
        CircuitCardArt = RegisterSprite(registry, "CircuitCardArt", Path.Combine(ModRootFolder.FullName, "Sprites/card_art", Path.GetFileName("circuit.png")));
        EnergyBoltCardArt = RegisterSprite(registry, "EnergyBoltCardArt", Path.Combine(ModRootFolder.FullName, "Sprites/card_art", Path.GetFileName("energy_bolt.png")));
        PowerNapTopCardArt = RegisterSprite(registry, "PowerNapTopCardArt", Path.Combine(ModRootFolder.FullName, "Sprites/card_art", Path.GetFileName("power_nap_top.png")));
        PowerNapBottomCardArt = RegisterSprite(registry, "PowerNapBottomCardArt", Path.Combine(ModRootFolder.FullName, "Sprites/card_art", Path.GetFileName("power_nap_bottom.png")));

        // Animations
        // {
        //     var dir_path = Path.Combine(ModRootFolder.FullName, "Sprites", "talk_scared");
        //     var files = Directory.GetFiles(dir_path).Select(e => new FileInfo(e)).ToArray();
        //     for (int i = 0; i < files.Length; i++)
        //     {
        //         TalkScaredSprites.Add(RegisterSprite(registry, "TalkScared" + i, files[i].Name));
        //     }
        // }


        // Duos

        if (DuoArtifactsApi != null) RegisterDuoSprites(registry);
    }

    void RegisterDuoSprites(ISpriteRegistry registry) {
        if (ModRootFolder == null)
            throw new Exception("Root Folder not set");

        PerfectInsulationSprite = RegisterSprite(registry, "PerfectInsulation", Path.Combine(ModRootFolder.FullName, "Sprites", "artifact_icons", "duos", Path.GetFileName("perfect_insulation.png")));
        UltraLightBatteriesSprite = RegisterSprite(registry, "UltraLightBatteries", Path.Combine(ModRootFolder.FullName, "Sprites", "artifact_icons", "duos", Path.GetFileName("ultralight_batteries.png")));
        OverdriveFeedbackSprite = RegisterSprite(registry, "OverdriveFeedback", Path.Combine(ModRootFolder.FullName, "Sprites", "artifact_icons", "duos", Path.GetFileName("overdrive_feedback.png")));
        ThunderstrikeSprite = RegisterSprite(registry, "Thunderstrike", Path.Combine(ModRootFolder.FullName, "Sprites", "artifact_icons", "duos", Path.GetFileName("thunderstrike.png")));
        EmergencyVentilatorSprite = RegisterSprite(registry, "EmergencyVentilator", Path.Combine(ModRootFolder.FullName, "Sprites", "artifact_icons", "duos", Path.GetFileName("emergency_ventilator.png")));
        VersionControlSprite = RegisterSprite(registry, "VersionControl", Path.Combine(ModRootFolder.FullName, "Sprites", "artifact_icons", "duos", Path.GetFileName("version_control.png")));
        SpellboardSprite = RegisterSprite(registry, "Spellboard", Path.Combine(ModRootFolder.FullName, "Sprites", "artifact_icons", "duos", Path.GetFileName("spellboard.png")));
        VirtualTreadmillSprite = RegisterSprite(registry, "VirtualTreadmill", Path.Combine(ModRootFolder.FullName, "Sprites", "artifact_icons", "duos", Path.GetFileName("virtual_treadmill.png")));
        WaxWingsSprite = RegisterSprite(registry, "WaxWings", Path.Combine(ModRootFolder.FullName, "Sprites", "artifact_icons", Path.GetFileName("wax_wings.png")));
    }

    public static System.Drawing.Color Eddie_PrimaryColor = System.Drawing.Color.FromArgb(230, 225, 100);

    public void LoadManifest(IDeckRegistry registry)
    {
        ExternalSprite cardArtDefault = ExternalSprite.GetRaw((int)Enum.Parse<Spr>("cards_colorless"));
        ExternalSprite borderSprite = EddieCardFrame ?? throw new Exception();
        EddieDeck = new ExternalDeck(
            "Eddie.EddieDeck",
            Eddie_PrimaryColor,
            System.Drawing.Color.Black,
            cardArtDefault,
            borderSprite,
            null);
        registry.RegisterDeck(EddieDeck);
    }

    void IGlossaryManifest.LoadManifest(IGlossaryRegisty registry)
    {
        AddShortCircuitGlossary = new ExternalGlossary("Eddie.Glossary.AddShortCircuitDesc", "EddieAddShortCircuitAction", false, ExternalGlossary.GlossayType.action, ApplyShortCircuitIcon ?? throw new Exception("Missing Apply Short Circuit Icon"));
        AddShortCircuitGlossary.AddLocalisation("en", "Apply short-circuit", "Make a card short-circuit {0}.", null);
        registry.RegisterGlossary(AddShortCircuitGlossary);

        // ShortCircuitGlossary = new ExternalGlossary("Eddie.Glossary.ShortCircuitDesc", "EddieShortCircuitTrait", false, ExternalGlossary.GlossayType.cardtrait, ShortCircuitIcon ?? throw new Exception("Missing Short Circuit Icon"));
        // ShortCircuitGlossary.AddLocalisation("en", "Short-Circuit", "This card does its actions twice in a row, but costs 1 more <c=energy>ENERGY</c> to play.", null);
        // registry.RegisterGlossary(ShortCircuitGlossary);
        ShortCircuitGlossary = new ExternalGlossary("Eddie.Glossary.ShortCircuitDesc", "EddieShortCircuitTrait", false, ExternalGlossary.GlossayType.cardtrait, ShortCircuitIcon ?? throw new Exception("Missing Short Circuit Icon"));
        ShortCircuitGlossary.AddLocalisation("en", "Short-Circuit", "If this card costs 0 energy, discard the 2 leftmost cards after playing it.", null);
        registry.RegisterGlossary(ShortCircuitGlossary);

        AddInfiniteGlossary = new ExternalGlossary("Eddie.Glossary.AddInfiniteDesc", "EddieAddInfiniteAction", false, ExternalGlossary.GlossayType.action, ApplyInfiniteIcon ?? throw new Exception("Missing Apply Infinite Icon"));
        AddInfiniteGlossary.AddLocalisation("en", "Make infinite", "Make a card infinite {0}.", null);
        registry.RegisterGlossary(AddInfiniteGlossary);

        DiscardLeftmostGlossary = new ExternalGlossary("Eddie.Glossary.DiscardLeftmostDesc", "EddieDiscardLeftmostAction", false, ExternalGlossary.GlossayType.action, ExternalSprite.GetRaw((int)Enum.Parse<Spr>("icons_discardCard")));
        DiscardLeftmostGlossary.AddLocalisation("en", "Discard Leftmost", "Discard the {0} leftmost cards.", null);
        registry.RegisterGlossary(DiscardLeftmostGlossary);

        MakeFreeGlossary = new ExternalGlossary("Eddie.Glossary.MakeFreeDesc", "EddieMakeFreeAction", false, ExternalGlossary.GlossayType.action, ExternalSprite.GetRaw((int)Enum.Parse<Spr>("icons_discount")));
        MakeFreeGlossary.AddLocalisation("en", "Make free", "Make a card cost 0 <c=energy>ENERGY</c> {0}.", null);
        registry.RegisterGlossary(MakeFreeGlossary);

        CheapGlossary = new ExternalGlossary("Eddie.Glossary.CheapDesc", "EddieCheapTrait", false, ExternalGlossary.GlossayType.cardtrait, CheapIcon ?? throw new Exception("Missing Cheap Icon"));
        CheapGlossary.AddLocalisation("en", "Cheap", "This card starts each combat with a <c=cardtrait>discount</c> of {0} energy.", null);
        registry.RegisterGlossary(CheapGlossary);

        PowerCellGlossary = new ExternalGlossary("Eddie.Glossary.PowerCellGlossary", "EddiePowerCellMidrow", false, ExternalGlossary.GlossayType.midrow, PowerCellIcon ?? throw new Exception("Missing Power Cell Icon"));
        PowerCellGlossary.AddLocalisation("en", "Power Cell", "Blocks one attack. Gain 1 <c=energy>ENERGY</c> when this is destroyed.", null);
        registry.RegisterGlossary(PowerCellGlossary);

        // LeftmostCardGlossary = new ExternalGlossary("Eddie.Glossary.LeftmostCardGlossary", "EddieLeftmostCardMisc", false, ExternalGlossary.GlossayType.actionMisc, LeftmostCardIcon ?? throw new Exception("Missing Leftmost Card Icon"));
        // LeftmostCardGlossary.AddLocalisation("en", "Leftmost Card", "This affects the leftmost card in your hand.", null);
        // registry.RegisterGlossary(LeftmostCardGlossary);

        TemporaryHurtGlossary = new ExternalGlossary("Eddie.Glossary.TemporaryHurtGlossary", "EddieTemporaryHurtAction", false, ExternalGlossary.GlossayType.action, TemporaryHurtIcon ?? throw new Exception("Missing Temporary Hurt Icon"));
        TemporaryHurtGlossary.AddLocalisation("en", "Temporary Hull Loss", "Lose {0} hull. Regain that hull at the start of next turn or at the end of combat.", null);
        registry.RegisterGlossary(TemporaryHurtGlossary);

        XIsEnergyGlossary = new ExternalGlossary("Eddie.Glossary.XIsEnergyGlossary", "EddieXIsEnergyAction", false, ExternalGlossary.GlossayType.action, EnergyIcon ?? throw new Exception("Missing Energy Icon"));
        XIsEnergyGlossary.AddLocalisation("en", "", "<c=action>X</c> = Your <c=energy>ENERGY</c> after paying for this card{0}.", null);
        registry.RegisterGlossary(XIsEnergyGlossary);

        DiscountHandGlossary = new ExternalGlossary("Eddie.Glossary.DiscountHandGlossary", "EddieDiscountHandAction", false, ExternalGlossary.GlossayType.action, ExternalSprite.GetRaw((int)Enum.Parse<Spr>("icons_discount")));
        DiscountHandGlossary.AddLocalisation("en", "Discount Hand", "Every card in your hand becomes <c=cardtrait>discounted</c> by {0} energy.", null);
        registry.RegisterGlossary(DiscountHandGlossary);

        ExpensiveHandGlossary = new ExternalGlossary("Eddie.Glossary.ExpensiveHandGlossary", "EddieExpensiveHandAction", false, ExternalGlossary.GlossayType.action, ExternalSprite.GetRaw((int)Enum.Parse<Spr>("icons_expensive")));
        ExpensiveHandGlossary.AddLocalisation("en", "Make Hand Expensive", "Every card in your hand becomes more <c=cardtrait>expensive</c> by energy.", null);
        registry.RegisterGlossary(ExpensiveHandGlossary);

        MoveEnemyLeftGlossary = new ExternalGlossary("Eddie.Glossary.MoveEnemyLeftGlossary", "EddieMoveEnemyLeftAction", false, ExternalGlossary.GlossayType.action, ExternalSprite.GetRaw((int)Enum.Parse<Spr>("icons_moveLeftEnemy")));
        MoveEnemyLeftGlossary.AddLocalisation("en", "Move Enemy Left", "The enemy will instantly move {0} spaces to the <c=keyword>LEFT</c>.", null);
        registry.RegisterGlossary(MoveEnemyLeftGlossary);

        MoveEnemyRightGlossary = new ExternalGlossary("Eddie.Glossary.MoveEnemyRightGlossary", "EddieMoveEnemyRightAction", false, ExternalGlossary.GlossayType.action, ExternalSprite.GetRaw((int)Enum.Parse<Spr>("icons_moveRightEnemy")));
        MoveEnemyRightGlossary.AddLocalisation("en", "Move Enemy Right", "The enemy will instantly move {0} spaces to the <c=keyword>RIGHT</c>.", null);
        registry.RegisterGlossary(MoveEnemyRightGlossary);
    }

    private ExternalCard RegisterCard(ICardRegistry registry, Type type, ExternalSprite? art, string? name = null, ExternalDeck? deck = null)
    {
        if (deck == null)
            deck = EddieDeck ?? throw new Exception("No default deck available");
        if (art == null)
            throw new Exception("Card art for " + name + " not found");
        var card = new ExternalCard("Eddie.Cards." + type.Name, type, art, deck);
        card.AddLocalisation((name == null) ? type.Name : name);
        registry.RegisterCard(card);
        return card!;
    }

    void ICardManifest.LoadManifest(ICardRegistry registry)
    {
        ExternalSprite cardArtDefault = ExternalSprite.GetRaw((int)Enum.Parse<Spr>("cards_colorless"));

        ChannelCard = RegisterCard(registry, typeof(Channel), ChannelCardArt, "Channel");

        ReverseEngineerCard = RegisterCard(registry, typeof(ReverseEngineer), cardArtDefault, "Reverse-Engineer");

        PowerNapCard = RegisterCard(registry, typeof(PowerNap), PowerNapTopCardArt, "Power Nap");

        PowerCellCard = RegisterCard(registry, typeof(PowerCell), cardArtDefault, "Power Cell");

        RefundShotCard = RegisterCard(registry, typeof(RefundShot), cardArtDefault, "Refund Shot");

        SolarSailingCard = RegisterCard(registry, typeof(SolarSailing), cardArtDefault, "Solar Sailing");

        EnergyBoltCard = RegisterCard(registry, typeof(EnergyBolt), EnergyBoltCardArt, "Energy Bolt");

        PowerSinkCard = RegisterCard(registry, typeof(PowerSink), cardArtDefault, "Power Sink");

        RummageCard = RegisterCard(registry, typeof(Rummage), RummageCardArt, "Rummage");

        BorrowCard = RegisterCard(registry, typeof(Borrow), cardArtDefault, "Borrow");

        InterferenceCard = RegisterCard(registry, typeof(Interference), cardArtDefault, "Interference");

        ChargeCannonsCard = RegisterCard(registry, typeof(ChargeCannons), cardArtDefault, "Charge Cannons");

        ChargeShieldsCard = RegisterCard(registry, typeof(ChargeShields), cardArtDefault, "Charge Shields");

        ChargeThrustersCard = RegisterCard(registry, typeof(ChargeThrusters), cardArtDefault, "Charge Thrusters");

        GarageSaleCard = RegisterCard(registry, typeof(GarageSale), cardArtDefault, "Garage Sale");

        ShortTermSolutionCard = RegisterCard(registry, typeof(ShortTermSolution), cardArtDefault, "Short-Term Solution");

        AmplifyCard = RegisterCard(registry, typeof(Amplify), cardArtDefault, "Amplify");

        // OrganizeCard = new ExternalCard("Eddie.Cards.Organize", typeof(Organize), cardArtDefault, EddieDeck);
        // registry.RegisterCard(OrganizeCard);
        // OrganizeCard.AddLocalisation("Organize");

        InnovationCard = RegisterCard(registry, typeof(Innovation), cardArtDefault, "Innovation");

        CircuitCard = RegisterCard(registry, typeof(Circuit), CircuitCardArt, "Circuit");

        JumpstartCard = RegisterCard(registry, typeof(Jumpstart), cardArtDefault, "Jump-Start");

        RenewableResourceCard = RegisterCard(registry, typeof(RenewableResource), cardArtDefault, "Renewable Resource");

        GammaRayCard = RegisterCard(registry, typeof(GammaRay), GammaRayCardArt, "Gamma Ray");


        SurgeCard = RegisterCard(registry, typeof(Surge), cardArtDefault, "Surge");

        if (DuoArtifactsApi != null)
            LightweightCard = RegisterCard(registry, typeof(Lightweight), ExternalSprite.GetRaw((int)Enum.Parse<Spr>("cards_Fleetfoot")), "Lightweight", DuoArtifactsApi.DuoArtifactDeck);
    }

    void ICharacterManifest.LoadManifest(ICharacterRegistry registry)
    {
        EddieCharacter = new ExternalCharacter("Eddie.Character.Eddie",
            EddieDeck ?? throw new Exception("Missing Deck"),
            EddiePanelFrame ?? throw new Exception("Missing Potrait"),
            new Type[] { typeof(Channel), typeof(PowerNap) },
            new Type[0],
            EddieDefaultAnimation ?? throw new Exception("missing default animation"),
            EddieMiniAnimation ?? throw new Exception("missing mini animation"));

        EddieCharacter.AddNameLocalisation("Eddie");

        EddieCharacter.AddDescLocalisation("<c=e6e164>EDDIE</c>\nYour ship's electrician. Their cards offer <c=keyword>flexible</c> ways to <c=keyword>spend, store and gain</c> <c=energy>energy</c>.");

        registry.RegisterCharacter(EddieCharacter);
    }

    void IAnimationManifest.LoadManifest(IAnimationRegistry registry)
    {
        EddieDefaultAnimation = new ExternalAnimation("Eddie.Animation.EddieDefault",
            EddieDeck ?? throw new Exception("missing deck"),
            "neutral", false,
            new ExternalSprite[] { EddiePortrait ?? throw new Exception("missing potrait") });

        registry.RegisterAnimation(EddieDefaultAnimation);

        EddieMiniAnimation = new ExternalAnimation("Eddie.Animation.EddieMini",
            EddieDeck ?? throw new Exception("missing deck"),
            "mini", false,
            new ExternalSprite[] { EddieMini ?? throw new Exception("missing mini") });

        registry.RegisterAnimation(EddieMiniAnimation);

        EddieGameoverAnimation = new ExternalAnimation("Eddie.Animation.GameOver", EddieDeck, "gameover", false, new ExternalSprite[] { EddiePortrait! });
        EddieSquintAnimation = new ExternalAnimation("Eddie.Animation.Squint", EddieDeck, "squint", false, new ExternalSprite[] { EddiePortrait! });
        registry.RegisterAnimation(EddieGameoverAnimation);
    }

    public void BootMod(IModLoaderContact contact) {

        Instance = this;
        DuoArtifactsApi = contact.LoadedManifests.Any(m => m.Name == "Shockah.DuoArtifacts") ? contact.GetApi<IDuoArtifactsApi>("Shockah.DuoArtifacts") : null;
        SogginsApi = contact.LoadedManifests.Any(m => m.Name == "Shockah.Soggins") ? contact.GetApi<ISogginsApi>("Shockah.Soggins") : null;
        
        Harmony harmony = new("TheJazMaster.Eddie");
        
        harmony.TryPatch(
            logger: Logger!,
            original: typeof(Card).GetMethod("RenderAction"),
            transpiler: new HarmonyMethod(typeof(RenderingPatch).GetMethod("XEqualsPatch", BindingFlags.Static | BindingFlags.NonPublic))
        );
        harmony.TryPatch(
            logger: Logger!,
            original: typeof(Card).GetMethod("RenderAction"),
            transpiler: new HarmonyMethod(typeof(RenderingPatch).GetMethod("DiscountHandPatch", BindingFlags.Static | BindingFlags.NonPublic))
        );

        harmony.TryPatch(
            logger: Logger!,
            original: typeof(Combat).GetMethod("TryPlayCard"),
            transpiler: new HarmonyMethod(typeof(ShortCircuit).GetMethod("ShortCircuitDiscardMaybe", BindingFlags.Static | BindingFlags.NonPublic))
        );

        harmony.TryPatch(
            logger: Logger!,
            original: typeof(Combat).GetMethod("TryPlayCard"),
            transpiler: new HarmonyMethod(typeof(Cheap).GetMethod("RemoveFreeForTurn", BindingFlags.Static | BindingFlags.NonPublic))
        );
        harmony.TryPatch(
            logger: Logger!,
            original: typeof(Ship).GetMethod("OnAfterTurn"),
            postfix: new HarmonyMethod(typeof(Cheap).GetMethod("ResetFreeForTurn", BindingFlags.Static | BindingFlags.NonPublic))
        );
        harmony.TryPatch(
            logger: Logger!,
            original: typeof(Card).GetMethod("Render"),
            transpiler: new HarmonyMethod(typeof(Cheap).GetMethod("RenderFreeIcon", BindingFlags.Static | BindingFlags.NonPublic))
        );

        harmony.TryPatch(
            logger: Logger!,
            original: typeof(Combat).GetMethod("ReturnCardsToDeck"),
            postfix: new HarmonyMethod(typeof(ShortCircuit).GetMethod("ShortCircuitRemoveOverride", BindingFlags.Static | BindingFlags.NonPublic))
        );
        harmony.TryPatch(
            logger: Logger!,
            original: typeof(Card).GetMethod("GetDataWithOverrides"),
            postfix: new HarmonyMethod(typeof(InfiniteOverride).GetMethod("OverrideInfinite", BindingFlags.Static | BindingFlags.NonPublic))
        );
        harmony.TryPatch(
            logger: Logger!,
            original: typeof(Combat).GetMethod("ReturnCardsToDeck"),
            postfix: new HarmonyMethod(typeof(InfiniteOverride).GetMethod("InfiniteRemoveOverride", BindingFlags.Static | BindingFlags.NonPublic))
        );
        
        harmony.TryPatch(
            logger: Logger!,
            original: typeof(Combat).GetMethod("Make", BindingFlags.Static | BindingFlags.Public),
            postfix: new HarmonyMethod(typeof(Cheap).GetMethod("SetCheapDiscount", BindingFlags.Static | BindingFlags.NonPublic))
        );
        
        harmony.TryPatch(
            logger: Logger!,
            original: typeof(Card).GetMethod("GetDataWithOverrides"),
            postfix: new HarmonyMethod(typeof(Cheap).GetMethod("SetFree", BindingFlags.Static | BindingFlags.NonPublic))
        );
        
        harmony.TryPatch(
            logger: Logger!,
            original: typeof(Combat).GetMethod("ReturnCardsToDeck"),
            postfix: new HarmonyMethod(typeof(Cheap).GetMethod("RemoveFree", BindingFlags.Static | BindingFlags.NonPublic))
        );
        harmony.TryPatch(
            logger: Logger!,
            original: typeof(Colors).GetMethod("LookupColor", BindingFlags.Static | BindingFlags.Public),
            prefix: new HarmonyMethod(typeof(EddieColor).GetMethod("LookupColor", BindingFlags.Static | BindingFlags.NonPublic))
        );
        harmony.TryPatch(
            logger: Logger!,
            original: typeof(Card).GetMethod("GetAllTooltips", BindingFlags.Instance | BindingFlags.Public),
            transpiler: new HarmonyMethod(typeof(Manifest).GetMethod("TraitTooltips", BindingFlags.Static | BindingFlags.NonPublic))
        );
        harmony.TryPatch(
            logger: Logger!,
            original: typeof(Card).GetMethod("Render", BindingFlags.Instance | BindingFlags.Public),
            transpiler: new HarmonyMethod(typeof(Manifest).GetMethod("TraitIcons", BindingFlags.Static | BindingFlags.NonPublic))
        );
        harmony.TryPatch(
            logger: Logger!,
			original: typeof(DB).GetMethod("SetLocale", BindingFlags.Public | BindingFlags.Static),
			postfix: new HarmonyMethod(typeof(Manifest).GetMethod("DB_SetLocale_Postfix", BindingFlags.Static | BindingFlags.NonPublic))
		);
    }

    private static IEnumerable<CodeInstruction> TraitTooltips(IEnumerable<CodeInstruction> iseq, ILGenerator il, MethodBase originalMethod)
    {
        bool worked = false;
        int? local_index = null;
        foreach (LocalVariableInfo info in originalMethod.GetMethodBody()!.LocalVariables)
        {
            if (info.LocalType == typeof(CardData))
            {
                local_index = info.LocalIndex;
                break;
            }
        }

        var newLocal = il.DeclareLocal(typeof(CheapCard));

        using IEnumerator<CodeInstruction> iter = iseq.GetEnumerator();
        while(iter.MoveNext()) {
            yield return iter.Current;
            if(!TranspilerUtils.IsLocalLoad(iter.Current) || TranspilerUtils.ExtractLocalIndex(iter.Current) != local_index) {
                continue;
            }
            var card_data_local_load_opcode = iter.Current.opcode;
            var card_data_local_operand = iter.Current.operand;

            if(!iter.MoveNext()) {
                break;
            }
            yield return iter.Current;

            if(iter.Current.opcode != OpCodes.Ldfld || ((FieldInfo) iter.Current.operand).Name != "floppable") {
                continue;
            }

            if(!iter.MoveNext()) {
                break;
            }
            yield return iter.Current;

            if(iter.Current.opcode != OpCodes.Brfalse_S) {
                continue;
            }
            var latestLabel = (Label) iter.Current.operand;

            if(!iter.MoveNext()) {
                break;
            }
            yield return iter.Current;

            if(!TranspilerUtils.IsLocalLoad(iter.Current)) {
                continue;
            }
            var list_local_load_opcode = iter.Current.opcode;
            var list_local_operand = iter.Current.operand;

            List<CodeInstruction> candidates = new List<CodeInstruction>();
            while (iter.MoveNext() && !(iter.Current.labels is List<Label> list && list.Contains(latestLabel))) {
                candidates.Add(iter.Current);
                yield return iter.Current;
            }

            if (!(iter.Current.labels is List<Label> && (iter.Current.labels as List<Label>).Contains(latestLabel)))
                break;
            

            var midLabel = il.DefineLabel();
            var endLabel = il.DefineLabel();
            {
                yield return new CodeInstruction(OpCodes.Ldarg_0).WithLabels(latestLabel);
                yield return new CodeInstruction(OpCodes.Ldc_I4_1);
                yield return new CodeInstruction(OpCodes.Call, typeof(ShortCircuit).GetMethod("DoesShortCircuit", BindingFlags.Static | BindingFlags.Public));
                yield return new CodeInstruction(OpCodes.Brfalse, midLabel);

                yield return new CodeInstruction(list_local_load_opcode, list_local_operand);
                yield return new CodeInstruction(OpCodes.Call, typeof(Manifest).GetProperty("ShortCircuitGlossary", BindingFlags.Static | BindingFlags.Public)!.GetMethod);
                yield return new CodeInstruction(OpCodes.Call, typeof(ExternalGlossary).GetMethod("get_Head", BindingFlags.Instance | BindingFlags.Public));
                yield return new CodeInstruction(candidates[1].opcode, candidates[1].operand); // empty array
                yield return new CodeInstruction(candidates[2].opcode, candidates[2].operand); // new TTGlossary
                yield return new CodeInstruction(candidates[3].opcode, candidates[3].operand); // Add
            }
            {
                yield return new CodeInstruction(OpCodes.Ldarg_0).WithLabels(midLabel);
                yield return new CodeInstruction(OpCodes.Isinst, typeof(CheapCard));
                yield return new CodeInstruction(OpCodes.Stloc, newLocal);
                yield return new CodeInstruction(OpCodes.Ldloc, newLocal);
                yield return new CodeInstruction(OpCodes.Brfalse, endLabel);
                yield return new CodeInstruction(OpCodes.Ldloc, newLocal);
                yield return new CodeInstruction(OpCodes.Callvirt, typeof(CheapCard).GetMethod("GetCheapDiscount", BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly));
                yield return new CodeInstruction(OpCodes.Brfalse, endLabel);

                yield return new CodeInstruction(list_local_load_opcode, list_local_operand);
                yield return new CodeInstruction(OpCodes.Call, typeof(Manifest).GetProperty("CheapGlossary", BindingFlags.Static | BindingFlags.Public)!.GetMethod);
                yield return new CodeInstruction(OpCodes.Call, typeof(ExternalGlossary).GetMethod("get_Head", BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly));
                yield return new CodeInstruction(OpCodes.Ldc_I4_1);
                yield return new CodeInstruction(OpCodes.Newarr, typeof(object));
                yield return new CodeInstruction(OpCodes.Dup);
                yield return new CodeInstruction(OpCodes.Ldc_I4_0);
                yield return new CodeInstruction(OpCodes.Ldloc, newLocal);
                yield return new CodeInstruction(OpCodes.Callvirt, typeof(CheapCard).GetMethod("GetCheapDiscount", BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly));
                yield return new CodeInstruction(OpCodes.Call, typeof(Math).GetMethod("Abs", BindingFlags.Static | BindingFlags.Public, new Type[] {typeof(int)}));
                yield return new CodeInstruction(OpCodes.Box, typeof(int)); // ??
                yield return new CodeInstruction(OpCodes.Stelem_Ref); // set array[0] to value
                yield return new CodeInstruction(candidates[2].opcode, candidates[2].operand); // new TTGlossary
                yield return new CodeInstruction(candidates[3].opcode, candidates[3].operand); // Add
            }

            iter.Current.labels.Remove(latestLabel);
            iter.Current.labels.Add(endLabel);
            yield return iter.Current;
            worked = true;
            break;
        }

        while (iter.MoveNext()) {
            yield return iter.Current;
        }
        if (!worked)
            throw new Exception("TraitIcons transpiler failed to find match");
    }

    [HarmonyDebug]
    private static IEnumerable<CodeInstruction> TraitIcons(IEnumerable<CodeInstruction> iseq, ILGenerator il, MethodBase originalMethod)
    { 
        bool worked = false;
        int? local_index = null;
        foreach (LocalVariableInfo info in originalMethod.GetMethodBody()!.LocalVariables)
        {
            if (info.LocalType == typeof(CardData))
            {
                local_index = info.LocalIndex;
                break;
            }
        }
        var newLocal = il.DeclareLocal(typeof(CheapCard));

        using IEnumerator<CodeInstruction> iter = iseq.GetEnumerator();
        while(iter.MoveNext()) {
            // yield return iter.Current;

            // if(iter.Current.opcode != OpCodes.Call || !(iter.Current.operand is MethodInfo info) || info.Name != "Sprite") {//info != typeof(Draw).GetMethod("Sprite", BindingFlags.Static | BindingFlags.Public, new Type[] {typeof(Spr?), typeof(double), typeof(double), typeof(bool), typeof(bool), typeof(double), typeof(Vec), typeof(Vec), typeof(Vec), typeof(Rect), typeof(Color), typeof(BlendState), typeof(SamplerState), typeof(Effect)})) {
            //     continue;
            // }

            // if(!iter.MoveNext()) {
            //     break;
            // }
            yield return iter.Current;

            if(!TranspilerUtils.IsLocalLoad(iter.Current) || TranspilerUtils.ExtractLocalIndex(iter.Current) != local_index) {
                continue;
            }
            var card_data_local_load_opcode = iter.Current.opcode;
            var card_data_local_operand = iter.Current.operand;

            if(!iter.MoveNext()) {
                break;
            }
            yield return iter.Current;

            if(iter.Current.opcode != OpCodes.Ldfld || ((FieldInfo) iter.Current.operand).Name != "buoyant") {
                continue;
            }

            if(!iter.MoveNext()) {
                break;
            }
            yield return iter.Current;

            if(iter.Current.opcode != OpCodes.Brfalse_S && iter.Current.opcode != OpCodes.Brfalse) {
                continue;
            }
            var latestLabel = (Label) iter.Current.operand;

            if(!iter.MoveNext()) {
                break;
            }
            yield return iter.Current;

            if(iter.Current.opcode != OpCodes.Ldc_I4) {
                continue;
            }

            List<CodeInstruction> candidates = new List<CodeInstruction>();
            while (iter.MoveNext() && !(iter.Current.labels is List<Label> list && list.Contains(latestLabel))) {
                candidates.Add(iter.Current);
                yield return iter.Current;
            }

            if (!(iter.Current.labels is List<Label> && (iter.Current.labels as List<Label>).Contains(latestLabel)))
                break;


            var midLabel = il.DefineLabel();
            var endLabel = il.DefineLabel();
            {
                yield return new CodeInstruction(OpCodes.Ldarg_0).WithLabels(latestLabel);
                
                yield return new CodeInstruction(OpCodes.Ldc_I4_1);
                yield return new CodeInstruction(OpCodes.Call, typeof(ShortCircuit).GetMethod("DoesShortCircuit", BindingFlags.Static | BindingFlags.Public));
                yield return new CodeInstruction(OpCodes.Brfalse, midLabel);

                yield return new CodeInstruction(OpCodes.Call, typeof(Manifest).GetMethod("GetShortCircuitIcon", BindingFlags.Static | BindingFlags.NonPublic));
                foreach (var instr in candidates) {
                    yield return new CodeInstruction(instr.opcode, instr.operand);
                }
            }
            {
                yield return new CodeInstruction(OpCodes.Ldarg_0).WithLabels(midLabel);
                yield return new CodeInstruction(OpCodes.Isinst, typeof(CheapCard));
                yield return new CodeInstruction(OpCodes.Stloc, newLocal);
                yield return new CodeInstruction(OpCodes.Ldloc, newLocal);
                yield return new CodeInstruction(OpCodes.Brfalse, endLabel);
                yield return new CodeInstruction(OpCodes.Ldloc, newLocal);
                yield return new CodeInstruction(OpCodes.Callvirt, typeof(CheapCard).GetMethod("GetCheapDiscount", BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly));
                yield return new CodeInstruction(OpCodes.Brfalse, endLabel);
                
                yield return new CodeInstruction(OpCodes.Call, typeof(Manifest).GetMethod("GetCheapIcon", BindingFlags.Static | BindingFlags.NonPublic));
                foreach (var instr in candidates) {
                    yield return new CodeInstruction(instr.opcode, instr.operand);
                }
            }

            iter.Current.labels.Remove(latestLabel);
            iter.Current.labels.Add(endLabel);
            yield return iter.Current;
            worked = true;
            break;
        }

        while (iter.MoveNext()) {
            yield return iter.Current;
        }
        if (!worked)
            throw new Exception("TraitIcons transpiler failed to find match");
    }

    private static int GetShortCircuitIcon() {
        return (int)(ShortCircuitIcon?.Id ?? throw new Exception("bruh"));
    }

    private static int GetCheapIcon() {
        return (int)(CheapIcon?.Id ?? throw new Exception("bruh"));
    }

    private static void DB_SetLocale_Postfix() {
        DB.currentLocale.strings["showcards.addedShortCircuit"] = "Set cost to 0, added <c=cardtrait>short-circuit</c>!";
    }

}
