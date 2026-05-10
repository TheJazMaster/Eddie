
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;
using Shockah.Soggins;

namespace TheJazMaster.Eddie.Artifacts;

public class WaxWings : Artifact, ISmugHook, IRegisterableArtifact
{
	static ISogginsApi? SogginsAPI => ModEntry.Instance.SogginsApi;

	public bool active = false;

	public static void Register(Deck deck, IModHelper helper, IPluginPackage<IModManifest> package)
	{
		if (ModEntry.Instance.DuoArtifactsApi != null && SogginsAPI != null) {
			IRegisterableArtifact.Register(
				MethodBase.GetCurrentMethod()!.DeclaringType!,
				ModEntry.Instance.DuoArtifactsApi.DuoArtifactVanillaDeck,
				[ArtifactPool.Common],
				helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile($"Sprites/artifact_icons/duos/wax_wings.png")).Sprite
			);
			ModEntry.Instance.DuoArtifactsApi.RegisterDuoArtifact(MethodBase.GetCurrentMethod()!.DeclaringType!, [ModEntry.Instance.EddieDeck, SogginsAPI.SogginsVanillaDeck]);
		}
	}

	public override void OnQueueEmptyDuringPlayerTurn(State state, Combat combat)
	{
		active = SogginsAPI!.IsOversmug(state, state.ship);
	}

	public void OnCardBotchedBySmug(State state, Combat combat, Card card) {
		if (active) {
			Pulse();
			combat.Queue([
				new ADelay {
					time = 0.3
				},
				new AEnergy {
					changeAmount = 2
				}
			]);
		}
	}
}
