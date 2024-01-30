namespace TheJazMaster.Eddie.Artifacts;

[ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common }, extraGlossary = new string[] { "status.overdrive" })]
public class OverdriveFeedback : Artifact, OverdriveReductionPreventerArtifact
{
	bool active = true;

	public override void OnTurnStart(State state, Combat combat) {
		active = false;
	}

	public override void OnTurnEnd(State state, Combat combat) {
		if (combat.energy > 0)
		{
			active = true;
		}
	}

	public bool ShouldReduceOverdrive(State s, Combat c) {
		return !active;
	}
}