namespace TheJazMaster.Eddie.Artifacts
{
	
	[ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common })]
    public class PerfectInsulation : Artifact, OvershieldArtifact
    {
		bool active = true;

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