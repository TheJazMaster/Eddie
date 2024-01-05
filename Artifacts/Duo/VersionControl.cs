namespace Eddie.Artifacts;

[ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common }, extraGlossary = new string[] { "cardtrait.exhaust" })]
public class VersionControl : Artifact, OnExhaustArtifact
{
	int counter = 0;

	public override int? GetDisplayNumber(State s)
	{
		return counter;
	}

	public void OnExhaustCard(State s, Combat c, Card card) {
		counter++;
		if (counter == 4) {
			counter = 0;
			c.Queue(new AEnergy {
				changeAmount = 1
			});
		}
	}
}