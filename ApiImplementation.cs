using CobaltCoreModding.Definitions.ExternalItems;
using Nickel;

namespace TheJazMaster.Eddie;

public sealed class ApiImplementation : IEddieApi
{
	private static ModEntry Instance => ModEntry.Instance;


	public Deck EddieDeck
		=> Instance.EddieDeck;

	// public ExternalGlossary ShortCircuitGlossary
	// 	=> Manifest.ShortCircuitGlossary!;
	// public ExternalGlossary CheapGlossary
	// 	=> Manifest.CheapGlossary!;

    public Status CircuitStatus => StatusManager.CircuitStatus;
    public Status ClosedCircuitStatus => StatusManager.ClosedCircuitStatus;
    public Status GainEnergyEveryTurnStatus => StatusManager.MoreEnergyStatus;
    public Status HealNextTurnStatus => ModEntry.Instance.KokoroApi.TempHull.RegainHullLaterStatus;

	public ICardTraitEntry CheapTrait => CheapManager.CheapTrait;
	public ICardTraitEntry ShortCircuitTrait => ShortCircuitManager.ShortCircuitTrait;

	public void SetFree(Card card, bool? overrideValue = null, bool? oncePerTurnOnlyValue = null, int? permanentValue = null) {
		CheapManager.SetFree(card, overrideValue, oncePerTurnOnlyValue, permanentValue);
	}

	public bool UsedFreeOncePerTurn(Card card) {
		return CheapManager.UsedFreeOncePerTurn(card);
	}

	public bool IsFree(Card card, bool withOncePerTurnLimit = true) {
		return CheapManager.IsFree(card, withOncePerTurnLimit);
	}

	public bool IsFreeOncePerTurn(Card card) {
		return CheapManager.IsFreeOncePerTurn(card);
	}

	public int CostsLessPermanent(Card card) {
		return CheapManager.CostsLessPermanent(card);
	}
}
