namespace Eddie.Artifacts
{
	[ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common })]
    public class SolarLamp : Artifact
    {
        public bool turnedOn = false;

		public override Spr GetSprite()
		{
			if (!turnedOn)
			{
				return (Spr)(Manifest.SolarLampOffSprite?.Id ?? throw new Exception("No Solar Lamp Off sprite"));
			}
			return (Spr)(Manifest.SolarLampOnSprite?.Id ?? throw new Exception("No Solar Lamp On sprite"));
		}

		public SolarLamp()
        {
            Manifest.EventHub.ConnectToEvent<Tuple<Combat, AMove>>("Eddie.OnMoveEvent", OnMove);
        }

		private void OnMove(Tuple<Combat, AMove> evt)
		{
			var move_action = evt.Item2;
			if (move_action.targetPlayer && move_action.fromEvade)
			{
				turnedOn = false;
			}
		}

		public override void OnTurnStart(State s, Combat c)
		{
			if (c.turn != 1 && c.isPlayerTurn)
			{
				if (turnedOn)
					c.QueueImmediate(new AStatus
					{
						status = Status.evade,
						statusAmount = 1,
						targetPlayer = true,
						artifactPulse = Key()
					});
				else
					turnedOn = true;

			}
		}

        public override void OnRemoveArtifact(State state)
        {
            Manifest.EventHub.DisconnectFromEvent<Tuple<Combat, AMove>>("Eddie.OnMoveEvent", OnMove);
        }

		public override void OnCombatEnd(State state)
		{
			turnedOn = true;
		}
    }
}