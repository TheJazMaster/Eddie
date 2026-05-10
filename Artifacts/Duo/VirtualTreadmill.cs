using System.Reflection;
using Nanoray.PluginManager;
using Nickel;

namespace TheJazMaster.Eddie.Artifacts;

[ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common }, extraGlossary = ["cardtrait.infinite"])]
public class VirtualTreadmill : Artifact, IRegisterableArtifact
{


	public static void Register(Deck deck, IModHelper helper, IPluginPackage<IModManifest> package)
	{
		if (ModEntry.Instance.DuoArtifactsApi != null) {
			IRegisterableArtifact.Register(
				MethodBase.GetCurrentMethod()!.DeclaringType!,
				ModEntry.Instance.DuoArtifactsApi.DuoArtifactVanillaDeck,
				[ArtifactPool.Common],
				helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile($"Sprites/artifact_icons/duos/virtual_treadmill.png")).Sprite
			);
			ModEntry.Instance.DuoArtifactsApi.RegisterDuoArtifact(MethodBase.GetCurrentMethod()!.DeclaringType!, [ModEntry.Instance.EddieDeck, Deck.catartifact]);

            helper.Content.Cards.OnGetDynamicInnateCardTraitOverrides += (_, data) => {
                if (!data.State.EnumerateAllArtifacts().OfType<VirtualTreadmill>().Any()) return;

                var meta = data.Card.GetMeta();
                if (meta.deck == Deck.colorless && meta.dontOffer) {
                    data.SetOverride(helper.Content.Cards.InfiniteCardTrait, true);
                    data.SetOverride(ShortCircuitManager.ShortCircuitTrait, true);
                }
            };
		}
	}
    

    public override List<Tooltip>? GetExtraTooltips() => [new TTGlossary("cardtrait.infinite")];
}
