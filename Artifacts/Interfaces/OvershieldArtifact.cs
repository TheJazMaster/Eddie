namespace TheJazMaster.Eddie.Artifacts;

public interface OvershieldArtifact
{
    void OnOvershield(State s, Combat c, int overshield, bool targetPlayer);
}
