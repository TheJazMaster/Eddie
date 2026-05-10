using System.Reflection;
using Nickel;
using Nanoray.PluginManager;

namespace TheJazMaster.Eddie.Artifacts;

public class Spellboard : Artifact, IRegisterableArtifact
{
	private static ModEntry Instance => ModEntry.Instance;
	private static readonly Lazy<bool> isDuosLoaded = new(() => Instance.DuoArtifactsApi != null); 

	private static readonly List<Deck> dizzyBooksDuo = [Deck.dizzy, Deck.shard];

	static int recursionLevel = 0;

	public static void Register(Deck deck, IModHelper helper, IPluginPackage<IModManifest> package)
	{
		if (ModEntry.Instance.DuoArtifactsApi != null) {
			IRegisterableArtifact.Register(
				MethodBase.GetCurrentMethod()!.DeclaringType!,
				ModEntry.Instance.DuoArtifactsApi.DuoArtifactVanillaDeck,
				[ArtifactPool.Common],
				helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile($"Sprites/artifact_icons/duos/spellboard.png")).Sprite
			);
			ModEntry.Instance.DuoArtifactsApi.RegisterDuoArtifact(MethodBase.GetCurrentMethod()!.DeclaringType!, [ModEntry.Instance.EddieDeck, Deck.shard]);

            helper.Content.Cards.OnGetDynamicInnateCardTraitOverrides += (_, data) => {
                if (!data.State.EnumerateAllArtifacts().OfType<Spellboard>().Any()) return;

				Card card = data.Card; State s = data.State;

                recursionLevel++;
				if (recursionLevel < 2)
					foreach (CardAction action in card.GetActions(s, (s.route as Combat) ?? DB.fakeCombat)) {
						if (!action.disabled && action.shardcost.HasValue) {

							var resources = s.ship.Get(Status.shard);
							if (isDuosLoaded.Value)
								resources += s.EnumerateAllArtifacts().Any(a => Instance.DuoArtifactsApi!.GetDuoArtifactOwnership(a)?.SetEquals(dizzyBooksDuo) ?? false) ? s.ship.Get(Status.shield) : 0;

							if (action.shardcost.Value <= resources) {
								data.SetOverride(helper.Content.Cards.InfiniteCardTrait, true);
								break;
							}
						}
					}
				recursionLevel = 0;
            };
		}
	}

	public override List<Tooltip>? GetExtraTooltips() => [
		.. StatusMeta.GetTooltips(Status.shard, 3),
		new TTGlossary("cardtrait.infinite")
	];
}
