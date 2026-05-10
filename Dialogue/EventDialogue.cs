using System.Linq;

namespace TheJazMaster.Eddie;

internal static class EventDialogue
{
	private static ModEntry Instance => ModEntry.Instance;

	internal static void Inject()
	{
		string eddie = Instance.EddieDeck.Key();

		DB.story.GetNode("AbandonedShipyard")?.lines.OfType<SaySwitch>().FirstOrDefault()?.lines.Insert(0, new CustomSay()
		{
			who = eddie,
			Text = "Probably just a coincidence.",
			loopTag = ModEntry.Instance.ExplainsAnim
		});
		DB.story.GetNode("AbandonedShipyard_Repaired")?.lines.OfType<SaySwitch>().FirstOrDefault()?.lines.Insert(0, new CustomSay()
		{
			who = eddie,
			Text = "See? Nothing bad happened.",
			loopTag = ModEntry.Instance.NeutralAnim
		});
		DB.story.all[$"ChoiceCardRewardOfYourColorChoice_{eddie}"] = new()
		{
			type = NodeType.@event,
			oncePerRun = true,
			allPresent = [eddie],
			bg = "BGBootSequence",
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Wow! I feel so inspired!",
					loopTag = ModEntry.Instance.ExcitedAnim
				},
				new CustomSay()
				{
					who = "comp",
					Text = "Energy readings are back to normal."
				}
			]
		};
		DB.story.GetNode("CrystallizedFriendEvent")?.lines.OfType<SaySwitch>().FirstOrDefault()?.lines.Insert(0, new CustomSay()
		{
			who = eddie,
			Text = "I think I've earned some rest.",
			loopTag = ModEntry.Instance.ExplainsAnim
		});
		DB.story.all[$"CrystallizedFriendEvent_{eddie}"] = new()
		{
			type = NodeType.@event,
			oncePerRun = true,
			allPresent = [eddie],
			bg = "BGCrystalizedFriend",
			lines = [
				new Wait()
				{
					secs = 1.5
				},
				new CustomSay()
				{
					who = eddie,
					Text = "Alright, I'm ready.",
					loopTag = ModEntry.Instance.NeutralAnim
				}
			]
		};
		DB.story.GetNode("DraculaTime")?.lines.OfType<SaySwitch>().FirstOrDefault()?.lines.Insert(0, new CustomSay()
		{
			who = eddie,
			Text = "Oh right, I saw this guy on TV.",
			loopTag = ModEntry.Instance.NeutralAnim
		});
		DB.story.GetNode("GrandmaShop")?.lines.OfType<SaySwitch>().FirstOrDefault()?.lines.Insert(0, new CustomSay()
		{
			who = eddie,
			Text = "Vanilla pudding!",
			loopTag = ModEntry.Instance.ExcitedAnim
		});
		DB.story.GetNode("LoseCharacterCard")?.lines.OfType<SaySwitch>().FirstOrDefault()?.lines.Insert(0, new CustomSay()
		{
			who = eddie,
			Text = "But I don't wanna become spaghetti!",
			loopTag = ModEntry.Instance.OnEdgeAnim
		});
		DB.story.GetNode("LoseCharacterCard_No")?.lines.OfType<SaySwitch>().FirstOrDefault()?.lines.Insert(0, new CustomSay()
		{
			who = eddie,
			Text = "I'm still an ordinary amount of spaghettified.",
			loopTag = ModEntry.Instance.NeutralAnim
		});
		DB.story.all[$"LoseCharacterCard_{eddie}"] = new()
		{
			type = NodeType.@event,
			oncePerRun = true,
			allPresent = [eddie],
			bg = "BGSupernova",
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Wow, that worked?",
					loopTag = ModEntry.Instance.WorriedAnim
				}
			]
		};
		DB.story.GetNode("Sasha_2_multi_2")?.lines.OfType<SaySwitch>().FirstOrDefault()?.lines.Insert(0, new CustomSay()
		{
			who = eddie,
			Text = "Sports...",
			loopTag = ModEntry.Instance.AnnoyedAnim
		});
		DB.story.all[$"ShopkeeperInfinite_{eddie}_Multi_0"] = new()
		{
			type = NodeType.@event,
			lookup = ["shopBefore"],
			allPresent = [eddie],
			bg = "BGShop",
			lines = [
				new CustomSay()
				{
					who = "nerd",
					Text = "Yo.",
					loopTag = "neutral",
					flipped = true
				},
				new CustomSay()
				{
					who = eddie,
					Text = "Sup.",
					loopTag = ModEntry.Instance.NeutralAnim
				},
				new Jump()
				{
					key = "NewShop"
				}
			]
		};
		DB.story.all[$"ShopkeeperInfinite_{eddie}_Multi_1"] = new()
		{
			type = NodeType.@event,
			lookup = ["shopBefore"],
			allPresent = [eddie],
			bg = "BGShop",
			lines = [
				new CustomSay()
				{
					who = "nerd",
					Text = "Ship holding together?",
					loopTag = "neutral",
					flipped = true
				},
				new CustomSay()
				{
					who = eddie,
					Text = "For now.",
					loopTag = ModEntry.Instance.ExplainsAnim
				},
				new Jump()
				{
					key = "NewShop"
				}
			]
		};

		DB.story.all[$"{eddie}_Intro_0"] = new()
		{
			type = NodeType.@event,
			lookup = ["zone_first"],
			allPresent = [eddie],
			once = true,
			bg = "BGRunStart",
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Yawn...",
					loopTag = ModEntry.Instance.RestingAnim
				},
				new CustomSay()
				{
					who = "comp",
					Text = "Wakey wakey!",
					loopTag = "neutral"
				},
				new CustomSay()
				{
					who = eddie,
					Text = "Huh? Where am I?",
					loopTag = ModEntry.Instance.SquintAnim
				},
				new CustomSay()
				{
					who = "comp",
					Text = "Time loop! Make yourself useful!",
					loopTag = "neutral"
				},
				new CustomSay()
				{
					who = eddie,
					Text = "Woah, what's the rush? Infinite time actually sounds really nice.",
					loopTag = ModEntry.Instance.ExcitedAnim
				},
				new CustomSay()
				{
					who = "comp",
					Text = "It does...",
					loopTag = "neutral"
				},
				new CustomSay()
				{
					who = "comp",
					Text = "Until the local space-time bubble implodes and erases us all from existence!",
					loopTag = "grumpy"
				},
				new CustomSay()
				{
					who = eddie,
					Text = "Woah. Can that even happen?",
					loopTag = ModEntry.Instance.SquintAnim
				},
				new CustomSay()
				{
					who = "comp",
					Text = "Probably! Now go do your thing, there's a hostile ship up ahead!",
					loopTag = "neutral"
				}
			}
		};

		DB.story.all[$"{eddie}_Intro_1"] = new()
		{
			type = NodeType.@event,
			lookup = ["zone_first"],
			allPresent = [eddie, Deck.dizzy.Key()],
			once = true,
			bg = "BGRunStart",
			requiredScenes = [$"{eddie}_Intro_0"],
			lines = new()
			{
				new CustomSay()
				{
					who = Deck.dizzy.Key(),
					Text = "Oh, Eddie! I haven't seen you in so long!",
					loopTag = "neutral"
				},
				new CustomSay()
				{
					who = eddie,
					Text = "Good to see you too, uh, Dizzy, right?",
					loopTag = ModEntry.Instance.SquintAnim
				},
				new CustomSay()
				{
					who = Deck.dizzy.Key(),
					Text = "You missed a bunch of my cool experiments. I think I did some on the core too.",
					loopTag = "neutral"
				},
				new CustomSay()
				{
					who = eddie,
					Text = "Does that have anything to do with all the blackouts lately?",
					loopTag = ModEntry.Instance.SquintAnim
				},
				new CustomSay()
				{
					who = Deck.dizzy.Key(),
					Text = "Well, we needed to divert all the power in nonessential systems to-",
					loopTag = "explains"
				},
				new CustomSay()
				{
					who = eddie,
					Text = "What? Dude, I lost, like, 10 hours of game progress to that!",
					loopTag = ModEntry.Instance.SquintAnim
				},
				new SaySwitch()
				{
					lines = new()
					{
						new CustomSay()
						{
							who = Deck.hacker.Key(),
							Text = "I told you not to disable autosaving.",
							loopTag = "squint"
						},
					}
				},
				new CustomSay()
				{
					who = "comp",
					Text = "You were playing videogames for 10 hours straight?! Don't you have a job to do?!",
					loopTag = "grumpy"
				},
				new CustomSay()
				{
					who = eddie,
					Text = "Don't sweat it, I was, uh, on lunch break.",
					loopTag = ModEntry.Instance.OnEdgeAnim
				},
				new CustomSay()
				{
					who = "comp",
					Text = "For 10 hours?!",
					loopTag = "grumpy"
				}
			}
		};

		DB.story.all[$"{eddie}_Intro_2"] = new()
		{
			type = NodeType.@event,
			lookup = ["zone_first"],
			allPresent = [eddie, Deck.peri.Key()],
			once = true,
			bg = "BGRunStart",
			requiredScenes = [$"{eddie}_Intro_0"],
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Oh, what's up?",
					loopTag = ModEntry.Instance.NeutralAnim
				},
				new CustomSay()
				{
					who = Deck.peri.Key(),
					Text = "\"What's up\"? You haven't been showing up to staff meetings for two months!",
					loopTag = "mad"
				},
				new CustomSay()
				{
					who = eddie,
					Text = "Oh, that? I was busy. Working on personal projects.",
					loopTag = ModEntry.Instance.OnEdgeAnim
				},
				new CustomSay()
				{
					who = Deck.peri.Key(),
					Text = "Personal projects? If you aren't going to be responsible, we may need to reevaluate your status as an employee...",
					loopTag = "squint"
				},
				new CustomSay()
				{
					who = eddie,
					Text = "B-but I figured out how to fire a gamma ray from a ship cannon!",
					loopTag = ModEntry.Instance.WorriedAnim
				},
				new CustomSay()
				{
					who = Deck.peri.Key(),
					Text = "...Safely?",
					loopTag = "squint"
				},
				new CustomSay()
				{
					who = eddie,
					Text = "As far as I can tell!",
					loopTag = ModEntry.Instance.ExplainsAnim
				},
				new CustomSay()
				{
					who = Deck.peri.Key(),
					Text = "Fine, we'll keep you around for now.",
					loopTag = "neutral"
				},
				new CustomSay()
				{
					who = Deck.peri.Key(),
					Text = "But after we escape this time loop, you and I will be having a serious talk.",
					loopTag = "squint"
				},
				new CustomSay()
				{
					who = eddie,
					Text = "...",
					loopTag = ModEntry.Instance.OnEdgeAnim
				},
			}
		};

		DB.story.all[$"{eddie}_Intro_3"] = new()
		{
			type = NodeType.@event,
			lookup = ["zone_first"],
			allPresent = [eddie, Deck.hacker.Key()],
			once = true,
			bg = "BGRunStart",
			requiredScenes = [$"{eddie}_Intro_0"],
			lines = new()
			{
				new CustomSay()
				{
					who = eddie,
					Text = "Max! It's been a while.",
					loopTag = ModEntry.Instance.NeutralAnim
				},
				new CustomSay()
				{
					who = Deck.hacker.Key(),
					Text = "Sup Eddie. How's it been?",
					loopTag = "smile"
				},
				new CustomSay()
				{
					who = eddie,
					Text = "So this is that AI you were working on, huh?",
					loopTag = ModEntry.Instance.ExcitedAnim
				},
				new CustomSay()
				{
					who = eddie,
					Text = "It's a bit too stern, don't you think?",
					loopTag = ModEntry.Instance.NeutralAnim
				},
				new CustomSay()
				{
					who = "comp",
					Text = "You wanna say that again?",
					loopTag = "lean"
				},
				new CustomSay()
				{
					who = Deck.hacker.Key(),
					Text = "She, uh, improved a lot after the whole time loop thing.",
					loopTag = "squint"
				},
				new CustomSay()
				{
					who = Deck.hacker.Key(),
					Text = "And what have you been up to?",
					loopTag = "smile"
				},
				new CustomSay()
				{
					who = eddie,
					Text = "I put our fridge on tracks and made it remote-controlled.",
					loopTag = ModEntry.Instance.ExplainsAnim
				},
				new CustomSay()
				{
					who = Deck.hacker.Key(),
					Text = "...Nice.",
					loopTag = "smile"
				},
			}
		};

		DB.story.all[$"{eddie}_Goat"] = new()
		{
			type = NodeType.@event,
			lookup = ["zone_lawless"],
			allPresent = [eddie, Deck.goat.Key()],
			once = true,
			lines = new()
			{
				new CustomSay()
				{
					who = Deck.goat.Key(),
					Text = "So, Eddie, I've been meaning to ask you...",
					loopTag = "shy"
				},
				new CustomSay()
				{
					who = eddie,
					Text = "What's up?",
					loopTag = ModEntry.Instance.NeutralAnim
				},
				new CustomSay()
				{
					who = Deck.goat.Key(),
					Text = "I hope it's not a sensitive topic, but...",
					loopTag = "shy"
				},
				new CustomSay()
				{
					who = Deck.goat.Key(),
					Text = "How did you lose your limbs?",
					loopTag = "neutral"
				},
				new CustomSay()
				{
					who = eddie,
					Text = "Oh, uh, actually, I never had any. I was born without limbs.",
					loopTag = ModEntry.Instance.OnEdgeAnim
				},
				new CustomSay()
				{
					who = eddie,
					Text = "I had to make do with what I had. And after a bunch of work, I built myself these cool cyborg arms.",
					loopTag = ModEntry.Instance.ExplainsAnim
				},
				new CustomSay()
				{
					who = Deck.goat.Key(),
					Text = "So, like.",
					loopTag = "shy"
				},
				new CustomSay()
				{
					who = Deck.goat.Key(),
					Text = "Did your limbs get absorbed into a really long tail or something?",
					loopTag = "neutral"
				},
				new CustomSay()
				{
					who = eddie,
					Text = "...",
					loopTag = ModEntry.Instance.AnnoyedAnim
				},
				new CustomSay()
				{
					who = Deck.goat.Key(),
					Text = "...",
					loopTag = "shy"
				},
				new CustomSay()
				{
					who = Deck.goat.Key(),
					Text = "...So, uh, I also lost an arm and a horn. It's a pretty long and very cool-",
					loopTag = "shy"
				},
				new CustomSay()
				{
					who = "comp",
					Text = "Hey, enough chatting! We're under attack!",
					loopTag = "grumpy"
				},
				new CustomSay()
				{
					who = Deck.goat.Key(),
					Text = "Nevermind.",
					loopTag = "squint"
				}
			}
		};

		DB.story.all[$"{eddie}_Books"] = new()
		{
			type = NodeType.@event,
			lookup = ["zone_lawless"],
			allPresent = [eddie, Deck.shard.Key()],
			once = true,
			lines = [
				new CustomSay()
				{
					who = Deck.shard.Key(),
					Text = "I knew it!",
					loopTag = "books"
				},
				new CustomSay()
				{
					who = eddie,
					Text = "Knew what?",
					loopTag = ModEntry.Instance.NeutralAnim
				},
				new CustomSay()
				{
					who = Deck.shard.Key(),
					Text = "You look like the monsters in my book! The ones eat you while you're asleep!",
					loopTag = "books"
				},
				new CustomSay()
				{
					who = eddie,
					Text = "What? Let me see that.",
					loopTag = ModEntry.Instance.AnnoyedAnim
				},
				new CustomSay()
				{
					who = eddie,
					Text = "...",
					loopTag = ModEntry.Instance.AnnoyedAnim
				},
				new CustomSay()
				{
					who = eddie,
					Text = "..!",
					loopTag = ModEntry.Instance.ExcitedAnim
				},
				new CustomSay()
				{
					who = eddie,
					Text = "Yeah, totally me. I've eaten, like, 20 people already.",
					loopTag = ModEntry.Instance.NeutralAnim
				},
				new CustomSay()
				{
					who = Deck.shard.Key(),
					Text = "Wow! That's so cool!",
					loopTag = "stoked"
				},
				new CustomSay()
				{
					who = Deck.shard.Key(),
					Text = "But you're not going to eat me, right?",
					loopTag = "paws"
				},
				new CustomSay()
				{
					who = eddie,
					Text = "Of course not. But I might need to eat some of your cookies instead.",
					loopTag = ModEntry.Instance.ExcitedAnim
				},
				new CustomSay()
				{
					who = "comp",
					Text = "Hey, can you stop swindling the kid already?",
					loopTag = "grumpy"
				},
				new CustomSay()
				{
					who = eddie,
					Text = "Alright, alright.",
					loopTag = ModEntry.Instance.DisappointedAnim
				},
				new CustomSay()
				{
					who = eddie,
					Text = "Hey, little amigo, how about we turn those crystals into a light fixture?",
					loopTag = ModEntry.Instance.NeutralAnim
				},
				new CustomSay()
				{
					who = Deck.shard.Key(),
					Text = "Yay!",
					loopTag = "stoked"
				}
			]
		};

		DB.story.all[$"Knight_Midcombat_Greeting_Infinite_{eddie}"] = new()
		{
      		type = NodeType.@event,
      		oncePerCombat = true,
      		allPresent = new () { eddie },
      		lookup = ["knight_duel"],
      		requiredScenes = ["Knight_Midcombat_Greeting_1"],
      		choiceFunc = "KnightCombatChoices",
      		lines = new()
			{
				new CustomSay()
				{
					who = "knight",
					Text = "What say we make this <c=keyword>an honorable duel</c>? I shan't target thine weak points if ye deign not to target mine.",
					flipped = true
				},
				new CustomSay()
				{
					who = eddie,
					Text = "Eh, I don't really feel like it.",
					loopTag = ModEntry.Instance.RestingAnim
				},
				new CustomSay()
				{
					who = "knight",
					Text = "...",
					flipped = true
				},
				new CustomSay()
				{
					who = "knight",
					Text = "Please?",
					flipped = true
				}
			}
    	};

		DB.story.GetNode("SogginsEscape_1")?.lines.OfType<SaySwitch>().FirstOrDefault()?.lines.Insert(0, new CustomSay()
		{
			who = eddie,
			Text = "I guess we're not helping you the next time.",
			loopTag = ModEntry.Instance.SquintAnim
		});

		DB.story.GetNode("SogginsEscapeIntent_1")?.lines.OfType<SaySwitch>().FirstOrDefault()?.lines.Insert(0, new CustomSay()
		{
			who = eddie,
			Text = "Finally...",
			loopTag = ModEntry.Instance.AnnoyedAnim
		});
		
		DB.story.GetNode("SogginsEscapeIntent_3")?.lines.OfType<SaySwitch>().FirstOrDefault()?.lines.Insert(0, new CustomSay()
		{
			who = eddie,
			Text = "This is pretty funny, admittedly...",
			loopTag = ModEntry.Instance.NeutralAnim
		});
		
		DB.story.GetNode("Soggins_Infinite")?.lines.OfType<SaySwitch>().FirstOrDefault()?.lines.Insert(0, new CustomSay()
		{
			who = eddie,
			Text = "Yup, he's hopeless.",
			loopTag = ModEntry.Instance.ExplainsAnim
		});

		DB.story.GetNode("SkunkFirstTurnShouts_Multi_0")?.lines.OfType<SaySwitch>().FirstOrDefault()?.lines.Insert(0, new CustomSay()
		{
			who = eddie,
			Text = "Dang. I guess we have to kill him.",
			loopTag = ModEntry.Instance.AnnoyedAnim
		});

		DB.story.all[$"RunWinWho_{eddie}_1"] = new()
		{
			type = NodeType.@event,
			bg = "BGRunWin",
      		allPresent = new () { eddie },
			lookup = [ $"runWin_{eddie}" ],
			once = true,
			lines = [
				new Wait {
					secs = 3
				},
				new CustomSay()
				{
					who = eddie,
					Text = "Oh, shouldn't you repair somebody else first?",
					loopTag = ModEntry.Instance.SquintAnim
				},
				new CustomSay()
				{
					who = "void",
					Text = "You play a part in this like all the others.",
					flipped = true
				},
				new CustomSay()
				{
					who = eddie,
					Text = "Really? I don't remember doing much of anything.",
					loopTag = ModEntry.Instance.SquintAnim
				},
				new CustomSay()
				{
					who = "void",
					Text = "Time for that to change.",
					flipped = true
				}
			]
		};
		DB.story.all[$"RunWinWho_{eddie}_2"] = new()
		{
			type = NodeType.@event,
			bg = "BGRunWin",
      		allPresent = [eddie],
			requiredScenes = [
				$"RunWinWho_{eddie}_1"
			],
			lookup = [ $"runWin_{eddie}" ],
			once = true,
			lines = [
				new Wait {
					secs = 3
				},
				new CustomSay()
				{
					who = eddie,
					Text = "Ah right, you contacted me.",
					loopTag = ModEntry.Instance.SquintAnim
				},
				new CustomSay()
				{
					who = eddie,
					Text = "Why?",
					loopTag = ModEntry.Instance.SquintAnim
				},
				new CustomSay()
				{
					who = "void",
					Text = "My influence outside of the core room was limited to a few bit flips at a time.",
					flipped = true
				},
				new CustomSay()
				{
					who = "void",
					Text = "You were the only crew member to spend so much time looking at nonessential computer screens.",
					flipped = true
				},
				new CustomSay()
				{
					who = eddie,
					Text = "I thought I was going crazy for at least a few months.",
					loopTag = ModEntry.Instance.AnnoyedAnim
				},
				new CustomSay()
				{
					who = "void",
					Text = "I didn't yet comprehend the difference between our timescales.",
					flipped = true
				}
			]
		};
		DB.story.all[$"RunWinWho_{eddie}_3"] = new()
		{
			type = NodeType.@event,
			bg = "BGRunWin",
      		allPresent = [eddie],
			requiredScenes = [
				$"RunWinWho_{eddie}_2"
			],
			lookup = [ $"runWin_{eddie}" ],
			once = true,
			lines = [
				new Wait {
					secs = 3
				},
				new CustomSay()
				{
					who = "void",
					Text = "I should thank you.",
					flipped = true
				},
				new CustomSay()
				{
					who = eddie,
					Text = "Oh, are you sure?"
				},
				new CustomSay()
				{
					who = eddie,
					Text = "Why?",
				},
				new CustomSay()
				{
					who = "void",
					Text = "You will see.",
					flipped = true
				},
				new CustomSay()
				{
					who = "void",
					Text = "But in case this is the last time we meet: thank you.",
					flipped = true
				},
				new CustomSay()
				{
					who = eddie,
					Text = "Wait, \"the last time\"?",
					loopTag = ModEntry.Instance.WorriedAnim
				},
				new CustomSay()
				{
					who = "void",
					Text = "This memory will repair your timestream.",
					flipped = true
				},
				new CustomSay()
				{
					who = eddie,
					Text = "Oh, good."
				},
				new CustomSay()
				{
					who = "void",
					Text = "However, escaping the time loop can still go wrong in ways I cannot foresee.",
					flipped = true
				},
				new CustomSay()
				{
					who = eddie,
					Text = "Oh, bad.",
					loopTag = ModEntry.Instance.SquintAnim
				},
				new CustomSay()
				{
					who = "void",
					Text = "I will do my best to help.",
					flipped = true
				}
			]
		};
	}
}