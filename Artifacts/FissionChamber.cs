namespace Eddie.Artifacts
{
	[ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common })]
    public class FissionChamber : Artifact, XAffectorArtifact, OvershieldArtifact
    {
		public override Spr GetSprite()
		{
			return (Spr)(Manifest.FissionChamberSprite?.Id ?? throw new Exception("No Fission Chamber sprite"));
		}

		public int AffectX(int value) {
			return value + 1;
		}

		public void OnOvershield(State s, Combat c, int n, bool targetPlayer) {}
    }
}