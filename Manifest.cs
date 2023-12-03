using CobaltCoreModding.Definitions;
using CobaltCoreModding.Definitions.ExternalItems;
using CobaltCoreModding.Definitions.ModContactPoints;
using CobaltCoreModding.Definitions.ModManifests;
using HarmonyLib;
using Eddie.Cards;
using Microsoft.Extensions.Logging;
using System.Reflection;
using static System.Reflection.BindingFlags;

namespace Eddie
{
    public partial class Manifest : ISpriteManifest, IDeckManifest, IGlossaryManifest, ICardManifest, ICharacterManifest, IAnimationManifest, IModManifest//, ICustomEventManifest, IArtifactManifest
    {

        public IEnumerable<DependencyEntry> Dependencies => Array.Empty<DependencyEntry>();

        public ILogger? Logger { get; set; }

        public static ExternalGlossary? AddShortCircuitGlossary { get; private set; }
        public static ExternalGlossary? ShortCircuitGlossary { get; private set; }
        public static ExternalGlossary? AddInfiniteGlossary { get; private set; }
        public static ExternalGlossary? MakeFreeGlossary { get; private set; }
        public static ExternalGlossary? CheapGlossary { get; private set; }
        public static ExternalGlossary? CircuitGlossary { get; private set; }
        public static ExternalGlossary? ClosedCircuitGlossary { get; private set; }
        public static ExternalGlossary? PowerCellGlossary { get; private set; }
        public static ExternalGlossary? LeftmostCardGlossary { get; private set; }
        public static ExternalGlossary? TemporaryHurtGlossary { get; private set; }
        public static ExternalGlossary? XIsEnergyGlossary { get; private set; }

        public static ExternalSprite? ShortCircuitIcon { get; private set; }
        public static ExternalSprite? CheapIcon { get; private set; }
        public static ExternalSprite? CircuitIcon { get; private set; }
        public static ExternalSprite? ClosedCircuitIcon { get; private set; }
        public static ExternalSprite? LoseEnergyEveryTurnIcon { get; private set; }
        public static ExternalSprite? PowerCellSprite { get; private set; }
        public static ExternalSprite? PowerCellIcon { get; private set; }
        public static ExternalSprite? LeftmostCardIcon { get; private set; }
        public static ExternalSprite? TemporaryHurtIcon { get; private set; }
        public static ExternalSprite? HealNextTurnIcon { get; private set; }
        public static ExternalSprite? EnergyIcon { get; private set; }

        public static ExternalSprite? EddieCardFrame { get; private set; }
        public static ExternalSprite? EddieUncommonCardFrame { get; private set; }
        public static ExternalSprite? EddieRareCardFrame { get; private set; }
        public static ExternalSprite? EddiePanelFrame { get; private set; }
        public static ExternalSprite? FrazzledWiresSprite { get; private set; }
        public static ExternalSprite? SolarLampOnSprite { get; private set; }
        public static ExternalSprite? SolarLampOffSprite { get; private set; }
        public static ExternalSprite? ElectromagneticCoilSprite { get; private set; }
        // public static ExternalSprite? PerpetualMotionEngineSprite { get; private set; }
        public static ExternalSprite? SolarPanelsOnSprite { get; private set; }
        public static ExternalSprite? SolarPanelsOffSprite { get; private set; }

        public static ExternalSprite? FissionChamberSprite { get; private set; }

        public static ExternalCharacter? EddieCharacter { get; private set; }
        public static ExternalDeck? EddieDeck { get; private set; }
        public static ExternalAnimation? EddieDefaultAnimation { get; private set; }
        public static ExternalSprite? EddiePortrait { get; private set; }
        public static ExternalSprite? EddieMini { get; private set; }
        public static ExternalAnimation? EddieMiniAnimation { get; private set; }
        public static ExternalAnimation? EddieGameoverAnimation { get; private set; }
        
        public static ExternalCard? ChannelCard { get; private set; }
        public static ExternalCard? PowerNapCard { get; private set; }
        public static ExternalCard? RepurposeCard { get; private set; }
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

        public static List<ExternalSprite> TalkScaredSprites { get; private set; } = new List<ExternalSprite>();

        public DirectoryInfo? ModRootFolder { get; set; }
        public string Name { get; init; } = "TheJazMaster.Eddie";
        public DirectoryInfo? GameRootFolder { get; set; }


