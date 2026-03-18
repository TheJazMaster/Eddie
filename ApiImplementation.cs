using CobaltCoreModding.Definitions.ExternalItems;
using Nickel;

namespace TheJazMaster.Eddie;

public sealed class ApiImplementation : IEddieApi
{
	private static Manifest Instance => Manifest.Instance;


	public Deck EddieDeck
		=> (Deck)Manifest.EddieDeck.Id!;

	public ExternalGlossary ShortCircuitGlossary
		=> Manifest.ShortCircuitGlossary!;
	public ExternalGlossary CheapGlossary
		=> Manifest.CheapGlossary!;

    public Status CircuitStatus => (Status)Manifest.CircuitStatus.Id!;
    public Status ClosedCircuitStatus => (Status)Manifest.ClosedCircuitStatus.Id!;
    public Status LoseEnergyEveryTurnStatus => (Status)Manifest.LoseEnergyEveryTurnStatus.Id!;
    public Status GainEnergyEveryTurnStatus => (Status)Manifest.GainEnergyEveryTurnStatus.Id!;
    public Status HealNextTurnStatus => (Status)Manifest.HealNextTurnStatus.Id!;

	public ICardTraitEntry CheapTrait => Manifest.CheapTrait;
	public ICardTraitEntry ShortCircuitTrait => Manifest.ShortCircuitTrait;

	public void SetFree(Card card, bool? overrideValue = null, bool? oncePerTurnOnlyValue = null, int? permanentValue = null) {
		Cheap.SetFree(card, overrideValue, oncePerTurnOnlyValue, permanentValue);
	}

	public bool UsedFreeOncePerTurn(Card card) {
		return Cheap.UsedFreeOncePerTurn(card);
	}

	public bool IsFree(Card card, bool withOncePerTurnLimit = true) {
		return Cheap.IsFree(card, withOncePerTurnLimit);
	}

	public bool IsFreeOncePerTurn(Card card) {
		return Cheap.IsFreeOncePerTurn(card);
	}

	public int CostsLessPermanent(Card card) {
		return Cheap.CostsLessPermanent(card);
	}
}
