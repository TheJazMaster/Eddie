using TheJazMaster.Eddie.Cards;

namespace TheJazMaster.Eddie.Artifacts;

[ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common }, extraGlossary = new string[] { "cardtrait.infinite" })]
public class VirtualTreadmill : Artifact, ICardDataAffectorArtifact
{
	public void AffectCardData(State s, Card card, ref CardData originalData) {
		if (card is CannonColorless || card is BasicShieldColorless || card is DodgeColorless || card is DroneshiftColorless) {
			originalData.infinite = true;
			ShortCircuit.SetShortCircuit(card, innateValue: true);
		}
	}

	public override void OnRemoveArtifact(State s) {
		foreach (Card card in s.deck) {
			if (card is CannonColorless || card is BasicShieldColorless || card is DodgeColorless || card is DroneshiftColorless) {
				ShortCircuit.SetShortCircuit(card, innateValue: false);
			}
		}
	}
}