        void ISpriteManifest.LoadManifest(ISpriteRegistry artRegistry)
        {
            if (ModRootFolder == null)
                throw MakeInformativeException(Logger, "Root Folder not set");

            // Artifacts
            {
                var path = Path.Combine(ModRootFolder.FullName, "Sprites", "artifact_icons", Path.GetFileName("decorative_salmon.png"));
                FrazzledWiresSprite = new ExternalSprite("Eddie.FrazzledWires", new FileInfo(path));
                artRegistry.RegisterArt(FrazzledWiresSprite);
            }
            {
                var path = Path.Combine(ModRootFolder.FullName, "Sprites", "artifact_icons", Path.GetFileName("decorative_salmon.png"));
                SolarLampOnSprite = new ExternalSprite("Eddie.SolarLampOn", new FileInfo(path));
                artRegistry.RegisterArt(SolarLampOnSprite);
            }
            {
                var path = Path.Combine(ModRootFolder.FullName, "Sprites", "artifact_icons", Path.GetFileName("decorative_salmon.png"));
                SolarLampOffSprite = new ExternalSprite("Eddie.SolarLampOff", new FileInfo(path));
                artRegistry.RegisterArt(SolarLampOffSprite);
            }
            {
                var path = Path.Combine(ModRootFolder.FullName, "Sprites", "artifact_icons", Path.GetFileName("decorative_salmon.png"));
                SolarPanelsOnSprite = new ExternalSprite("Eddie.SolarPanelsOn", new FileInfo(path));
                artRegistry.RegisterArt(SolarPanelsOnSprite);
            }
            {
                var path = Path.Combine(ModRootFolder.FullName, "Sprites", "artifact_icons", Path.GetFileName("decorative_salmon.png"));
                SolarPanelsOffSprite = new ExternalSprite("Eddie.SolarPanelsOff", new FileInfo(path));
                artRegistry.RegisterArt(SolarPanelsOffSprite);
            }
            {
                var path = Path.Combine(ModRootFolder.FullName, "Sprites", "artifact_icons", Path.GetFileName("lucky_lore.png"));
                ElectromagneticCoilSprite = new ExternalSprite("Eddie.ElectromagneticCoil", new FileInfo(path));
                artRegistry.RegisterArt(ElectromagneticCoilSprite);
            }
            // {
            //     var path = Path.Combine(ModRootFolder.FullName, "Sprites", "artifact_icons", Path.GetFileName("freebies.png"));
            //     PerpetualMotionEngineSprite = new ExternalSprite("Eddie.perpetual_motion_engine", new FileInfo(path));
            //     artRegistry.RegisterArt(PerpetualMotionEngineSprite);
            // }
            {
                var path = Path.Combine(ModRootFolder.FullName, "Sprites", "artifact_icons", Path.GetFileName("quantum_lure_box.png"));
                FissionChamberSprite = new ExternalSprite("Eddie.FissionChamber", new FileInfo(path));
                artRegistry.RegisterArt(FissionChamberSprite);
            }


            // Character sprites
            {
                var path = Path.Combine(ModRootFolder.FullName, "Sprites", Path.GetFileName("JohannaDefault.png"));
                EddiePortrait = new ExternalSprite("Eddie.EddiePotrait", new FileInfo(path));
                artRegistry.RegisterArt(EddiePortrait);
            }
            {
                var path = Path.Combine(ModRootFolder.FullName, "Sprites", Path.GetFileName("JohannaMini.png"));
                EddieMini = new ExternalSprite("Eddie.EddieMini", new FileInfo(path));
                artRegistry.RegisterArt(EddieMini);
            }
            {
                var path = Path.Combine(ModRootFolder.FullName, "Sprites", Path.GetFileName("JoFrame.png"));
                EddiePanelFrame = new ExternalSprite("Eddie.EddiePanelFrame", new FileInfo(path));
                artRegistry.RegisterArt(EddiePanelFrame);
            }


            // Deck sprites
            {
                var path = Path.Combine(ModRootFolder.FullName, "Sprites", Path.GetFileName("JoCardFrame.png"));
                EddieCardFrame = new ExternalSprite("Eddie.EddieCardFrame", new FileInfo(path));
                artRegistry.RegisterArt(EddieCardFrame);
            }
            {
                var path = Path.Combine(ModRootFolder.FullName, "Sprites", Path.GetFileName("JoCardFrameUC.png"));
                EddieUncommonCardFrame = new ExternalSprite("Eddie.EddieUncommonCardFrame", new FileInfo(path));
                artRegistry.RegisterArt(EddieUncommonCardFrame);
            }
            {
                var path = Path.Combine(ModRootFolder.FullName, "Sprites", Path.GetFileName("JoCardFrameRA.png"));
                EddieRareCardFrame = new ExternalSprite("Eddie.EddieRareCardFrame", new FileInfo(path));
                artRegistry.RegisterArt(EddieRareCardFrame);
            }

            // Icons
            {
                var path = Path.Combine(ModRootFolder.FullName, "Sprites", Path.GetFileName("HookIcon.png"));
                LoseEnergyEveryTurnIcon = new ExternalSprite("Eddie.LoseEnergyEveryTurnIcon", new FileInfo(path));
                artRegistry.RegisterArt(LoseEnergyEveryTurnIcon);
            }
            {
                var path = Path.Combine(ModRootFolder.FullName, "Sprites", Path.GetFileName("HookLeftIcon.png"));
                CircuitIcon = new ExternalSprite("Eddie.CircuitIcon", new FileInfo(path));
                artRegistry.RegisterArt(CircuitIcon);
            }
            {
                var path = Path.Combine(ModRootFolder.FullName, "Sprites", Path.GetFileName("HookRightIcon.png"));
                ClosedCircuitIcon = new ExternalSprite("Eddie.ClosedCircuitIcon", new FileInfo(path));
                artRegistry.RegisterArt(ClosedCircuitIcon);
            }
            {
                var path = Path.Combine(ModRootFolder.FullName, "Sprites/action_icons", Path.GetFileName("scm.png"));
                ShortCircuitIcon = new ExternalSprite("Eddie.ShortCircuitIcon", new FileInfo(path));
                artRegistry.RegisterArt(ShortCircuitIcon);
            }
            {
                var path = Path.Combine(ModRootFolder.FullName, "Sprites/action_icons", Path.GetFileName("scm.png"));
                CheapIcon = new ExternalSprite("Eddie.CheapIcon", new FileInfo(path));
                artRegistry.RegisterArt(CheapIcon);
            }
            {
                var path = Path.Combine(ModRootFolder.FullName, "Sprites/action_icons", Path.GetFileName("hcm.png"));
                LeftmostCardIcon = new ExternalSprite("Eddie.LeftmostCardIcon", new FileInfo(path));
                artRegistry.RegisterArt(LeftmostCardIcon);
            }
            {
                var path = Path.Combine(ModRootFolder.FullName, "Sprites/action_icons", Path.GetFileName("scm.png"));
                TemporaryHurtIcon = new ExternalSprite("Eddie.TemporaryHurtIcon", new FileInfo(path));
                artRegistry.RegisterArt(TemporaryHurtIcon);
            }
            {
                var path = Path.Combine(ModRootFolder.FullName, "Sprites/action_icons", Path.GetFileName("scm.png"));
                HealNextTurnIcon = new ExternalSprite("Eddie.HealNextTurnIcon", new FileInfo(path));
                artRegistry.RegisterArt(HealNextTurnIcon);
            }
            {
                var path = Path.Combine(ModRootFolder.FullName, "Sprites/action_icons", Path.GetFileName("cm.png"));
                PowerCellIcon = new ExternalSprite("Eddie.PowerCellIcon", new FileInfo(path));
                artRegistry.RegisterArt(PowerCellIcon);
            }
            {
                var path = Path.Combine(ModRootFolder.FullName, "Sprites/icons", Path.GetFileName("energy.png"));
                EnergyIcon = new ExternalSprite("Eddie.EnergyIcon", new FileInfo(path));
                artRegistry.RegisterArt(EnergyIcon);
            }

            // Animations
            {
                var dir_path = Path.Combine(ModRootFolder.FullName, "Sprites", "talk_scared");
                var files = Directory.GetFiles(dir_path).Select(e => new FileInfo(e)).ToArray();
                for (int i = 0; i < files.Length; i++)
                {
                    var spr = (new ExternalSprite("Eddie.TalkScared" + i, files[i]));
                    TalkScaredSprites.Add(spr);
                    artRegistry.RegisterArt(spr);
                }
            }
        }

