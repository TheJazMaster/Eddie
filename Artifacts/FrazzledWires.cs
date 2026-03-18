using TheJazMaster.Eddie.Actions;
using TheJazMaster.Eddie.DialogueAdditions;

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

		DB.story.all[$"Artifact{Key()}_1"] = new()
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
					Text = "There can be no greatness without a little risk.",
					loopTag = Manifest.EddieExplainsAnimation.Tag
				},
				new SaySwitch()
				{
					lines = [
						new CustomSay()
						{
							who = Deck.dizzy.Key(),
							Text = "And we're living proof!",
							loopTag = "neutral"
						},
						new CustomSay()
						{
							who = Deck.riggs.Key(),
							Text = "Yeah! We're trailblazers!",
							loopTag = "neutral"
						},
						new CustomSay()
						{
							who = Deck.peri.Key(),
							Text = "A looming fire hazard does make me more vigilant.",
							loopTag = "squint"
						},
						new CustomSay()
						{
							who = Deck.goat.Key(),
							Text = "This wiring doesn't seem all that great.",
							loopTag = "squint"
						},
						new CustomSay()
						{
							who = Deck.eunice.Key(),
							Text = "I know, I'm pretty great myself.",
							loopTag = "neutral"
						},
						new CustomSay()
						{
							who = Deck.hacker.Key(),
							Text = "Survivor bias, or something.",
							loopTag = "neutral"
						}
					]
				}.MaybeAdd(StoryVarsAdditions.SogginsName != null, new CustomSay() {
					who = StoryVarsAdditions.SogginsName!,
					Text = "Words to live by."
				})
			}
		};
	}

	public override List<Tooltip>? GetExtraTooltips()
	{
		return [new TTGlossary(Manifest.ShortCircuitGlossary?.Head!)];
	}
}