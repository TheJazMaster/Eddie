using Eddie.Cards;

namespace Eddie.Artifacts
{
	[ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Boss })]
    public class DeconstructionGoggles : Artifact
    {
		public override Spr GetSprite()
		{
			return (Spr)(Manifest.DeconstructionGogglesSprite?.Id ?? throw new Exception("No Deconstruction Goggles sprite"));
		}

		public override void OnCombatStart(State state, Combat combat)
		{
			combat.QueueImmediate(new AAddCard
			{
				card = new ReverseEngineer(),
				destination = CardDestination.Hand,
				amount = 1
			});
			combat.QueueImmediate(new AAddCard
			{
				card = new ReverseEngineer(),
				destination = CardDestination.Hand,
				amount = 1
			});
		}

		public override List<Tooltip>? GetExtraTooltips()
		{
			return new List<Tooltip>
			{
				new TTCard
				{
					card = new ReverseEngineer()
				},
				new TTGlossary("cardtrait.retain")
			};
		}
    }
}