        private static System.Drawing.Color Eddie_PrimaryColor = System.Drawing.Color.FromArgb(230, 225, 100);

        public void LoadManifest(IDeckRegistry registry)
        {
            ExternalSprite cardArtDefault = ExternalSprite.GetRaw((int)Spr.cards_colorless);
            ExternalSprite borderSprite = EddieCardFrame ?? throw MakeInformativeException(Logger);
            EddieDeck = new ExternalDeck(
                "Eddie.EddieDeck",
                Eddie_PrimaryColor,
                System.Drawing.Color.White,
                cardArtDefault,
                borderSprite,
                null);
            registry.RegisterDeck(EddieDeck);
        }

        void IGlossaryManifest.LoadManifest(IGlossaryRegisty registry)
        {
            AddShortCircuitGlossary = new ExternalGlossary("Eddie.Glossary.AddShortCircuitDesc", "EddieAddShortCircuitAction", false, ExternalGlossary.GlossayType.actionMisc, ShortCircuitIcon ?? throw MakeInformativeException(Logger, "Missing Short Circuit Icon"));
            AddShortCircuitGlossary.AddLocalisation("en", "Apply short-circuit", "Make a card short-circuit {0}.", null);
            registry.RegisterGlossary(AddShortCircuitGlossary);

            ShortCircuitGlossary = new ExternalGlossary("Eddie.Glossary.ShortCircuitDesc", "EddieShortCircuitTrait", false, ExternalGlossary.GlossayType.cardtrait, ShortCircuitIcon ?? throw MakeInformativeException(Logger, "Missing Short Circuit Icon"));
            ShortCircuitGlossary.AddLocalisation("en", "Short-Circuit", "This card activates its effects twice, but costs 1 more <c=energy>ENERGY</c> to play.", null);
            registry.RegisterGlossary(ShortCircuitGlossary);

            AddInfiniteGlossary = new ExternalGlossary("Eddie.Glossary.AddInfiniteDesc", "EddieAddInfiniteAction", false, ExternalGlossary.GlossayType.actionMisc, ExternalSprite.GetRaw((int)Enum.Parse<Spr>("icons_infinite")));
            AddInfiniteGlossary.AddLocalisation("en", "Make infinite", "Make a card infinite {0}.", null);
            registry.RegisterGlossary(AddInfiniteGlossary);

            MakeFreeGlossary = new ExternalGlossary("Eddie.Glossary.MakeFreeDesc", "EddieMakeFreeAction", false, ExternalGlossary.GlossayType.actionMisc, ExternalSprite.GetRaw((int)Enum.Parse<Spr>("icons_discount")));
            MakeFreeGlossary.AddLocalisation("en", "Make free", "Make a card cost 0 <c=energy>ENERGY</c> {0}.", null);
            registry.RegisterGlossary(MakeFreeGlossary);

            CheapGlossary = new ExternalGlossary("Eddie.Glossary.CheapDesc", "EddieCheapTrait", false, ExternalGlossary.GlossayType.cardtrait, CheapIcon ?? throw MakeInformativeException(Logger, "Missing Cheap Icon"));
            CheapGlossary.AddLocalisation("en", "Cheap", "This card starts each combat with a {0}-energy <c=cardtrait>discount</c>.", null);
            registry.RegisterGlossary(CheapGlossary);

            PowerCellGlossary = new ExternalGlossary("Eddie.Glossary.PowerCellGlossary", "EddiePowerCellMidrow", false, ExternalGlossary.GlossayType.midrow, PowerCellIcon ?? throw MakeInformativeException(Logger, "Missing Power Cell Icon"));
            PowerCellGlossary.AddLocalisation("en", "Power Cell", "Blocks one attack. Gain 1 energy when this is destroyed.", null);
            registry.RegisterGlossary(PowerCellGlossary);

            LeftmostCardGlossary = new ExternalGlossary("Eddie.Glossary.LeftmostCardGlossary", "EddieLeftmostCardMisc", false, ExternalGlossary.GlossayType.actionMisc, LeftmostCardIcon ?? throw MakeInformativeException(Logger, "Missing Leftmost Card Icon"));
            LeftmostCardGlossary.AddLocalisation("en", "Leftmost Card", "This affects the leftmost card in your hand.", null);
            registry.RegisterGlossary(LeftmostCardGlossary);

            CircuitGlossary = new ExternalGlossary("Eddie.Glossary.CircuitGlossary", "EddieCircuitStatus", false, ExternalGlossary.GlossayType.action, CircuitIcon ?? throw MakeInformativeException(Logger, "Missing Circuit Icon"));
            CircuitGlossary.AddLocalisation("en", "Circuit", "Gain {0} Closed Circuit at the start of your turn.", null);
            registry.RegisterGlossary(CircuitGlossary);

            ClosedCircuitGlossary = new ExternalGlossary("Eddie.Glossary.ClosedCircuitGlossary", "EddieClosedCircuitStatus", false, ExternalGlossary.GlossayType.actionMisc, ClosedCircuitIcon ?? throw MakeInformativeException(Logger, "Missing Closed Circuit Icon"));
            ClosedCircuitGlossary.AddLocalisation("en", "Closed Circuit", "When a card would be discarded, lose 1 Closed Circuit instead.", null);
            registry.RegisterGlossary(ClosedCircuitGlossary);

            TemporaryHurtGlossary = new ExternalGlossary("Eddie.Glossary.TemporaryHurtGlossary", "EddieTemporaryHurtAction", false, ExternalGlossary.GlossayType.actionMisc, TemporaryHurtIcon ?? throw MakeInformativeException(Logger, "Missing Temporary Hurt Icon"));
            TemporaryHurtGlossary.AddLocalisation("en", "Temporary Hull Loss", "Lose {0} hull. Regain that hull at the start of next turn.", null);
            registry.RegisterGlossary(TemporaryHurtGlossary);

            XIsEnergyGlossary = new ExternalGlossary("Eddie.Glossary.XIsEnergyGlossary", "EddieXIsEnergyAction", false, ExternalGlossary.GlossayType.actionMisc, EnergyIcon ?? throw MakeInformativeException(Logger, "Missing Energy Icon"));
            XIsEnergyGlossary.AddLocalisation("en", "", "<c=action>X</c> = Your <c=status>ENERGY</c> after paying the card's cost{0}.", null);
            registry.RegisterGlossary(XIsEnergyGlossary);
        }

