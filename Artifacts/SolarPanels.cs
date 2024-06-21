namespace TheJazMaster.Eddie.Artifacts;

[ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Boss }, extraGlossary = new string[] { "status.evade" })]
public class SolarPanels : Artifact, IRegisterableArtifact, IOnMoveArtifact
{
	public bool turnedOn = true;

	public override Spr GetSprite()
	{
		if (!turnedOn)
		{
			return (Spr)(Manifest.SolarPanelsOffSprite?.Id ?? throw new Exception("No Solar Panels Off sprite"));
		}
		return (Spr)(Manifest.SolarPanelsOnSprite?.Id ?? throw new Exception("No Solar Panels On sprite"));
	}

	public void OnMove(State s, Combat c, AMove move)
	{
		if (move.targetPlayer && move.fromEvade) {
			turnedOn = false;
			move.dialogueSelector = $".{Key()}RuinedTrigger";
		}
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

	public override void OnCombatEnd(State state)
	{
		turnedOn = true;
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
					Text = "Riggs, I especially don't want any of your signature \"maneuvers\" right now.",
					loopTag = Manifest.EddieSquintAnimation.Tag
				},
				new CustomSay()
				{
					who = Deck.riggs.Key(),
					Text = "Hey, I can be gentle too!",
					loopTag = "neutral"
				},
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
				new SaySwitch()
				{
					lines = new()
					{
						new CustomSay()
						{
							who = eddie,
							Text = "Let's keep those panels angled properly or they might break.",
							loopTag = Manifest.EddieSquintAnimation.Tag
						},
						new CustomSay()
						{
							who = eddie,
							Text = "No sudden movements! Those solar panels need to stay aligned.",
							loopTag = Manifest.EddieSquintAnimation.Tag
						},
						new CustomSay()
						{
							who = eddie,
							Text = "These solar panels are pretty finnicky, be careful.",
							loopTag = Manifest.EddieWorriedAnimation.Tag
						},
					}
				},
				new SaySwitch()
				{
					lines = new()
					{
						new CustomSay()
						{
							who = "comp",
							Text = "They're about to fall off...",
							loopTag = "squint"
						},
						new CustomSay()
						{
							who = Deck.eunice.Key(),
							Text = "Which dump did you get these from?",
							loopTag = "sly"
						},
						new CustomSay()
						{
							who = Deck.shard.Key(),
							Text = "I won't make the sun plates sad Ed, I promise!",
							loopTag = "paws"
						},
						new CustomSay()
						{
							who = Deck.hacker.Key(),
							Text = "The autopilot should be stable enough.",
							loopTag = "neutral"
						},
						new CustomSay()
						{
							who = Deck.riggs.Key(),
							Text = "Sheesh, big ask.",
							loopTag = "neutral"
						},
						new CustomSay()
						{
							who = Deck.goat.Key(),
							Text = "How about we move the enemy instead?",
							loopTag = "neutral"
						}
					}
				}
			}
		};

		DB.story.all[$"Artifact{Key()}Ruined_0"] = new()
		{
			type = NodeType.combat,
			oncePerCombat = true,
			lookup = new() { $"{Key()}RuinedTrigger" },
			oncePerCombatTags = new() { $"{Key()}RuinedTag" },
			allPresent = new() { eddie },
			hasArtifacts = new() { Key() },
			lines = new()
			{
				new SaySwitch()
				{
					lines = new()
					{
						new CustomSay()
						{
							who = eddie,
							Text = "Oh come on...",
							loopTag = Manifest.EddieDisappointedAnimation.Tag
						},
						new CustomSay()
						{
							who = eddie,
							Text = "Readjusting...",
							loopTag = Manifest.EddieSquintAnimation.Tag
						},
						new CustomSay()
						{
							who = eddie,
							Text = "Ah, the panels.",
							loopTag = Manifest.EddieSquintAnimation.Tag
						},
						new CustomSay()
						{
							who = eddie,
							Text = "No sudden movements, please!",
							loopTag = Manifest.EddieSquintAnimation.Tag
						},
						new CustomSay()
						{
							who = eddie,
							Text = "Shoot. Let's try that again.",
							loopTag = Manifest.EddieDisappointedAnimation.Tag
						},
						new CustomSay()
						{
							who = eddie,
							Text = "That's not going to be enough sunglight.",
							loopTag = Manifest.EddieSquintAnimation.Tag
						}
					}
				},
			}
		};

		DB.story.all[$"Artifact{Key()}Ruined_0"] = new()
		{
			type = NodeType.combat,
			oncePerRun = true,
			lookup = new() { $"{Key()}RuinedTrigger" },
			oncePerCombatTags = new() { $"{Key()}RuinedTag" },
			allPresent = new() { eddie },
			hasArtifacts = new() { Key() },
			lines = new()
			{
				new SaySwitch()
				{
					lines = new()
					{
						new CustomSay()
						{
							who = eddie,
							Text = "Oh come on...",
							loopTag = Manifest.EddieDisappointedAnimation.Tag
						},
						new CustomSay()
						{
							who = eddie,
							Text = "Readjusting...",
							loopTag = Manifest.EddieSquintAnimation.Tag
						},
						new CustomSay()
						{
							who = eddie,
							Text = "Ah, the panels.",
							loopTag = Manifest.EddieSquintAnimation.Tag
						},
						new CustomSay()
						{
							who = eddie,
							Text = "No sudden movements, please!",
							loopTag = Manifest.EddieSquintAnimation.Tag
						},
						new CustomSay()
						{
							who = eddie,
							Text = "Shoot. Let's try that again.",
							loopTag = Manifest.EddieDisappointedAnimation.Tag
						},
						new CustomSay()
						{
							who = eddie,
							Text = "That's not going to be enough sunglight.",
							loopTag = Manifest.EddieSquintAnimation.Tag
						}
					}
				},
				new SaySwitch()
				{
					lines = new()
					{
						new CustomSay()
						{
							who = "comp",
							Text = "We have bigger fish to fry.",
							loopTag = "lean"
						},
						new CustomSay()
						{
							who = Deck.eunice.Key(),
							Text = "Yeah, you take care of that.",
							loopTag = "neutral"
						},
						new CustomSay()
						{
							who = Deck.hacker.Key(),
							Text = "That's about what I expected.",
							loopTag = "neutral"
						},
						new CustomSay()
						{
							who = Deck.hacker.Key(),
							Text = "I'll see what I can do...",
							loopTag = "neutral"
						},
						new CustomSay()
						{
							who = Deck.hacker.Key(),
							Text = "That was inevitable.",
							loopTag = "neutral"
						},
						new CustomSay()
						{
							who = Deck.riggs.Key(),
							Text = "Oops!",
							loopTag = "neutral"
						},
						new CustomSay()
						{
							who = Deck.riggs.Key(),
							Text = "That was our only option.",
							loopTag = "neutral"
						},
						new CustomSay()
						{
							who = Deck.riggs.Key(),
							Text = "It was for the greater good.",
							loopTag = "neutral"
						},
						new CustomSay()
						{
							who = Deck.goat.Key(),
							Text = "Oh, let me help you with that.",
							loopTag = "neutral"
						},
						new CustomSay()
						{
							who = Deck.goat.Key(),
							Text = "That's an easy fix.",
							loopTag = "writing"
						},
						new CustomSay()
						{
							who = Deck.dizzy.Key(),
							Text = "We didn't want to shield that.",
							loopTag = "neutral"
						},
						new CustomSay()
						{
							who = Deck.dizzy.Key(),
							Text = "Shields are important too.",
							loopTag = "neutral"
						},
						new CustomSay()
						{
							who = Deck.shard.Key(),
							Text = "Sorry, sun plates!",
							loopTag = "paws"
						},
						new CustomSay()
						{
							who = Deck.shard.Key(),
							Text = "We'll fix them together!",
							loopTag = "neutral"
						},
					}
				}
			}
		};
	}
}