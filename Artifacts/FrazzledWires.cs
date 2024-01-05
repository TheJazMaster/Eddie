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
				filterMinCost = 1,
				filterMaxCost = 1,
				browseAction = new CardSelectAddShortCircuitAndMakeFreeForever(),
				browseSource = CardBrowse.Source.Deck
			});
		}
    }
}