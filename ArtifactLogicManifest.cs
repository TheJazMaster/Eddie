using Eddie.Artifacts;
using CobaltCoreModding.Definitions.ExternalItems;
using CobaltCoreModding.Definitions.ModContactPoints;
using CobaltCoreModding.Definitions.ModManifests;
using System.Reflection;
using System.Reflection.Emit;
using FMOD;
using FSPRO;
using Microsoft.Xna.Framework.Input;
using ILInstruction = Mono.Cecil.Cil.Instruction;

using HarmonyLib;

namespace Eddie
{
    public partial class Manifest : IArtifactManifest, ICustomEventManifest
    {
        private static ICustomEventHub? _eventHub;
        internal static ICustomEventHub EventHub { get => _eventHub ?? throw new Exception(); set => _eventHub = value; }

        public static ExternalArtifact? FrazzledWiresArtifact { get; private set; }
        public static ExternalArtifact? SolarLampArtifact { get; private set; }
        public static ExternalArtifact? ElectromagneticCoilArtifact { get; private set; }
        public static ExternalArtifact? SolarPanelsArtifact { get; private set; }
        public static ExternalArtifact? FissionChamberArtifact { get; private set; }

        public void LoadManifest(IArtifactRegistry registry)
        {
			SolarLampArtifact = new ExternalArtifact("Eddie.Artifacts.SolarLamp", typeof(SolarLamp), SolarLampOnSprite ?? throw new Exception("missing SolarLamp sprite"), ownerDeck: Manifest.EddieDeck ?? throw new Exception("Missing Eddie deck."));
			SolarLampArtifact.AddLocalisation( "SOLAR LAMP", "Each turn, if you didn't use any <c=status>EVADE</c> last turn, gain 1 evade.");
			registry.RegisterArtifact(SolarLampArtifact);

			SolarPanelsArtifact = new ExternalArtifact("Eddie.Artifacts.SolarPanels", typeof(SolarPanels), SolarPanelsOnSprite ?? throw new Exception("missing SolarPanels sprite"), ownerDeck: Manifest.EddieDeck ?? throw new Exception("Missing Eddie deck."));
			SolarPanelsArtifact.AddLocalisation( "SOLAR PANELS", "Gain 1 extra <c=energy>ENERGY</c> on the first turn. Each turn, if you didn't use any <c=status>EVADE</c> last turn, gain 1 extra <c=energy>ENERGY</c>.");
			registry.RegisterArtifact(SolarPanelsArtifact);

			ElectromagneticCoilArtifact = new ExternalArtifact("Eddie.Artifacts.ElectromagneticCoil", typeof(ElectromagneticCoil), ElectromagneticCoilSprite ?? throw new Exception("missing ElectromagneticCoil sprite"), ownerDeck: Manifest.EddieDeck ?? throw new Exception("Missing Eddie deck."));
			ElectromagneticCoilArtifact.AddLocalisation( "ELECTROMAGNETIC COIL", "If you end your turn with more than 0 <c=energy>ENERGY</c>, gain 1 <c=status>EVADE</c>.");
			registry.RegisterArtifact(ElectromagneticCoilArtifact);

			FrazzledWiresArtifact = new ExternalArtifact("Eddie.Artifacts.FrazzledWires", typeof(FrazzledWires), FrazzledWiresSprite ?? throw new Exception("missing FrazzledWires sprite"), ownerDeck: Manifest.EddieDeck ?? throw new Exception("Missing Eddie deck."));
			FrazzledWiresArtifact.AddLocalisation( "Frazzled Wires", "Choose a card that does not <c=cardtrait>short-circuit</c> in your deck. It gains <c=cardtrait>short-circuit</c>.");
			registry.RegisterArtifact(FrazzledWiresArtifact);

            // Patching status logic
            var harmony = new Harmony("Eddie.Status");

            SolarLampEventLogic(harmony);
        }

		public void LoadManifest(ICustomEventHub eventHub)
		{
            _eventHub = eventHub;

            eventHub.MakeEvent<Tuple<Combat, AMove>>("Eddie.OnMoveEvent");
		}

		private void SolarLampEventLogic(Harmony harmony)
        {
            var a_move_begin_method = typeof(AMove).GetMethod("Begin") ?? throw MakeInformativeException(Logger, "Couldn't find AMove.Begin method");
            var a_move_begin_post = typeof(Manifest).GetMethod("FireOnMoveEvent", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic) ?? throw MakeInformativeException(Logger, "Couldnt find Manifest.FireOnMoveEvent method");
            harmony.Patch(a_move_begin_method, postfix: new HarmonyMethod(a_move_begin_post));
        }

		private static void FireOnMoveEvent(AMove __instance, Combat c, State s)
		{
            Manifest.EventHub.SignalEvent<Tuple<Combat, AMove>>("Eddie.OnMoveEvent", new(c, __instance));
		}

		private static void OnMoveEvent(Combat c, int dir, bool targetPlayer, bool fromEvade)
		{
			
		}
    }
}