        void ICardManifest.LoadManifest(ICardRegistry registry)
        {
            var card_art = ExternalSprite.GetRaw((int)Spr.cards_colorless);

            //  var mass_upgrade_art = ExternalSprite.GetRaw((int)Spr.adap);

            ChannelCard = new ExternalCard("Eddie.Cards.Channel", typeof(Channel), card_art, EddieDeck);
            registry.RegisterCard(ChannelCard);
            ChannelCard.AddLocalisation("Channel");

            RepurposeCard = new ExternalCard("Eddie.Cards.Repurpose", typeof(Repurpose), card_art, null);
            registry.RegisterCard(RepurposeCard);
            RepurposeCard.AddLocalisation("Repurpose");

            PowerNapCard = new ExternalCard("Eddie.Cards.PowerNap", typeof(PowerNap), card_art, EddieDeck);
            registry.RegisterCard(PowerNapCard);
            PowerNapCard.AddLocalisation("Power Nap");

            PowerCellCard = new ExternalCard("Eddie.Cards.PowerCell", typeof(PowerCell), card_art, EddieDeck);
            registry.RegisterCard(PowerCellCard);
            PowerCellCard.AddLocalisation("Power Cell");

            RefundShotCard = new ExternalCard("Eddie.Cards.RefundShot", typeof(RefundShot), card_art, EddieDeck);
            registry.RegisterCard(RefundShotCard);
            RefundShotCard.AddLocalisation("Refund Shot");

            SolarSailingCard = new ExternalCard("Eddie.Cards.SolarSailing", typeof(SolarSailing), card_art, EddieDeck);
            registry.RegisterCard(SolarSailingCard);
            SolarSailingCard.AddLocalisation("Solar Sailing");

            EnergyBoltCard = new ExternalCard("Eddie.Cards.EnergyBolt", typeof(EnergyBolt), card_art, EddieDeck);
            registry.RegisterCard(EnergyBoltCard);
            EnergyBoltCard.AddLocalisation("Energy Bolt");

            PowerSinkCard = new ExternalCard("Eddie.Cards.PowerSink", typeof(PowerSink), card_art, EddieDeck);
            registry.RegisterCard(PowerSinkCard);
            PowerSinkCard.AddLocalisation("Power Sink");

            RummageCard = new ExternalCard("Eddie.Cards.Rummage", typeof(Rummage), card_art, EddieDeck);
            registry.RegisterCard(RummageCard);
            RummageCard.AddLocalisation("Rummage");

            BorrowCard = new ExternalCard("Eddie.Cards.Borrow", typeof(Borrow), card_art, EddieDeck);
            registry.RegisterCard(BorrowCard);
            BorrowCard.AddLocalisation("Borrow");

            InterferenceCard = new ExternalCard("Eddie.Cards.Interference", typeof(Interference), card_art, EddieDeck);
            registry.RegisterCard(InterferenceCard);
            InterferenceCard.AddLocalisation("Interference");

            ChargeCannonsCard = new ExternalCard("Eddie.Cards.ChargeCannons", typeof(ChargeCannons), card_art, EddieDeck);
            registry.RegisterCard(ChargeCannonsCard);
            ChargeCannonsCard.AddLocalisation("Charge Cannons");

            ChargeShieldsCard = new ExternalCard("Eddie.Cards.ChargeShields", typeof(ChargeShields), card_art, EddieDeck);
            registry.RegisterCard(ChargeShieldsCard);
            ChargeShieldsCard.AddLocalisation("Charge Shields");

            ChargeThrustersCard = new ExternalCard("Eddie.Cards.ChargeThrusters", typeof(ChargeThrusters), card_art, EddieDeck);
            registry.RegisterCard(ChargeThrustersCard);
            ChargeThrustersCard.AddLocalisation("Charge Thrusters");

            GarageSaleCard = new ExternalCard("Eddie.Cards.GarageSale", typeof(GarageSale), card_art, EddieDeck);
            registry.RegisterCard(GarageSaleCard);
            GarageSaleCard.AddLocalisation("Garage Sale");

            ShortTermSolutionCard = new ExternalCard("Eddie.Cards.ShortTermSolution", typeof(ShortTermSolution), card_art, EddieDeck);
            registry.RegisterCard(ShortTermSolutionCard);
            ShortTermSolutionCard.AddLocalisation("Short-Term Solution");

            AmplifyCard = new ExternalCard("Eddie.Cards.Amplify", typeof(Amplify), card_art, EddieDeck);
            registry.RegisterCard(AmplifyCard);
            AmplifyCard.AddLocalisation("Amplify");

            // OrganizeCard = new ExternalCard("Eddie.Cards.Organize", typeof(Organize), card_art, EddieDeck);
            // registry.RegisterCard(OrganizeCard);
            // OrganizeCard.AddLocalisation("Organize");

            InnovationCard = new ExternalCard("Eddie.Cards.Innovation", typeof(Innovation), card_art, EddieDeck);
            registry.RegisterCard(InnovationCard);
            InnovationCard.AddLocalisation("Innovation");

            CircuitCard = new ExternalCard("Eddie.Cards.Circuit", typeof(Circuit), card_art, EddieDeck);
            registry.RegisterCard(CircuitCard);
            CircuitCard.AddLocalisation("Circuit");

            JumpstartCard = new ExternalCard("Eddie.Cards.Jumpstart", typeof(Jumpstart), card_art, EddieDeck);
            registry.RegisterCard(JumpstartCard);
            JumpstartCard.AddLocalisation("Jump-Start");

            RenewableResourceCard = new ExternalCard("Eddie.Cards.RenewableResource", typeof(RenewableResource), card_art, EddieDeck);
            registry.RegisterCard(RenewableResourceCard);
            RenewableResourceCard.AddLocalisation("Renewable Resource");

            GammaRayCard = new ExternalCard("Eddie.Cards.GammaRay", typeof(GammaRay), card_art, EddieDeck);
            registry.RegisterCard(GammaRayCard);
            GammaRayCard.AddLocalisation("Gamma Ray");
        }

