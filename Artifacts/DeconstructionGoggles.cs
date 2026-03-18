using TheJazMaster.Eddie.Cards;
using TheJazMaster.Eddie.DialogueAdditions;

namespace TheJazMaster.Eddie.Artifacts;

[ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Boss })]
public class DeconstructionGoggles : Artifact, IRegisterableArtifact
{
	public override Spr GetSprite()
	{
		return (Spr)(Manifest.DeconstructionGogglesSprite?.Id ?? throw new Exception("No Deconstruction Goggles sprite"));
	}

	public override void OnCombatStart(State state, Combat combat)
	{
		combat.QueueImmediate(new AAddCard
		{
			card = new ReverseEngineer(),
			destination = CardDestination.Hand,
			amount = 1
		});
		combat.QueueImmediate(new AAddCard
		{
			card = new ReverseEngineer(),
			destination = CardDestination.Hand,
			amount = 1
		});
	}

	public override List<Tooltip>? GetExtraTooltips()
	{
		return new List<Tooltip>
		{
			new TTCard
			{
				card = new ReverseEngineer()
			},
			new TTGlossary("cardtrait.retain")
		};
	}

	public void InjectDialogue()
	{
		var eddie = Manifest.EddieDeck.GlobalName;

		DB.story.all[$"Artifact{Key()}_0"] = new()
		{
			type = NodeType.combat,
			oncePerRun = true,
			oncePerCombatTags = new() { $"{Key()}Tag" },
			allPresent = new() { eddie, "comp" },
			hasArtifacts = new() { Key() },
			maxTurnsThisCombat = 1,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Let's tear some things apart!",
					loopTag = Manifest.EddieDefaultAnimation.Tag
				},
				new SaySwitch()
				{
					lines = new()
					{
						new CustomSay()
						{
							who = Deck.goat.Key(),
							Text = "Not my drones, right?",
							loopTag = "panic"
						},
						new CustomSay()
						{
							who = Deck.peri.Key(),
							Text = "No!",
							loopTag = "squint"
						},
						new CustomSay()
						{
							who = Deck.dizzy.Key(),
							Text = "Ooooh.",
							loopTag = "neutral"
						},
					}
				},
			}
		};

		DB.story.all[$"Artifact{Key()}_1"] = new()
		{
			type = NodeType.combat,
			oncePerRun = true,
			oncePerCombatTags = new() { $"{Key()}Tag" },
			allPresent = new() { eddie, "comp" },
			hasArtifacts = new() { Key() },
			maxTurnsThisCombat = 1,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "CAT! Deconstruction mode!",
					loopTag = Manifest.EddieExcitedAnimation.Tag
				},
				new CustomSay()
				{
					who = "comp",
					Text = "What are you talking about?",
					loopTag = "grumpy"
				},
			}
		};

		if (StoryVarsAdditions.SogginsName != null)
			DB.story.all[$"Artifact{Key()}_2"] = new()
			{
				type = NodeType.combat,
				oncePerRun = true,
				oncePerCombatTags = new() { $"{Key()}Tag" },
				allPresent = new() { eddie, StoryVarsAdditions.SogginsName },
				hasArtifacts = new() { Key() },
				maxTurnsThisCombat = 1,
				lines = new()
				{
					new CustomSay()
					{
						who = StoryVarsAdditions.SogginsName,
						Text = "I think my ideas are worth deconstructing."
					},
					new CustomSay()
					{
						who = eddie,
						Text = "Yeah - for good.",
						loopTag = Manifest.EddieAnnoyedAnimation.Tag
					},
				}
			};
	}
}