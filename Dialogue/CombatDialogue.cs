using Shockah.Kokoro;
using TheJazMaster.Eddie.DialogueAdditions;
using TwosCompany;

namespace TheJazMaster.Eddie;

internal static class CombatDialogue
{
	internal static ModEntry Instance => ModEntry.Instance;
	private static IKokoroApi.IV2 KokoroApi => Instance.KokoroApi;

	internal static void Inject()
	{
		string eddie = Instance.EddieDeck.Key();

		DB.story.all.Add($"BlockedALotOfAttacksWithArmor_{eddie}", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
			oncePerRun = true,
			oncePerCombatTags = ["YowzaThatWasALOTofArmorBlock"],
			enemyShotJustHit = true,
			minDamageBlockedByPlayerArmorThisTurn = 3,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Who needs to do anything when you have armor.",
					loopTag = Instance.NeutralAnim
				}
			]
		});
		DB.story.all.Add($"DizzyWentMissing_{eddie}", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
			oncePerCombatTags = ["dizzyWentMissing"],
			lastTurnPlayerStatuses = [Status.missingDizzy],
			priority = true,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Dizzy? This isn't funny.",
					loopTag = Instance.WorriedAnim
				}
			]
		});
		DB.story.all.Add($"RiggsWentMissing_{eddie}", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
			oncePerCombatTags = ["riggsWentMissing"],
			lastTurnPlayerStatuses = [Status.missingRiggs],
			priority = true,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "I'm sure she won't mind if I take her boba.",
					loopTag = Instance.ExplainsAnim
				}
			]
		});
		DB.story.all.Add($"PeriWentMissing_{eddie}", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
			oncePerCombatTags = ["periWentMissing"],
			lastTurnPlayerStatuses = [Status.missingPeri],
			priority = true,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Does this mean I'm off the hook?",
					loopTag = Instance.OnEdgeAnim
				}
			]
		});
		DB.story.all.Add($"IsaacWentMissing_{eddie}", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
			oncePerCombatTags = ["isaacWentMissing"],
			lastTurnPlayerStatuses = [Status.missingIsaac],
			priority = true,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "He'll be back, right? I'm sure he'll be back.",
					loopTag = Instance.WorriedAnim
				}
			]
		});
		DB.story.all.Add($"DrakeWentMissing_{eddie}", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
			oncePerCombatTags = ["drakeWentMissing"],
			lastTurnPlayerStatuses = [Status.missingDrake],
			priority = true,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "I thought we had something...",
					loopTag = Instance.DisappointedAnim
				}
			]
		});
		DB.story.all.Add($"MaxWentMissing_{eddie}", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
			oncePerCombatTags = ["maxWentMissing"],
			lastTurnPlayerStatuses = [Status.missingMax],
			priority = true,
			lines = [
				new SaySwitch() {
					lines = [
						new CustomSay()
						{
							who = eddie,
							Text = "MAX!",
							loopTag = Instance.OnEdgeAnim
						},
						new CustomSay()
						{
							who = eddie,
							Text = "Max can't die. He respawns!",
							loopTag = Instance.SquintAnim
						}
					]
				}
			]
		});
		DB.story.all.Add($"BooksWentMissing_{eddie}", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
			oncePerCombatTags = ["booksWentMissing"],
			lastTurnPlayerStatuses = [Status.missingBooks],
			priority = true,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Is this time magic again?",
					loopTag = Instance.SquintAnim
				}
			]
		});
		DB.story.all.Add($"CatWentMissing_{eddie}", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
			oncePerCombatTags = ["CatWentMissing"],
			lastTurnPlayerStatuses = [Status.missingCat],
			priority = true,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Oh, did I do something wrong again?",
					loopTag = Instance.OnEdgeAnim
				}
			]
		});
		DB.story.all.Add($"{eddie}JustHitBig_0", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
			whoDidThat = Instance.EddieDeck,
			playerShotJustHit = true,
			minDamageDealtToEnemyThisAction = 9,
			lines = [
				new SaySwitch {
					lines = [
						new CustomSay()
						{
							who = eddie,
							Text = "Awesome!",
							loopTag = Instance.ExcitedAnim
						},
						new CustomSay()
						{
							who = eddie,
							Text = "Oh yeah..."
						}
					]
				}
			]
		});

		DB.story.all.Add($"Duo_AboutToDieAndLoop_{eddie}0", new()
		{
			type = NodeType.combat,
			allPresent = [eddie, Deck.dizzy.Key()],
			enemyShotJustHit = true,
			maxHull = 2,
			oncePerCombatTags = ["aboutToDie"],
			oncePerRun = true,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Lights out...",
					loopTag = Instance.DisappointedAnim
				},
				new CustomSay()
				{
					who = Deck.dizzy.Key(),
					Text = "At least try to get us out of this.",
					loopTag = "frown"
				},
			]
		});
		DB.story.all.Add($"Duo_AboutToDieAndLoop_{eddie}1", new()
		{
			type = NodeType.combat,
			allPresent = [eddie, "comp"],
			enemyShotJustHit = true,
			maxHull = 2,
			oncePerCombatTags = ["aboutToDie"],
			oncePerRun = true,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Do we HAVE to try again?",
					loopTag = Instance.SquintAnim
				},
				new CustomSay()
				{
					who = "comp",
					Text = "You can stay behind, if you'd prefer that.",
					loopTag = "squint"
				}
			]
		});
		DB.story.all.Add($"Duo_AboutToDieAndLoop_{eddie}2", new()
		{
			type = NodeType.combat,
			allPresent = [eddie, Deck.goat.Key()],
			enemyShotJustHit = true,
			maxHull = 2,
			oncePerCombatTags = ["aboutToDie"],
			oncePerRun = true,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Better luck next time, amigos.",
					loopTag = Instance.ExplainsAnim
				},
				new CustomSay()
				{
					who = Deck.goat.Key(),
					Text = "Adios...",
					loopTag = "eyesClosed"
				},
			]
		});
		DB.story.all.Add($"Duo_AboutToDieAndLoop_{eddie}3", new()
		{
			type = NodeType.combat,
			allPresent = [eddie, Deck.riggs.Key()],
			enemyShotJustHit = true,
			maxHull = 2,
			oncePerCombatTags = ["aboutToDie"],
			oncePerRun = true,
			lines = [
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
					loopTag = Instance.OnEdgeAnim
				}
			]
		});
		DB.story.all.Add($"Duo_AboutToDieAndLoop_{eddie}4", new()
		{
			type = NodeType.combat,
			allPresent = [eddie, Deck.peri.Key()],
			enemyShotJustHit = true,
			maxHull = 2,
			oncePerCombatTags = ["aboutToDie"],
			oncePerRun = true,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "It's gonna look really cool if we make it out of this one.",
					loopTag = Instance.ExplainsAnim
				},
				new CustomSay()
				{
					who = Deck.peri.Key(),
					Text = "Then get to it!",
					loopTag = "mad"
				}
			]
		});
		DB.story.all.Add($"Duo_AboutToDieAndLoop_{eddie}5", new()
		{
			type = NodeType.combat,
			allPresent = [eddie, Deck.eunice.Key()],
			enemyShotJustHit = true,
			maxHull = 2,
			oncePerCombatTags = ["aboutToDie"],
			oncePerRun = true,
			lines = [
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
					loopTag = Instance.NeutralAnim
				}
			]
		});
		DB.story.all.Add($"Duo_AboutToDieAndLoop_{eddie}6", new()
		{
			type = NodeType.combat,
			allPresent = [eddie, Deck.hacker.Key()],
			enemyShotJustHit = true,
			maxHull = 2,
			oncePerCombatTags = ["aboutToDie"],
			oncePerRun = true,
			lines = [
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
					loopTag = Instance.SquintAnim
				}
			]
		});
		DB.story.all.Add($"Duo_AboutToDieAndLoop_{eddie}7", new()
		{
			type = NodeType.combat,
			allPresent = [eddie, Deck.shard.Key()],
			enemyShotJustHit = true,
			maxHull = 2,
			oncePerCombatTags = ["aboutToDie"],
			oncePerRun = true,
			lines = new()
			{
				new CustomSay()
				{
					who = Deck.shard.Key(),
					Text = "Dying sucks!",
					loopTag = "squint"
				},
				new CustomSay()
				{
					who = eddie,
					Text = "At least we'll get some cryo-sleep.",
					loopTag = Instance.ExplainsAnim
				}
			}
		});
		DB.story.all.Add($"Duo_AboutToDieAndLoop_{eddie}8", new()
		{
			type = NodeType.combat,
			allPresent = [eddie, Deck.hacker.Key()],
			enemyShotJustHit = true,
			maxHull = 2,
			oncePerCombatTags = ["aboutToDie"],
			oncePerRun = true,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Max, enter your second phase!",
					loopTag = Instance.ExcitedAnim
				},
				new CustomSay()
				{
					who = Deck.hacker.Key(),
					Text = "NGRAAAAAAHHHHHH!",
					loopTag = "squint"
				}
			]
		});
		DB.story.all.Add($"Duo_AboutToDieAndLoop_{eddie}9", new()
		{
			type = NodeType.combat,
			allPresent = [eddie, Deck.dizzy.Key()],
			enemyShotJustHit = true,
			maxHull = 2,
			oncePerCombatTags = ["aboutToDie"],
			oncePerRun = true,
			lines = [
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
					loopTag = Instance.WorriedAnim
				}
			]
		});
		DB.story.all.Add($"Duo_AboutToDieAndLoop_{eddie}10", new()
		{
			type = NodeType.combat,
			allPresent = [eddie, Deck.peri.Key()],
			enemyShotJustHit = true,
			maxHull = 2,
			oncePerCombatTags = ["aboutToDie"],
			oncePerRun = true,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Man, I don't want to loop again.",
					loopTag = Instance.SquintAnim
				},
				new CustomSay()
				{
					who = Deck.peri.Key(),
					Text = "Too bad!",
					loopTag = "mad"
				}
			]
		});
		DB.story.all.Add($"Duo_AboutToDieAndLoop_{eddie}11", new()
		{
			type = NodeType.combat,
			allPresent = [eddie, Deck.riggs.Key()],
			enemyShotJustHit = true,
			maxHull = 2,
			oncePerCombatTags = ["aboutToDie"],
			oncePerRun = true,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Don't worry, I'm sure our pilot will get us out of this.",
					loopTag = Instance.NeutralAnim
				},
				new CustomSay()
				{
					who = Deck.riggs.Key(),
					Text = "I wouldn't count on it!",
					loopTag = "nervous"
				}
			]
		});
		DB.story.all.Add($"Duo_AboutToDieAndLoop_{eddie}12", new()
		{
			type = NodeType.combat,
			allPresent = [eddie, Deck.goat.Key()],
			enemyShotJustHit = true,
			maxHull = 2,
			oncePerCombatTags = ["aboutToDie"],
			oncePerRun = true,
			lines = [
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
					loopTag = Instance.NeutralAnim
				}
			]
		});
		DB.story.all.Add($"Duo_AboutToDieAndLoop_{eddie}13", new()
		{
			type = NodeType.combat,
			allPresent = [eddie, Deck.eunice.Key()],
			enemyShotJustHit = true,
			maxHull = 2,
			oncePerCombatTags = ["aboutToDie"],
			oncePerRun = true,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "I'll miss our time together, Drake.",
					loopTag = Instance.ExcitedAnim
				},
				new CustomSay()
				{
					who = Deck.eunice.Key(),
					Text = "Okay. I won't.",
					loopTag = "squint"
				}
			]
		});
		DB.story.all.Add($"Duo_AboutToDieAndLoop_{eddie}14", new()
		{
			type = NodeType.combat,
			allPresent = [eddie, Deck.shard.Key()],
			enemyShotJustHit = true,
			maxHull = 2,
			oncePerCombatTags = ["aboutToDie"],
			oncePerRun = true,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "I've gotten through worse.",
					loopTag = Instance.NeutralAnim
				},
				new CustomSay()
				{
					who = Deck.shard.Key(),
					Text = "That's right! Never give up!",
					loopTag = "stoked"
				}
			]
		});
		DB.story.all.Add($"Duo_AboutToDieAndLoop_{eddie}15", new()
		{
			type = NodeType.combat,
			allPresent = [eddie, "comp"],
			enemyShotJustHit = true,
			maxHull = 2,
			oncePerCombatTags = ["aboutToDie"],
			oncePerRun = true,
			lines = [
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
					loopTag = Instance.ExplainsAnim
				}
			]
		});
		if (Instance.SogginsApi != null) {
			string soggins = Instance.SogginsApi.SogginsVanillaDeck.Key();
			DB.story.all.Add($"Duo_AboutToDieAndLoop_{eddie}16", new()
			{
				type = NodeType.combat,
				allPresent = [eddie, soggins],
				enemyShotJustHit = true,
				maxHull = 2,
				oncePerCombatTags = ["aboutToDie"],
				oncePerRun = true,
				lines = [
                    new CustomSay()
					{
						who = eddie,
						Text = "Try pressing more buttons!",
						loopTag = Instance.ExcitedAnim
					},
					new CustomSay()
					{
						who = soggins,
						Text = "Works every time.",
						loopTag = "neutral"
					}
				]
			});
			DB.story.all.Add($"Duo_AboutToDieAndLoop_{eddie}17", new()
			{
				type = NodeType.combat,
				allPresent = [eddie, soggins],
				enemyShotJustHit = true,
				maxHull = 2,
				oncePerCombatTags = ["aboutToDie"],
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
						loopTag = Instance.ExplainsAnim
					}
				}
			});
		}
		DB.story.all.Add($"EmptyHandWithEnergy_{eddie}_0", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
			handEmpty = true,
			minEnergy = 1,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "How did that happen?",
					loopTag = Instance.WorriedAnim
				}
			]
		});
		DB.story.all.Add($"EmptyHandWithEnergy_{eddie}_1", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
			handEmpty = true,
			minEnergy = 1,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Oh well, nothing left to do.",
					loopTag = Instance.ExplainsAnim
				}
			]
		});

		DB.story.all.Add($"EnemyArmorHitLots_{eddie}_0", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
			playerShotJustHit = true,
			minDamageBlockedByEnemyArmorThisTurn = 3,
			oncePerCombat = true,
			oncePerRun = true,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "We're in too deep now... We have to shoot the armor more.",
					loopTag = Instance.DisappointedAnim
				},
				new SaySwitch()
				{
					lines = [
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
					]
				}
			]
		});
		DB.story.all.Add($"EnemyArmorHitLots_{eddie}_1", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
			playerShotJustHit = true,
			minDamageBlockedByEnemyArmorThisTurn = 1,
			oncePerCombat = true,
			oncePerRun = true,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Aiming at unarmored parts is, like, a lot of effort.",
					loopTag = Instance.NeutralAnim
				},
				new SaySwitch()
				{
					lines = [
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
					]
				}
			]
		});
		DB.story.all.Add($"EnemyArmorHitLots_{eddie}_2", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
			playerShotJustHit = true,
			minDamageBlockedByEnemyArmorThisTurn = 1,
			oncePerCombat = true,
			oncePerRun = true,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Maybe we can shoot the armor off? No?",
					loopTag = Instance.DisappointedAnim
				},
				new SaySwitch()
				{
					lines = [
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
					]
				}
			]
		});
		DB.story.all.Add($"EnemyArmorHitLots_{eddie}_3", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
			playerShotJustHit = true,
			minDamageBlockedByEnemyArmorThisTurn = 1,
			oncePerCombat = true,
			oncePerRun = true,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Hey, enemy ship, could you scoot over a little?",
					loopTag = Instance.NeutralAnim
				},
				new SaySwitch()
				{
					lines = [
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
					]
				}
			]
		});

    	DB.story.all.Add($"WizardGeneralShouts_{eddie}_0", new() {
      		type = NodeType.combat,
      		turnStart = true,
      		allPresent = ["wizard", eddie],
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
					loopTag = Instance.WorriedAnim
				}
			}
    	});

		DB.story.all.Add($"ExpensiveCardPlayed_{eddie}_0", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
			minCostOfCardJustPlayed = 4,
			oncePerCombatTags = ["ExpensiveCardPlayed"],
			oncePerRun = true,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Oops, I think that tripped a breaker.",
					loopTag = Instance.OnEdgeAnim
				}
			]
		});
		DB.story.all.Add($"ExpensiveCardPlayed_{eddie}_1", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
			minCostOfCardJustPlayed = 4,
			oncePerCombatTags = ["ExpensiveCardPlayed"],
			oncePerRun = true,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "I think something caught fire.",
					loopTag = Instance.NeutralAnim
				},
				new SaySwitch()
				{
					lines = [
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
					]
				}
			]
		});
		DB.story.all.Add($"ExpensiveCardPlayed_{eddie}_2", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
			minCostOfCardJustPlayed = 4,
			oncePerCombatTags = ["ExpensiveCardPlayed"],
			oncePerRun = true,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "We should probably never do that again.",
					loopTag = Instance.ExplainsAnim
				},
				new SaySwitch()
				{
					lines = [
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
					]
				}
			]
		});

		DB.story.all.Add($"HandOnlyHasTrashCards_{eddie}", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
			handFullOfTrash = true,
			oncePerCombatTags = ["handOnlyHasTrashCards"],
			oncePerRun = true,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Eh, the trash isn't that bad.",
					loopTag = Instance.NeutralAnim
				},
				new SaySwitch()
				{
					lines = [
                        new CustomSay()
						{
							who = Deck.dizzy.Key(),
							Text = "But we have to clean it up eventually.",
							loopTag = "squint"
						}
					]
				}
			]
		});
		DB.story.all.Add($"HandOnlyHasUnplayableCards_{eddie}", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
			handFullOfUnplayableCards = true,
			oncePerCombatTags = ["handFullOfUnplayableCards"],
			oncePerRun = true,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "This is all pretty useless.",
					loopTag = Instance.SquintAnim
				}
			]
		});

		DB.story.all.Add($"WeDontOverlapWithEnemyAtAll_{eddie}_0", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
			shipsDontOverlapAtAll = true,
			oncePerCombatTags = ["NoOverlapBetweenShips"],
			priority = true,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "I guess that works.",
					loopTag = Instance.NeutralAnim
				}
			]
		});
		DB.story.all.Add($"WeDontOverlapWithEnemyAtAll_{eddie}_1", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
			shipsDontOverlapAtAll = true,
			oncePerCombatTags = ["NoOverlapBetweenShips"],
			priority = true,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Nice. Stay there, please.",
					loopTag = Instance.NeutralAnim
				}
			]
		});
		DB.story.all.Add($"WeDontOverlapWithEnemyAtAll_{eddie}_2", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
			shipsDontOverlapAtAll = true,
			oncePerCombatTags = ["NoOverlapBetweenShips"],
			priority = true,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Yeah, stay there if you know what's good for you!",
					loopTag = Instance.ExcitedAnim
				}
			]
		});

		DB.story.all.Add($"WeDontOverlapWithEnemyAtAllButWeDoHaveASeekerToDealWith_{eddie}", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
			shipsDontOverlapAtAll = true,
			oncePerCombatTags = ["NoOverlapBetweenShipsSeeker"],
			anyDronesHostile = ["missile_seeker"],
			nonePresent = ["crab"],
			priority = true,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Hm... I feel like we're forgetting something.",
					loopTag = Instance.SquintAnim
				},
				new SaySwitch()
				{
					lines = [
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
					]
				}
			]
		});

		DB.story.all.Add($"TooManyTurns_{eddie}", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
			minTurnsThisCombat = 15,
			oncePerCombatTags = ["tooManyTurns"],
			oncePerRun = true,
			turnStart = true,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "We're not done yet? Come on!",
					loopTag = Instance.DisappointedAnim
				}
			]
		});
		DB.story.all.Add($"ManyTurns_{eddie}_0", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
			minTurnsThisCombat = 9,
			oncePerCombatTags = ["manyTurns"],
			oncePerRun = true,
			turnStart = true,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "I need a siesta after this.",
					loopTag = Instance.NeutralAnim
				}
			]
		});
		DB.story.all.Add($"ManyTurns_{eddie}_1", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
			minTurnsThisCombat = 9,
			oncePerCombatTags = ["manyTurns"],
			oncePerRun = true,
			turnStart = true,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "I'm getting tired. Can we have a coffee break?",
					loopTag = Instance.SquintAnim
				}
			]
		});
		DB.story.all.Add($"ManyTurns_{eddie}_2", new()
		{
			type = NodeType.combat,
			allPresent = [eddie, Deck.riggs.Key()],
			minTurnsThisCombat = 9,
			hasArtifacts = ["CaffeineRush"],
			oncePerCombatTags = ["manyTurns"],
			oncePerRun = true,
			turnStart = true,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "I'm getting tired. Can we have a coffee break?",
					loopTag = Instance.SquintAnim
				},
				new CustomSay()
				{
					who = Deck.riggs.Key(),
					Text = "You can have some of mine.",
					loopTag = "bobaSlurp"
				}
			]
		});

		DB.story.all.Add($"OverheatCatFix_{eddie}", new()
		{
			type = NodeType.combat,
			wasGoingToOverheatButStopped = true,
			whoDidThat = Deck.colorless,
			allPresent = ["comp"],
			oncePerCombatTags = ["OverheatCatFix"],
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Good call, gotta keep the heat contained!",
					loopTag = Instance.ExcitedAnim
				},
				new CustomSay()
				{
					who = "comp",
					Text = "Yes, that's exactly what I was going for.",
					loopTag = "squint"
				}
			]
		});
		DB.story.GetNode("OverheatDrakeFix_Multi_6")?.lines.OfType<SaySwitch>().First().lines.Insert(0, new CustomSay()
		{
			who = eddie,
			Text = "Good call, gotta keep the heat contained!",
			loopTag = Instance.ExcitedAnim
		});

		DB.story.all.Add($"WeJustGainedHeatAndDrakeIsHere_{eddie}_0", new()
		{
      		type = NodeType.combat,
      		lastTurnPlayerStatuses = [Status.heat],
      		allPresent = [eddie, Deck.eunice.Key()],
      		oncePerCombatTags = ["DrakeCanYouDoSomethingAboutTheHeatPlease"],
      		lines = [
                new SaySwitch()
				{
					lines = [
                        new CustomSay()
						{
							who = eddie,
							Text = "Wow, this is nice!",
							loopTag = Instance.ExcitedAnim
						},
						new CustomSay()
						{
							who = eddie,
							Text = "I did feel a little cold before.",
							loopTag = Instance.NeutralAnim
						},
						new CustomSay()
						{
							who = eddie,
							Text = "Ah, cozy...",
							loopTag = Instance.NeutralAnim
						},
						new CustomSay()
						{
							who = eddie,
							Text = "A little heat never hurt anyone.",
							loopTag = Instance.NeutralAnim
						},
						new CustomSay()
						{
							who = eddie,
							Text = "I like this temperature more.",
							loopTag = Instance.NeutralAnim
						},
						new CustomSay()
						{
							who = eddie,
							Text = "Yeah! Crank it up!",
							loopTag = Instance.ExcitedAnim
						},
						new CustomSay()
						{
							who = eddie,
							Text = "We'll deal with the consequences later.",
							loopTag = Instance.NeutralAnim
						},
						new CustomSay()
						{
							who = eddie,
							Text = "We should always heat the ship like this.",
							loopTag = Instance.ExplainsAnim
						},
						new CustomSay()
						{
							who = eddie,
							Text = "We should do this more often.",
							loopTag = Instance.ExplainsAnim
						},
						new CustomSay()
						{
							who = eddie,
							Text = "I work better in the heat.",
							loopTag = Instance.ExplainsAnim
						},
					]
				},
				new SaySwitch()
				{
					lines = [
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
					]
				}
			]
		});

		DB.story.all.Add($"OverheatGeneric_{eddie}_0", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
			goingToOverheat = true,
			oncePerCombatTags = ["OverheatGeneric"],
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Ah, the consequences...",
					loopTag = Instance.AnnoyedAnim
				}
			]
		});
		DB.story.all.Add($"OverheatGeneric_{eddie}_1", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
			goingToOverheat = true,
			oncePerCombatTags = ["OverheatGeneric"],
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "I'll miss you, heat.",
					loopTag = Instance.DisappointedAnim
				}
			]
		});

		DB.story.all.Add($"PlayedManyCards_{eddie}_0", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
			handEmpty = true,
			minCardsPlayedThisTurn = 6,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "See? It all worked out.",
					loopTag = Instance.ExcitedAnim
				},
			]
		});

		DB.story.all.Add($"ThatsALotOfDamageToThem_{eddie}_0", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
			playerShotJustHit = true,
			minDamageDealtToEnemyThisTurn = 10,
			lines = [
				new SaySwitch() {
					lines = [
                        new CustomSay()
						{
							who = eddie,
							Text = "Nice.",
							loopTag = Instance.NeutralAnim
						},
						new CustomSay()
						{
							who = eddie,
							Text = "Awesome.",
							loopTag = Instance.NeutralAnim
						}
					]
				}
			]
		});
		DB.story.all.Add($"ThatsALotOfDamageToThem_{eddie}_1", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
			playerShotJustHit = true,
			minDamageDealtToEnemyThisTurn = 10,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Let's do that again!",
					loopTag = Instance.ExcitedAnim
				}
			]
		});

		DB.story.all.Add($"ThatsALotOfDamageToUs_{eddie}", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
			enemyShotJustHit = true,
			minDamageDealtToPlayerThisTurn = 3,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "That was wild!",
					loopTag = Instance.NeutralAnim
				}
			]
		});

		DB.story.GetNode("TookDamageHave2HP_Multi_1")?.lines.OfType<SaySwitch>().First().lines.Insert(0, new CustomSay()
		{
			who = eddie,
			Text = "Nearly, eh? That's fine.",
			loopTag = Instance.ExcitedAnim
		});

		DB.story.all.Add($"TookZeroDamageAtLowHealth_{eddie}", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
			enemyShotJustHit = true,
			maxDamageDealtToPlayerThisTurn = 0,
			maxHull = 2,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "That's the power of determination.",
					loopTag = Instance.NeutralAnim
				}
			]
		});
		DB.story.all.Add($"WeAreCorroded_{eddie}", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
			lastTurnPlayerStatuses = [Status.corrode],
			oncePerRun = true,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "This is gonna be a lot of work to fix.",
					loopTag = Instance.SquintAnim
				}
			]
		});
		DB.story.all.Add($"IMissedOopsie_{eddie}_0", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
			whoDidThat = Instance.EddieDeck,
			playerShotJustMissed = true,
			oncePerCombat = true,
			doesNotHaveArtifacts = ["Recalibrator", "GrazerBeam"],
			lines = [
                new CustomSay()
				{
					who = eddie,
					Text = "That was a test shot.",
					loopTag = Instance.NeutralAnim
				},
				new SaySwitch()
				{
					lines = [
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
					]
				}
			]
		});
		DB.story.all.Add($"IMissedOopsie_{eddie}_1", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
			whoDidThat = Instance.EddieDeck,
			playerShotJustMissed = true,
			oncePerCombat = true,
			doesNotHaveArtifacts = ["Recalibrator", "GrazerBeam"],
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Oops, wrong button.",
					loopTag = Instance.SquintAnim
				},
			]
		});
		DB.story.all.Add($"IMissedOopsie_{eddie}_2", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
			whoDidThat = Instance.EddieDeck,
			playerShotJustMissed = true,
			oncePerCombat = true,
			doesNotHaveArtifacts = ["Recalibrator", "GrazerBeam"],
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Wow, they're totally cheating.",
					loopTag = Instance.SquintAnim
				},
				new SaySwitch()
				{
					lines = [
                        new CustomSay()
						{
							who = Deck.eunice.Key(),
							Text = "Let me aim that thing!",
							loopTag = "mad"
						},
						new CustomSay()
						{
							who = Deck.hacker.Key(),
							Text = "Must have been cosmic particle interference.",
							loopTag = "neutral"
						}
					]
				}
			]
		});
		DB.story.all.Add($"WeMissedOopsie_{eddie}_0", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
			playerShotJustMissed = true,
			oncePerCombat = true,
			doesNotHaveArtifacts = ["Recalibrator", "GrazerBeam"],
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "No biggie.",
					loopTag = Instance.NeutralAnim
				}
			]
		});
		DB.story.all.Add($"WeMissedOopsie_{eddie}_1", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
			playerShotJustMissed = true,
			oncePerCombat = true,
			doesNotHaveArtifacts = ["Recalibrator", "GrazerBeam"],
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Happens to the best of us.",
					loopTag = Instance.NeutralAnim
				},
				new SaySwitch()
				{
					lines = [
                        new CustomSay()
						{
							who = Deck.peri.Key(),
							Text = "Let's not make this a habit.",
							loopTag = "mad"
						}
					]
				}
			]
		});
		DB.story.all.Add($"WeMissedOopsie_{eddie}_2", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
			playerShotJustMissed = true,
			oncePerCombat = true,
			doesNotHaveArtifacts = ["Recalibrator", "GrazerBeam"],
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "We'll get 'em next time.",
					loopTag = Instance.NeutralAnim
				},
				new SaySwitch()
				{
					lines = [
                        new CustomSay()
						{
							who = Deck.goat.Key(),
							Text = "Next time we should try aiming better.",
							loopTag = "neutral"
						}
					]
				}
			]
		});

		DB.story.all.Add($"WeGotHurtButNotTooBad_{eddie}_0", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
			enemyShotJustHit = true,
			minDamageDealtToPlayerThisTurn = 1,
			maxDamageDealtToPlayerThisTurn = 1,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Someone should fix that.",
					loopTag = Instance.NeutralAnim
				}
			]
		});
		DB.story.all.Add($"WeGotHurtButNotTooBad_{eddie}_1", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
			enemyShotJustHit = true,
			minDamageDealtToPlayerThisTurn = 1,
			maxDamageDealtToPlayerThisTurn = 1,
			oncePerRun = true,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Aw man, I think my couch was there.",
					loopTag = Instance.DisappointedAnim
				}
			]
		});
		DB.story.all.Add($"WeGotHurtButNotTooBad_{eddie}_2", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
			enemyShotJustHit = true,
			minDamageDealtToPlayerThisTurn = 1,
			maxDamageDealtToPlayerThisTurn = 1,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Hm, less scrap to work with.",
					loopTag = Instance.AnnoyedAnim
				}
			]
		});

		DB.story.all.Add($"WeGotShotButTookNoDamage_{eddie}_0", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
			enemyShotJustHit = true,
			maxDamageDealtToPlayerThisTurn = 0,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "See? Everything's fine."
				}
			]
		});

		DB.story.all.Add($"WeDidOverThreeDamage_{eddie}_0", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
			playerShotJustHit = true,
			minDamageDealtToEnemyThisTurn = 4,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "We can do better than that.",
					loopTag = Instance.NeutralAnim
				}
			]
		});
		DB.story.all.Add($"WeDidOverThreeDamage_{eddie}_1", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
			playerShotJustHit = true,
			minDamageDealtToEnemyThisTurn = 4,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "That's good damage.",
					loopTag = Instance.NeutralAnim
				}
			]
		});
		DB.story.all.Add($"WeDidOverThreeDamage_{eddie}_2", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
			playerShotJustHit = true,
			minDamageDealtToEnemyThisTurn = 4,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Nice.",
					loopTag = Instance.NeutralAnim
				}
			]
		});
		
		DB.story.all.Add($"WeDidOverFiveDamage_{eddie}_0", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
			playerShotJustHit = true,
			minDamageDealtToEnemyThisTurn = 6,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "That's more like it.",
					loopTag = Instance.NeutralAnim
				}
			]
		});
		DB.story.all.Add($"WeDidOverFiveDamage_{eddie}_1", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
			playerShotJustHit = true,
			minDamageDealtToEnemyThisTurn = 6,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "They'll be feeling that one.",
					loopTag = Instance.NeutralAnim
				}
			]
		});
		DB.story.all.Add($"WeDidOverFiveDamage_{eddie}_2", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
			playerShotJustHit = true,
			minDamageDealtToEnemyThisTurn = 6,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Lights out.",
					loopTag = Instance.NeutralAnim
				}
			]
		});
		DB.story.all.Add($"WeDidOverFiveDamage_{eddie}_3", new()
		{
			type = NodeType.combat,
			allPresent = [eddie, Deck.dizzy.Key()],
			playerShotJustHit = true,
			minDamageDealtToEnemyThisTurn = 6,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "This kind of power is what all true electricians strive for.",
					loopTag = Instance.ExplainsAnim
				},
				new CustomSay()
				{
					who = Deck.dizzy.Key(),
					Text = "Where exactly did you get your education?",
					loopTag = "squint"
				}
			]
		});

		DB.story.all.Add($"EnemyArmorPierced_Multi_{eddie}_0", new()
		{
			type = NodeType.combat,
			playerShotJustHit = true,
			playerJustPiercedEnemyArmor = true,
			oncePerCombatTags = ["EnemyArmorPierced"],
			oncePerRun = true,
			lines = [
                new CustomSay() {
					who = eddie,
					Text = "Why bother with armor?",
					loopTag = Instance.ExplainsAnim
				}
			]
		});
		DB.story.all.Add($"EnemyArmorPierced_{eddie}_1", new()
		{
			type = NodeType.combat,
			playerShotJustHit = true,
			allPresent = [eddie],
			playerJustPiercedEnemyArmor = true,
			oncePerCombatTags = ["EnemyArmorPierced"],
			oncePerRun = true,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Phew, I was afraid we'd need to care about the armor.",
					loopTag = Instance.RestingAnim
				}
			]
		});

		DB.story.all.Add($"EnemyHasBrittle_{eddie}_0", new()
		{
			type = NodeType.combat,
			enemyHasBrittlePart = true,
			allPresent = [eddie],
			oncePerRunTags = ["yelledAboutBrittle"],
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "A brittle part? Someone should get fired for that.",
					loopTag = Instance.ExplainsAnim
				}
			]
		});
		DB.story.all.Add($"EnemyHasBrittle_{eddie}_1", new()
		{
			type = NodeType.combat,
			enemyHasBrittlePart = true,
			allPresent = [eddie],
			oncePerRunTags = ["yelledAboutBrittle"],
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Brittle spot! Score!",
					loopTag = Instance.ExcitedAnim
				}
			]
		});
		DB.story.all.Add($"EnemyHasBrittle_{eddie}_2", new()
		{
			type = NodeType.combat,
			enemyHasBrittlePart = true,
			allPresent = [eddie],
			oncePerRunTags = ["yelledAboutBrittle"],
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Easy fight, just hit that brittle spot.",
					loopTag = Instance.ExplainsAnim
				}
			]
		});

		DB.story.all.Add($"EnemyHasWeakness_{eddie}_0", new()
		{
			type = NodeType.combat,
			enemyHasWeakPart = true,
			allPresent = [eddie],
			oncePerRunTags = ["yelledAboutWeakness"],
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Weakpoint!",
					loopTag = Instance.ExplainsAnim
				}
			]
		});
		DB.story.all.Add($"EnemyHasWeakness_{eddie}_1", new()
		{
			type = NodeType.combat,
			enemyHasWeakPart = true,
			allPresent = [eddie],
			oncePerRunTags = ["yelledAboutWeakness"],
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "If you think about it, hitting a weakpoint is basically like getting free energy.",
					loopTag = Instance.ExplainsAnim
				}
			]
		});
		DB.story.all.Add($"EnemyHasWeakness_{eddie}_2", new()
		{
			type = NodeType.combat,
			enemyHasWeakPart = true,
			allPresent = [eddie],
			oncePerRunTags = ["yelledAboutWeakness"],
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "We should hit that weakpoint, if it's not too much trouble.",
					loopTag = Instance.NeutralAnim
				}
			]
		});

		DB.story.all.Add($"PowerNapNap_{eddie}_0", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
      		oncePerCombatTags = ["PowerNap"],
			lookup = ["PowerNapNap"],
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "I'll make it up in a minute, I promise.",
					loopTag = Instance.RestingAnim
				}
			]
		});
		DB.story.all.Add($"PowerNapNap_{eddie}_1", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
      		oncePerCombatTags = ["PowerNap"],
			lookup = ["PowerNapNap"],
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Just let me stretch a little...",
					loopTag = Instance.RestingAnim
				}
			]
		});
		DB.story.all.Add($"PowerNapNap_{eddie}_2", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
      		oncePerCombatTags = ["PowerNap"],
			lookup = ["PowerNapNap"],
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "It's important to keep a clear head in dangerous situations.",
					loopTag = Instance.RestingAnim
				}
			]
		});
		DB.story.all.Add($"PowerNapNap_{eddie}_3", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
      		oncePerCombatTags = ["PowerNap"],
			lookup = ["PowerNapNap"],
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "I can feel my productivity increasing already!",
					loopTag = Instance.RestingAnim
				}
			]
		});
		DB.story.all.Add($"PowerNapNap_{eddie}_4", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
      		oncePerCombatTags = ["PowerNap"],
			lookup = ["PowerNapNap"],
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Just a sec.",
					loopTag = Instance.RestingAnim
				}
			]
		});

		DB.story.all.Add($"PowerNapAwake_{eddie}_0", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
      		oncePerCombatTags = ["PowerNap"],
			lookup = ["PowerNapAwake"],
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Fine, fine.",
					loopTag = Instance.AnnoyedAnim
				}
			]
		});
		DB.story.all.Add($"PowerNapAwake_{eddie}_1", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
      		oncePerCombatTags = ["PowerNap"],
			lookup = ["PowerNapAwake"],
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "I guess if it's an emergency.",
					loopTag = Instance.AnnoyedAnim
				}
			]
		});
		DB.story.all.Add($"PowerNapAwake_{eddie}_2", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
      		oncePerCombatTags = ["PowerNap"],
			lookup = ["PowerNapAwake"],
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Duty calls...",
					loopTag = Instance.AnnoyedAnim
				}
			]
		});
		DB.story.all.Add($"PowerNapAwake_{eddie}_3", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
      		oncePerCombatTags = ["PowerNap"],
			lookup = ["PowerNapAwake"],
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "That's fine. I'll take some time off during FTL.",
					loopTag = Instance.ExplainsAnim
				}
			]
		});
		DB.story.all.Add($"PowerNapAwake_{eddie}_4", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
      		oncePerCombatTags = ["PowerNap"],
			lookup = ["PowerNapAwake"],
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Can't catch a break, huh.",
					loopTag = Instance.AnnoyedAnim
				}
			]
		});
		DB.story.all.Add($"PowerNapAwake_{eddie}_5", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
      		oncePerCombatTags = ["PowerNap"],
			lookup = ["PowerNapAwake"],
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Yeah, yeah, I'm on it.",
					loopTag = Instance.AnnoyedAnim
				}
			]
		});
		DB.story.all.Add($"PowerNapAwake_{eddie}_6", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
      		oncePerCombatTags = ["PowerNap"],
			lookup = ["PowerNapAwake"],
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "I'll give it my all.",
					loopTag = Instance.AnnoyedAnim
				}
			]
		});

		DB.story.all.Add($"GammaRay_{eddie}_0", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
      		oncePerCombatTags = ["GammaRay"],
			lookup = ["GammaRay"],
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Wooooo!",
					loopTag = Instance.ExcitedAnim
				}
			]
		});
		DB.story.all.Add($"GammaRay_{eddie}_1", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
      		oncePerCombatTags = ["GammaRay"],
			lookup = ["GammaRay"],
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Yes!",
					loopTag = Instance.ExcitedAnim
				}
			]
		});
		DB.story.all.Add($"GammaRay_{eddie}_2", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
      		oncePerCombatTags = ["GammaRay"],
			lookup = ["GammaRay"],
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Awesome!",
					loopTag = Instance.ExcitedAnim
				}
			]
		});
		DB.story.all.Add($"GammaRay_{eddie}_3", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
      		oncePerCombatTags = ["GammaRay"],
			lookup = ["GammaRay"],
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "It's just as satisfying as I expected it to be!",
					loopTag = Instance.ExplainsAnim
				},
				new SaySwitch()
				{
					lines = [
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
					]
				}
			]
		});
		DB.story.all.Add($"GammaRay_{eddie}_4", new()
		{
			type = NodeType.combat,
			allPresent = [eddie, Deck.peri.Key()],
      		oncePerCombatTags = ["GammaRay"],
			lookup = ["GammaRay"],
			lines = [
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
					loopTag = Instance.ExcitedAnim
				}
			]
		});
		DB.story.all.Add($"GammaRay_{eddie}_5", new()
		{
			type = NodeType.combat,
			allPresent = [eddie, Deck.eunice.Key()],
      		oncePerCombatTags = ["GammaRay"],
			lookup = ["GammaRay"],
			lines = [
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
					loopTag = Instance.ExcitedAnim
				}
			]
		});
		DB.story.all.Add($"GammaRay_{eddie}_6", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
      		oncePerCombatTags = ["GammaRay"],
			lookup = ["GammaRay"],
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Was that cool or what?",
					loopTag = Instance.ExcitedAnim
				},
				new SaySwitch()
				{
					lines = [
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
					]
				}
			]
		});
		DB.story.all.Add($"GammaRay_{eddie}_7", new()
		{
			type = NodeType.combat,
			allPresent = [eddie],
      		oncePerCombatTags = ["GammaRay"],
			lookup = ["GammaRay"],
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Fire the gamma ray! We'll worry about the radiation later.",
					loopTag = Instance.ExcitedAnim
				},
				new SaySwitch()
				{
					lines = [
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
					]
				}
			]
		});

		DB.story.GetNode("CrabFacts1_Multi_0")?.lines.OfType<SaySwitch>().LastOrDefault()?.lines.Insert(0, new CustomSay()
		{
			who = eddie,
			Text = "I can't be bothered to fact-check that.",
			loopTag = Instance.NeutralAnim
		});
		

		DB.story.GetNode("JustPlayedASashaCard_Multi_2")?.lines.OfType<SaySwitch>().LastOrDefault()?.lines.Insert(0, new CustomSay()
		{
			who = eddie,
			Text = "Sports.",
			loopTag = Instance.SquintAnim
		});

		DB.story.all.Add($"PlayedInfinite_{eddie}_0", new StoryNode()
		{
			type = NodeType.combat,
			allPresent = [eddie],
			oncePerCombat = true,
			oncePerCombatTags = ["PlayedInfinite"],
			lines = [
				new SaySwitch()
				{
					lines =
                    [
                        new CustomSay()
						{
							who = eddie,
							Text = "We can do that again, if we'd like.",
							loopTag = Instance.NeutralAnim
						},
						new CustomSay()
						{
							who = eddie,
							Text = "Let's just do the same thing again.",
							loopTag = Instance.ExcitedAnim
						},
						new CustomSay()
						{
							who = eddie,
							Text = "It's important to keep your options open.",
							loopTag = Instance.ExplainsAnim
						},
						new CustomSay()
						{
							who = eddie,
							Text = "I'm a firm believer in renewables.",
							loopTag = Instance.ExplainsAnim
						},
						new CustomSay()
						{
							who = eddie,
							Text = "Should I stop?",
							loopTag = Instance.NeutralAnim
						},
						new CustomSay()
						{
							who = eddie,
							Text = "This should keep me occupied.",
							loopTag = Instance.NeutralAnim
						},
						new CustomSay()
						{
							who = eddie,
							Text = "Again!",
							loopTag = Instance.ExcitedAnim
						},
						new CustomSay()
						{
							who = eddie,
							Text = "Had enough yet?",
							loopTag = Instance.NeutralAnim
						},
					]
				}
			]
		}.WithModData(StoryVarsAdditions.CardJustPlayedWasInfiniteKey, true));
		DB.story.all.Add($"PlayedDiscount_{eddie}_0", new StoryNode()
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
							loopTag = Instance.ExplainsAnim
						},
						new CustomSay()
						{
							who = eddie,
							Text = "Don't ask me where I got this stuff. I don't even remember.",
							loopTag = Instance.NeutralAnim
						},
						new CustomSay()
						{
							who = eddie,
							Text = "Here's a life hack that can save you a bit of energy!",
							loopTag = Instance.ExplainsAnim
						},
						new CustomSay()
						{
							who = eddie,
							Text = "That's it for that discount.",
							loopTag = Instance.NeutralAnim
						},
						new CustomSay()
						{
							who = eddie,
							Text = "Cutting a few corners can save you some power.",
							loopTag = Instance.ExplainsAnim
						},
						new CustomSay()
						{
							who = eddie,
							Text = "Might as well do it while it's easy.",
							loopTag = Instance.NeutralAnim
						},
						new CustomSay()
						{
							who = eddie,
							Text = "It was fun while it lasted.",
							loopTag = Instance.DisappointedAnim
						},
						new CustomSay()
						{
							who = eddie,
							Text = "I'm a big fan of discounts.",
							loopTag = Instance.ExplainsAnim
						},
						new CustomSay()
						{
							who = eddie,
							Text = "What a bargain!",
							loopTag = Instance.ExcitedAnim
						}
					]
				}
			]
		}.WithModData(StoryVarsAdditions.MinDiscountOfCardJustPlayedKey, 1));

		DB.story.all.Add($"PlayedExpensive_{eddie}_0", new StoryNode()
		{
			type = NodeType.combat,
			allPresent = [eddie],
			oncePerCombat = true,
			oncePerCombatTags = ["PlayedExpensive"],
			lines = [
				new SaySwitch()
				{
					lines = [
                        new CustomSay()
						{
							who = eddie,
							Text = "Ew.",
							loopTag = Instance.AnnoyedAnim
						},
						new CustomSay()
						{
							who = eddie,
							Text = "What is this? Who thought of this?",
							loopTag = Instance.AnnoyedAnim
						},
						new CustomSay()
						{
							who = eddie,
							Text = "I hated every second of that.",
							loopTag = Instance.AnnoyedAnim
						},
						new CustomSay()
						{
							who = eddie,
							Text = "That really rubs me the wrong way.",
							loopTag = Instance.AnnoyedAnim
						},
						new CustomSay()
						{
							who = eddie,
							Text = "Never again.",
							loopTag = Instance.AnnoyedAnim
						}
					]
				}
			]
		}.WithModData(StoryVarsAdditions.MaxDiscountOfCardJustPlayedKey, -1));

		DB.story.all.Add($"JustPlayedASogginsCard_{eddie}_0", new() {
			type = NodeType.combat,
			whoDidThat = Deck.soggins,
			oncePerRun = true,
			allPresent = [eddie],
			lines = [
				new CustomSay() {
					who = eddie,
					Text = "We're still alive? That's good.",
					loopTag = Instance.NeutralAnim
				}
			]
		});

		DB.story.all.Add($"JustPlayedAToothCard_{eddie}_0", new() {
			type = NodeType.combat,
			whoDidThat = Deck.tooth,
			oncePerRun = true,
			allPresent = [eddie],
			lines = [
				new CustomSay() {
					who = eddie,
					Text = "You can get a lot of use out of junk like this.",
					loopTag = Instance.ExplainsAnim
				}
			]
		});

		DB.story.all.Add("summonEddie_0", new()
		{
			type = NodeType.combat,
			allPresent = ["comp"],
			lookup = ["summonEddie"],
			oncePerCombatTags = [$"summonEddieTag"],
			oncePerRun = true,
			lines = [
				new CustomSay()
				{
					who = "comp",
					Text = "Let's see what I can do with this.",
					loopTag = "neutral"
				}
			]
		});
		DB.story.all.Add("summonEddie_1", new()
		{
			type = NodeType.combat,
			allPresent = ["comp", eddie],
			lookup = ["summonEddie"],
			oncePerCombatTags = [$"summonEddieTag"],
			oncePerRun = true,
			lines = [
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
			]
		});

		DB.story.GetNode("DillianShouts")?.lines.OfType<SaySwitch>().First().lines.Insert(0, new CustomSay()
		{
			who = eddie,
			Text = "Whaddup.",
		});

		if (StoryVarsAdditions.SogginsName != null) {
			DB.story.all.Add($"{StoryVarsAdditions.SogginsName}_BotchResponse_{eddie}_0", new()
			{
				type = NodeType.combat,
				lookup = [$"{StoryVarsAdditions.SogginsName}_BotchResponse_{eddie}"],
				allPresent = [eddie],
				oncePerCombat = true,
				lines = [
					new CustomSay
					{
						who = eddie,
						Text = "Surely that won't happen again?"
					}
				]
			});
			DB.story.all.Add($"{StoryVarsAdditions.SogginsName}_BotchResponse_{eddie}_1", new()
			{
				type = NodeType.combat,
				lookup = [$"{StoryVarsAdditions.SogginsName}_BotchResponse_{eddie}"],
				allPresent = [eddie],
				oncePerCombat = true,
				lines = [
					new CustomSay
					{
						who = eddie,
						Text = "Happens to the best of us. Just a lot less often."
					}
				]
			});
			DB.story.all.Add($"{StoryVarsAdditions.SogginsName}_BotchResponse_{eddie}_2", new()
			{
				type = NodeType.combat,
				lookup = [$"{StoryVarsAdditions.SogginsName}_BotchResponse_{eddie}"],
				allPresent = [eddie],
				oncePerCombat = true,
				lines = [
					new CustomSay
					{
						who = eddie,
						Text = "Please tell me there is hidden genius at work here.",
						loopTag = "squint"
					}
				]
			});
			DB.story.all.Add($"{StoryVarsAdditions.SogginsName}_BotchResponseAlone_{eddie}_0", new()
			{
				type = NodeType.combat,
				lookup = [$"{StoryVarsAdditions.SogginsName}_BotchResponse_{eddie}"],
				allPresent = [eddie],
				nonePresent = [StoryVarsAdditions.SogginsName],
				oncePerCombat = true,
				lines = [
					new CustomSay
					{
						who = eddie,
						Text = "There is a Soggins inside all of us."
					}
				]
			});

			DB.story.all.Add($"{StoryVarsAdditions.SogginsName}_DoubleResponse_{eddie}_0", new()
			{
				type = NodeType.combat,
				lookup = [$"{StoryVarsAdditions.SogginsName}_DoubleResponse"],
				allPresent = [eddie],
				oncePerCombat = true,
				lines = [
					new CustomSay
					{
						who = eddie,
						Text = "I'm glad to see there's a method to the madness."
					}
				]
			});
			DB.story.all.Add($"{StoryVarsAdditions.SogginsName}_DoubleResponse_{eddie}_1", new()
			{
				type = NodeType.combat,
				lookup = [$"{StoryVarsAdditions.SogginsName}_DoubleResponse"],
				allPresent = [eddie],
				oncePerCombat = true,
				lines = [
					new CustomSay
					{
						who = eddie,
						Text = "Wow, that's a visionary approach."
					}
				]
			});
			DB.story.all.Add($"{StoryVarsAdditions.SogginsName}_DoubleResponse_{eddie}_2", new()
			{
				type = NodeType.combat,
				lookup = [$"{StoryVarsAdditions.SogginsName}_DoubleResponse"],
				allPresent = [eddie],
				oncePerCombat = true,
				lines = [
					new CustomSay
					{
						who = eddie,
						Text = "I'll take what I can get."
					}
				]
			});
		}


		
		Instance.Helper.ModRegistry.AwaitApiOrNull<ITwosAPI>("Mezz.TwosCompany", (api) => {
			if (api == null) return;
			string sorrelKey = "mezz_Sorrel";

			DB.story.all.Add("mezz_Ilya_WeJustGainedHeat_Eddie_0", new()
			{
				type = NodeType.combat,
				lastTurnPlayerStatuses = [Status.heat],
				allPresent = [eddie, api.IlyaDeck.GlobalName],
				oncePerCombatTags = ["IlyaCanYouDoSomethingAboutTheHeatPlease"],
				lines = [
					new SaySwitch()
					{
						lines = [
							new CustomSay()
							{
								who = eddie,
								Text = "Wow, this is nice!",
								loopTag = Instance.ExcitedAnim
							},
							new CustomSay()
							{
								who = eddie,
								Text = "I did feel a little cold before.",
								loopTag = Instance.NeutralAnim
							},
							new CustomSay()
							{
								who = eddie,
								Text = "Ah, cozy...",
								loopTag = Instance.NeutralAnim
							},
							new CustomSay()
							{
								who = eddie,
								Text = "A little heat never hurt anyone.",
								loopTag = Instance.NeutralAnim
							},
							new CustomSay()
							{
								who = eddie,
								Text = "I like this temperature more.",
								loopTag = Instance.NeutralAnim
							},
							new CustomSay()
							{
								who = eddie,
								Text = "Yeah! Crank it up!",
								loopTag = Instance.ExcitedAnim
							},
							new CustomSay()
							{
								who = eddie,
								Text = "We should always heat the ship like this.",
								loopTag = Instance.ExplainsAnim
							},
							new CustomSay()
							{
								who = eddie,
								Text = "We should do this more often.",
								loopTag = Instance.ExplainsAnim
							},
							new CustomSay()
							{
								who = eddie,
								Text = "I work better in the heat.",
								loopTag = Instance.ExplainsAnim
							},
						]
					},
					new SaySwitch()
					{
						lines = [
                            new CustomSay()
							{
								who = api.IlyaDeck.GlobalName,
								Text = "...sure?",
								loopTag = "bashful"
							},
							new CustomSay()
							{
								who = api.IlyaDeck.GlobalName,
								Text = "I'm glad you agree.",
								loopTag = "neutral"
							},
							new CustomSay()
							{
								who = api.IlyaDeck.GlobalName,
								Text = "I'll keep it coming.",
								loopTag = "neutral"
							},
						]
					}
				]
			});
			DB.story.all[$"EnemyArmorHitLots_{eddie}_0"].lines.OfType<SaySwitch>().First().lines.AddRange([
				new CustomSay()
				{
					who = api.GaussDeck.GlobalName,
					Text = "...",
					loopTag = "side"
				},
				new CustomSay()
				{
					who = api.IlyaDeck.GlobalName,
					Text = "It's not coming off.",
					loopTag = "neutral"
				},
				new CustomSay()
				{
					who = api.IsabelleDeck.GlobalName,
					Text = "Valiant, but foolish.",
					loopTag = "squint"
				},
				new CustomSay()
				{
					who = api.JostDeck.GlobalName,
					Text = "...",
					loopTag = "squint"
				},
				new CustomSay()
				{
					who = api.NolaDeck.GlobalName,
					Text = "Quite the competent crew you've got here, CAT.",
					loopTag = "smug"
				},
				new CustomSay()
				{
					who = sorrelKey,
					Text = "The tree which does not bend breaks in the wind."
				}
			]);
			DB.story.all[$"EnemyArmorHitLots_{eddie}_1"].lines.OfType<SaySwitch>().First().lines.AddRange([
				new CustomSay()
				{
					who = api.GaussDeck.GlobalName,
					Text = "...",
					loopTag = "side"
				},
				new CustomSay()
				{
					who = api.IsabelleDeck.GlobalName,
					Text = "Disgusting lack of ambition.",
					loopTag = "squint"
				},
				new CustomSay()
				{
					who = api.NolaDeck.GlobalName,
					Text = "Quite the competent crew you've got here, CAT.",
					loopTag = "smug"
				},
				new CustomSay()
				{
					who = sorrelKey,
					Text = "The tree which does not bend breaks in the wind."
				}
			]);
			DB.story.all[$"EnemyArmorHitLots_{eddie}_2"].lines.OfType<SaySwitch>().First().lines.AddRange([
				new CustomSay()
				{
					who = api.GaussDeck.GlobalName,
					Text = "...",
					loopTag = "side"
				},
				new CustomSay()
				{
					who = api.IsabelleDeck.GlobalName,
					Text = "Give it up.",
					loopTag = "squint"
				},
				new CustomSay()
				{
					who = api.IlyaDeck.GlobalName,
					Text = "It's not coming off.",
					loopTag = "neutral"
				},
				new CustomSay()
				{
					who = api.JostDeck.GlobalName,
					Text = "...",
					loopTag = "squint"
				},
				new CustomSay()
				{
					who = api.NolaDeck.GlobalName,
					Text = "Quite the competent crew you've got here, CAT.",
					loopTag = "smug"
				},
				new CustomSay()
				{
					who = sorrelKey,
					Text = "Stop. Find another way.",
					loopTag = "annoyed"
				}
			]);
			DB.story.all[$"EnemyArmorHitLots_{eddie}_3"].lines.OfType<SaySwitch>().First().lines.AddRange([
				new CustomSay()
				{
					who = api.GaussDeck.GlobalName,
					Text = "...",
					loopTag = "side"
				},
				new CustomSay()
				{
					who = api.IsabelleDeck.GlobalName,
					Text = "Yeah, that'll work.",
					loopTag = "squint"
				},
				new CustomSay()
				{
					who = api.JostDeck.GlobalName,
					Text = "...",
					loopTag = "squint"
				},
				new CustomSay()
				{
					who = api.NolaDeck.GlobalName,
					Text = "Quite the competent crew you've got here, CAT.",
					loopTag = "smug"
				},
				new CustomSay()
				{
					who = sorrelKey,
					Text = "Stop. Find another way.",
					loopTag = "annoyed"
				}
			]);
			DB.story.all[$"ExpensiveCardPlayed_{eddie}_1"].lines.OfType<SaySwitch>().First().lines.AddRange([
				new CustomSay()
				{
					who = api.GaussDeck.GlobalName,
					Text = "Have you heard of workplace safety?",
					loopTag = "squint"
				},
				new CustomSay()
				{
					who = api.IlyaDeck.GlobalName,
					Text = "..!",
					loopTag = "happy"
				},
				new CustomSay()
				{
					who = api.NolaDeck.GlobalName,
					Text = "Then go fix it.",
					loopTag = "annoyed"
				}
			]);
			DB.story.all[$"ExpensiveCardPlayed_{eddie}_2"].lines.OfType<SaySwitch>().First().lines.AddRange([
				new CustomSay()
				{
					who = api.GaussDeck.GlobalName,
					Text = "You would deny me such joy?",
					loopTag = "smug"
				},
				new CustomSay()
				{
					who = api.NolaDeck.GlobalName,
					Text = "No. Keep going."
				}
			]);
			DB.story.all[$"HandOnlyHasTrashCards_{eddie}"].lines.OfType<SaySwitch>().First().lines.AddRange([
				new CustomSay()
				{
					who = api.IsabelleDeck.GlobalName,
					Text = "I don't enjoy wallowing in filth.",
					loopTag = "angry"
				},
				new CustomSay()
				{
					who = api.GaussDeck.GlobalName,
					Text = "I'm surprised you get anything done."
				}
			]);
			DB.story.all[$"WeDontOverlapWithEnemyAtAllButWeDoHaveASeekerToDealWith_{eddie}"].lines.OfType<SaySwitch>().First().lines.AddRange([
				new CustomSay()
				{
					who = api.IlyaDeck.GlobalName,
					Text = "How did we miss that?",
					loopTag = "squint"
				},
				new CustomSay()
				{
					who = api.GaussDeck.GlobalName,
					Text = "I could've handled that.",
					loopTag = "squint"
				}
			]);
			DB.story.all[$"IMissedOopsie_{eddie}_2"].lines.OfType<SaySwitch>().First().lines.AddRange([
				new CustomSay()
				{
					who = api.JostDeck.GlobalName,
					Text = "There is no cheating in war."
				}
			]);
			DB.story.all[$"WeMissedOopsie_{eddie}_1"].lines.OfType<SaySwitch>().First().lines.AddRange([
				new CustomSay()
				{
					who = api.IsabelleDeck.GlobalName,
					Text = "A comfortable lie.",
					loopTag = "squint"
				},
				new CustomSay()
				{
					who = sorrelKey,
					Text = "Learn from your mistakes. Do better."
				}
			]);
			DB.story.all[$"WeMissedOopsie_{eddie}_2"].lines.OfType<SaySwitch>().First().lines.AddRange([
				new CustomSay()
				{
					who = api.IsabelleDeck.GlobalName,
					Text = "Do you ever take things seriously?",
					loopTag = "squint"
				},
				new CustomSay()
				{
					who = api.NolaDeck.GlobalName,
					Text = "Let's not encourage this kind of mindset.",
					loopTag = "annoyed"
				}
			]);
			DB.story.all[$"GammaRay_{eddie}_3"].lines.OfType<SaySwitch>().Last().lines.AddRange([
				new CustomSay()
				{
					who = api.JostDeck.GlobalName,
					Text = "I've never seen anything like this...",
					loopTag = "happy"
				},
				new CustomSay()
				{
					who = api.GaussDeck.GlobalName,
					Text = "Oh yeah, this is fantastic stuff.",
					loopTag = "datapad"
				}
			]);
			DB.story.all[$"GammaRay_{eddie}_6"].lines.OfType<SaySwitch>().Last().lines.AddRange([
				new CustomSay()
				{
					who = api.JostDeck.GlobalName,
					Text = "I've never seen anything like this...",
					loopTag = "happy"
				},
				new CustomSay()
				{
					who = api.GaussDeck.GlobalName,
					Text = "Just what I wanted to see!",
					loopTag = "happy"
				},
				new CustomSay()
				{
					who = api.IlyaDeck.GlobalName,
					Text = "This... is a bit much...",
					loopTag = "shocked"
				}
			]);
			DB.story.all[$"GammaRay_{eddie}_7"].lines.OfType<SaySwitch>().Last().lines.AddRange([
				new CustomSay()
				{
					who = api.GaussDeck.GlobalName,
					Text = "Don't worry, radiation poisoning doesn't persist between loops.",
					loopTag = "datapad"
				},
				new CustomSay()
				{
					who = api.NolaDeck.GlobalName,
					Text = "Always an exciting loop with you, isn't it?"
				}
			]);
			DB.story.all.Add($"IMissedOopsie_{eddie}_Isabelle_0", new()
			{
				type = NodeType.combat,
				allPresent = [eddie, api.IsabelleDeck.GlobalName],
				whoDidThat = Instance.EddieDeck,
				playerShotJustMissed = true,
				oncePerRun = true,
				hasArtifacts = ["TwosCompany.Artifacts.FlawlessCore"],
				lines = [
					new SaySwitch()
					{
						lines = [
							new CustomSay()
							{
								who = eddie,
								Text = "That was a test shot.",
								loopTag = Instance.NeutralAnim
							},
							new CustomSay()
							{
								who = eddie,
								Text = "Oops, wrong button.",
								loopTag = Instance.SquintAnim
							},
						]
					},
					new SaySwitch()
					{
						lines = [
							new CustomSay()
							{
								who = api.IsabelleDeck.GlobalName,
								Text = "I will gut you like a fish.",
								loopTag = "swordsquint"
							},
							new CustomSay()
							{
								who = api.IsabelleDeck.GlobalName,
								Text = "Say your prayers, serpent.",
								loopTag = "swordsquint"
							}
						]
					}
				]
			});
			
		
			


			DB.story.all.Add($"Duo_AboutToDieAndLoop_{eddie}18", new()
			{
				type = NodeType.combat,
				allPresent = [eddie, api.NolaDeck.GlobalName],
				enemyShotJustHit = true,
				maxHull = 2,
				oncePerCombatTags = ["aboutToDie"],
				oncePerRun = true,
				lines = [
                    new CustomSay()
					{
						who = eddie,
						Text = "Man, I dunno, I'm kinda not feeling motivated anymore.",
						loopTag = Instance.DisappointedAnim
					},
					new CustomSay()
					{
						who = api.NolaDeck.GlobalName,
						Text = "Can this wait?",
						loopTag = "annoyed"
					}
				]
			});
			DB.story.all.Add($"Duo_AboutToDieAndLoop_{eddie}19", new()
			{
				type = NodeType.combat,
				allPresent = [eddie, api.NolaDeck.GlobalName],
				enemyShotJustHit = true,
				maxHull = 2,
				oncePerCombatTags = ["aboutToDie"],
				oncePerRun = true,
				lines = [
					new CustomSay()
					{
						who = api.NolaDeck.GlobalName,
						Text = "Eddie. Focus. Try to find a way out of this.",
						loopTag = "squint"
					},
                    new CustomSay()
					{
						who = eddie,
						Text = "I- I'm not good under pressure!",
						loopTag = Instance.OnEdgeAnim
					}
				]
			});

			DB.story.all.Add($"Duo_AboutToDieAndLoop_{eddie}20", new()
			{
				type = NodeType.combat,
				allPresent = [eddie, api.IsabelleDeck.GlobalName],
				enemyShotJustHit = true,
				maxHull = 2,
				oncePerCombatTags = ["aboutToDie"],
				oncePerRun = true,
				lines = [
                    new CustomSay()
					{
						who = eddie,
						Text = "Eh. You win some, you lose some.",
						loopTag = Instance.ExplainsAnim
					},
					new CustomSay()
					{
						who = api.IsabelleDeck.GlobalName,
						Text = "Speak for yourself.",
						loopTag = "angry"
					}
				]
			});
			DB.story.all.Add($"Duo_AboutToDieAndLoop_{eddie}21", new()
			{
				type = NodeType.combat,
				allPresent = [eddie, api.IsabelleDeck.GlobalName],
				enemyShotJustHit = true,
				maxHull = 2,
				oncePerCombatTags = ["aboutToDie"],
				oncePerRun = true,
				lines = [
					new CustomSay()
					{
						who = api.IsabelleDeck.GlobalName,
						Text = "Don't go thinking you can give up already.",
						loopTag = "angry"
					},
                    new CustomSay()
					{
						who = eddie,
						Text = "Fiiiiine...",
						loopTag = Instance.AnnoyedLeftAnim
					}
				]
			});

			DB.story.all.Add($"Duo_AboutToDieAndLoop_{eddie}22", new()
			{
				type = NodeType.combat,
				allPresent = [eddie, api.IlyaDeck.GlobalName],
				enemyShotJustHit = true,
				maxHull = 2,
				oncePerCombatTags = ["aboutToDie"],
				oncePerRun = true,
				lines = [
                    new CustomSay()
					{
						who = eddie,
						Text = "The fire doesn't seem to be working out.",
						loopTag = Instance.SquintAnim
					},
					new CustomSay()
					{
						who = api.IlyaDeck.GlobalName,
						Text = "Just give it time...",
						loopTag = "bashful"
					}
				]
			});
			DB.story.all.Add($"Duo_AboutToDieAndLoop_{eddie}23", new()
			{
				type = NodeType.combat,
				allPresent = [eddie, api.IlyaDeck.GlobalName],
				enemyShotJustHit = true,
				maxHull = 2,
				oncePerCombatTags = ["aboutToDie"],
				oncePerRun = true,
				lines = [
					new CustomSay()
					{
						who = api.IlyaDeck.GlobalName,
						Text = "This... isn't working.",
						loopTag = "forlorn"
					},
                    new CustomSay()
					{
						who = eddie,
						Text = "Hey, c'mon, it's gonna work out in the end.",
						loopTag = Instance.WorriedAnim
					}
				]
			});

			DB.story.all.Add($"Duo_AboutToDieAndLoop_{eddie}24", new()
			{
				type = NodeType.combat,
				allPresent = [eddie, api.JostDeck.GlobalName],
				enemyShotJustHit = true,
				maxHull = 2,
				oncePerCombatTags = ["aboutToDie"],
				oncePerRun = true,
				lines = [
                    new CustomSay()
					{
						who = eddie,
						Text = "Bah, I'm not doing well.",
						loopTag = Instance.AnnoyedLeftAnim
					},
					new CustomSay()
					{
						who = api.JostDeck.GlobalName,
						Text = "Breathe. Stay focused. We still need you.",
						loopTag = "neutral"
					}
				]
			});
			DB.story.all.Add($"Duo_AboutToDieAndLoop_{eddie}25", new()
			{
				type = NodeType.combat,
				allPresent = [eddie, api.JostDeck.GlobalName],
				enemyShotJustHit = true,
				maxHull = 2,
				oncePerCombatTags = ["aboutToDie"],
				oncePerRun = true,
				lines = [
					new CustomSay()
					{
						who = api.JostDeck.GlobalName,
						Text = "When facing adversity, all one can do is to keep fighting.",
						loopTag = "swordneutral"
					},
                    new CustomSay()
					{
						who = eddie,
						Text = "That's deep.",
						loopTag = Instance.NeutralAnim
					}
				]
			});

			DB.story.all.Add($"Duo_AboutToDieAndLoop_{eddie}26", new()
			{
				type = NodeType.combat,
				allPresent = [eddie, api.GaussDeck.GlobalName],
				enemyShotJustHit = true,
				maxHull = 2,
				oncePerCombatTags = ["aboutToDie"],
				oncePerRun = true,
				lines = [
                    new CustomSay()
					{
						who = eddie,
						Text = "I think you gotta zap 'em more.",
						loopTag = Instance.SquintAnim
					},
					new CustomSay()
					{
						who = api.GaussDeck.GlobalName,
						Text = "That's what I've been doing this whole time!",
						loopTag = "angry"
					}
				]
			});
			DB.story.all.Add($"Duo_AboutToDieAndLoop_{eddie}27", new()
			{
				type = NodeType.combat,
				allPresent = [eddie, api.GaussDeck.GlobalName],
				enemyShotJustHit = true,
				maxHull = 2,
				oncePerCombatTags = ["aboutToDie"],
				oncePerRun = true,
				lines = [
					new CustomSay()
					{
						who = api.GaussDeck.GlobalName,
						Text = "The situation is dire. You gotta pull one of your insane electrical stunts!",
						loopTag = "happy"
					},
                    new CustomSay()
					{
						who = eddie,
						Text = "I'll cook something up.",
						loopTag = Instance.NeutralAnim
					}
				]
			});

			DB.story.all.Add($"Duo_AboutToDieAndLoop_{eddie}28", new()
			{
				type = NodeType.combat,
				allPresent = [eddie, sorrelKey],
				enemyShotJustHit = true,
				maxHull = 2,
				oncePerCombatTags = ["aboutToDie"],
				oncePerRun = true,
				lines = [
                    new CustomSay()
					{
						who = eddie,
						Text = "How about we start over?"
					},
					new CustomSay()
					{
						who = sorrelKey,
						Text = "Patience."
					}
				]
			});
			DB.story.all.Add($"Duo_AboutToDieAndLoop_{eddie}29", new()
			{
				type = NodeType.combat,
				allPresent = [eddie, sorrelKey],
				enemyShotJustHit = true,
				maxHull = 2,
				oncePerCombatTags = ["aboutToDie"],
				oncePerRun = true,
				lines = [
					new CustomSay()
					{
						who = sorrelKey,
						Text = "Hope dies last."
					},
                    new CustomSay()
					{
						who = eddie,
						Text = "Something like that.",
						loopTag = Instance.ExplainsAnim
					}
				]
			});
		});
	}
}