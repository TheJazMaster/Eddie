
namespace TheJazMaster.Eddie.Artifacts;

public class EmergencyThrusters : Artifact, IOnMoveArtifact
{
	public void InjectDialogue()
	{
		var eddie = Manifest.EddieDeck.GlobalName;

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