
using System.Reflection;
using Nanoray.PluginManager;
using Nickel;

namespace TheJazMaster.Eddie.Artifacts;

public class EmergencyThrusters : Artifact, ArtifactInterfaceManager.IOnMoveArtifact, IRegisterableArtifact
{
	public static void Register(Deck deck, IModHelper helper, IPluginPackage<IModManifest> package) {
		IRegisterableArtifact.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, Deck.colorless,
			[ArtifactPool.EventOnly],
			helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile($"Sprites/artifact_icons/emergency_thrusters.png")).Sprite,
			true
		);
	}

	public void InjectDialogue()
	{
		var eddie = ModEntry.Instance.EddieDeck.Key();

		DB.story.all[$"ArtifactChariot_{eddie}"] = new()
		{
			type = NodeType.combat,
			oncePerRunTags = [ Key() ],
			allPresent = [ eddie ],
			hasArtifacts = [ Key() ],
			lines = [
				new CustomSay()
				{
					who = eddie,
					Text = "Well, no need to pilot this thing."
				}
			]
		};

		DB.story.all[$"ArtifactChariot_dizzy"] = new()
		{
			type = NodeType.combat,
			oncePerRunTags = [ Key() ],
			allPresent = [ Deck.dizzy.Key() ],
			hasArtifacts = [ Key() ],
			lines = [
				new CustomSay()
				{
					who = Deck.dizzy.Key(),
					Text = "Moving this is gonna take a lot out of the shields.",
					loopTag = "serious"
				}
			]
		};

		DB.story.all[$"ArtifactChariot_riggs"] = new()
		{
			type = NodeType.combat,
			oncePerRunTags = [ Key() ],
			allPresent = [ Deck.riggs.Key() ],
			hasArtifacts = [ Key() ],
			lines = [
				new CustomSay()
				{
					who = Deck.riggs.Key(),
					Text = "Must... resist... urge to zoom..."
				}
			]
		};

		DB.story.all[$"ArtifactChariot_goat"] = new()
		{
			type = NodeType.combat,
			oncePerRunTags = [ Key() ],
			allPresent = [ Deck.goat.Key() ],
			hasArtifacts = [ Key() ],
			lines = [
				new CustomSay()
				{
					who = Deck.goat.Key(),
					Text = "I'm quite comfortable not moving, actually."
				}
			]
		};

		DB.story.all[$"ArtifactChariot_hacker"] = new()
		{
			type = NodeType.combat,
			oncePerRunTags = [ Key() ],
			allPresent = [ Deck.hacker.Key() ],
			hasArtifacts = [ Key() ],
			lines = [
				new CustomSay()
				{
					who = Deck.hacker.Key(),
					Text = "The autopilot should be stable enough, if I can get it going."
				}
			]
		};
	}
	
	public override void OnCombatStart(State state, Combat combat)
	{
		combat.Queue(new AStatus
		{
			status = Status.strafe,
			statusAmount = 1,
			targetPlayer = true,
			artifactPulse = Key()
		});
	}

	public void OnMove(State s, Combat c, AMove move)
	{
		if (move.fromEvade) {
			c.Queue(new AHurt {
				hurtAmount = 1,
				hurtShieldsFirst = true,
				targetPlayer = true,
				artifactPulse = Key()
			});
		}
	}

	public override List<Tooltip>? GetExtraTooltips()
	{
		return [
			new TTGlossary("status.strafe", 1),
			new TTGlossary("status.evade")
		];
	}
}