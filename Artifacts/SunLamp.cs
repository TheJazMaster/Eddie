namespace TheJazMaster.Eddie.Artifacts;

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
					artifactPulse = Key(),
					dialogueSelector = ".SunLamp"
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
			lookup = new() { $"{Key()}Trigger" },
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Ahh, I missed this.",
					loopTag = Manifest.EddieRestingAnimation.Tag
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
			lookup = new() { $"{Key()}Trigger" },
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "I hope you don't mind if I turn this on..",
					loopTag = Manifest.EddieRestingAnimation.Tag
				},
				new CustomSay()
				{
					who = Deck.dizzy.Key(),
					Text = "Not at all, go ahead!",
					loopTag = "explains"
				}
			}
		};
		DB.story.all[$"Artifact{Key()}_2"] = new()
		{
			type = NodeType.combat,
			oncePerRun = true,
			oncePerCombatTags = new() { $"{Key()}Tag" },
			allPresent = new() { eddie },
			hasArtifacts = new() { Key() },
			lookup = new() { $"{Key()}Trigger" },
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Finally, some peace and quiet.",
					loopTag = Manifest.EddieRestingAnimation.Tag
				},
				new CustomSay()
				{
					who = Deck.eunice.Key(),
					Text = "Quit hogging that thing!",
					loopTag = "mad"
				}
			}
		};
		DB.story.all[$"Artifact{Key()}_3"] = new()
		{
			type = NodeType.combat,
			oncePerRun = true,
			oncePerCombatTags = new() { $"{Key()}Tag" },
			allPresent = new() { eddie, Deck.peri.Key() },
			hasArtifacts = new() { Key() },
			lookup = new() { $"{Key()}Trigger" },
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "This lamp really brightens up the room!",
					loopTag = Manifest.EddieRestingAnimation.Tag
				},
				new CustomSay()
				{
					who = Deck.peri.Key(),
					Text = "Can someone turn off that lamp?",
					loopTag = "mad"
				}
			}
		};
		DB.story.all[$"Artifact{Key()}_4"] = new()
		{
			type = NodeType.combat,
			oncePerRun = true,
			oncePerCombatTags = new() { $"{Key()}Tag" },
			allPresent = new() { eddie, Deck.goat.Key() },
			hasArtifacts = new() { Key() },
			lookup = new() { $"{Key()}Trigger" },
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "I read about these things. They boost productivity by, like, a lot.",
					loopTag = Manifest.EddieRestingAnimation.Tag
				},
				new CustomSay()
				{
					who = Deck.goat.Key(),
					Text = "Now I want one.",
					loopTag = "neutral"
				}
			}
		};
		DB.story.all[$"Artifact{Key()}_5"] = new()
		{
			type = NodeType.combat,
			oncePerRun = true,
			oncePerCombatTags = new() { $"{Key()}Tag" },
			allPresent = new() { eddie },
			hasArtifacts = new() { Key() },
			lookup = new() { $"{Key()}Trigger" },
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "I do some of my best thinking while asleep.",
					loopTag = Manifest.EddieRestingAnimation.Tag
				}
			}
		};
		DB.story.all[$"Artifact{Key()}_6"] = new()
		{
			type = NodeType.combat,
			oncePerRun = true,
			oncePerCombatTags = new() { $"{Key()}Tag" },
			allPresent = new() { Deck.hacker.Key() },
			hasArtifacts = new() { Key() },
			lookup = new() { $"{Key()}Trigger" },
			lines = new()
			{
				new CustomSay()
				{
					who = Deck.hacker.Key(),
					Text = "Ouch, my eyes! I'm sensitive to sunlight.",
					loopTag = "squint"
				}
			}
		};
		DB.story.all[$"Artifact{Key()}_7"] = new()
		{
			type = NodeType.combat,
			oncePerRun = true,
			oncePerCombatTags = new() { $"{Key()}Tag" },
			allPresent = new() { eddie },
			hasArtifacts = new() { Key() },
			lookup = new() { $"{Key()}Trigger" },
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "It's important to keep your workplace comfortable.",
					loopTag = Manifest.EddieRestingAnimation.Tag
				}
			}
		};
		DB.story.all[$"Artifact{Key()}_8"] = new()
		{
			type = NodeType.combat,
			oncePerRun = true,
			oncePerCombatTags = new() { $"{Key()}Tag" },
			allPresent = new() { Deck.shard.Key() },
			hasArtifacts = new() { Key() },
			lookup = new() { $"{Key()}Trigger" },
			lines = new()
			{
				new CustomSay()
				{
					who = Deck.shard.Key(),
					Text = "Wow! It's like a mini-sun!",
					loopTag = "relaxed"
				}
			}
		};
	}
}