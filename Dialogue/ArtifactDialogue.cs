using TheJazMaster.Eddie.Artifacts;

namespace TheJazMaster.Eddie;

internal static class ArtifactDialogue
{
	internal static void Inject()
	{
		foreach (var artifactType in Manifest.AllArtifacts)
		{
			if (Activator.CreateInstance(artifactType) is not IRegisterableArtifact artifact)
				continue;
			artifact.InjectDialogue();
		}

		string eddie = Manifest.EddieDeck.GlobalName;

		DB.story.all[$"ArtifactEnergyPrep_{eddie}"] = new()
		{
			type = NodeType.combat,
			oncePerRun = true,
			lookup = new() { $"EnergyPrepTrigger" },
			allPresent = new() { eddie },
			hasArtifacts = new() { "EnergyPrep" },
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Turns out switching off the water heater was enough to fill these batteries.",
					loopTag = Manifest.EddieDefaultAnimation.Tag
				},
				new SaySwitch()
				{
					lines = new()
					{
						new CustomSay()
						{
							who = "comp",
							Text = "We have a water heater?",
							loopTag = "neutral"
						},
						new CustomSay()
						{
							who = Deck.eunice.Key(),
							Text = "I froze my butt off in the shower!",
							loopTag = "squint"
						}
					}
				}
			}
		};
		DB.story.all[$"ArtifactJetThrusters_{eddie}"] = new()
		{
			type = NodeType.combat,
			oncePerRun = true,
			allPresent = new() { eddie },
			hasArtifacts = new() { "JetThrusters" },
			maxTurnsThisCombat = 1,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "These thrusters are giving me space sickness.",
					loopTag = Manifest.EddieAnnoyedAnimation.Tag
				}
			}
		};

		DB.story.all[$"ArtifactEnergyRefund_{eddie}"] = new()
		{
			type = NodeType.combat,
			oncePerRun = true,
			oncePerCombatTags = new() { "EnergyRefund" },
			allPresent = new() { eddie },
			hasArtifacts = new() { "EnergyRefund" },
			minCostOfCardJustPlayed = 3,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "That refund's coming in handy.",
					loopTag = Manifest.EddieDefaultAnimation.Tag
				}
			}
		};
		DB.story.all[$"ArtifactEnergyRefund_{eddie}"] = new()
		{
			type = NodeType.combat,
			oncePerRun = true,
			oncePerCombatTags = new() { "EnergyRefund" },
			allPresent = new() { eddie },
			hasArtifacts = new() { "EnergyRefund" },
			minCostOfCardJustPlayed = 3,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "It's free energy.",
					loopTag = Manifest.EddieExplainsAnimation.Tag
				}
			}
		};

		DB.story.all[$"ArtifactFractureDetection_{eddie}"] = new()
		{
			type = NodeType.combat,
			oncePerRun = true,
			oncePerCombatTags = new() { "FractureDetectionBarks" },
			allPresent = new() { eddie },
			hasArtifacts = new() { "FractureDetection" },
			maxTurnsThisCombat = 1,
			turnStart = true,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Battle plan: shoot everything, find that fracture.",
					loopTag = Manifest.EddieDefaultAnimation.Tag
				}
			}
		};
		DB.story.all[$"ArtifactFractureDetection_{eddie}"] = new()
		{
			type = NodeType.combat,
			oncePerRun = true,
			oncePerCombatTags = new() { "FractureDetectionBarks" },
			allPresent = new() { eddie },
			hasArtifacts = new() { "FractureDetection" },
			maxTurnsThisCombat = 1,
			turnStart = true,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Finding this fracture is gonna take FOREVER.",
					loopTag = Manifest.EddieSquintAnimation.Tag
				}
			}
		};

		DB.story.all[$"ArtifactGeminiCoreBooster_{eddie}"] = new()
		{
			type = NodeType.combat,
			oncePerRunTags = new() { "GeminiCoreBooster" },
			allPresent = new() { eddie },
			hasArtifacts = new() { "GeminiCoreBooster" },
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "I never would have guessed.",
					loopTag = Manifest.EddieSquintAnimation.Tag
				}
			}
		};

		DB.story.all[$"ArtifactJumperCables_{eddie}"] = new()
		{
			type = NodeType.combat,
			oncePerRunTags = new() { "ArtifactJumperCablesReady" },
			allPresent = new() { eddie },
			hasArtifacts = new() { "JumperCables" },
			maxTurnsThisCombat = 1,
			maxHullPercent = 0.5,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Jump-starting a ship? Easy-peasy.",
					loopTag = Manifest.EddieExplainsAnimation.Tag
				}
			}
		};

		DB.story.all[$"ArtifactPowerDiversionMade{eddie}AttackFail"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie, Deck.peri.Key() },
			hasArtifacts = new() { "PowerDiversion" },
			playerShotJustHit = true,
			maxDamageDealtToEnemyThisAction = 0,
			whoDidThat = (Deck)Manifest.EddieDeck.Id!.Value,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Hey, what gives?",
					loopTag = Manifest.EddieAnnoyedAnimation.Tag
				},
				new CustomSay()
				{
					who = Deck.peri.Key(),
					Text = "You've had your fun.",
					loopTag = "nap"
				}
			}
		};
		DB.story.all[$"ArtifactOverclockedGenerator_{eddie}"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
			hasArtifacts = new() { "OverclockedGenerator" },
			turnStart = true,
			maxTurnsThisCombat = 1,
			oncePerCombatTags = new() { "OverclockedGenerator" },
			oncePerRun = true,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "The last time I overclocked a generator, the entire ship burnt down.",
					loopTag = Manifest.EddieExplainsAnimation.Tag
				},
				new SaySwitch()
				{
					lines = new()
					{
						new CustomSay()
						{
							who = Deck.hacker.Key(),
							Text = "That one was pretty funny.",
							loopTag = "smile"
						},
					}
				},
				new SaySwitch()
				{
					lines = new()
					{
						new CustomSay()
						{
							who = Deck.eunice.Key(),
							Text = "Yeah! Sabotage!",
							loopTag = "neutral"
						}
					}
				},
				new SaySwitch()
				{
					lines = new()
					{
						new CustomSay()
						{
							who = Deck.peri.Key(),
							Text = "...",
							loopTag = "panic"
						}
					}
				}
			}
		};

		DB.story.all[$"ArtifactCrosslink_{eddie}"] = new()
		{
			type = NodeType.combat,
			lookup = new() { "CrosslinkTrigger" },
      		oncePerCombatTags = new() { "CrosslinkTriggerTag" },
			allPresent = new() { eddie },
			nonePresent = new() { Deck.peri.Key() },
			hasArtifacts = new() { "Crosslink" },
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Don't tell Peri this, but I totally dismantled the Cobalt's crosslink a few weeks ago.",
					loopTag = Manifest.EddieFurtiveAnimation.Tag
				}
			}
		};

		DB.story.GetNode("ArtifactGeminiCore_Multi_4")?.lines.OfType<SaySwitch>().LastOrDefault()?.lines.Insert(0, new CustomSay()
		{
			who = eddie,
			Text = "I never would have guessed.",
			loopTag = Manifest.EddieDefaultAnimation.Tag
		});

		DB.story.all[$"ArtifactIonConverter_{eddie}_0"] = new()
		{
			type = NodeType.combat,
			lookup = new() { "IonConverterTrigger" },
      		oncePerCombatTags = new() { "IonConverterTag" },
			allPresent = new() { eddie },
			hasArtifacts = new() { "IonConverter" },
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Such luxury.",
					loopTag = Manifest.EddieDefaultAnimation.Tag
				}
			}
		};
		DB.story.all[$"ArtifactIonConverter_{eddie}_1"] = new()
		{
			type = NodeType.combat,
			lookup = new() { "IonConverterTrigger" },
      		oncePerCombatTags = new() { "IonConverterTag" },
			allPresent = new() { eddie },
			hasArtifacts = new() { "IonConverter" },
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

		DB.story.all[$"ArtifactStandbyMode_{eddie}_0"] = new()
		{
			type = NodeType.combat,
			oncePerRun = true,
			oncePerCombatTags = new() { "StandbyMode" },
			allPresent = new() { eddie, "comp" },
			hasArtifacts = new() { "StandbyMode" },
			maxTurnsThisCombat = 1,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Awesome, I can use the standby mode to procrastinate.",
					loopTag = Manifest.EddieDefaultAnimation.Tag
				},
				new CustomSay()
				{
					who = "comp",
					Text = "That's not helpful.",
					loopTag = "squint"
				}
			}
		};

		
	}
}