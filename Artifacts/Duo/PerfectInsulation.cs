using System.Reflection;
using Nanoray.PluginManager;
using Nickel;

namespace TheJazMaster.Eddie.Artifacts;

public class PerfectInsulation : Artifact, ArtifactInterfaceManager.IOvershieldArtifact, IRegisterableArtifact
{
	static Spr OnSprite;
	static Spr OffSprite;
	public bool active = true;

	public static void Register(Deck deck, IModHelper helper, IPluginPackage<IModManifest> package)
	{
		if (ModEntry.Instance.DuoArtifactsApi != null) {
			OnSprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile($"Sprites/artifact_icons/duos/perfect_insulation_on.png")).Sprite;
			OffSprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile($"Sprites/artifact_icons/duos/perfect_insulation_off.png")).Sprite;

			IRegisterableArtifact.Register(
				MethodBase.GetCurrentMethod()!.DeclaringType!,
				ModEntry.Instance.DuoArtifactsApi.DuoArtifactVanillaDeck,
				[ArtifactPool.Common],
				OnSprite
			);
			ModEntry.Instance.DuoArtifactsApi.RegisterDuoArtifact(MethodBase.GetCurrentMethod()!.DeclaringType!, [ModEntry.Instance.EddieDeck, Deck.dizzy]);
		}
	}

	public override Spr GetSprite() => active ? OnSprite : OffSprite;

	public void OnOvershield(State s, Combat c, int amount, bool targetPlayer) {
		if (targetPlayer && active && amount > 0) {
			c.QueueImmediate(new AEnergy {
				changeAmount = 2,
				artifactPulse = Key()
			});
			active = false;
		}
	}

	public override void OnCombatEnd(State s)
	{
		active = true;
	}

    public override List<Tooltip>? GetExtraTooltips() => [new TTGlossary("status.shieldAlt")];
}