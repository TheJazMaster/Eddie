
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;

namespace TheJazMaster.Eddie.Artifacts;

public class VersionControl : Artifact, ArtifactInterfaceManager.IOnExhaustArtifact, IRegisterableArtifact
{
	public int counter = 0;

	public static void Register(Deck deck, IModHelper helper, IPluginPackage<IModManifest> package)
	{
		if (ModEntry.Instance.DuoArtifactsApi != null) {
			IRegisterableArtifact.Register(
				MethodBase.GetCurrentMethod()!.DeclaringType!,
				ModEntry.Instance.DuoArtifactsApi.DuoArtifactVanillaDeck,
				[ArtifactPool.Common],
				helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile($"Sprites/artifact_icons/duos/version_control.png")).Sprite
			);
			ModEntry.Instance.DuoArtifactsApi.RegisterDuoArtifact(MethodBase.GetCurrentMethod()!.DeclaringType!, [ModEntry.Instance.EddieDeck, Deck.hacker]);
		}
	}

	public override int? GetDisplayNumber(State s) {
		return counter;
	}

	public void OnExhaustCard(State s, Combat c, Card card) {
		counter++;
		if (counter == 4) {
			counter = 0;
			c.Queue(new AEnergy {
				changeAmount = 1,
				artifactPulse = Key()
			});
		}
	}

    public override List<Tooltip>? GetExtraTooltips() => [new TTGlossary("cardtrait.exhaust")];
}