using TheJazMaster.Eddie.Artifacts;
using TwosCompany;

namespace TheJazMaster.Eddie;

internal static class ArtifactDialogue
{
	internal static void Inject()
	{
		foreach (var artifactType in ModEntry.AllArtifacts)
		{
			if (Activator.CreateInstance(artifactType) is not IDialogueArtifact artifact)
				continue;
			artifact.InjectDialogue();
		}

		string eddie = ModEntry.Instance.EddieDeck.Key();

		DB.story.all[$"ArtifactEnergyPrep_{eddie}"] = new()
		{
			type = NodeType.combat,
			oncePerRun = true,
			allPresent = [eddie],
			hasArtifacts = ["EnergyPrep"],
			doesNotHaveArtifacts = ["ControlRods"],
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Turns out switching off the water heater was enough to fill these batteries.",
					loopTag = ModEntry.Instance.NeutralAnim
				},
				new SaySwitch()
				{
					lines = [
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
					]
				}
			]
		};

		DB.story.all[$"ArtifactEnergyPrep_{eddie}_Ares"] = new()
		{
			type = NodeType.combat,
			oncePerRun = true,
			allPresent = [eddie],
			hasArtifacts = ["EnergyPrep", "ControlRods"],
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "The inactive cannon was still using power during FTL, so I switched that off.",
					loopTag = ModEntry.Instance.NeutralAnim
				}
			]
		};

		DB.story.all[$"ArtifactJetThrusters_{eddie}"] = new()
		{
			type = NodeType.combat,
			oncePerRun = true,
			allPresent = [eddie],
			hasArtifacts = ["JetThrusters"],
			maxTurnsThisCombat = 1,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "These thrusters are giving me space sickness.",
					loopTag = ModEntry.Instance.AnnoyedAnim
				}
			]
		};

		DB.story.all[$"ArtifactEnergyRefund_{eddie}_0"] = new()
		{
			type = NodeType.combat,
			oncePerRun = true,
			oncePerCombatTags = ["EnergyRefund"],
			allPresent = [eddie],
			hasArtifacts = ["EnergyRefund"],
			minCostOfCardJustPlayed = 3,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "That refund's coming in handy.",
					loopTag = ModEntry.Instance.NeutralAnim
				}
			]
		};
		DB.story.all[$"ArtifactEnergyRefund_{eddie}_1"] = new()
		{
			type = NodeType.combat,
			oncePerRun = true,
			oncePerCombatTags = ["EnergyRefund"],
			allPresent = [eddie],
			hasArtifacts = ["EnergyRefund"],
			minCostOfCardJustPlayed = 3,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "It's free energy.",
					loopTag = ModEntry.Instance.ExplainsAnim
				}
			]
		};
		DB.story.all[$"ArtifactEnergyRefund_{eddie}_2"] = new()
		{
			type = NodeType.combat,
			oncePerRun = true,
			oncePerCombatTags = ["EnergyRefund"],
			allPresent = [eddie],
			hasArtifacts = ["EnergyRefund"],
			minCostOfCardJustPlayed = 3,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "A refund? For free?",
					loopTag = ModEntry.Instance.ExcitedAnim
				}
			]
		};

		DB.story.all[$"ArtifactFractureDetection_{eddie}"] = new()
		{
			type = NodeType.combat,
			oncePerRun = true,
			oncePerCombatTags = ["FractureDetectionBarks"],
			allPresent = [eddie],
			hasArtifacts = ["FractureDetection"],
			maxTurnsThisCombat = 1,
			turnStart = true,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Battle plan: shoot everything, find that fracture.",
					loopTag = ModEntry.Instance.NeutralAnim
				}
			]
		};
		DB.story.all[$"ArtifactFractureDetection_{eddie}"] = new()
		{
			type = NodeType.combat,
			oncePerRun = true,
			oncePerCombatTags = ["FractureDetectionBarks"],
			allPresent = [eddie],
			hasArtifacts = ["FractureDetection"],
			maxTurnsThisCombat = 1,
			turnStart = true,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Finding this fracture is gonna take FOREVER.",
					loopTag = ModEntry.Instance.DisappointedAnim
				}
			]
		};

		DB.story.all[$"ArtifactAresCannon_{eddie}"] = new()
		{
			type = NodeType.combat,
			oncePerRunTags = ["AresCannon"],
			allPresent = [eddie],
			hasArtifacts = ["AresCannon"],
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Man, the ergonomics on this ship are terrible.",
					loopTag = ModEntry.Instance.AnnoyedAnim
				}
			]
		};

		DB.story.all[$"ArtifactAresCannonV2_{eddie}"] = new()
		{
			type = NodeType.combat,
			oncePerRunTags = ["AresCannonV2"],
			allPresent = [eddie],
			hasArtifacts = ["AresCannonV2"],
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "This ship? Trying way too hard.",
					loopTag = ModEntry.Instance.NeutralAnim
				}
			]
		};

		DB.story.all[$"ArtifactGeminiCoreBooster_{eddie}"] = new()
		{
			type = NodeType.combat,
			oncePerRunTags = ["GeminiCoreBooster"],
			allPresent = [eddie],
			hasArtifacts = ["GeminiCoreBooster"],
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Damn, the booster ruined the symmetry.",
					loopTag = ModEntry.Instance.AnnoyedAnim
				}
			]
		};

		DB.story.all[$"ArtifactJumperCables_{eddie}"] = new()
		{
			type = NodeType.combat,
			oncePerRunTags = ["ArtifactJumperCablesReady"],
			allPresent = [eddie],
			hasArtifacts = ["JumperCables"],
			maxTurnsThisCombat = 1,
			maxHullPercent = 0.5,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Jump-starting a ship? Easy-peasy.",
					loopTag = ModEntry.Instance.ExplainsAnim
				}
			}
		};

		DB.story.all[$"ArtifactJumperCablesUnneeded_{eddie}"] = new()
		{
			type = NodeType.combat,
			oncePerRunTags = ["ArtifactJumperCablesUnneeded"],
			allPresent = [eddie],
			hasArtifacts = ["JumperCables"],
			maxTurnsThisCombat = 1,
			minHullPercent = 1,
			lines = [
                new CustomSay()
				{
					who = eddie,
					Text = "Uh, does anyone need me to jump-start the ship?",
					loopTag = ModEntry.Instance.DisappointedAnim
				},
				new SaySwitch()
				{
					lines = [
                        new CustomSay()
						{
							who = Deck.dizzy.Key(),
							Text = "Sorry dude.",
							loopTag = "serious"
						},
						new CustomSay()
						{
							who = Deck.goat.Key(),
							Text = "Not really.",
							loopTag = "neutral"
						},
						new CustomSay()
						{
							who = Deck.riggs.Key(),
							Text = "Nah, we're good.",
							loopTag = "neutral"
						},
						new CustomSay()
						{
							who = Deck.hacker.Key(),
							Text = "Maybe next time.",
							loopTag = "serious"
						},
					]
				},
			]
		};

		DB.story.all[$"ArtifactPowerDiversionMade{eddie}AttackFail"] = new()
		{
			type = NodeType.combat,
			allPresent = [eddie, Deck.peri.Key()],
			hasArtifacts = ["PowerDiversion"],
			playerShotJustHit = true,
			maxDamageDealtToEnemyThisAction = 0,
			whoDidThat = ModEntry.Instance.EddieDeck,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Hey, what gives?",
					loopTag = ModEntry.Instance.AnnoyedAnim
				},
				new CustomSay()
				{
					who = Deck.peri.Key(),
					Text = "You've had your fun.",
					loopTag = "nap"
				}
			]
		};
		DB.story.all[$"ArtifactOverclockedGenerator_{eddie}"] = new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
			hasArtifacts = ["OverclockedGenerator"],
			turnStart = true,
			maxTurnsThisCombat = 1,
			oncePerCombatTags = ["OverclockedGenerator"],
			oncePerRun = true,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "The last time I overclocked a generator, the entire ship burnt down.",
					loopTag = ModEntry.Instance.ExplainsAnim
				},
				new SaySwitch()
				{
					lines = [
                        new CustomSay()
						{
							who = Deck.hacker.Key(),
							Text = "That one was pretty funny.",
							loopTag = "smile"
						},
                        new CustomSay()
						{
							who = Deck.eunice.Key(),
							Text = "Yeah! Sabotage!",
							loopTag = "neutral"
						},
                        new CustomSay()
						{
							who = Deck.peri.Key(),
							Text = "...",
							loopTag = "panic"
						}
					]
				}
			]
		};

		DB.story.all[$"ArtifactCrosslink_{eddie}"] = new()
		{
			type = NodeType.combat,
			lookup = ["CrosslinkTrigger"],
      		oncePerCombatTags = ["CrosslinkTriggerTag"],
			allPresent = [eddie],
			nonePresent = [Deck.peri.Key()],
			hasArtifacts = ["Crosslink"],
			oncePerRun = true,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Don't tell Peri this, but I totally dismantled the Cobalt's crosslink a few weeks ago.",
					loopTag = ModEntry.Instance.OnEdgeAnim
				}
			]
		};

		DB.story.GetNode("ArtifactGeminiCore_Multi_4")?.lines.OfType<SaySwitch>().LastOrDefault()?.lines.Insert(0, new CustomSay()
		{
			who = eddie,
			Text = "I never would have guessed.",
			loopTag = ModEntry.Instance.SquintAnim
		});

		DB.story.all[$"ArtifactIonConverter_{eddie}_0"] = new()
		{
			type = NodeType.combat,
			lookup = ["IonConverterTrigger"],
      		oncePerCombatTags = ["IonConverterTag"],
			allPresent = [eddie],
			hasArtifacts = ["IonConverter"],
			oncePerRun = true,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Such luxury.",
					loopTag = ModEntry.Instance.NeutralAnim
				}
			]
		};
		DB.story.all[$"ArtifactIonConverter_{eddie}_1"] = new()
		{
			type = NodeType.combat,
			lookup = ["IonConverterTrigger"],
      		oncePerCombatTags = ["IonConverterTag"],
			allPresent = [eddie],
			hasArtifacts = ["IonConverter"],
			oncePerRun = true,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Now that's what I call efficiency.",
					loopTag = ModEntry.Instance.NeutralAnim
				}
			]
		};

		DB.story.all[$"ArtifactStandbyMode_{eddie}_0"] = new()
		{
			type = NodeType.combat,
			oncePerRun = true,
			oncePerCombatTags = ["StandbyMode"],
			allPresent = [eddie, "comp"],
			hasArtifacts = ["StandbyMode"],
			maxTurnsThisCombat = 1,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Awesome, I can use the standby mode to procrastinate.",
					loopTag = ModEntry.Instance.NeutralAnim
				},
				new CustomSay()
				{
					who = "comp",
					Text = "That's not helpful.",
					loopTag = "squint"
				}
			]
		};

		
		ModEntry.Instance.Helper.ModRegistry.AwaitApiOrNull<ITwosAPI>("Mezz.TwosCompany", (api) => {
			if (api == null) return;
			string sorrelKey = "mezz_Sorrel";

			DB.story.all[$"ArtifactOverclockedGenerator_{eddie}"].lines.OfType<SaySwitch>().Last().lines.AddRange([
				new CustomSay()
				{
					who = api.IlyaDeck.GlobalName,
					Text = "I should try that...",
					loopTag = "intense"
				},
				new CustomSay()
				{
					who = api.GaussDeck.GlobalName,
					Text = "Something tells me you didn't JUST overclock that generator.",
					loopTag = "squint"
				}
			]);

			DB.story.all[$"ArtifactJumperCablesUnneeded_{eddie}"].lines.OfType<SaySwitch>().Last().lines.AddRange([
				new CustomSay()
				{
					who = sorrelKey,
					Text = "Count your peaceful days before they are gone.",
					loopTag = "neutral"
				},
			]);

			DB.story.all[$"ArtifactEnergyPrep_{eddie}"].lines.OfType<SaySwitch>().Last().lines.AddRange([
				new CustomSay()
				{
					who = api.NolaDeck.GlobalName,
					Text = "The hardware of this operation leaves a lot to be desired.",
					loopTag = "annoyed"
				},
			]);
		});
	}
}