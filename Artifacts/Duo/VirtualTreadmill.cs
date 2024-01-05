using Eddie.Cards;

namespace Eddie.Artifacts;

[ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common })]
public class VirtualTreadmill : Artifact, CardDataAffectorArtifact
{
	public void AffectCardData(State s, Card card, ref CardData originalData) {
		if (card is CannonColorless || card is BasicShieldColorless || card is DodgeColorless || card is DroneshiftColorless) {
			originalData.infinite = true;
			ShortCircuit.short_circuit.AddOrUpdate(card, true);
		}
	}

	public override void OnRemoveArtifact(State s) {
		foreach (Card card in s.deck) {
			if (card is CannonColorless || card is BasicShieldColorless || card is DodgeColorless || card is DroneshiftColorless) {
				ShortCircuit.short_circuit.Remove(card);
			}
		}
	}
}
