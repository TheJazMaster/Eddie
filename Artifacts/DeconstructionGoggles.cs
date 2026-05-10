using System.Reflection;
using Nanoray.PluginManager;
using Nickel;
using TheJazMaster.Eddie.Cards;
using TheJazMaster.Eddie.DialogueAdditions;

namespace TheJazMaster.Eddie.Artifacts;

public class DeconstructionGoggles : Artifact, IRegisterableArtifact
{
    public static void Register(Deck deck, IModHelper helper, IPluginPackage<IModManifest> package)
    {
        IRegisterableArtifact.Register(
			MethodBase.GetCurrentMethod()!.DeclaringType!,
			ModEntry.Instance.EddieDeck,
			[ArtifactPool.Boss],
			helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile($"Sprites/artifact_icons/deconstruction_goggles.png")).Sprite,
			true
		);
    }

	public override void OnCombatStart(State state, Combat combat)
	{
		combat.Queue(new AAddCard {
			card = new ReverseEngineer(),
			destination = CardDestination.Hand,
			amount = 2,
			artifactPulse = Key()
		});
	}

	public override List<Tooltip>? GetExtraTooltips() => [
		new TTCard {
			card = new ReverseEngineer()
		},
		new TTGlossary("cardtrait.retain")
	];

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
			maxTurnsThisCombat = 1,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Let's tear some things apart!",
					loopTag = ModEntry.Instance.NeutralAnim
				},
				new SaySwitch()
				{
					lines = [
                        new CustomSay()
						{
							who = Deck.goat.Key(),
							Text = "Not my drones, right?",
							loopTag = "panic"
						},
						new CustomSay()
						{
							who = Deck.peri.Key(),
							Text = "No!",
							loopTag = "squint"
						},
						new CustomSay()
						{
							who = Deck.dizzy.Key(),
							Text = "Ooooh.",
							loopTag = "neutral"
						},
					]
				},
			]
		};

		DB.story.all[$"Artifact{Key()}_1"] = new()
		{
			type = NodeType.combat,
			oncePerRun = true,
			oncePerCombatTags = [$"{Key()}Tag"],
			allPresent = [eddie, "comp"],
			hasArtifacts = [Key()],
			maxTurnsThisCombat = 1,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "CAT! Deconstruction mode!",
					loopTag = ModEntry.Instance.ExcitedAnim
				},
				new CustomSay()
				{
					who = "comp",
					Text = "What are you talking about?",
					loopTag = "grumpy"
				},
			]
		};

		if (StoryVarsAdditions.SogginsName != null)
			DB.story.all[$"Artifact{Key()}_2"] = new()
			{
				type = NodeType.combat,
				oncePerRun = true,
				oncePerCombatTags = [$"{Key()}Tag"],
				allPresent = [eddie, StoryVarsAdditions.SogginsName],
				hasArtifacts = [Key()],
				maxTurnsThisCombat = 1,
				lines = [
                    new CustomSay()
					{
						who = StoryVarsAdditions.SogginsName,
						Text = "I think my ideas are worth deconstructing."
					},
					new CustomSay()
					{
						who = eddie,
						Text = "Yeah — for good.",
						loopTag = ModEntry.Instance.ExplainsAnim
					},
				]
			};
	}
}