using Eddie.Cards;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace Eddie.Artifacts;

[ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common })]
public class Spellboard : Artifact, CardDataAffectorArtifact
{
	private static Manifest Instance => Manifest.Instance;
	private static readonly Lazy<bool> isDuosLoaded = new Lazy<bool>(() => Instance.DuoArtifactsApi != null); 

	private static readonly List<Deck> dizzyBooksDuo = new List<Deck> {Enum.Parse<Deck>("dizzy"), Enum.Parse<Deck>("shard")};

	int recursionLevel = 0;

	// bool hasBookDizzyArtifact = false;

	// public override void OnCombatStart(State s, Combat c) {
	// 	hasBookDizzyArtifact = s.EnumerateAllArtifacts().Any(a => a.GetType().Name == "BooksDizzyArtifact");
	// }
	
	public void AffectCardData(State s, Card card, ref CardData originalData) {
		recursionLevel++;
		if (recursionLevel < 2)
			foreach (CardAction action in card.GetActions(s, (s.route as Combat) ?? DB.fakeCombat)) {
				if (!action.disabled && action.shardcost.HasValue) {

					var resources = s.ship.Get(Status.shard);
					if (isDuosLoaded.Value)
						resources += (s.EnumerateAllArtifacts().Any(a => Instance.DuoArtifactsApi!.GetDuoArtifactOwnership(a)?.SetEquals(dizzyBooksDuo) ?? false) ? s.ship.Get(Status.shield) : 0);

					if (action.shardcost.Value <= resources) {
						originalData.infinite = true;
						break;
					}
				}
			}
		recursionLevel = 0;
	}
}
