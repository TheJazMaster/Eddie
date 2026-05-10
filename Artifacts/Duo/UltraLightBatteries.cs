using System.Reflection;
using Nanoray.PluginManager;
using Nickel;
using TheJazMaster.Eddie.Cards;

namespace TheJazMaster.Eddie.Artifacts;

public class UltraLightBatteries : Artifact, IRegisterableArtifact
{
	public bool active = true;

	public static void Register(Deck deck, IModHelper helper, IPluginPackage<IModManifest> package)
	{
		if (ModEntry.Instance.DuoArtifactsApi != null) {
			IRegisterableArtifact.Register(
				MethodBase.GetCurrentMethod()!.DeclaringType!,
				ModEntry.Instance.DuoArtifactsApi.DuoArtifactVanillaDeck,
				[ArtifactPool.Common],
				helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile($"Sprites/artifact_icons/duos/ultralight_batteries.png")).Sprite
			);
			ModEntry.Instance.DuoArtifactsApi.RegisterDuoArtifact(MethodBase.GetCurrentMethod()!.DeclaringType!, [ModEntry.Instance.EddieDeck, Deck.riggs]);
		}
	}

	public override void OnQueueEmptyDuringPlayerTurn(State state, Combat combat)
	{
		if (combat.energy == 0)
		{
			if (active) {
				active = false;
				combat.QueueImmediate(new AAddCard {
					card = new Lightweight(),
					destination = CardDestination.Hand
				});
			}
		} else {
			active = true;
		}
	}

	public override List<Tooltip>? GetExtraTooltips() => [new TTCard {
		card = new Lightweight()
	}];
}