        void ICharacterManifest.LoadManifest(ICharacterRegistry registry)
        {
            EddieCharacter = new ExternalCharacter("Eddie.Character.Eddie",
                EddieDeck ?? throw MakeInformativeException(Logger, "Missing Deck"),
                EddiePanelFrame ?? throw MakeInformativeException(Logger, "Missing Potrait"),
                new Type[] { typeof(Channel), typeof(PowerNap) },
                new Type[0],
                EddieDefaultAnimation ?? throw MakeInformativeException(Logger, "missing default animation"),
                EddieMiniAnimation ?? throw MakeInformativeException(Logger, "missing mini animation"));

            EddieCharacter.AddNameLocalisation("Eddie");

            EddieCharacter.AddDescLocalisation("<c=e6e164>EDDIE</c>\nYour electrician. His cards offer flexible ways to spend, store and gain energy.");

            registry.RegisterCharacter(EddieCharacter);
        }

        void IAnimationManifest.LoadManifest(IAnimationRegistry registry)
        {
            EddieDefaultAnimation = new ExternalAnimation("Eddie.Animation.EddieDefault",
                EddieDeck ?? throw MakeInformativeException(Logger, "missing deck"),
                "neutral", false,
                new ExternalSprite[] { EddiePortrait ?? throw MakeInformativeException(Logger, "missing potrait") });

            registry.RegisterAnimation(EddieDefaultAnimation);

            EddieMiniAnimation = new ExternalAnimation("Eddie.Animation.EddieMini",
               EddieDeck ?? throw MakeInformativeException(Logger, "missing deck"),
               "mini", false,
               new ExternalSprite[] { EddieMini ?? throw MakeInformativeException(Logger, "missing mini") });

            registry.RegisterAnimation(EddieMiniAnimation);

            EddieGameoverAnimation = new ExternalAnimation("Eddie.Animation.GameOver", EddieDeck, "gameover", false, TalkScaredSprites);
            registry.RegisterAnimation(EddieGameoverAnimation);
        }

