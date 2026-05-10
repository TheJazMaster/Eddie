using System.Reflection;
using Nanoray.PluginManager;
using Nickel;
using TheJazMaster.TyAndSasha;

namespace TheJazMaster.Eddie.Artifacts;

public class FissionChamber : Artifact, XIncreaseManager.IXAffectorArtifact, IRegisterableArtifact, ITyAndSashaApi.IHook
{
    public static void Register(Deck deck, IModHelper helper, IPluginPackage<IModManifest> package)
    {
        IRegisterableArtifact.Register(
			MethodBase.GetCurrentMethod()!.DeclaringType!,
			ModEntry.Instance.EddieDeck,
			[ArtifactPool.Common],
			helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile($"Sprites/artifact_icons/fission_chamber.png")).Sprite
		);
    }

	public int AffectX(int value) {
		return value + 1;
	}

	public int AffectX(ITyAndSashaApi.IHook.IAffectXArgs args) => 1;

	public void InjectDialogue()
	{
		var eddie = ModEntry.Instance.EddieDeck.Key();

		DB.story.all[$"Artifact{Key()}_0"] = new()
		{
			type = NodeType.combat,
			oncePerRun = true,
			oncePerCombatTags = [ $"{Key()}Tag" ],
			allPresent = [ eddie, Deck.riggs.Key() ],
			hasArtifacts = [ Key() ],
			maxTurnsThisCombat = 1,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Hey Riggs, what if I were to put a nuclear reactor in your gun?",
					loopTag = ModEntry.Instance.NeutralAnim
				},
				new CustomSay()
				{
					who = Deck.riggs.Key(),
					Text = "I'm listening...",
					loopTag = "neutral"
				}
			]
		};
		DB.story.all[$"Artifact{Key()}_1"] = new()
		{
			type = NodeType.combat,
			oncePerRun = true,
			oncePerCombatTags = [ $"{Key()}Tag" ],
			allPresent = [ eddie, Deck.dizzy.Key() ],
			hasArtifacts = [ Key() ],
			maxTurnsThisCombat = 1,
			lines = [
				new CustomSay()
				{
					who = Deck.dizzy.Key(),
					Text = "Hey Eddie, I'm getting some concerning readings on our shields...",
					loopTag = "geiger"
				},
				new CustomSay()
				{
					who = eddie,
					Text = "Don't worry about it.",
					loopTag = ModEntry.Instance.NeutralAnim
				}
			]
		};
		DB.story.all[$"Artifact{Key()}_2"] = new()
		{
			type = NodeType.combat,
			oncePerRun = true,
			oncePerCombatTags = [ $"{Key()}Tag" ],
			allPresent = [ eddie, Deck.peri.Key() ],
			hasArtifacts = [ Key() ],
			maxTurnsThisCombat = 1,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Hey Peri, this should boost the inverter!", //TODO: change for 1.3
					loopTag = ModEntry.Instance.ExcitedAnim
				},
				new CustomSay()
				{
					who = Deck.peri.Key(),
					Text = "I'll keep that in mind.",
					loopTag = "neutral"
				}
			]
		};
		DB.story.all[$"Artifact{Key()}_3"] = new()
		{
			type = NodeType.combat,
			oncePerRun = true,
			oncePerCombatTags = [ $"{Key()}Tag" ],
			allPresent = [ eddie, Deck.hacker.Key() ],
			hasArtifacts = [ Key() ],
			maxTurnsThisCombat = 1,
			lines = [
				new CustomSay()
				{
					who = Deck.hacker.Key(),
					Text = "Why is my console hooked up to a nuclear reactor?",
					loopTag = "intense"
				},
				new CustomSay()
				{
					who = eddie,
					Text = "I'm trying something out.",
					loopTag = ModEntry.Instance.NeutralAnim
				}
			]
		};
		DB.story.all[$"Artifact{Key()}_4"] = new()
		{
			type = NodeType.combat,
			oncePerRun = true,
			oncePerCombatTags = [ $"{Key()}Tag" ],
			allPresent = [ eddie, Deck.eunice.Key() ],
			hasArtifacts = [ Key() ],
			maxTurnsThisCombat = 1,
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "X marks the spot, Drake.",
					loopTag = ModEntry.Instance.NeutralAnim
				},
				new CustomSay()
				{
					who = Deck.eunice.Key(),
					Text = "Shut up, nerd.",
					loopTag = "mad"
				}
			]
		};
	}
}