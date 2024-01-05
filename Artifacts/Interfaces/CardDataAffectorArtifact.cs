namespace Eddie.Artifacts;

public interface CardDataAffectorArtifact
{
	void AffectCardData(State s, Card card, ref CardData data);
}