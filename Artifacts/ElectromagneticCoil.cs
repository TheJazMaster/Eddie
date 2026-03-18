namespace TheJazMaster.Eddie.Artifacts;

[ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common }, extraGlossary = new string[] { "status.evade" })]
public class ElectromagneticCoil : Artifact, IRegisterableArtifact
{
	public override Spr GetSprite()
	{
		return (Spr)(Manifest.ElectromagneticCoilSprite?.Id ?? throw new Exception("No Electromagnetic Coil sprite"));
	}

	public override void OnTurnEnd(State state, Combat combat)
	{
		if (combat.energy > 0)
		{
			combat.QueueImmediate(new AStatus
			{
				status = Status.evade,
				statusAmount = 1,
				targetPlayer = true,
				artifactPulse = Key(),
				dialogueSelector = $".{Key()}Trigger"
			});
		}
	}

	public override List<Tooltip>? GetExtraTooltips()
	{
		var list = base.GetExtraTooltips() ?? new();
		list.Add(new TTGlossary("status.evade", "1"));
		return list;
	}

	public void InjectDialogue()
	{
		var eddie = Manifest.EddieDeck.GlobalName;

		DB.story.all[$"Artifact{Key()}_0"] = new()
		{
			type = NodeType.combat,
			lookup = new() { $"{Key()}Trigger" },
			oncePerCombatTags = new() { $"{Key()}Tag" },
			oncePerRun = true,
			allPresent = new() { eddie },
			hasArtifacts = new() { Key() },
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Now that's what I call efficiency.",
					loopTag = Manifest.EddieDefaultAnimation.Tag
				}
			}
		};
		DB.story.all[$"Artifact{Key()}_1"] = new()
		{
			type = NodeType.combat,
			lookup = new() { $"{Key()}Trigger" },
			oncePerCombatTags = new() { $"{Key()}Tag" },
			oncePerRun = true,
			allPresent = new() { eddie },
			hasArtifacts = new() { Key() },
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Look at this thing! Pretty cool, right?",
					loopTag = Manifest.EddieDefaultAnimation.Tag
				}
			}
		};
		DB.story.all[$"Artifact{Key()}_2"] = new()
		{
			type = NodeType.combat,
			lookup = new() { $"{Key()}Trigger" },
			oncePerCombatTags = new() { $"{Key()}Tag" },
			oncePerRun = true,
			allPresent = new() { eddie },
			hasArtifacts = new() { Key() },
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "We don't even have to do anything!",
					loopTag = Manifest.EddieExplainsAnimation.Tag
				}
			}
		};
	}
}