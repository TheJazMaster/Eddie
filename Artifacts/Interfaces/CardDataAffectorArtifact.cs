namespace TheJazMaster.Eddie.Artifacts;

public interface ICardDataAffectorArtifact
{
	void AffectCardData(State s, Card card, ref CardData data);
}