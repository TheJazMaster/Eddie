using TheJazMaster.Eddie.Actions;

namespace TheJazMaster.Eddie.Artifacts;

[ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common })]
public class FrazzledWires : Artifact, IRegisterableArtifact
{
	public override void OnReceiveArtifact(State state)
	{
		state.GetCurrentQueue().QueueImmediate(new ACardSelect
		{
			filterMinCost = 1,
			filterMaxCost = 1,
			browseAction = new CardSelectAddShortCircuitAndMakeCheaperForever(),
			browseSource = CardBrowse.Source.Deck
		});
	}

	public void InjectDialogue()
	{
		var eddie = Manifest.EddieDeck.GlobalName;

		DB.story.all[$"Artifact{Key()}_0"] = new()
		{
			type = NodeType.combat,
			oncePerRun = true,
			oncePerCombatTags = new() { $"{Key()}Tag" },
			allPresent = new() { eddie },
			hasArtifacts = new() { Key() },
			maxTurnsThisCombat = 1,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "I tried fixing some broken wiring in the back. Let's see if it explodes.",
					loopTag = Manifest.EddieDefaultAnimation.Tag
				}
			}
		};
	}
}