namespace TheJazMaster.Eddie.Artifacts;
	
[ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common })]
public class Thunderstrike : Artifact
{
	public Thunderstrike()
	{
		Manifest.EventHub.ConnectToEvent<Tuple<Combat, AMove>>("Eddie.OnMoveEvent", OnMove);
	}

	private void OnMove(Tuple<Combat, AMove> evt)
	{
		var combat = evt.Item1;
		var move_action = evt.Item2;
		if (!move_action.targetPlayer && combat.isPlayerTurn)
		{
			combat.QueueImmediate(new AHurt
			{
				targetPlayer = false,
				hurtAmount = 1,
				artifactPulse = Key()
			});
		}
	}

	public override void OnRemoveArtifact(State state)
	{
		Manifest.EventHub.DisconnectFromEvent<Tuple<Combat, AMove>>("Eddie.OnMoveEvent", OnMove);
	}
}