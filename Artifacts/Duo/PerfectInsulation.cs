namespace TheJazMaster.Eddie.Artifacts
{
	
	[ArtifactMeta(pools = [ArtifactPool.Common], extraGlossary = ["status.shieldAlt"])]
    public class PerfectInsulation : Artifact, OvershieldArtifact
    {
		public bool active = true;

		public override Spr GetSprite()
		{
			if (!active)
			{
				return (Spr)Manifest.PerfectInsulationOffSprite!.Id!;
			}
			return (Spr)Manifest.PerfectInsulationOnSprite!.Id!;
		}

        public void OnOvershield(State s, Combat c, int amount, bool targetPlayer) {
			if (targetPlayer && active && amount > 0) {
				c.QueueImmediate(new AEnergy {
					changeAmount = 2,
					artifactPulse = Key()
				});
				active = false;
			}
		}

		public override void OnCombatEnd(State s)
		{
			active = true;
		}
    }
}