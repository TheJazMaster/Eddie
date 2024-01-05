using Eddie.Cards;

namespace Eddie.Artifacts;

[ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common })]
public class UltraLightBatteries : Artifact
{
	bool active = true;

	public override void OnQueueEmptyDuringPlayerTurn(State state, Combat combat)
	{
		if (combat.energy == 0)
		{
			if (active) {
				active = false;
				combat.QueueImmediate(new AAddCard
				{
					timer = 0.4,
					card = new Lightweight {
						temporaryOverride = true
					},
					destination = CardDestination.Hand
				});
			}
		} else {
			active = true;
		}
	}

	public override List<Tooltip>? GetExtraTooltips()
	{
		var tooltips = base.GetExtraTooltips() ?? new();
		tooltips.Add(new TTCard { card = new Lightweight() });
		return tooltips;
	}
}
