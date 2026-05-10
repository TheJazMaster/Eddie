using System.Reflection;
using Nanoray.PluginManager;
using Nickel;
using TheJazMaster.Eddie.DialogueAdditions;
using TwosCompany;

namespace TheJazMaster.Eddie.Artifacts;

public class SolarPanels : Artifact, IRegisterableArtifact, ArtifactInterfaceManager.IOnMoveArtifact
{
	static Spr UsedSprite;
	static Spr UnusedSprite;
	public bool used = false;

    public static void Register(Deck deck, IModHelper helper, IPluginPackage<IModManifest> package)
    {
		UsedSprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile($"Sprites/artifact_icons/solar_panels_off.png")).Sprite;
		UnusedSprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile($"Sprites/artifact_icons/solar_panels.png")).Sprite;

        IRegisterableArtifact.Register(
			MethodBase.GetCurrentMethod()!.DeclaringType!,
			ModEntry.Instance.EddieDeck,
			[ArtifactPool.Boss],
			UnusedSprite,
			true
		);
    }
	
	public override Spr GetSprite() => used ? UsedSprite : UnusedSprite;

	public void OnMove(State s, Combat c, AMove move)
	{
		if (move.targetPlayer && move.fromEvade && !used) {
			used = true;
			c.QueueImmediate(new AStatus {
				status = Status.energyLessNextTurn,
				statusAmount = 1,
				targetPlayer = true,
				artifactPulse = Key(),
				dialogueSelector = $".{Key()}RuinedTrigger"
			});
		}
	}

	public override void OnTurnStart(State s, Combat c)
	{
		used = false;
	}

	public override void OnCombatEnd(State state)
	{
		used = false;
	}

    public override void OnReceiveArtifact(State state)
    {
        state.ship.baseEnergy += 1;
    }

    public override void OnRemoveArtifact(State state)
    {
        state.ship.baseEnergy -= 1;
    }

	public void InjectDialogue()
	{
		var eddie = ModEntry.Instance.EddieDeck.Key();

		DB.story.all[$"Artifact{Key()}_0"] = new()
		{
			type = NodeType.combat,
			oncePerRun = true,
			oncePerCombatTags = [$"{Key()}Tag"],
			allPresent = [eddie, Deck.riggs.Key()],
			hasArtifacts = [Key()],
			maxTurnsThisCombat = 1,
			lines = [
                new CustomSay()
				{
					who = eddie,
					Text = "Riggs, I especially don't want any of your signature \"maneuvers\" right now.",
					loopTag = ModEntry.Instance.SquintAnim
				},
				new CustomSay()
				{
					who = Deck.riggs.Key(),
					Text = "Hey, I can be gentle too!",
					loopTag = "neutral"
				},
			]
		};
		
		DB.story.all[$"Artifact{Key()}_1"] = new()
		{
			type = NodeType.combat,
			oncePerRun = true,
			oncePerCombatTags = [$"{Key()}Tag"],
			allPresent = [eddie],
			hasArtifacts = [Key()],
			maxTurnsThisCombat = 1,
			lines = [
				new SaySwitch()
				{
					lines = [
                        new CustomSay()
						{
							who = eddie,
							Text = "Let's keep those panels angled properly or they might break.",
							loopTag = ModEntry.Instance.SquintAnim
						},
						new CustomSay()
						{
							who = eddie,
							Text = "No sudden movements! Those solar panels need to stay aligned.",
							loopTag = ModEntry.Instance.SquintAnim
						},
						new CustomSay()
						{
							who = eddie,
							Text = "These solar panels are pretty finnicky, be careful.",
							loopTag = ModEntry.Instance.WorriedAnim
						},
					]
				},
				new SaySwitch()
				{
					lines = [
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
							Text = "I won't ruin the sun plates, Ed, promise!",
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
						},
					]
				}.MaybeAdd(StoryVarsAdditions.SogginsName != null, new CustomSay() {
					who = StoryVarsAdditions.SogginsName!,
					Text = "I'll be as calm as a clam!"
				})
			]
		};
		ModEntry.Instance.Helper.ModRegistry.AwaitApiOrNull<ITwosAPI>("Mezz.TwosCompany", (api) => {
			if (api == null) return;

			DB.story.all[$"Artifact{Key()}_1"].lines.OfType<SaySwitch>().Last().lines.AddRange([
				new CustomSay {
					who = api.IsabelleDeck.GlobalName,
					Text = "I'll be as graceful as a butterfly.",
					loopTag = "snide"
				}
			]);
		});

		DB.story.all[$"Artifact{Key()}Ruined_0"] = new()
		{
			type = NodeType.combat,
			oncePerRun = true,
			lookup = [$"{Key()}RuinedTrigger"],
			oncePerCombatTags = [$"{Key()}RuinedTag"],
			allPresent = [eddie],
			hasArtifacts = [Key()],
			lines = [
				new SaySwitch()
				{
					lines = [
                        new CustomSay()
						{
							who = eddie,
							Text = "Oh come on...",
							loopTag = ModEntry.Instance.DisappointedAnim
						},
						new CustomSay()
						{
							who = eddie,
							Text = "Readjusting...",
							loopTag = ModEntry.Instance.SquintAnim
						},
						new CustomSay()
						{
							who = eddie,
							Text = "Ah, the panels.",
							loopTag = ModEntry.Instance.SquintAnim
						},
						new CustomSay()
						{
							who = eddie,
							Text = "No sudden movements, please!",
							loopTag = ModEntry.Instance.SquintAnim
						},
						new CustomSay()
						{
							who = eddie,
							Text = "Shoot. Let's try that again.",
							loopTag = ModEntry.Instance.DisappointedAnim
						},
						new CustomSay()
						{
							who = eddie,
							Text = "That's not going to be enough sunglight.",
							loopTag = ModEntry.Instance.SquintAnim
						}
					]
				},
				new SaySwitch()
				{
					lines = [
                        new CustomSay()
						{
							who = "comp",
							Text = "We have bigger fish to fry.",
							loopTag = "lean"
						},
						new CustomSay()
						{
							who = "comp",
							Text = "Man...",
							loopTag = "squint"
						},
						new CustomSay()
						{
							who = Deck.eunice.Key(),
							Text = "Yeah, you take care of that.",
							loopTag = "neutral"
						},
						new CustomSay()
						{
							who = Deck.eunice.Key(),
							Text = "Wow, I'm glad we're in good hands.",
							loopTag = "mad"
						},
						new CustomSay()
						{
							who = Deck.eunice.Key(),
							Text = "These panels were a piece of junk anyway.",
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
							Text = "Sorry, shields are important too.",
							loopTag = "neutral"
						},
						new CustomSay()
						{
							who = Deck.dizzy.Key(),
							Text = "We need more shields for next time.",
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
						new CustomSay()
						{
							who = Deck.shard.Key(),
							Text = "There's always next time!",
							loopTag = "neutral"
						},
					]
				}.MaybeAdd(StoryVarsAdditions.SogginsName != null, new CustomSay() {
					who = StoryVarsAdditions.SogginsName!,
					Text = "Wasn't me!"
				}).MaybeAdd(StoryVarsAdditions.SogginsName != null, new CustomSay() {
					who = StoryVarsAdditions.SogginsName!,
					Text = "You installed them wrong."
				}).MaybeAdd(StoryVarsAdditions.SogginsName != null, new CustomSay() {
					who = StoryVarsAdditions.SogginsName!,
					Text = "Huh? Is something wrong?"
				})
			]
		};
	}
}