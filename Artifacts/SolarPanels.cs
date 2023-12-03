namespace Eddie.Artifacts
{
	[ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Boss })]
    public class SolarPanels : Artifact
    {
        public bool turnedOn = true;

		public override Spr GetSprite()
		{
			if (!turnedOn)
			{
				return (Spr)(Manifest.SolarLampOnSprite?.Id ?? throw new Exception("No Solar Panels Off sprite"));
			}
			return (Spr)(Manifest.SolarPanelsOnSprite?.Id ?? throw new Exception("No Solar Panels On sprite"));
		}

		public SolarPanels()
        {
            Manifest.EventHub.ConnectToEvent<Tuple<Combat, AMove>>("Eddie.OnMoveEvent", OnMove);
        }

		private void OnMove(Tuple<Combat, AMove> evt)
		{
			var move_action = evt.Item2;
			if (move_action.targetPlayer && move_action.fromEvade)
				turnedOn = false;
		}

		public override void OnTurnStart(State s, Combat c)
		{
			if (c.isPlayerTurn)
			{
				if (turnedOn)
					c.QueueImmediate(new AEnergy
					{
						changeAmount = 1,
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