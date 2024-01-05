namespace Eddie.Artifacts
{
	
	[ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common }, extraGlossary = new string[] { "status.evade" })]
    public class SunLamp : Artifact
    {
        public bool turnedOn = true;
        public bool charged = true;

		public override Spr GetSprite()
		{
			if (!charged)
			{
				return (Spr)(Manifest.SunLampUnchargedSprite?.Id ?? throw new Exception("No Solar Lamp Uncharged sprite"));
			}
			if (!turnedOn)
			{
				return (Spr)(Manifest.SunLampOffSprite?.Id ?? throw new Exception("No Solar Lamp Off sprite"));
			}
			return (Spr)(Manifest.SunLampOnSprite?.Id ?? throw new Exception("No Solar Lamp On sprite"));
		}

		public SunLamp()
        {
            Manifest.EventHub.ConnectToEvent<Tuple<Combat, AMove>>("Eddie.OnMoveEvent", OnMove);
        }

		private void OnMove(Tuple<Combat, AMove> evt)
		{
			var move_action = evt.Item2;
			if (move_action.targetPlayer && move_action.fromEvade)
			{
				turnedOn = false;
				if (!charged) {
					charged = true;
					Pulse();
				}
			}
		}

		public override void OnTurnStart(State s, Combat c)
		{
			if (c.turn != 1 && c.isPlayerTurn)
			{
				if (turnedOn && charged) {
					c.QueueImmediate(new AStatus
					{
						status = Status.evade,
						statusAmount = 1,
						targetPlayer = true,
						artifactPulse = Key()
					});
					charged = false;
				} else
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
			charged = true;
		}
    }
}