using System.Reflection;
using Nanoray.PluginManager;
using Nickel;
using TheJazMaster.Eddie.Actions;
using TheJazMaster.Eddie.DialogueAdditions;

namespace TheJazMaster.Eddie.Artifacts;

public class FrazzledWires : Artifact, IRegisterableArtifact
{
    public static void Register(Deck deck, IModHelper helper, IPluginPackage<IModManifest> package)
    {
        IRegisterableArtifact.Register(
			MethodBase.GetCurrentMethod()!.DeclaringType!,
			ModEntry.Instance.EddieDeck,
			[ArtifactPool.Common],
			helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile($"Sprites/artifact_icons/frazzled_wires.png")).Sprite
		);
    }
	
	public override void OnReceiveArtifact(State state)
	{
		state.GetCurrentQueue().QueueImmediate(new ACardSelect
		{
			filterMinCost = 1,
			filterMaxCost = 1,
			browseAction = new CardSelectAddShortCircuitAndMakeCheaperForever(),
			browseSource = CardBrowse.Source.Deck
		});
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
			maxTurnsThisCombat = 1,
			turnStart = true,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "I tried fixing some broken wiring in the back. Let's see if it explodes.",
					loopTag = ModEntry.Instance.NeutralAnim
				}
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
			turnStart = true,
			lines = [
                new CustomSay()
				{
					who = eddie,
					Text = "There can be no greatness without a little risk.",
					loopTag = ModEntry.Instance.ExplainsAnim
				},
				new SaySwitch()
				{
					lines = [
						new CustomSay()
						{
							who = Deck.dizzy.Key(),
							Text = "And we're living proof!",
							loopTag = "neutral"
						},
						new CustomSay()
						{
							who = Deck.riggs.Key(),
							Text = "Yeah! We're trailblazers!",
							loopTag = "neutral"
						},
						new CustomSay()
						{
							who = Deck.peri.Key(),
							Text = "A looming fire hazard does make me more vigilant.",
							loopTag = "squint"
						},
						new CustomSay()
						{
							who = Deck.goat.Key(),
							Text = "This wiring doesn't seem all that great.",
							loopTag = "squint"
						},
						new CustomSay()
						{
							who = Deck.eunice.Key(),
							Text = "I know, I'm pretty great myself.",
							loopTag = "neutral"
						},
						new CustomSay()
						{
							who = Deck.hacker.Key(),
							Text = "Survivor bias, or something.",
							loopTag = "neutral"
						}
					]
				}.MaybeAdd(StoryVarsAdditions.SogginsName != null, new CustomSay() {
					who = StoryVarsAdditions.SogginsName!,
					Text = "Words to live by."
				})
			]
		};
	}

	public override List<Tooltip>? GetExtraTooltips() => 
		[.. ShortCircuitManager.ShortCircuitTrait.Configuration.Tooltips!.Invoke(DB.fakeState, null)];
}