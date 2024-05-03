namespace TheJazMaster.Eddie.Artifacts;
	
[ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common })]
public class Thunderstrike : Artifact, IOnMoveArtifact
{
	public void OnMove(State s, Combat c, AMove move)
	{
		if (!move.targetPlayer && c.isPlayerTurn)
		{
			c.QueueImmediate(new AHurt
			{
				targetPlayer = false,
				hurtAmount = 1,
				artifactPulse = Key()
			});
		}
	}
}