        public void BootMod(IModLoaderContact contact) {
            Harmony harmony = new("Eddie");
            {
                var render_action_method = typeof(Card).GetMethod("RenderAction") ?? throw MakeInformativeException(Logger, "Couldn't find Card.RenderAction method");
                var patch = typeof(RenderingPatch).GetMethod("XEqualsPatch", BindingFlags.Static | BindingFlags.NonPublic) ?? throw MakeInformativeException(Logger, "Couldnt find RenderingPatch.MidRenderAssign method");
                harmony.Patch(render_action_method, transpiler: new HarmonyMethod(patch));
            }
            {
                var card_get_data_with_overrides_method = typeof(Card).GetMethod("GetDataWithOverrides") ?? throw MakeInformativeException(Logger, "Couldn't find Card.GetDataWithOverrides method");
                var patch = typeof(ShortCircuit).GetMethod("ShortCircuitIncreaseCost", BindingFlags.Static | BindingFlags.NonPublic) ?? throw MakeInformativeException(Logger, "Couldnt find ShortCircuit.ShortCircuitIncreaseCost method");
                harmony.Patch(card_get_data_with_overrides_method, postfix: new HarmonyMethod(patch));
            }
            {
                var combat_try_play_card_method = typeof(Combat).GetMethod("TryPlayCard") ?? throw MakeInformativeException(Logger, "Couldn't find Combat.TryPlayCard method");
                var patch = typeof(ShortCircuit).GetMethod("ShortCircuitPlayTwice", BindingFlags.Static | BindingFlags.NonPublic) ?? throw MakeInformativeException(Logger, "Couldnt find ShortCircuit.ShortCircuitPlayTwice method");
                harmony.Patch(combat_try_play_card_method, transpiler: new HarmonyMethod(patch));
            }
            {
                var combat_return_cards_to_deck_method = typeof(Combat).GetMethod("ReturnCardsToDeck") ?? throw MakeInformativeException(Logger, "Couldn't find Combat.ReturnCardsToDeck method");
                var patch = typeof(ShortCircuit).GetMethod("ShortCircuitRemoveOverride", BindingFlags.Static | BindingFlags.NonPublic) ?? throw MakeInformativeException(Logger, "Couldnt find ShortCircuit.ShortCircuitRemoveOverride method");
                harmony.Patch(combat_return_cards_to_deck_method, postfix: new HarmonyMethod(patch));
            }
            {
                var card_get_data_with_overrides_method = typeof(Card).GetMethod("GetDataWithOverrides") ?? throw MakeInformativeException(Logger, "Couldn't find Card.GetDataWithOverrides method");
                var patch = typeof(InfiniteOverride).GetMethod("OverrideInfinite", BindingFlags.Static | BindingFlags.NonPublic) ?? throw MakeInformativeException(Logger, "Couldnt find InfiniteOverride.InfiniteOverride method");
                harmony.Patch(card_get_data_with_overrides_method, postfix: new HarmonyMethod(patch));
            }
            {
                var combat_return_cards_to_deck_method = typeof(Combat).GetMethod("ReturnCardsToDeck") ?? throw MakeInformativeException(Logger, "Couldn't find Combat.ReturnCardsToDeck method");
                var patch = typeof(InfiniteOverride).GetMethod("InfiniteRemoveOverride", BindingFlags.Static | BindingFlags.NonPublic) ?? throw MakeInformativeException(Logger, "Couldnt find InfiniteOverride.InfiniteRemoveOverride method");
                harmony.Patch(combat_return_cards_to_deck_method, postfix: new HarmonyMethod(patch));
            }
            {
                var combat_make_method = typeof(Combat).GetMethod("Make", BindingFlags.Static | BindingFlags.Public) ?? throw MakeInformativeException(Logger, "Couldn't find Combat.Make method");
                var patch = typeof(Cheap).GetMethod("SetCheapDiscount", BindingFlags.Static | BindingFlags.NonPublic) ?? throw MakeInformativeException(Logger, "Couldnt find Cheap.SetCheapDiscount method");
                harmony.Patch(combat_make_method, postfix: new HarmonyMethod(patch));
            }
            {
                var card_get_data_with_overrides_method = typeof(Card).GetMethod("GetDataWithOverrides") ?? throw MakeInformativeException(Logger, "Couldn't find Card.GetDataWithOverrides method");
                var patch = typeof(Cheap).GetMethod("SetFree", BindingFlags.Static | BindingFlags.NonPublic) ?? throw MakeInformativeException(Logger, "Couldnt find Cheap.SetFree method");
                harmony.Patch(card_get_data_with_overrides_method, postfix: new HarmonyMethod(patch));
            }
            {
                var combat_return_cards_to_deck_method = typeof(Combat).GetMethod("ReturnCardsToDeck") ?? throw MakeInformativeException(Logger, "Couldn't find Combat.ReturnCardsToDeck method");
                var patch = typeof(Cheap).GetMethod("RemoveFree", BindingFlags.Static | BindingFlags.NonPublic) ?? throw MakeInformativeException(Logger, "Couldnt find Cheap.RemoveFree method");
                harmony.Patch(combat_return_cards_to_deck_method, postfix: new HarmonyMethod(patch));
            }
            // {
            //     var combat_player_won_method = typeof(Combat).GetMethod("PlayerWon", BindingFlags.NonPublic | BindingFlags.Instance) ?? throw MakeInformativeException(Logger, "Couldn't find Combat.PlayerWon method");
            //     var patch = typeof(Cheap).GetMethod("SetDefaultDiscount", BindingFlags.Static | BindingFlags.NonPublic) ?? throw MakeInformativeException(Logger, "Couldnt find Cheap.SetDefaultDiscount method");
            //     harmony.Patch(combat_player_won_method, transpiler: new HarmonyMethod(patch));
            // }
            // {
            //     var patch = typeof(RenderingPatch).GetMethod("EqualsXPatch", BindingFlags.Static | BindingFlags.NonPublic) ?? throw MakeInformativeException(Logger, "Couldnt find RenderingPatch.MidRenderAssign method");
            //     harmony.Patch(render_action_method, transpiler: new HarmonyMethod(patch));
            // }
        }

        // public void BootMod(IModLoaderContact contact)
        // {
        //     {
        //         //create action draw code for agrow cluster
        //         var harmony = new Harmony("EWanderer.Eddie.AGrowClusterRendering");

        //         var card_render_action_method = typeof(Card).GetMethod("RenderAction", BindingFlags.Public | BindingFlags.Static) ?? throw MakeInformativeException(Logger, );

        //         var card_render_action_prefix = this.GetType().GetMethod("AGrowClusterRenderActionPrefix", BindingFlags.NonPublic | BindingFlags.Static);

        //         harmony.Patch(card_render_action_method, prefix: new HarmonyMethod(card_render_action_prefix));

        //     }
        // }


        public Exception MakeInformativeException(ILogger? Logger, string message = "")
        {
            if (Logger != null)
                Logger.LogInformation(message);
            return new Exception(message);
        }

    }
}
