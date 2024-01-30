namespace TheJazMaster.Eddie.Artifacts;

[ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common })]
public class FissionChamber : Artifact, XAffectorArtifact
{
	public override Spr GetSprite()
	{
		return (Spr)(Manifest.FissionChamberSprite?.Id ?? throw new Exception("No Fission Chamber sprite"));
	}

	public int AffectX(int value) {
		return value + 1;
	}

	public void InjectDialogue()
	{
		var eddie = Manifest.EddieDeck.GlobalName;

		DB.story.all[$"Artifact{Key()}_0"] = new()
		{
			type = NodeType.combat,
			oncePerRun = true,
			oncePerCombatTags = new() { $"{Key()}Tag" },
			allPresent = new() { eddie, Deck.riggs.Key() },
			hasArtifacts = new() { Key() },
			maxTurnsThisCombat = 1,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Hey Riggs, what if I were to put a nuclear reactor in your gun?",
					loopTag = Manifest.EddieDefaultAnimation.Tag
				},
				new CustomSay()
				{
					who = Deck.riggs.Key(),
					Text = "I'm listening...",
					loopTag = "neutral"
				}
			}
		};
		DB.story.all[$"Artifact{Key()}_1"] = new()
		{
			type = NodeType.combat,
			oncePerRun = true,
			oncePerCombatTags = new() { $"{Key()}Tag" },
			allPresent = new() { eddie, Deck.dizzy.Key() },
			hasArtifacts = new() { Key() },
			maxTurnsThisCombat = 1,
			lines = new()
			{
				new CustomSay()
				{
					who = Deck.dizzy.Key(),
					Text = "Hey Eddie, I'm getting some concerning readings on our shields...",
					loopTag = "geiger"
				},
				new CustomSay()
				{
					who = eddie,
					Text = "Don't worry about it.",
					loopTag = Manifest.EddieDefaultAnimation.Tag
				}
			}
		};
		DB.story.all[$"Artifact{Key()}_2"] = new()
		{
			type = NodeType.combat,
			oncePerRun = true,
			oncePerCombatTags = new() { $"{Key()}Tag" },
			allPresent = new() { eddie, Deck.peri.Key() },
			hasArtifacts = new() { Key() },
			maxTurnsThisCombat = 1,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Hey Peri, this should boost the inverter!",
					loopTag = Manifest.EddieExcitedAnimation.Tag
				},
				new CustomSay()
				{
					who = Deck.peri.Key(),
					Text = "I'll keep that in mind.",
					loopTag = "neutral"
				}
			}
		};
		DB.story.all[$"Artifact{Key()}_3"] = new()
		{
			type = NodeType.combat,
			oncePerRun = true,
			oncePerCombatTags = new() { $"{Key()}Tag" },
			allPresent = new() { eddie, Deck.hacker.Key() },
			hasArtifacts = new() { Key() },
			maxTurnsThisCombat = 1,
			lines = new()
			{
				new CustomSay()
				{
					who = Deck.hacker.Key(),
					Text = "Why is my console hooked up to a nuclear reactor?",
					loopTag = "intense"
				},
				new CustomSay()
				{
					who = eddie,
					Text = "I'm trying something out.",
					loopTag = Manifest.EddieDefaultAnimation.Tag
				}
			}
		};
	}
}