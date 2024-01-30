using CobaltCoreModding.Definitions.ExternalItems;

namespace TheJazMaster.Eddie;

public interface IEddieApi
{
	ExternalDeck EddieDeck { get; }

	ExternalGlossary ShortCircuitGlossary { get; }
	ExternalGlossary CheapGlossary { get; }

	void SetFree(Card card, bool? overrideValue = null, bool? oncePerTurnOnlyValue = null, bool? permanentValue = null);

	bool UsedFreeOncePerTurn(Card card);

	bool IsFree(Card card, bool withOncePerTurnLimit = true);

	bool IsFreeOncePerTurn(Card card);

	bool IsFreePermanent(Card card);
}
