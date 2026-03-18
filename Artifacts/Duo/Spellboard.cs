using TheJazMaster.Eddie.Cards;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace TheJazMaster.Eddie.Artifacts;

[ArtifactMeta(pools = [ArtifactPool.Common])]
public class Spellboard : Artifact, ICardDataAffectorArtifact
{
	private static Manifest Instance => Manifest.Instance;
	private static readonly Lazy<bool> isDuosLoaded = new(() => Instance.DuoArtifactsApi != null); 

	private static readonly List<Deck> dizzyBooksDuo = [Deck.dizzy, Deck.shard];

	int recursionLevel = 0;
	
	public void AffectCardData(State s, Card card, ref CardData originalData) {
		recursionLevel++;
		if (recursionLevel < 2)
			foreach (CardAction action in card.GetActions(s, (s.route as Combat) ?? DB.fakeCombat)) {
				if (!action.disabled && action.shardcost.HasValue) {

					var resources = s.ship.Get(Status.shard);
					if (isDuosLoaded.Value)
						resources += s.EnumerateAllArtifacts().Any(a => Instance.DuoArtifactsApi!.GetDuoArtifactOwnership(a)?.SetEquals(dizzyBooksDuo) ?? false) ? s.ship.Get(Status.shield) : 0;

					if (action.shardcost.Value <= resources) {
						originalData.infinite = true;
						break;
					}
				}
			}
		recursionLevel = 0;
	}

	public override List<Tooltip>? GetExtraTooltips() => [
		.. StatusMeta.GetTooltips(Status.shard, 3),
		new TTGlossary("cardtrait.infinite")
	];
}
