
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;

namespace TheJazMaster.Eddie.Artifacts;

public class EmergencyVentilator : Artifact, IRegisterableArtifact
{
    public static void Register(Deck deck, IModHelper helper, IPluginPackage<IModManifest> package)
    {
		if (ModEntry.Instance.DuoArtifactsApi != null) {
			IRegisterableArtifact.Register(
				MethodBase.GetCurrentMethod()!.DeclaringType!,
				ModEntry.Instance.DuoArtifactsApi.DuoArtifactVanillaDeck,
				[ArtifactPool.Common],
				helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile($"Sprites/artifact_icons/duos/emergency_ventilator.png")).Sprite
			);
			ModEntry.Instance.DuoArtifactsApi.RegisterDuoArtifact(MethodBase.GetCurrentMethod()!.DeclaringType!, [ModEntry.Instance.EddieDeck, Deck.eunice]);
		}
    }
	
	public override void OnTurnEnd(State state, Combat combat)
	{
		if (combat.energy > 0)
		{
			new AStatus {
				status = Status.heat,
				statusAmount = -2,
				targetPlayer = true,
				artifactPulse = Key()
			}.Begin(MG.inst.g, state, combat);
			Pulse();
		}
	}

	public override List<Tooltip>? GetExtraTooltips() => StatusMeta.GetTooltips(Status.heat, 3);
}