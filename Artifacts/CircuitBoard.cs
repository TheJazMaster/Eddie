using System.Reflection;
using Nanoray.PluginManager;
using Nickel;

namespace TheJazMaster.Eddie.Artifacts;

public class CircuitBoard : Artifact, IRegisterableArtifact
{
    public static void Register(Deck deck, IModHelper helper, IPluginPackage<IModManifest> package)
    {
        IRegisterableArtifact.Register(
			MethodBase.GetCurrentMethod()!.DeclaringType!,
			ModEntry.Instance.EddieDeck,
			[ArtifactPool.Common],
			helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile($"Sprites/artifact_icons/circuit_board.png")).Sprite
		);
    }

	public override void OnTurnStart(State state, Combat combat)
	{
		if (combat.turn == 1)
		{
			combat.QueueImmediate(new AStatus
			{
				status = StatusManager.ClosedCircuitStatus,
				statusAmount = 2,
				targetPlayer = true,
				artifactPulse = Key()
			});
		}
	}

	public override List<Tooltip>? GetExtraTooltips() => StatusMeta.GetTooltips(StatusManager.ClosedCircuitStatus, 2);

	public void InjectDialogue()
	{
		var eddie = ModEntry.Instance.EddieCharacter.CharacterType;

		DB.story.all[$"Artifact{Key()}_0"] = new()
		{
			type = NodeType.combat,
			turnStart = true,
			maxTurnsThisCombat = 1,
			oncePerCombatTags = [$"{Key()}Tag"],
			oncePerRun = true,
			allPresent = [eddie],
			hasArtifacts = [Key()],
			lines = [
                new CustomSay()
				{
					who = eddie,
					Text = "Let's make the most out of what we have.",
					loopTag = ModEntry.Instance.NeutralAnim
				}
			]
		};
		DB.story.all[$"Artifact{Key()}_1"] = new()
		{
			type = NodeType.combat,
			turnStart = true,
			maxTurnsThisCombat = 1,
			oncePerCombatTags = [$"{Key()}Tag"],
			oncePerRun = true,
			allPresent = [eddie],
			hasArtifacts = [Key()],
			lines = [
                new CustomSay()
				{
					who = eddie,
					Text = "We should think this through a little",
					loopTag = ModEntry.Instance.SquintAnim
				}
			]
		};
	}
}