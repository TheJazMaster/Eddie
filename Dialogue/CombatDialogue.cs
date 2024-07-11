using TheJazMaster.Eddie.DialogueAdditions;

namespace TheJazMaster.Eddie;

internal static class CombatDialogue
{
	internal static Manifest Instance => Manifest.Instance;
	private static IKokoroApi KokoroApi => Instance.KokoroApi;

	internal static void Inject()
	{
		string eddie = Manifest.EddieDeck.GlobalName;

		DB.story.all[$"BlockedALotOfAttacksWithArmor_{eddie}"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
			oncePerRun = true,
			oncePerCombatTags = new() { "YowzaThatWasALOTofArmorBlock" },
			enemyShotJustHit = true,
			minDamageBlockedByPlayerArmorThisTurn = 3,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Who needs to do anything when you have armor.",
					loopTag = Manifest.EddieDefaultAnimation.Tag
				}
			}
		};
		DB.story.all[$"DizzyWentMissing_{eddie}"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
			oncePerCombatTags = new() { "dizzyWentMissing" },
			lastTurnPlayerStatuses = new() { Status.missingDizzy },
			priority = true,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Dizzy? This isn't funny.",
					loopTag = Manifest.EddieWorriedAnimation.Tag
				}
			}
		};
		DB.story.all[$"RiggsWentMissing_{eddie}"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
			oncePerCombatTags = new() { "riggsWentMissing" },
			lastTurnPlayerStatuses = new() { Status.missingRiggs },
			priority = true,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "I'm sure she won't mind if I take her boba.",
					loopTag = Manifest.EddieExplainsAnimation.Tag
				}
			}
		};
		DB.story.all[$"PeriWentMissing_{eddie}"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
			oncePerCombatTags = new() { "periWentMissing" },
			lastTurnPlayerStatuses = new() { Status.missingPeri },
			priority = true,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Does this mean I'm off the hook?",
					loopTag = Manifest.EddieOnEdgeAnimation.Tag
				}
			}
		};
		DB.story.all[$"IsaacWentMissing_{eddie}"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
			oncePerCombatTags = new() { "isaacWentMissing" },
			lastTurnPlayerStatuses = new() { Status.missingIsaac },
			priority = true,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "He'll be back, right? I'm sure he'll be back.",
					loopTag = Manifest.EddieWorriedAnimation.Tag
				}
			}
		};
		DB.story.all[$"DrakeWentMissing_{eddie}"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
			oncePerCombatTags = new() { "drakeWentMissing" },
			lastTurnPlayerStatuses = new() { Status.missingDrake },
			priority = true,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "I thought we had something...",
					loopTag = Manifest.EddieDisappointedAnimation.Tag
				}
			}
		};
		DB.story.all[$"MaxWentMissing_{eddie}"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
			oncePerCombatTags = new() { "maxWentMissing" },
			lastTurnPlayerStatuses = new() { Status.missingMax },
			priority = true,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "MAX!",
					loopTag = Manifest.EddieOnEdgeAnimation.Tag
				}
			}
		};
		DB.story.all[$"MaxWentMissing_{eddie}"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
			oncePerCombatTags = new() { "maxWentMissing" },
			lastTurnPlayerStatuses = new() { Status.missingMax },
			priority = true,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Max doesn't die. He respawns!",
					loopTag = Manifest.EddieSquintAnimation.Tag
				}
			}
		};
		DB.story.all[$"BooksWentMissing_{eddie}"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
			oncePerCombatTags = new() { "booksWentMissing" },
			lastTurnPlayerStatuses = new() { Status.missingBooks },
			priority = true,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Is this time magic again?",
					loopTag = Manifest.EddieSquintAnimation.Tag
				}
			}
		};
		DB.story.all[$"CatWentMissing_{eddie}"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
			oncePerCombatTags = new() { "CatWentMissing" },
			lastTurnPlayerStatuses = new() { Status.missingCat },
			priority = true,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Oh, did I do something wrong again?",
					loopTag = Manifest.EddieOnEdgeAnimation.Tag
				}
			}
		};
		// DB.story.all[$"{eddie}JustHitBig_0"] = new() //TODO: make gamma ray trigger this
		// {
		// 	type = NodeType.combat,
		// 	allPresent = new() { eddie },
		// 	whoDidThat = (Deck)Manifest.EddieDeck.Id!.Value,
		// 	playerShotJustHit = true,
		// 	minDamageDealtToEnemyThisAction = 9,
		// 	lines = new()
		// 	{
		// 		new CustomSay()
		// 		{
		// 			who = eddie,
		// 			Text = "As expected.",
		// 			loopTag = Dialogue.CurrentSmugLoopTag
		// 		},
		// 		new SaySwitch()
		// 		{
		// 			lines = new()
		// 			{
		// 				new CustomSay()
		// 				{
		// 					who = Deck.peri.Key(),
		// 					Text = "...",
		// 					loopTag = "squint"
		// 				},
		// 				new CustomSay()
		// 				{
		// 					who = Deck.shard.Key(),
		// 					Text = "!",
		// 					loopTag = "stoked"
		// 				}
		// 			}
		// 		}
		// 	}
		// };

		DB.story.all[$"Duo_AboutToDieAndLoop_{eddie}0"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie, Deck.dizzy.Key() },
			enemyShotJustHit = true,
			maxHull = 2,
			oncePerCombatTags = new() { "aboutToDie" },
			oncePerRun = true,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Lights out.",
					loopTag = Manifest.EddieDisappointedAnimation.Tag
				},
				new CustomSay()
				{
					who = Deck.dizzy.Key(),
					Text = "At least try to get us out of this.",
					loopTag = "frown"
				},
			}
		};
		DB.story.all[$"Duo_AboutToDieAndLoop_{eddie}1"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie, "comp" },
			enemyShotJustHit = true,
			maxHull = 2,
			oncePerCombatTags = new() { "aboutToDie" },
			oncePerRun = true,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Do we HAVE to try again?",
					loopTag = Manifest.EddieSquintAnimation.Tag
				},
				new CustomSay()
				{
					who = "comp",
					Text = "You can stay behind, if you'd prefer that.",
					loopTag = "squint"
				}
			}
		};
		DB.story.all[$"Duo_AboutToDieAndLoop_{eddie}2"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie, Deck.goat.Key() },
			enemyShotJustHit = true,
			maxHull = 2,
			oncePerCombatTags = new() { "aboutToDie" },
			oncePerRun = true,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Better luck next time, amigos.",
					loopTag = Manifest.EddieExplainsAnimation.Tag
				},
				new CustomSay()
				{
					who = Deck.goat.Key(),
					Text = "Adios...",
					loopTag = "eyesClosed"
				},
			}
		};
		DB.story.all[$"Duo_AboutToDieAndLoop_{eddie}3"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie, Deck.riggs.Key() },
			enemyShotJustHit = true,
			maxHull = 2,
			oncePerCombatTags = new() { "aboutToDie" },
			oncePerRun = true,
			lines = new()
			{
				new CustomSay()
				{
					who = Deck.riggs.Key(),
					Text = "What if we, uh, ram into them at full speed?",
					loopTag = "neutral"
				},
				new CustomSay()
				{
					who = eddie,
					Text = "Worth a shot! I'll rig the ship to explode.",
					loopTag = Manifest.EddieOnEdgeAnimation.Tag
				}
			}
		};
		DB.story.all[$"Duo_AboutToDieAndLoop_{eddie}4"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie, Deck.peri.Key() },
			enemyShotJustHit = true,
			maxHull = 2,
			oncePerCombatTags = new() { "aboutToDie" },
			oncePerRun = true,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "It's gonna look really cool if we make it out of this one.",
					loopTag = Manifest.EddieExplainsAnimation.Tag
				},
				new CustomSay()
				{
					who = Deck.peri.Key(),
					Text = "Then get to it!",
					loopTag = "mad"
				}
			}
		};
		DB.story.all[$"Duo_AboutToDieAndLoop_{eddie}5"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie, Deck.eunice.Key() },
			enemyShotJustHit = true,
			maxHull = 2,
			oncePerCombatTags = new() { "aboutToDie" },
			oncePerRun = true,
			lines = new()
			{
				new CustomSay()
				{
					who = Deck.eunice.Key(),
					Text = "Hey, any tricks up your sleeves?",
					loopTag = "mad"
				},
				new CustomSay()
				{
					who = eddie,
					Text = "I'll give it my best.",
					loopTag = Manifest.EddieDefaultAnimation.Tag
				}
			}
		};
		DB.story.all[$"Duo_AboutToDieAndLoop_{eddie}6"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie, Deck.hacker.Key() },
			enemyShotJustHit = true,
			maxHull = 2,
			oncePerCombatTags = new() { "aboutToDie" },
			oncePerRun = true,
			lines = new()
			{
				new CustomSay()
				{
					who = Deck.hacker.Key(),
					Text = "Game over, man.",
					loopTag = "mad"
				},
				new CustomSay()
				{
					who = eddie,
					Text = "No, there's always a way.",
					loopTag = Manifest.EddieSquintAnimation.Tag
				}
			}
		};
		DB.story.all[$"Duo_AboutToDieAndLoop_{eddie}7"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie, Deck.shard.Key() },
			enemyShotJustHit = true,
			maxHull = 2,
			oncePerCombatTags = new() { "aboutToDie" },
			oncePerRun = true,
			lines = new()
			{
				new CustomSay()
				{
					who = Deck.shard.Key(),
					Text = "Dying sucks!",
					loopTag = "mad"
				},
				new CustomSay()
				{
					who = eddie,
					Text = "At least we'll get some cryo-sleep.",
					loopTag = Manifest.EddieExplainsAnimation.Tag
				}
			}
		};
		DB.story.all[$"Duo_AboutToDieAndLoop_{eddie}8"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie, Deck.hacker.Key() },
			enemyShotJustHit = true,
			maxHull = 2,
			oncePerCombatTags = new() { "aboutToDie" },
			oncePerRun = true,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Max, enter your second phase!",
					loopTag = Manifest.EddieExcitedAnimation.Tag
				},
				new CustomSay()
				{
					who = Deck.hacker.Key(),
					Text = "NGRAAAAAAHHHHHH!",
					loopTag = "squint"
				}
			}
		};
		DB.story.all[$"Duo_AboutToDieAndLoop_{eddie}9"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie, Deck.dizzy.Key() },
			enemyShotJustHit = true,
			maxHull = 2,
			oncePerCombatTags = new() { "aboutToDie" },
			oncePerRun = true,
			lines = new()
			{
				new CustomSay()
				{
					who = Deck.dizzy.Key(),
					Text = "The shields won't hold...",
					loopTag = "serious"
				},
				new CustomSay()
				{
					who = eddie,
					Text = "All power to shields!",
					loopTag = Manifest.EddieWorriedAnimation.Tag
				}
			}
		};
		DB.story.all[$"Duo_AboutToDieAndLoop_{eddie}10"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie, Deck.peri.Key() },
			enemyShotJustHit = true,
			maxHull = 2,
			oncePerCombatTags = new() { "aboutToDie" },
			oncePerRun = true,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Man, I don't want to loop again.",
					loopTag = Manifest.EddieSquintAnimation.Tag
				},
				new CustomSay()
				{
					who = Deck.peri.Key(),
					Text = "Too bad!",
					loopTag = "mad"
				}
			}
		};
		DB.story.all[$"Duo_AboutToDieAndLoop_{eddie}11"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie, Deck.riggs.Key() },
			enemyShotJustHit = true,
			maxHull = 2,
			oncePerCombatTags = new() { "aboutToDie" },
			oncePerRun = true,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Don't worry, I'm sure our pilot will get us out of this.",
					loopTag = Manifest.EddieDefaultAnimation.Tag
				},
				new CustomSay()
				{
					who = Deck.riggs.Key(),
					Text = "I wouldn't count on it!",
					loopTag = "nervous"
				}
			}
		};
		DB.story.all[$"Duo_AboutToDieAndLoop_{eddie}12"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie, Deck.goat.Key() },
			enemyShotJustHit = true,
			maxHull = 2,
			oncePerCombatTags = new() { "aboutToDie" },
			oncePerRun = true,
			lines = new()
			{
				new CustomSay()
				{
					who = Deck.goat.Key(),
					Text = "Today... we got... blown... to... bits...",
					loopTag = "writing"
				},
				new CustomSay()
				{
					who = eddie,
					Text = "Can I get an honorary mention?",
					loopTag = Manifest.EddieDefaultAnimation.Tag
				}
			}
		};
		DB.story.all[$"Duo_AboutToDieAndLoop_{eddie}13"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie, Deck.eunice.Key() },
			enemyShotJustHit = true,
			maxHull = 2,
			oncePerCombatTags = new() { "aboutToDie" },
			oncePerRun = true,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "I'll miss our time together, Drake.",
					loopTag = Manifest.EddieExcitedAnimation.Tag
				},
				new CustomSay()
				{
					who = Deck.eunice.Key(),
					Text = "Okay. I won't.",
					loopTag = "squint"
				}
			}
		};
		DB.story.all[$"Duo_AboutToDieAndLoop_{eddie}14"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie, Deck.shard.Key() },
			enemyShotJustHit = true,
			maxHull = 2,
			oncePerCombatTags = new() { "aboutToDie" },
			oncePerRun = true,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "I've gotten through worse.",
					loopTag = Manifest.EddieDefaultAnimation.Tag
				},
				new CustomSay()
				{
					who = Deck.shard.Key(),
					Text = "That's right! Never give up!",
					loopTag = "stoked"
				}
			}
		};
		DB.story.all[$"Duo_AboutToDieAndLoop_{eddie}15"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie, "comp" },
			enemyShotJustHit = true,
			maxHull = 2,
			oncePerCombatTags = new() { "aboutToDie" },
			oncePerRun = true,
			lines = new()
			{
				new CustomSay()
				{
					who = "comp",
					Text = "Sorry guys! I'll try harder next loop!",
					loopTag = "worried"
				},
				new CustomSay()
				{
					who = eddie,
					Text = "We did our best.",
					loopTag = Manifest.EddieExplainsAnimation.Tag
				}
			}
		};
		if (Manifest.Instance.SogginsApi != null) {
			string soggins = Manifest.Instance.SogginsApi?.SogginsDeck.GlobalName ?? "";
			DB.story.all[$"Duo_AboutToDieAndLoop_{eddie}16"] = new()
			{
				type = NodeType.combat,
				allPresent = new() { eddie, soggins },
				enemyShotJustHit = true,
				maxHull = 2,
				oncePerCombatTags = new() { "aboutToDie" },
				oncePerRun = true,
				lines = new()
				{
					new CustomSay()
					{
						who = eddie,
						Text = "Try pressing more buttons!",
						loopTag = Manifest.EddieExcitedAnimation.Tag
					},
					new CustomSay()
					{
						who = soggins,
						Text = "Works every time.",
						loopTag = "neutral"
					}
				}
			};
			DB.story.all[$"Duo_AboutToDieAndLoop_{eddie}17"] = new()
			{
				type = NodeType.combat,
				allPresent = new() { eddie, soggins },
				enemyShotJustHit = true,
				maxHull = 2,
				oncePerCombatTags = new() { "aboutToDie" },
				oncePerRun = true,
				lines = new()
				{
					new CustomSay()
					{
						who = soggins,
						Text = "This is just like that one time with the missiles.",
						loopTag = "neutral"
					},
					new CustomSay()
					{
						who = eddie,
						Text = "Classic Soggins.",
						loopTag = Manifest.EddieExplainsAnimation.Tag
					}
				}
			};
		}

		DB.story.all[$"EmptyHandWithEnergy_{eddie}_0"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
			handEmpty = true,
			minEnergy = 1,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "How did that happen?",
					loopTag = Manifest.EddieWorriedAnimation.Tag
				}
			}
		};
		DB.story.all[$"EmptyHandWithEnergy_{eddie}_1"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
			handEmpty = true,
			minEnergy = 1,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Oh well, nothing left to do.",
					loopTag = Manifest.EddieExplainsAnimation.Tag
				}
			}
		};

		DB.story.all[$"EnemyArmorHitLots_{eddie}_0"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
			playerShotJustHit = true,
			minDamageBlockedByEnemyArmorThisTurn = 3,
			oncePerCombat = true,
			oncePerRun = true,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "We're in too deep now... We have to shoot the armor more.",
					loopTag = Manifest.EddieDisappointedAnimation.Tag
				},
				new SaySwitch()
				{
					lines = new()
					{
						new CustomSay()
						{
							who = Deck.dizzy.Key(),
							Text = "I don't think that will help.",
							loopTag = "neutral"
						},
						new CustomSay()
						{
							who = Deck.goat.Key(),
							Text = "Oh, um, okay?",
							loopTag = "shy"
						},
						new CustomSay()
						{
							who = Deck.riggs.Key(),
							Text = "Yeah, I agree.",
							loopTag = "neutral"
						},
						new CustomSay()
						{
							who = "comp",
							Text = "That makes no sense.",
							loopTag = "squint"
						},
						new CustomSay()
						{
							who = Deck.peri.Key(),
							Text = "Please don't...",
							loopTag = "mad"
						},
						new CustomSay()
						{
							who = Deck.hacker.Key(),
							Text = "Don't give up just yet.",
							loopTag = "neutral"
						},
					}
				}
			}
		};
		DB.story.all[$"EnemyArmorHitLots_{eddie}_1"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
			playerShotJustHit = true,
			minDamageBlockedByEnemyArmorThisTurn = 1,
			oncePerCombat = true,
			oncePerRun = true,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Aiming at unarmored parts is, like, a lot of effort.",
					loopTag = Manifest.EddieDefaultAnimation.Tag
				},
				new SaySwitch()
				{
					lines = new()
					{
						new CustomSay()
						{
							who = Deck.riggs.Key(),
							Text = "Yeah, I agree.",
							loopTag = "neutral"
						},
						new CustomSay()
						{
							who = Deck.hacker.Key(),
							Text = "Don't give up just yet.",
							loopTag = "neutral"
						},
					}
				}
			}
		};
		DB.story.all[$"EnemyArmorHitLots_{eddie}_2"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
			playerShotJustHit = true,
			minDamageBlockedByEnemyArmorThisTurn = 1,
			oncePerCombat = true,
			oncePerRun = true,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Maybe we can shoot the armor off? No?",
					loopTag = Manifest.EddieDisappointedAnimation.Tag
				},
				new SaySwitch()
				{
					lines = new()
					{
						new CustomSay()
						{
							who = Deck.dizzy.Key(),
							Text = "I don't think that will work.",
							loopTag = "neutral"
						},
						new CustomSay()
						{
							who = "comp",
							Text = "That makes no sense.",
							loopTag = "squint"
						},
						new CustomSay()
						{
							who = Deck.peri.Key(),
							Text = "Please don't...",
							loopTag = "mad"
						},
					}
				}
			}
		};
		DB.story.all[$"EnemyArmorHitLots_{eddie}_3"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
			playerShotJustHit = true,
			minDamageBlockedByEnemyArmorThisTurn = 1,
			oncePerCombat = true,
			oncePerRun = true,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Hey, enemy ship, could you scoot over a little?",
					loopTag = Manifest.EddieDefaultAnimation.Tag
				},
				new SaySwitch()
				{
					lines = new() 
					{
						new CustomSay()
						{
							who = Deck.dizzy.Key(),
							Text = "I don't think that will work.",
							loopTag = "neutral"
						},
						new CustomSay()
						{
							who = Deck.riggs.Key(),
							Text = "Or I could move US over?",
							loopTag = "neutral"
						},
						new CustomSay()
						{
							who = "comp",
							Text = "That makes no sense.",
							loopTag = "squint"
						},
						new CustomSay()
						{
							who = Deck.hacker.Key(),
							Text = "It was worth a shot, I guess.",
							loopTag = "neutral"
						},
					}
				}
			}
		};

    	DB.story.all[$"WizardGeneralShouts_{eddie}_0"] = new() {
      		type = NodeType.combat,
      		turnStart = true,
      		allPresent = new() {"wizard", eddie},
      		enemyIntent = "wizardMagic",
      		lines = new()
			{
				new CustomSay()
				{
					who = "wizard",
					Text = "I cast Itchy Back!",
				},
				new CustomSay()
				{
					who = eddie,
					Text = "Damn, this guy means business!",
					loopTag = Manifest.EddieWorriedAnimation.Tag
				}
			}
    	};

		DB.story.all[$"ExpensiveCardPlayed_{eddie}_0"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
			minCostOfCardJustPlayed = 4,
			oncePerCombatTags = new() { "ExpensiveCardPlayed" },
			oncePerRun = true,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Oops, I think that tripped a breaker.",
					loopTag = Manifest.EddieOnEdgeAnimation.Tag
				}
			}
		};
		DB.story.all[$"ExpensiveCardPlayed_{eddie}_1"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
			minCostOfCardJustPlayed = 4,
			oncePerCombatTags = new() { "ExpensiveCardPlayed" },
			oncePerRun = true,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "I think something caught fire.",
					loopTag = Manifest.EddieDefaultAnimation.Tag
				},
				new SaySwitch()
				{
					lines = new()
					{
						new CustomSay()
						{
							who = Deck.eunice.Key(),
							Text = "Cool.",
							loopTag = "neutral"
						},
						new CustomSay()
						{
							who = Deck.hacker.Key(),
							Text = "Rad.",
							loopTag = "smile"
						}
					}
				}
			}
		};
		DB.story.all[$"ExpensiveCardPlayed_{eddie}_1"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
			minCostOfCardJustPlayed = 4,
			oncePerCombatTags = new() { "ExpensiveCardPlayed" },
			oncePerRun = true,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "We should probably never do that again.",
					loopTag = Manifest.EddieExplainsAnimation.Tag
				},
				new SaySwitch()
				{
					lines = new()
					{
						new CustomSay()
						{
							who = Deck.riggs.Key(),
							Text = "Yeah alright.",
							loopTag = "neutral"
						},
						new CustomSay()
						{
							who = Deck.dizzy.Key(),
							Text = "Gotcha.",
							loopTag = "neutral"
						},
					}
				}
			}
		};

		DB.story.all[$"HandOnlyHasTrashCards_{eddie}"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
			handFullOfTrash = true,
			oncePerCombatTags = new() { "handOnlyHasTrashCards" },
			oncePerRun = true,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Eh, the trash isn't that bad.",
					loopTag = Manifest.EddieDefaultAnimation.Tag
				},
				new SaySwitch()
				{
					lines = new()
					{
						new CustomSay()
						{
							who = Deck.dizzy.Key(),
							Text = "But we have to clean it up eventually.",
							loopTag = "squint"
						}
					}
				}
			}
		};
		DB.story.all[$"HandOnlyHasUnplayableCards_{eddie}"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
			handFullOfUnplayableCards = true,
			oncePerCombatTags = new() { "handFullOfUnplayableCards" },
			oncePerRun = true,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "This is all pretty useless.",
					loopTag = Manifest.EddieSquintAnimation.Tag
				}
			}
		};

		DB.story.all[$"WeDontOverlapWithEnemyAtAll_{eddie}_0"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
			shipsDontOverlapAtAll = true,
			oncePerCombatTags = new() { "NoOverlapBetweenShips" },
			priority = true,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "I guess that works.",
					loopTag = Manifest.EddieDefaultAnimation.Tag
				}
			}
		};
		DB.story.all[$"WeDontOverlapWithEnemyAtAll_{eddie}_1"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
			shipsDontOverlapAtAll = true,
			oncePerCombatTags = new() { "NoOverlapBetweenShips" },
			priority = true,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Nice. Stay there, please.",
					loopTag = Manifest.EddieDefaultAnimation.Tag
				}
			}
		};
		DB.story.all[$"WeDontOverlapWithEnemyAtAll_{eddie}_2"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
			shipsDontOverlapAtAll = true,
			oncePerCombatTags = new() { "NoOverlapBetweenShips" },
			priority = true,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Yeah, stay there if you know what's good for you!",
					loopTag = Manifest.EddieExcitedAnimation.Tag
				}
			}
		};

		DB.story.all[$"WeDontOverlapWithEnemyAtAllButWeDoHaveASeekerToDealWith_{eddie}"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
			shipsDontOverlapAtAll = true,
			oncePerCombatTags = new() { "NoOverlapBetweenShipsSeeker" },
			anyDronesHostile = new() { "missile_seeker" },
			nonePresent = new() { "crab" },
			priority = true,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Hm... I feel like we're forgetting something.",
					loopTag = Manifest.EddieSquintAnimation.Tag
				},
				new SaySwitch()
				{
					lines = new()
					{
						new CustomSay()
						{
							who = Deck.eunice.Key(),
							Text = "Are you kidding me?",
							loopTag = "mad"
						},
						new CustomSay()
						{
							who = "comp",
							Text = "You don't say...",
							loopTag = "squint"
						},
					}
				}
			}
		};

		DB.story.all[$"ManyTurns_{eddie}"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
			minTurnsThisCombat = 9,
			oncePerCombatTags = new() { "manyTurns" },
			oncePerRun = true,
			turnStart = true,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "I need a siesta after this.",
					loopTag = Manifest.EddieDefaultAnimation.Tag
				}
			}
		};
		DB.story.all[$"ManyTurns_{eddie}"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
			minTurnsThisCombat = 9,
			oncePerCombatTags = new() { "manyTurns" },
			oncePerRun = true,
			turnStart = true,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "I'm getting tired. Can we have a coffee break?",
					loopTag = Manifest.EddieSquintAnimation.Tag
				}
			}
		};
		DB.story.all[$"ManyTurns_{eddie}"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie, Deck.riggs.Key() },
			minTurnsThisCombat = 9,
			hasArtifacts = new() { "CaffeineRush" },
			oncePerCombatTags = new() { "manyTurns" },
			oncePerRun = true,
			turnStart = true,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "I'm getting tired. Can we have a coffee break?",
					loopTag = Manifest.EddieSquintAnimation.Tag
				},
				new CustomSay()
				{
					who = Deck.riggs.Key(),
					Text = "You can have some of mine.",
					loopTag = "bobaSlurp"
				}
			}
		};

		DB.story.all[$"OverheatCatFix_{eddie}"] = new()
		{
			type = NodeType.combat,
			wasGoingToOverheatButStopped = true,
			whoDidThat = Deck.colorless,
			allPresent = new() { "comp" },
			oncePerCombatTags = new() { "OverheatCatFix" },
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Good call, gotta keep the heat contained!",
					loopTag = Manifest.EddieExcitedAnimation.Tag
				},
				new CustomSay()
				{
					who = "comp",
					Text = "Yes, that's exactly what I was going for.",
					loopTag = "squint"
				}
			}
		};
		DB.story.GetNode("OverheatDrakeFix_Multi_6")?.lines.OfType<SaySwitch>().FirstOrDefault()?.lines.Insert(0, new CustomSay()
		{
			who = eddie,
			Text = "Good call, gotta keep the heat contained!",
			loopTag = Manifest.EddieExcitedAnimation.Tag
		});

		DB.story.all["WeJustGainedHeatAndDrakeIsHere_{eddie}_0"] = new()
		{
      		type = NodeType.combat,
      		lastTurnPlayerStatuses = new() { Status.heat },
      		allPresent = new() { eddie, Deck.eunice.Key() },
      		oncePerCombatTags = new() { "DrakeCanYouDoSomethingAboutTheHeatPlease" },
      		lines = new()
        	{
				new SaySwitch()
				{
					lines = new()
					{
						new CustomSay()
						{
							who = eddie,
							Text = "Wow, this is nice!",
							loopTag = Manifest.EddieExcitedAnimation.Tag
						},
						new CustomSay()
						{
							who = eddie,
							Text = "I did feel a little cold before.",
							loopTag = Manifest.EddieDefaultAnimation.Tag
						},
						new CustomSay()
						{
							who = eddie,
							Text = "Ah, cozy...",
							loopTag = Manifest.EddieDefaultAnimation.Tag
						},
						new CustomSay()
						{
							who = eddie,
							Text = "A little heat never hurt anyone.",
							loopTag = Manifest.EddieDefaultAnimation.Tag
						},
						new CustomSay()
						{
							who = eddie,
							Text = "I like this temperature more.",
							loopTag = Manifest.EddieDefaultAnimation.Tag
						},
						new CustomSay()
						{
							who = eddie,
							Text = "Yeah! Crank it up!",
							loopTag = Manifest.EddieExcitedAnimation.Tag
						},
						new CustomSay()
						{
							who = eddie,
							Text = "We'll deal with the consequences later.",
							loopTag = Manifest.EddieDefaultAnimation.Tag
						},
						new CustomSay()
						{
							who = eddie,
							Text = "We should always heat the ship like this.",
							loopTag = Manifest.EddieExplainsAnimation.Tag
						},
						new CustomSay()
						{
							who = eddie,
							Text = "We should do this more often.",
							loopTag = Manifest.EddieExplainsAnimation.Tag
						},
						new CustomSay()
						{
							who = eddie,
							Text = "I work better in the heat.",
							loopTag = Manifest.EddieExplainsAnimation.Tag
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
							Text = "I know, right?",
							loopTag = "sly"
						},
						new CustomSay()
						{
							who = Deck.eunice.Key(),
							Text = "Finally, someone gets it.",
							loopTag = "neutral"
						},
						new CustomSay()
						{
							who = Deck.eunice.Key(),
							Text = "Exactly!",
							loopTag = "neutral"
						},
						new CustomSay()
						{
							who = Deck.eunice.Key(),
							Text = "That's what I'm saying!",
							loopTag = "neutral"
						},
						new CustomSay()
						{
							who = Deck.eunice.Key(),
							Text = "...Alright, don't make it weird.",
							loopTag = "mad"
						},
					}
				}
			}
		};

		DB.story.all[$"OverheatGeneric_{eddie}"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
			goingToOverheat = true,
			oncePerCombatTags = new() { "OverheatGeneric" },
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Ah, the consequences...",
					loopTag = Manifest.EddieAnnoyedAnimation.Tag
				}
			}
		};
		DB.story.all[$"OverheatGeneric_{eddie}"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
			goingToOverheat = true,
			oncePerCombatTags = new() { "OverheatGeneric" },
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "I'll miss you, heat.",
					loopTag = Manifest.EddieDisappointedAnimation.Tag
				}
			}
		};

		DB.story.all[$"PlayedManyCards_{eddie}_0"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
			handEmpty = true,
			minCardsPlayedThisTurn = 6,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "See? It all worked out.",
					loopTag = Manifest.EddieExcitedAnimation.Tag
				},
			}
		};

		DB.story.all[$"ThatsALotOfDamageToThem_{eddie}"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
			playerShotJustHit = true,
			minDamageDealtToEnemyThisTurn = 10,
			lines = new()
			{
				new SaySwitch() {
					lines = new()
					{
						new CustomSay()
						{
							who = eddie,
							Text = "Nice.",
							loopTag = Manifest.EddieDefaultAnimation.Tag
						},
						new CustomSay()
						{
							who = eddie,
							Text = "Awesome.",
							loopTag = Manifest.EddieDefaultAnimation.Tag
						}
					}
				}
			}
		};
		DB.story.all[$"ThatsALotOfDamageToThem_{eddie}"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
			playerShotJustHit = true,
			minDamageDealtToEnemyThisTurn = 10,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Let's do that again!",
					loopTag = Manifest.EddieExcitedAnimation.Tag
				}
			}
		};

		DB.story.all[$"ThatsALotOfDamageToUs_{eddie}"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
			enemyShotJustHit = true,
			minDamageDealtToPlayerThisTurn = 3,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "That was wild!",
					loopTag = Manifest.EddieDefaultAnimation.Tag
				}
			}
		};

		DB.story.GetNode("TookDamageHave2HP_Multi_1")?.lines.OfType<SaySwitch>().FirstOrDefault()?.lines.Insert(0, new CustomSay()
		{
			who = eddie,
			Text = "Nearly, eh? That's fine.",
			loopTag = Manifest.EddieExcitedAnimation.Tag
		});

		DB.story.all[$"TookZeroDamageAtLowHealth_{eddie}"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
			enemyShotJustHit = true,
			maxDamageDealtToPlayerThisTurn = 0,
			maxHull = 2,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "That's the power of determination.",
					loopTag = Manifest.EddieDefaultAnimation.Tag
				}
			}
		};
		DB.story.all[$"WeAreCorroded_{eddie}"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
			lastTurnPlayerStatuses = new() { Status.corrode },
			oncePerRun = true,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "This is gonna be a lot of work to fix.",
					loopTag = Manifest.EddieSquintAnimation.Tag
				}
			}
		};
		DB.story.all[$"IMissedOopsie_{eddie}_0"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
			whoDidThat = (Deck)Manifest.EddieDeck.Id!.Value,
			playerShotJustMissed = true,
			oncePerCombat = true,
			doesNotHaveArtifacts = new() { "Recalibrator", "GrazerBeam" },
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "That was a test shot.",
					loopTag = Manifest.EddieDefaultAnimation.Tag
				},
				new SaySwitch()
				{
					lines = new()
					{
						new CustomSay()
						{
							who = Deck.eunice.Key(),
							Text = "Uh-huh.",
							loopTag = "squint"
						},
						new CustomSay()
						{
							who = Deck.goat.Key(),
							Text = "What were you testing?",
							loopTag = "neutral"
						}
					}
				}
			}
		};
		DB.story.all[$"IMissedOopsie_{eddie}_1"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
			whoDidThat = (Deck)Manifest.EddieDeck.Id!.Value,
			playerShotJustMissed = true,
			oncePerCombat = true,
			doesNotHaveArtifacts = new() { "Recalibrator", "GrazerBeam" },
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Oops, wrong button.",
					loopTag = Manifest.EddieSquintAnimation.Tag
				},
			}
		};
		DB.story.all[$"IMissedOopsie_{eddie}_2"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
			whoDidThat = (Deck)Manifest.EddieDeck.Id!.Value,
			playerShotJustMissed = true,
			oncePerCombat = true,
			doesNotHaveArtifacts = new() { "Recalibrator", "GrazerBeam" },
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Wow, they're totally cheating.",
					loopTag = Manifest.EddieSquintAnimation.Tag
				},
				new SaySwitch()
				{
					lines = new()
					{
						new CustomSay()
						{
							who = Deck.eunice.Key(),
							Text = "Let me aim that thing!",
							loopTag = "mad"
						},
						new CustomSay()
						{
							who = Deck.hacker.Key(),
							Text = "Must have been the cosmic particles.",
							loopTag = "neutral"
						}
					}
				}
			}
		};
		DB.story.all[$"WeMissedOopsie_{eddie}_0"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
			playerShotJustMissed = true,
			oncePerCombat = true,
			doesNotHaveArtifacts = new() { "Recalibrator", "GrazerBeam" },
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "No biggie.",
					loopTag = Manifest.EddieDefaultAnimation.Tag
				}
			}
		};
		DB.story.all[$"WeMissedOopsie_{eddie}_1"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
			playerShotJustMissed = true,
			oncePerCombat = true,
			doesNotHaveArtifacts = new() { "Recalibrator", "GrazerBeam" },
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Happens to the best of us.",
					loopTag = Manifest.EddieDefaultAnimation.Tag
				},
				new SaySwitch()
				{
					lines = new()
					{
						new CustomSay()
						{
							who = Deck.peri.Key(),
							Text = "Let's not make this a habit.",
							loopTag = "mad"
						}
					}
				}
			}
		};
		DB.story.all[$"WeMissedOopsie_{eddie}_2"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
			playerShotJustMissed = true,
			oncePerCombat = true,
			doesNotHaveArtifacts = new() { "Recalibrator", "GrazerBeam" },
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "We'll get 'em next time.",
					loopTag = Manifest.EddieDefaultAnimation.Tag
				},
				new SaySwitch()
				{
					lines = new()
					{
						new CustomSay()
						{
							who = Deck.goat.Key(),
							Text = "Next time we should try aiming better.",
							loopTag = "neutral"
						}
					}
				}
			}
		};

		DB.story.all[$"WeGotHurtButNotTooBad_{eddie}_0"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
			enemyShotJustHit = true,
			minDamageDealtToPlayerThisTurn = 1,
			maxDamageDealtToPlayerThisTurn = 1,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Someone should fix that.",
					loopTag = Manifest.EddieDefaultAnimation.Tag
				}
			}
		};
		DB.story.all[$"WeGotHurtButNotTooBad_{eddie}_1"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
			enemyShotJustHit = true,
			minDamageDealtToPlayerThisTurn = 1,
			maxDamageDealtToPlayerThisTurn = 1,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Aw man, that was my favorite chunk of hull.",
					loopTag = Manifest.EddieDisappointedAnimation.Tag
				}
			}
		};

		DB.story.all[$"WeDidOverThreeDamage_{eddie}_0"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
			playerShotJustHit = true,
			minDamageDealtToEnemyThisTurn = 4,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "We can do better than that.",
					loopTag = Manifest.EddieDefaultAnimation.Tag
				}
			}
		};
		DB.story.all[$"WeDidOverThreeDamage_{eddie}_1"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
			playerShotJustHit = true,
			minDamageDealtToEnemyThisTurn = 4,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "That's good damage.",
					loopTag = Manifest.EddieDefaultAnimation.Tag
				}
			}
		};
		DB.story.all[$"WeDidOverThreeDamage_{eddie}_1"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
			playerShotJustHit = true,
			minDamageDealtToEnemyThisTurn = 4,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Nice.",
					loopTag = Manifest.EddieDefaultAnimation.Tag
				}
			}
		};
		
		DB.story.all[$"WeDidOverFiveDamage_{eddie}_0"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
			playerShotJustHit = true,
			minDamageDealtToEnemyThisTurn = 6,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "That's more like it.",
					loopTag = Manifest.EddieDefaultAnimation.Tag
				}
			}
		};
		DB.story.all[$"WeDidOverFiveDamage_{eddie}_1"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
			playerShotJustHit = true,
			minDamageDealtToEnemyThisTurn = 6,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "They'll be feeling that one.",
					loopTag = Manifest.EddieDefaultAnimation.Tag
				}
			}
		};
		DB.story.all[$"WeDidOverFiveDamage_{eddie}_2"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
			playerShotJustHit = true,
			minDamageDealtToEnemyThisTurn = 6,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Lights out.",
					loopTag = Manifest.EddieDefaultAnimation.Tag
				}
			}
		};
		DB.story.all[$"WeDidOverFiveDamage_{eddie}_3"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie, Deck.dizzy.Key() },
			playerShotJustHit = true,
			minDamageDealtToEnemyThisTurn = 6,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "This kind of power is what all true electricians strive for.",
					loopTag = Manifest.EddieExplainsAnimation.Tag
				},
				new CustomSay()
				{
					who = Deck.dizzy.Key(),
					Text = "Where exactly did you get your education?",
					loopTag = "squint"
				}
			}
		};

		DB.story.all[$"EnemyArmorPierced_Multi_{eddie}_0"] = new()
		{
			type = NodeType.combat,
			playerShotJustHit = true,
			playerJustPiercedEnemyArmor = true,
			oncePerCombatTags = new() {"EnemyArmorPierced"},
			oncePerRun = true,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Heh, nice armor.",
					loopTag = Manifest.EddieDefaultAnimation.Tag
				}
			},
			allPresent = new() {"eunice"}
		};
		DB.story.all[$"EnemyArmorPierced_{eddie}_1"] = new()
		{
			type = NodeType.combat,
			playerShotJustHit = true,
			allPresent = new() {eddie},
			playerJustPiercedEnemyArmor = true,
			oncePerCombatTags = new() {"EnemyArmorPierced"},
			oncePerRun = true,
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Phew, I was afraid we'd need to care about the armor.",
					loopTag = Manifest.EddieRestingAnimation.Tag
				}
			}
		};

		DB.story.all[$"EnemyHasBrittle_{eddie}_0"] = new()
		{
			type = NodeType.combat,
			enemyHasBrittlePart = true,
			allPresent = new() {eddie},
			oncePerRunTags = new() {"yelledAboutBrittle"},
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "A brittle part? Someone should get fired for that.",
					loopTag = Manifest.EddieExplainsAnimation.Tag
				}
			}
		};
		DB.story.all[$"EnemyHasBrittle_{eddie}_1"] = new()
		{
			type = NodeType.combat,
			enemyHasBrittlePart = true,
			allPresent = new() {eddie},
			oncePerRunTags = new() {"yelledAboutBrittle"},
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Brittle spot! Score!",
					loopTag = Manifest.EddieExcitedAnimation.Tag
				}
			}
		};
		DB.story.all[$"EnemyHasBrittle_{eddie}_2"] = new()
		{
			type = NodeType.combat,
			enemyHasBrittlePart = true,
			allPresent = new() {eddie},
			oncePerRunTags = new() {"yelledAboutBrittle"},
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Easy fight, just hit that brittle spot.",
					loopTag = Manifest.EddieExplainsAnimation.Tag
				}
			}
		};

		DB.story.all[$"EnemyHasWeakness_{eddie}_0"] = new()
		{
			type = NodeType.combat,
			enemyHasWeakPart = true,
			allPresent = new() {eddie},
			oncePerRunTags = new() {"yelledAboutWeakness"},
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Weakpoint!",
					loopTag = Manifest.EddieExplainsAnimation.Tag
				}
			}
		};
		DB.story.all[$"EnemyHasWeakness_{eddie}_1"] = new()
		{
			type = NodeType.combat,
			enemyHasWeakPart = true,
			allPresent = new() {eddie},
			oncePerRunTags = new() {"yelledAboutWeakness"},
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "If you think about it, hitting a weakpoint is basically like getting free energy.",
					loopTag = Manifest.EddieExplainsAnimation.Tag
				}
			}
		};
		DB.story.all[$"EnemyHasWeakness_{eddie}_2"] = new()
		{
			type = NodeType.combat,
			enemyHasWeakPart = true,
			allPresent = new() {eddie},
			oncePerRunTags = new() {"yelledAboutWeakness"},
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "We should hit that weakpoint, if it's not too much trouble.",
					loopTag = Manifest.EddieDefaultAnimation.Tag
				}
			}
		};

		DB.story.all[$"PowerNapNap_{eddie}_0"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
      		oncePerCombatTags = new() { "PowerNap" },
			lookup = new() { "PowerNapNap" },
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "I'll make it up in a minute, I promise.",
					loopTag = Manifest.EddieRestingAnimation.Tag
				}
			}
		};
		DB.story.all[$"PowerNapNap_{eddie}_1"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
      		oncePerCombatTags = new() { "PowerNap" },
			lookup = new() { "PowerNapNap" },
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Just let me stretch a little...",
					loopTag = Manifest.EddieRestingAnimation.Tag
				}
			}
		};
		DB.story.all[$"PowerNapNap_{eddie}_2"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
      		oncePerCombatTags = new() { "PowerNap" },
			lookup = new() { "PowerNapNap" },
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "It's important to keep a clear head in dangerous situations.",
					loopTag = Manifest.EddieRestingAnimation.Tag
				}
			}
		};
		DB.story.all[$"PowerNapNap_{eddie}_3"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
      		oncePerCombatTags = new() { "PowerNap" },
			lookup = new() { "PowerNapNap" },
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "I can feel my productivity increasing already!",
					loopTag = Manifest.EddieRestingAnimation.Tag
				}
			}
		};
		DB.story.all[$"PowerNapNap_{eddie}_4"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
      		oncePerCombatTags = new() { "PowerNap" },
			lookup = new() { "PowerNapNap" },
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Just a sec.",
					loopTag = Manifest.EddieRestingAnimation.Tag
				}
			}
		};

		DB.story.all[$"PowerNapAwake_{eddie}_0"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
      		oncePerCombatTags = new() { "PowerNap" },
			lookup = new() { "PowerNapAwake" },
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Fine, fine.",
					loopTag = Manifest.EddieAnnoyedAnimation.Tag
				}
			}
		};
		DB.story.all[$"PowerNapAwake_{eddie}_1"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
      		oncePerCombatTags = new() { "PowerNap" },
			lookup = new() { "PowerNapAwake" },
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "I guess if it's an emergency.",
					loopTag = Manifest.EddieAnnoyedAnimation.Tag
				}
			}
		};
		DB.story.all[$"PowerNapAwake_{eddie}_2"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
      		oncePerCombatTags = new() { "PowerNap" },
			lookup = new() { "PowerNapAwake" },
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Duty calls...",
					loopTag = Manifest.EddieAnnoyedAnimation.Tag
				}
			}
		};
		DB.story.all[$"PowerNapAwake_{eddie}_3"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
      		oncePerCombatTags = new() { "PowerNap" },
			lookup = new() { "PowerNapAwake" },
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "That's fine. I'll take some time off during FTL.",
					loopTag = Manifest.EddieExplainsAnimation.Tag
				}
			}
		};
		DB.story.all[$"PowerNapAwake_{eddie}_4"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
      		oncePerCombatTags = new() { "PowerNap" },
			lookup = new() { "PowerNapAwake" },
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Can't catch a break, huh.",
					loopTag = Manifest.EddieAnnoyedAnimation.Tag
				}
			}
		};
		DB.story.all[$"PowerNapAwake_{eddie}_5"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
      		oncePerCombatTags = new() { "PowerNap" },
			lookup = new() { "PowerNapAwake" },
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Yeah, yeah, I'm on it.",
					loopTag = Manifest.EddieAnnoyedAnimation.Tag
				}
			}
		};
		DB.story.all[$"PowerNapAwake_{eddie}_6"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
      		oncePerCombatTags = new() { "PowerNap" },
			lookup = new() { "PowerNapAwake" },
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "I'll give it my all.",
					loopTag = Manifest.EddieAnnoyedAnimation.Tag
				}
			}
		};

		DB.story.all[$"GammaRay_{eddie}_0"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
      		oncePerCombatTags = new() { "GammaRay" },
			lookup = new() { "GammaRay" },
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Wooooo!",
					loopTag = Manifest.EddieExcitedAnimation.Tag
				}
			}
		};
		DB.story.all[$"GammaRay_{eddie}_1"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
      		oncePerCombatTags = new() { "GammaRay" },
			lookup = new() { "GammaRay" },
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Yes!",
					loopTag = Manifest.EddieExcitedAnimation.Tag
				}
			}
		};
		DB.story.all[$"GammaRay_{eddie}_2"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
      		oncePerCombatTags = new() { "GammaRay" },
			lookup = new() { "GammaRay" },
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Awesome!",
					loopTag = Manifest.EddieExcitedAnimation.Tag
				}
			}
		};
		DB.story.all[$"GammaRay_{eddie}_3"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
      		oncePerCombatTags = new() { "GammaRay" },
			lookup = new() { "GammaRay" },
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "It's just as satisfying as I expected it to be!",
					loopTag = Manifest.EddieExplainsAnimation.Tag
				},
				new SaySwitch()
				{
					lines = new()
					{
						new CustomSay()
						{
							who = Deck.riggs.Key(),
							Text = "Wow, carnage!",
							loopTag = "banana"
						},
						new CustomSay()
						{
							who = Deck.hacker.Key(),
							Text = "Sweet.",
							loopTag = "smile"
						},
						new CustomSay()
						{
							who = "comp",
							Text = "That's scary...",
							loopTag = "worried"
						},
					}
				}
			}
		};
		DB.story.all[$"GammaRay_{eddie}_4"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie, Deck.peri.Key() },
      		oncePerCombatTags = new() { "GammaRay" },
			lookup = new() { "GammaRay" },
			lines = new()
			{
				new CustomSay()
				{
					who = Deck.peri.Key(),
					Text = "Wow!",
					loopTag = "panic"
				},
				new CustomSay()
				{
					who = eddie,
					Text = "I know, right!",
					loopTag = Manifest.EddieExcitedAnimation.Tag
				}
			}
		};
		DB.story.all[$"GammaRay_{eddie}_5"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie, Deck.eunice.Key() },
      		oncePerCombatTags = new() { "GammaRay" },
			lookup = new() { "GammaRay" },
			lines = new()
			{
				new CustomSay()
				{
					who = Deck.eunice.Key(),
					Text = "Now you're speaking my language.",
					loopTag = "sly"
				},
				new CustomSay()
				{
					who = eddie,
					Text = "Cool, right?",
					loopTag = Manifest.EddieExcitedAnimation.Tag
				}
			}
		};
		DB.story.all[$"GammaRay_{eddie}_6"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
      		oncePerCombatTags = new() { "GammaRay" },
			lookup = new() { "GammaRay" },
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Was that cool or what?",
					loopTag = Manifest.EddieExcitedAnimation.Tag
				},
				new SaySwitch()
				{
					lines = new()
					{
						new CustomSay()
						{
							who = Deck.dizzy.Key(),
							Text = "...Um...",
							loopTag = "geiger"
						},
						new CustomSay()
						{
							who = Deck.shard.Key(),
							Text = "Very!",
							loopTag = "paws"
						},
						new CustomSay()
						{
							who = Deck.goat.Key(),
							Text = "Oh boy...",
							loopTag = "panic"
						},
					}
				}
			}
		};
		DB.story.all[$"GammaRay_{eddie}_7"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
      		oncePerCombatTags = new() { "GammaRay" },
			lookup = new() { "GammaRay" },
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Fire the gamma ray! We'll worry about the radiation later.",
					loopTag = Manifest.EddieExcitedAnimation.Tag
				},
				new SaySwitch()
				{
					lines = new()
					{
						new CustomSay()
						{
							who = Deck.dizzy.Key(),
							Text = "Sure, why not.",
							loopTag = "shrug"
						},
						new CustomSay()
						{
							who = Deck.goat.Key(),
							Text = "When's later?",
							loopTag = "shy"
						}
					}
				}
			}
		};

		DB.story.GetNode("CrabFacts1_Multi_0")?.lines.OfType<SaySwitch>().LastOrDefault()?.lines.Insert(0, new CustomSay()
		{
			who = eddie,
			Text = "I can't be bothered to fact-check that.",
			loopTag = Manifest.EddieDefaultAnimation.Tag
		});
		

		DB.story.GetNode("JustPlayedASashaCard_Multi_2")?.lines.OfType<SaySwitch>().LastOrDefault()?.lines.Insert(0, new CustomSay()
		{
			who = eddie,
			Text = "Sports.",
			loopTag = Manifest.EddieSquintAnimation.Tag
		});

		StoryNode node;
		node = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
			oncePerCombat = true,
			oncePerCombatTags = new() { "PlayedInfinite" },
			lines = new()
			{
				new SaySwitch()
				{
					lines = new()
					{
						new CustomSay()
						{
							who = eddie,
							Text = "We can do that again, if we'd like.",
							loopTag = Manifest.EddieDefaultAnimation.Tag
						},
						new CustomSay()
						{
							who = eddie,
							Text = "Let's just do the same thing again.",
							loopTag = Manifest.EddieExcitedAnimation.Tag
						},
						new CustomSay()
						{
							who = eddie,
							Text = "It's important to keep your options open.",
							loopTag = Manifest.EddieExplainsAnimation.Tag
						},
						new CustomSay()
						{
							who = eddie,
							Text = "I'm a firm believer in renewables.",
							loopTag = Manifest.EddieExplainsAnimation.Tag
						},
						new CustomSay()
						{
							who = eddie,
							Text = "Should I stop?",
							loopTag = Manifest.EddieDefaultAnimation.Tag
						},
						new CustomSay()
						{
							who = eddie,
							Text = "This should keep me occupied.",
							loopTag = Manifest.EddieDefaultAnimation.Tag
						},
						new CustomSay()
						{
							who = eddie,
							Text = "Again!",
							loopTag = Manifest.EddieExcitedAnimation.Tag
						},
						new CustomSay()
						{
							who = eddie,
							Text = "Had enough yet?",
							loopTag = Manifest.EddieDefaultAnimation.Tag
						},
					}
				}
			}
		};
		KokoroApi.SetExtensionData(node, StoryVarsAdditions.CardJustPlayedWasInfiniteKey, true);
		DB.story.all[$"PlayedInfinite_{eddie}_0"] = node;

		node = new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
			oncePerCombat = true,
			oncePerCombatTags = ["PlayedDiscount"],
			lines =
			[
				new SaySwitch()
				{
					lines =
					[
						new CustomSay()
						{
							who = eddie,
							Text = "It's a dubious trick, but it's cheap!",
							loopTag = Manifest.EddieExplainsAnimation.Tag
						},
						new CustomSay()
						{
							who = eddie,
							Text = "Don't ask me where I got this stuff. I don't even remember.",
							loopTag = Manifest.EddieDefaultAnimation.Tag
						},
						new CustomSay()
						{
							who = eddie,
							Text = "Here's a life hack that can save you a bit of energy!",
							loopTag = Manifest.EddieExplainsAnimation.Tag
						},
						new CustomSay()
						{
							who = eddie,
							Text = "That's it for that discount.",
							loopTag = Manifest.EddieDefaultAnimation.Tag
						},
						new CustomSay()
						{
							who = eddie,
							Text = "Cutting a few corners can save you some power.",
							loopTag = Manifest.EddieExplainsAnimation.Tag
						},
						new CustomSay()
						{
							who = eddie,
							Text = "Might as well do it while it's easy.",
							loopTag = Manifest.EddieDefaultAnimation.Tag
						},
						new CustomSay()
						{
							who = eddie,
							Text = "It was fun while it lasted.",
							loopTag = Manifest.EddieDisappointedAnimation.Tag
						},
						new CustomSay()
						{
							who = eddie,
							Text = "I'm a big fan of discounts.",
							loopTag = Manifest.EddieExplainsAnimation.Tag
						},
						new CustomSay()
						{
							who = eddie,
							Text = "What a bargain!",
							loopTag = Manifest.EddieExcitedAnimation.Tag
						}
					]
				}
			]
		};
		KokoroApi.SetExtensionData(node, StoryVarsAdditions.MinDiscountOfCardJustPlayedKey, 1);
		DB.story.all[$"PlayedDiscount_{eddie}_0"] = node;

		node = new()
		{
			type = NodeType.combat,
			allPresent = new() { eddie },
			oncePerCombat = true,
			oncePerCombatTags = new() { "PlayedExpensive" },
			lines = new()
			{
				new SaySwitch()
				{
					lines = new()
					{
						new CustomSay()
						{
							who = eddie,
							Text = "Ew.",
							loopTag = Manifest.EddieAnnoyedAnimation.Tag
						},
						new CustomSay()
						{
							who = eddie,
							Text = "What is this? Who thought of this?",
							loopTag = Manifest.EddieAnnoyedAnimation.Tag
						},
						new CustomSay()
						{
							who = eddie,
							Text = "I hated every second of that.",
							loopTag = Manifest.EddieAnnoyedAnimation.Tag
						},
						new CustomSay()
						{
							who = eddie,
							Text = "That really rubs me the wrong way.",
							loopTag = Manifest.EddieAnnoyedAnimation.Tag
						},
						new CustomSay()
						{
							who = eddie,
							Text = "Never again.",
							loopTag = Manifest.EddieAnnoyedAnimation.Tag
						}
					}
				}
			}
		};
		KokoroApi.SetExtensionData(node, StoryVarsAdditions.MaxDiscountOfCardJustPlayedKey, -1);
		DB.story.all[$"PlayedExpensive_{eddie}_0"] = node;

		DB.story.all["$JustPlayedASogginsCard_{eddie}_0"] = new() {
			type = NodeType.combat,
			whoDidThat = Deck.soggins,
			oncePerRun = true,
			allPresent = new() { eddie },
			lines = new()
			{
				new CustomSay() {
					who = eddie,
					Text = "We're still alive? That's good.",
					loopTag = Manifest.EddieDefaultAnimation.Tag
				}
			}
		};

		DB.story.all["$JustPlayedAToothCard_{eddie}_0"] = new() {
			type = NodeType.combat,
			whoDidThat = Deck.tooth,
			oncePerRun = true,
			allPresent = [eddie],
			lines = [
				new CustomSay() {
					who = eddie,
					Text = "You can get a lot of use out of junk like this.",
					loopTag = Manifest.EddieExplainsAnimation.Tag
				}
			]
		};

		DB.story.all["summonEddie_0"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { "comp" },
			lookup = new() { "summonEddie" },
			oncePerCombatTags = new() { $"summonEddieTag" },
			oncePerRun = true,
			lines = new() {
				new CustomSay()
				{
					who = "comp",
					Text = "Let's see what I can do with this.",
					loopTag = "neutral"
				}
			}
		};

		DB.story.all["summonEddie_1"] = new()
		{
			type = NodeType.combat,
			allPresent = new() { "comp", eddie },
			lookup = new() { "summonEddie" },
			oncePerCombatTags = new() { $"summonEddieTag" },
			oncePerRun = true,
			lines = new() {
				new CustomSay()
				{
					who = eddie,
					Text = "Copying me? I'm flattered.",
					loopTag = "neutral"
				},
				new CustomSay()
				{
					who = "comp",
					Text = "You've got some good ideas.",
					loopTag = "neutral"
				}
			}
		};

		DB.story.GetNode("DillianShouts")?.lines.OfType<SaySwitch>().FirstOrDefault()?.lines.Insert(0, new CustomSay()
		{
			who = eddie,
			Text = "Whaddup.",
		});
	}
}