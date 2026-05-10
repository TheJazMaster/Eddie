using System.Reflection;
using Nanoray.PluginManager;
using Nickel;

namespace TheJazMaster.Eddie.Artifacts;

public class ElectromagneticCoil : Artifact, IRegisterableArtifact
{
    public static void Register(Deck deck, IModHelper helper, IPluginPackage<IModManifest> package)
    {
        IRegisterableArtifact.Register(
			MethodBase.GetCurrentMethod()!.DeclaringType!,
			ModEntry.Instance.EddieDeck,
			[ArtifactPool.Common],
			helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile($"Sprites/artifact_icons/electromagnetic_coil.png")).Sprite
		);
    }

	public override void OnTurnEnd(State state, Combat combat)
	{
		if (combat.energy > 0)
		{
			combat.QueueImmediate(new AStatus
			{
				status = Status.evade,
				statusAmount = 1,
				targetPlayer = true,
				artifactPulse = Key(),
				dialogueSelector = $".{Key()}Trigger"
			});
		}
	}

	public override List<Tooltip>? GetExtraTooltips() => StatusMeta.GetTooltips(Status.evade, 1);

	public void InjectDialogue()
	{
		var eddie = ModEntry.Instance.EddieDeck.Key();

		DB.story.all[$"Artifact{Key()}_0"] = new()
		{
			type = NodeType.combat,
			lookup = [$"{Key()}Trigger"],
			oncePerCombatTags = [$"{Key()}Tag"],
			oncePerRun = true,
			allPresent = [eddie],
			hasArtifacts = [Key()],
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Now that's what I call efficiency.",
					loopTag = ModEntry.Instance.NeutralAnim
				}
			]
		};
		DB.story.all[$"Artifact{Key()}_1"] = new()
		{
			type = NodeType.combat,
			lookup = [$"{Key()}Trigger"],
			oncePerCombatTags = [$"{Key()}Tag"],
			oncePerRun = true,
			allPresent = [eddie],
			hasArtifacts = [Key()],
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Look at this thing! Pretty cool, right?",
					loopTag = ModEntry.Instance.NeutralAnim
				}
			]
		};
		DB.story.all[$"Artifact{Key()}_2"] = new()
		{
			type = NodeType.combat,
			lookup = [$"{Key()}Trigger"],
			oncePerCombatTags = [$"{Key()}Tag"],
			oncePerRun = true,
			allPresent = [eddie],
			hasArtifacts = [Key()],
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "We don't even have to do anything!",
					loopTag = ModEntry.Instance.ExplainsAnim
				}
			]
		};
	}
}