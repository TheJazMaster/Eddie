
namespace TheJazMaster.Eddie.Artifacts;

[ArtifactMeta(pools = [ArtifactPool.Common])]
public class EmergencyVentilator : Artifact
{
	public override void OnTurnEnd(State state, Combat combat)
	{
		if (combat.energy > 0)
		{
			combat.QueueImmediate(new AStatus
			{
				status = Status.heat,
				statusAmount = -1,
				targetPlayer = true,
				artifactPulse = Key()
			});
		}
	}

	public override List<Tooltip>? GetExtraTooltips() => StatusMeta.GetTooltips(Status.heat, 3);
}