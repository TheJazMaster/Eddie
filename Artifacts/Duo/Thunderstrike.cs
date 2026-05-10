using System.Reflection;
using Nanoray.PluginManager;
using Nickel;

namespace TheJazMaster.Eddie.Artifacts;
	
public class Thunderstrike : Artifact, ArtifactInterfaceManager.IOnMoveArtifact, IRegisterableArtifact
{
	public static void Register(Deck deck, IModHelper helper, IPluginPackage<IModManifest> package)
	{
		if (ModEntry.Instance.DuoArtifactsApi != null) {
			IRegisterableArtifact.Register(
				MethodBase.GetCurrentMethod()!.DeclaringType!,
				ModEntry.Instance.DuoArtifactsApi.DuoArtifactVanillaDeck,
				[ArtifactPool.Common],
				helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile($"Sprites/artifact_icons/duos/thunderstrike.png")).Sprite
			);
			ModEntry.Instance.DuoArtifactsApi.RegisterDuoArtifact(MethodBase.GetCurrentMethod()!.DeclaringType!, [ModEntry.Instance.EddieDeck, Deck.goat]);
		}
	}

	public void OnMove(State s, Combat c, AMove move)
	{
		if (!move.targetPlayer && c.isPlayerTurn)
		{
			c.QueueImmediate(new AHurt
			{
				targetPlayer = false,
				hurtAmount = Math.Abs(move.dir),
				artifactPulse = Key()
			});
		}
	}
}