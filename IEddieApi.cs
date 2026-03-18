using CobaltCoreModding.Definitions.ExternalItems;
using Nickel;

namespace TheJazMaster.Eddie;

public interface IEddieApi
{
	Deck EddieDeck { get; }

	ExternalGlossary ShortCircuitGlossary { get; }
	ExternalGlossary CheapGlossary { get; }
    Status CircuitStatus { get; }
    Status ClosedCircuitStatus { get; }
    Status LoseEnergyEveryTurnStatus { get; }
    Status GainEnergyEveryTurnStatus { get; }
    Status HealNextTurnStatus { get; }

	ICardTraitEntry CheapTrait { get; }
	ICardTraitEntry ShortCircuitTrait { get; }

	void SetFree(Card card, bool? overrideValue = null, bool? oncePerTurnOnlyValue = null, int? permanentValue = null);

	bool UsedFreeOncePerTurn(Card card);

	bool IsFree(Card card, bool withOncePerTurnLimit = true);

	bool IsFreeOncePerTurn(Card card);

	int CostsLessPermanent(Card card);
}
