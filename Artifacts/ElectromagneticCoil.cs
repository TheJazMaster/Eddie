namespace Eddie.Artifacts
{
	[ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common })]
    public class ElectromagneticCoil : Artifact
    {
		public override Spr GetSprite()
		{
			return (Spr)(Manifest.ElectromagneticCoilSprite?.Id ?? throw new Exception("No Electromagnetic Coil sprite"));
		}

		public override void OnTurnEnd(State state, Combat combat)
		{
			if (combat.energy > 0)
			{
				combat.QueueImmediate(new AStatus
				{
					status = Status.evade,
					statusAmount = 1,
					targetPlayer = true,
					artifactPulse = Key()//,
					// dialogueSelector = ".IonConverterTrigger"
				});
			}
		}

		public override List<Tooltip>? GetExtraTooltips()
		{
			List<Tooltip> list = new List<Tooltip>();
			list.Add(new TTGlossary("status.evade", "1"));
			return list;
		}
    }
}