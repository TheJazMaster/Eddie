namespace Eddie.Artifacts;

public interface OverdriveReductionPreventerArtifact {
    bool ShouldReduceOverdrive(State s, Combat c);
}
