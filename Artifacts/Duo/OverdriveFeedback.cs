
using System.ComponentModel;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using Nickel;
using Shockah.Kokoro;

namespace TheJazMaster.Eddie.Artifacts;

public class OverdriveFeedback : Artifact, IRegisterableArtifact, IKokoroApi.IV2.IStatusLogicApi.IHook, IKokoroApi.IV2.IHookPriority
{
	public bool active = false;

    public static void Register(Deck deck, IModHelper helper, IPluginPackage<IModManifest> package)
    {
		if (ModEntry.Instance.DuoArtifactsApi != null) {
			IRegisterableArtifact.Register(
				MethodBase.GetCurrentMethod()!.DeclaringType!,
				ModEntry.Instance.DuoArtifactsApi.DuoArtifactVanillaDeck,
				[ArtifactPool.Common],
				helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile($"Sprites/artifact_icons/duos/overdrive_feedback.png")).Sprite
			);
			ModEntry.Instance.DuoArtifactsApi.RegisterDuoArtifact(MethodBase.GetCurrentMethod()!.DeclaringType!, [ModEntry.Instance.EddieDeck, Deck.peri]);
		}
    }

	public bool ShouldKeepOverdrive(State s, Combat c) {
		return c.energy > 0;
	}

    public override void OnTurnEnd(State state, Combat combat)
    {
		active = combat.energy > 0;
    }

	public double HookPriority => 20;

	public IReadOnlySet<Status> GetStatusesToCallTurnTriggerHooksFor(IKokoroApi.IV2.IStatusLogicApi.IHook.IGetStatusesToCallTurnTriggerHooksForArgs args)
		=> new HashSet<Status> { Status.overdrive };

    public bool HandleStatusTurnAutoStep(IKokoroApi.IV2.IStatusLogicApi.IHook.IHandleStatusTurnAutoStepArgs args) => active;

    public override List<Tooltip>? GetExtraTooltips() => [
		new TTGlossary("status.overdriveAlt", null)
	];
}