using System.Reflection;
using Nanoray.PluginManager;
using Nickel;
using TheJazMaster.Eddie.DialogueAdditions;

namespace TheJazMaster.Eddie.Artifacts;

public class SunLamp : Artifact, ArtifactInterfaceManager.IOnMoveArtifact, IRegisterableArtifact
{	
	static Spr OnSprite;
	static Spr OffSprite;
	public bool turnedOn = true;

    public static void Register(Deck deck, IModHelper helper, IPluginPackage<IModManifest> package)
    {
		OnSprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile($"Sprites/artifact_icons/sun_lamp.png")).Sprite;
		OffSprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile($"Sprites/artifact_icons/sun_lamp_off.png")).Sprite;

        IRegisterableArtifact.Register(
			MethodBase.GetCurrentMethod()!.DeclaringType!,
			ModEntry.Instance.EddieDeck,
			[ArtifactPool.Common],
			OnSprite
		);
    }

	public override Spr GetSprite() => turnedOn ? OnSprite : OffSprite;

	public void OnMove(State s, Combat c, AMove move)
	{
		if (move.targetPlayer && move.fromEvade)
		{
			turnedOn = false;
		}
	}

	public override void OnTurnEnd(State s, Combat c)
	{
		if (turnedOn) {
			c.QueueImmediate(new AStatus
			{
				status = Status.tempShield,
				statusAmount = 1,
				targetPlayer = true,
				artifactPulse = Key(),
				dialogueSelector = $".{Key()}Trigger"
			});
		} else
			turnedOn = true;

	}

	public override void OnCombatEnd(State state)
	{
		turnedOn = true;
	}

	public void InjectDialogue()
	{
		var eddie = ModEntry.Instance.EddieDeck.Key();

		DB.story.all[$"Artifact{Key()}_0"] = new()
		{
			type = NodeType.combat,
			oncePerRun = true,
			oncePerCombatTags = [$"{Key()}Tag"],
			allPresent = [eddie],
			hasArtifacts = [Key()],
			lookup = [$"{Key()}Trigger"],
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Ahh, I missed this.",
					loopTag = ModEntry.Instance.RestingAnim
				}
			]
		};
		DB.story.all[$"Artifact{Key()}_1"] = new()
		{
			type = NodeType.combat,
			oncePerRun = true,
			oncePerCombatTags = [$"{Key()}DuoTag"],
			allPresent = [eddie, Deck.dizzy.Key()],
			hasArtifacts = [Key()],
			lookup = [$"{Key()}Trigger"],
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "I hope you don't mind if I turn this on..",
					loopTag = ModEntry.Instance.RestingAnim
				},
				new CustomSay()
				{
					who = Deck.dizzy.Key(),
					Text = "Not at all, go ahead!",
					loopTag = "explains"
				}
			]
		};
		DB.story.all[$"Artifact{Key()}_2"] = new()
		{
			type = NodeType.combat,
			oncePerRun = true,
			oncePerCombatTags = [$"{Key()}DuoTag"],
			allPresent = [eddie, Deck.eunice.Key()],
			hasArtifacts = [Key()],
			lookup = [$"{Key()}Trigger"],
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Finally, some peace and quiet.",
					loopTag = ModEntry.Instance.RestingAnim
				},
				new CustomSay()
				{
					who = Deck.eunice.Key(),
					Text = "Quit hogging that thing!",
					loopTag = "mad"
				}
			]
		};
		DB.story.all[$"Artifact{Key()}_3"] = new()
		{
			type = NodeType.combat,
			oncePerRun = true,
			oncePerCombatTags = [$"{Key()}DuoTag"],
			allPresent = [eddie, Deck.peri.Key()],
			hasArtifacts = [Key()],
			lookup = [$"{Key()}Trigger"],
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "This lamp really brightens up the room!",
					loopTag = ModEntry.Instance.RestingAnim
				},
				new CustomSay()
				{
					who = Deck.peri.Key(),
					Text = "Can someone turn off that lamp?",
					loopTag = "mad"
				}
			]
		};
		DB.story.all[$"Artifact{Key()}_4"] = new()
		{
			type = NodeType.combat,
			oncePerRun = true,
			oncePerCombatTags = [$"{Key()}DuoTag"],
			allPresent = [eddie, Deck.goat.Key()],
			hasArtifacts = [Key()],
			lookup = [$"{Key()}Trigger"],
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "I read about these things. They boost productivity by, like, a lot.",
					loopTag = ModEntry.Instance.RestingAnim
				},
				new CustomSay()
				{
					who = Deck.goat.Key(),
					Text = "Now I want one...",
					loopTag = "neutral"
				}
			]
		};
		DB.story.all[$"Artifact{Key()}_5"] = new()
		{
			type = NodeType.combat,
			oncePerRun = true,
			oncePerCombatTags = [$"{Key()}Tag"],
			allPresent = [eddie],
			hasArtifacts = [Key()],
			lookup = [$"{Key()}Trigger"],
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "I do some of my best thinking while asleep.",
					loopTag = ModEntry.Instance.RestingAnim
				}
			]
		};
		DB.story.all[$"Artifact{Key()}_6"] = new()
		{
			type = NodeType.combat,
			oncePerRun = true,
			oncePerCombatTags = [$"{Key()}DuoTag"],
			allPresent = [Deck.hacker.Key()],
			hasArtifacts = [Key()],
			lookup = [$"{Key()}Trigger"],
			lines = [
				new CustomSay()
				{
					who = Deck.hacker.Key(),
					Text = "Ouch, my eyes! I'm sensitive to sunlight.",
					loopTag = "squint"
				}
			]
		};
		DB.story.all[$"Artifact{Key()}_7"] = new()
		{
			type = NodeType.combat,
			oncePerRun = true,
			oncePerCombatTags = [$"{Key()}Tag"],
			allPresent = [eddie],
			hasArtifacts = [Key()],
			lookup = [$"{Key()}Trigger"],
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "It's important to keep your workplace comfortable.",
					loopTag = ModEntry.Instance.RestingAnim
				}
			]
		};
		DB.story.all[$"Artifact{Key()}_8"] = new()
		{
			type = NodeType.combat,
			oncePerRun = true,
			oncePerCombatTags = [$"{Key()}DuoTag"],
			allPresent = [Deck.shard.Key()],
			hasArtifacts = [Key()],
			lookup = [$"{Key()}Trigger"],
			lines = [
                new CustomSay()
				{
					who = Deck.shard.Key(),
					Text = "Wow! It's like a mini-sun!",
					loopTag = "relaxed"
				}
			]
		};

		if (StoryVarsAdditions.SogginsName != null)
			DB.story.all[$"Artifact{Key()}_9"] = new()
			{
				type = NodeType.combat,
				oncePerRun = true,
				oncePerCombatTags = [$"{Key()}DuoTag"],
				allPresent = [StoryVarsAdditions.SogginsName],
				hasArtifacts = [Key()],
				lookup = [$"{Key()}Trigger"],
				lines = [
                    new CustomSay()
					{
						who = StoryVarsAdditions.SogginsName,
						Text = "This light is bad for my skin."
					}
				]
			};
	}
}