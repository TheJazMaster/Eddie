using Eddie.Actions;

namespace Eddie.Artifacts
{
	[ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common })]
    public class FrazzledWires : Artifact
    {
		public override void OnReceiveArtifact(State state)
		{
			state.GetCurrentQueue().QueueImmediate(new ACardSelect
			{
				browseAction = new CardSelectAddShortCircuitForever(),
				browseSource = CardBrowse.Source.Deck
			});
		}
    }
}