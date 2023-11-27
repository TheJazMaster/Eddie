using CobaltCoreModding.Definitions;
using CobaltCoreModding.Definitions.ExternalItems;
using CobaltCoreModding.Definitions.ModContactPoints;
using CobaltCoreModding.Definitions.ModManifests;
using HarmonyLib;
using Eddie.Actions;
using Eddie.Cards;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Eddie
{
    public partial class Manifest : ISpriteManifest, IDeckManifest, IGlossaryManifest, ICardManifest, ICharacterManifest, IAnimationManifest, IStatusManifest, ICustomEventManifest, IArtifactManifest, IShipPartManifest, IShipManifest, IStartershipManifest, IModManifest
    {

        public IEnumerable<DependencyEntry> Dependencies => Array.Empty<DependencyEntry>();

        public ILogger? Logger { get; set; }

        public static ExternalGlossary? ShortCircuitGlossary { get; private set; }
        public static ExternalGlossary? CircuitGlossary { get; private set; }
        public static ExternalGlossary? ClosedCircuitGlossary { get; private set; }
        public static ExternalGlossary? PowerCellGlossary { get; private set; }

        public static ExternalGlossary? LeftmostCardGlossary { get; private set; }

        public static ExternalSprite? ShortCircuitIcon { get; private set; }
        public static ExternalSprite? CircuitIcon { get; private set; }
        public static ExternalSprite? ClosedCircuitIcon { get; private set; }
        public static ExternalSprite? PowerCellSprite { get; private set; }
        public static ExternalSprite? PowerCellIcon { get; private set; }
        public static ExternalSprite? LeftmostCardIcon { get; private set; }

        public static ExternalSprite? EddieCardFrame { get; private set; }
        public static ExternalSprite? EddieUncommonCardFrame { get; private set; }
        public static ExternalSprite? EddieRareCardFrame { get; private set; }
        public static ExternalSprite? EddiePanelFrame { get; private set; }
        public static ExternalSprite? FrazzledWiresSprite { get; private set; }
        public static ExternalSprite? SolarLampSprite { get; private set; }
        public static ExternalSprite? RoboVacuumSprite { get; private set; }
        public static ExternalSprite? PerpetualMotionEngineSprite { get; private set; }
        public static ExternalSprite? FissionChamberSprite { get; private set; }

        public static ExternalCharacter? EddieCharacter { get; private set; }
        public static ExternalDeck? EddieDeck { get; private set; }
        public static ExternalAnimation? EddieDefaultAnimation { get; private set; }
        public static ExternalSprite? EddiePortrait { get; private set; }
        public static ExternalSprite? EddieMini { get; private set; }
        public static ExternalAnimation? EddieMiniAnimation { get; private set; }
        public static ExternalAnimation? EddieGameoverAnimation { get; private set; }
        
        public static ExternalCard? ChannelCard { get; private set; }
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
        public static ExternalCard? GarageSaleCard { get; private set; }
        public static ExternalCard? UnsustainableAssaultCard { get; private set; }
        public static ExternalCard? AmplifyCard { get; private set; }
        public static ExternalCard? OrganizeCard { get; private set; }
        public static ExternalCard? InnovationCard { get; private set; }
        public static ExternalCard? CircuitCard { get; private set; }
        public static ExternalCard? RenewableResourceCard { get; private set; }
        public static ExternalCard? GammaRayCard { get; private set; }
        public static ExternalCard? JumpstartCard { get; private set; }

        public DirectoryInfo? ModRootFolder { get; set; }
        public string Name { get; init; } = "TheJazMaster.Eddie";
        public DirectoryInfo? GameRootFolder { get; set; }


        void ISpriteManifest.LoadManifest(ISpriteRegistry artRegistry)
        {
            if (ModRootFolder == null)
                throw new Exception("Root Folder not set");

            //artifacts

            {
                var path = Path.Combine(ModRootFolder.FullName, "Sprites", "artifact_icons", Path.GetFileName("decorative_salmon.png"));
                FrazzledWiresSprite = new ExternalSprite("Eddie.frazzled_wires", new FileInfo(path));
                artRegistry.RegisterArt(DecorativeSalmonSprite);
            }

            {
                var path = Path.Combine(ModRootFolder.FullName, "Sprites", "artifact_icons", Path.GetFileName("intertial_engine.png"));
                SolarLampSprite = new ExternalSprite("Eddie.solar_lamo", new FileInfo(path));
                artRegistry.RegisterArt(InertialEngineSprite);
            }

            {
                var path = Path.Combine(ModRootFolder.FullName, "Sprites", "artifact_icons", Path.GetFileName("lucky_lore.png"));
                RoboVacuumSprite = new ExternalSprite("Eddie.robo_vacuum", new FileInfo(path));
                artRegistry.RegisterArt(LuckyLureSprite);
            }

            {
                var path = Path.Combine(ModRootFolder.FullName, "Sprites", "artifact_icons", Path.GetFileName("freebies.png"));
                PerpetualMotionEngineSprite = new ExternalSprite("Eddie.perpetual_motion_engine", new FileInfo(path));
                artRegistry.RegisterArt(FreebiesSprite);
            }

            {
                var path = Path.Combine(ModRootFolder.FullName, "Sprites", "artifact_icons", Path.GetFileName("quantum_lure_box.png"));
                FissionChamberSprite = new ExternalSprite("Eddie.fission_chamber", new FileInfo(path));
                artRegistry.RegisterArt(QuantumLureBoxSprite);
            }


            //load the character sprite

            {
                var path = Path.Combine(ModRootFolder.FullName, "Sprites", Path.GetFileName("JohannaDefault.png"));
                EddiePortrait = new ExternalSprite("Eddie.EddiePotrait", new FileInfo(path));
                artRegistry.RegisterArt(JohannaPotrait);
            }

            {
                var path = Path.Combine(ModRootFolder.FullName, "Sprites", Path.GetFileName("JohannaMini.png"));
                EddieMini = new ExternalSprite("Eddie.EddieMini", new FileInfo(path));
                artRegistry.RegisterArt(JohannaMini);
            }

            {
                var path = Path.Combine(ModRootFolder.FullName, "Sprites", Path.GetFileName("JoFrame.png"));
                EddiePanelFrame = new ExternalSprite("Eddie.EddiePanelFrame", new FileInfo(path));
                artRegistry.RegisterArt(JohannaPanelFrame);
            }


            // deck sprite

            {
                var path = Path.Combine(ModRootFolder.FullName, "Sprites", Path.GetFileName("JoCardFrame.png"));
                EddieCardFrame = new ExternalSprite("Eddie.EddieCardFrame", new FileInfo(path));
                artRegistry.RegisterArt(JohannaCardFrame);
            }

            {
                var path = Path.Combine(ModRootFolder.FullName, "Sprites", Path.GetFileName("JoCardFrameUC.png"));
                JohannaUncommonCardFrame = new ExternalSprite("Eddie.EddieUncommonCardFrame", new FileInfo(path));
                artRegistry.RegisterArt(JohannaUncommonCardFrame);
            }

            {
                var path = Path.Combine(ModRootFolder.FullName, "Sprites", Path.GetFileName("JoCardFrameRA.png"));
                JohannaRareCardFrame = new ExternalSprite("Eddie.EddieRareCardFrame", new FileInfo(path));
                artRegistry.RegisterArt(JohannaRareCardFrame);
            }

        }

        private static System.Drawing.Color Eddie_PrimaryColor = System.Drawing.Color.FromArgb(230, 255, 163);

        public void LoadManifest(IDeckRegistry registry)
        {
            ExternalSprite cardArtDefault = ExternalSprite.GetRaw((int)Spr.cards_colorless);
            ExternalSprite borderSprite = EddieCardFrame ?? throw new Exception();
            EddieDeck = new ExternalDeck(
                "Eddie.EddieDeck",
              Johanna_Primary_Color,
                System.Drawing.Color.White,
                cardArtDefault,
                borderSprite,
                null);
            registry.RegisterDeck(EddieDeck);
        }

        void IGlossaryManifest.LoadManifest(IGlossaryRegisty registry)
        {
            ShortCircuitGlossary = new ExternalGlossary("Eddie.Glossary.ShortCircuitDesc", "EddieShortCircuitTrait", false, ExternalGlossary.GlossayType.cardtrait, ShortCircuitIcon ?? throw new Exception("Missing Short Circuit Icon"));
            ShortCircuitGlossary.AddLocalisation("en", "Short-Circuit", "This card activates its effects twice. When a card gains Short-Circuit, its energy cost increases by 1.", null);
            registry.RegisterGlossary(ShortCircuitGlossary);

            PowerCellGlossary = new ExternalGlossary("Eddie.Glossary.PowerCellGlossary", "EddiePowerCellMidrow", false, ExternalGlossary.GlossayType.midrow, PowerCellIcon ?? throw new Exception("Missing Power Cell Icon"));
            PowerCellGlossary.AddLocalisation("en", "Power Cell", "Blocks one attack. Gain 1 energy when this is destroyed.", null);
            registry.RegisterGlossary(PowerCellGlossary);

            LeftmostCardGlossary = new ExternalGlossary("Eddie.Glossary.LeftmostCardGlossary", "EddieLeftmostCardMisc", false, ExternalGlossary.GlossayType.actionMisc, LeftmostCardIcon ?? throw new Exception("Missing Leftmost Card Icon"));
            LeftmostCardGlossary.AddLocalisation("en", "Leftmost Card", "This affects the leftmost card in your hand.", null);
            registry.RegisterGlossary(LeftmostCardGlossary);

            CircuitGlossary = new ExternalGlossary("Eddie.Glossary.CircuitGlossary", "EddieCircuitStatus", false, ExternalGlossary.GlossayType.action, CircuitIcon ?? throw new Exception("Missing Circuit Icon"));
            ClusterMissileHE_Glossary.AddLocalisation("en", "Circuit", "Gain {0} Closed Circuit at the start of your turn.", null);
            registry.RegisterGlossary(ClusterMissileHE_Glossary);

            ClosedCircuitGlossary = new ExternalGlossary("Eddie.Glossary.ClosedCircuitGlossary", "EddieClosedCircuitStatus", false, ExternalGlossary.GlossayType.actionMisc, SeekerClusterMissleIcon ?? throw new Exception("Missing Closed Circuit Icon"));
            ClosedCircuitGlossary.AddLocalisation("en", "Closed Circuit", "When a card would be discarded, lose 1 Closed Circuit instead.", null);
            registry.RegisterGlossary(ClosedCircuitGlossary);
        }

        void ICardManifest.LoadManifest(ICardRegistry registry)
        {
            var card_art = ExternalSprite.GetRaw((int)Spr.cards_colorless);

            //  var mass_upgrade_art = ExternalSprite.GetRaw((int)Spr.adap);

            ChannelCard = new ExternalCard("Eddie.Cards.Channel", typeof(Channel), card_art, EddieDeck);
            registry.RegisterCard(ChannelCard);
            ReelInCard.AddLocalisation("Channel");

            RepurposeCard = new ExternalCard("Eddie.Cards.Repurpose", typeof(Repurpose), card_art, EddieDeck);
            registry.RegisterCard(RepurposeCard);
            RepurposeCard.AddLocalisation("Repurpose");

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

            GarageSaleCard = new ExternalCard("Eddie.Cards.GarageSale", typeof(GarageSale), card_art, EddieDeck);
            registry.RegisterCard(GarageSaleCard);
            GarageSaleCard.AddLocalisation("Garage Sale");

            UnsustainableAssaultCard = new ExternalCard("Eddie.Cards.UnsustainableAssault", typeof(UnsustainableAssault), readjust_art, EddieDeck);
            registry.RegisterCard(UnsustainableAssaultCard);
            UnsustainableAssaultCard.AddLocalisation("Unsustainable Assault");

            AmplifyCard = new ExternalCard("Eddie.Cards.Amplify", typeof(Amplify), card_art, EddieDeck);
            registry.RegisterCard(AmplifyCard);
            AmplifyCard.AddLocalisation("Amplify");

            OrganizeCard = new ExternalCard("Eddie.Cards.Organize", typeof(Organize), card_art, EddieDeck);
            registry.RegisterCard(OrganizeCard);
            OrganizeCard.AddLocalisation("Organize");

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
                EddieDeck ?? throw new Exception("Missing Deck"),
                EddiePanelFrame ?? throw new Exception("Missing Potrait"),
                new Type[] { typeof(Channel), typeof(Repurpose) },
                new Type[0],
                EddieDefaultAnimation ?? throw new Exception("missing default animation"),
                EddieMiniAnimation ?? throw new Exception("missing mini animation"));

            JohannaCharacter.AddNameLocalisation("Eddie");

            JohannaCharacter.AddDescLocalisation("<c=dizzy>EDDIE</c>\nYour energy specialist. His cards focus on different ways to gain or spend energy.");

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

            EddieGameoverAnimation = new ExternalAnimation("Eddie.Animation.GameOver", EddieDeck, "gameover", false, TalkScaredSprites);
            registry.RegisterAnimation(EddieGameoverAnimation);
        }

        // public void BootMod(IModLoaderContact contact)
        // {
        //     {
        //         //create action draw code for agrow cluster
        //         var harmony = new Harmony("EWanderer.Eddie.AGrowClusterRendering");

        //         var card_render_action_method = typeof(Card).GetMethod("RenderAction", BindingFlags.Public | BindingFlags.Static) ?? throw new Exception();

        //         var card_render_action_prefix = this.GetType().GetMethod("AGrowClusterRenderActionPrefix", BindingFlags.NonPublic | BindingFlags.Static);

        //         harmony.Patch(card_render_action_method, prefix: new HarmonyMethod(card_render_action_prefix));

        //     }
        // }

    }
}
