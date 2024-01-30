namespace TheJazMaster.Eddie.Artifacts;

[ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common }, extraGlossary = new string[] { "status.heat" })]
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
				artifactPulse = Key()//,
				// dialogueSelector = ".IonConverterTrigger"
			});
		}
	}

	public override List<Tooltip>? GetExtraTooltips()
	{
		var list = base.GetExtraTooltips() ?? new();
		list.Add(new TTGlossary("status.heat", "1"));
		return list;
	}
}