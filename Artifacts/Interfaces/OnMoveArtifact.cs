namespace TheJazMaster.Eddie.Artifacts;

public interface IOnMoveArtifact
{
	void OnMove(State s, Combat c, AMove move);
}