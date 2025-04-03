using TheJazMaster.Eddie.Actions;

namespace TheJazMaster.Eddie.Cards;

[CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
public class Amplify : Card
{
	public override string Name() => "Amplify";

	public override CardData GetData(State state)
	{
		return new CardData
		{
			cost = upgrade == Upgrade.A ? 2 : 3,
			exhaust = upgrade != Upgrade.B,
			art = StableSpr.cards_Overclock
		};
	}

	public override List<CardAction> GetActions(State s, Combat c) => new()
	{
		new AStatus
		{
			targetPlayer = true,
			status = (Status)Manifest.GainEnergyEveryTurnStatus.Id!,
			statusAmount = 1
		},
		new AStatus
		{
			targetPlayer = true,
			status = Status.energyLessNextTurn,
			statusAmount = 3
		}
	};
}



[CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
public class Borrow : Card
{
	public override string Name() => "Borrow";

	public override CardData GetData(State state)
	{
		return new CardData
		{
			cost = 0,
			floppable = upgrade == Upgrade.B,
			art = upgrade == Upgrade.B ? (flipped ? (Spr)Manifest.BorrowBottomCardArt.Id! : (Spr)Manifest.BorrowTopCardArt.Id!) : StableSpr.cards_ExtraBattery,
			artTint = upgrade == Upgrade.B ? "ffffff" : null
		};
	}

	public override List<CardAction> GetActions(State s, Combat c)
	{
		switch (upgrade) {
			case Upgrade.None:
				return new List<CardAction>
				{
					new AEnergy
					{
						changeAmount = 2
					},
					new AStatus
					{
						targetPlayer = true,
						status = Status.energyLessNextTurn,
						statusAmount = 1 
					},
					new AHurtAndHealLater
					{
						targetPlayer = true,
						hurtAmount = 1
					}
				};
			case Upgrade.A:
				return new List<CardAction>
				{
					new AEnergy
					{
						changeAmount = 2
					},
					new AStatus
					{
						targetPlayer = true,
						status = Status.energyLessNextTurn,
						statusAmount = 1 
					},
					new ADrawCard
					{
						count = 1
					},
					new AStatus
					{
						targetPlayer = true,
						status = Status.drawLessNextTurn,
						statusAmount = 1
					},
					new AHurtAndHealLater
					{
						targetPlayer = true,
						hurtAmount = 1
					}
				};
			case Upgrade.B:
				return new List<CardAction>
				{
					new AEnergy
					{
						changeAmount = 1,
						disabled = flipped
					},
					new AHurtAndHealLater
					{
						targetPlayer = true,
						hurtAmount = 1,
						disabled = flipped
					},
					new ADummyAction(),
					new ADrawCard
					{
						count = 1,
						disabled = !flipped
					},
					new AHurtAndHealLater
					{
						targetPlayer = true,
						hurtAmount = 1,
						disabled = !flipped
					},
				};
			default:
				return new List<CardAction>();
		}
	}
}



[CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
public class Channel : Card
{
	public override string Name() => "Channel";

	public override CardData GetData(State state)
	{
		Spr? art = null;
		if (upgrade != Upgrade.None)
			art = flipped ? (Spr)Manifest.ChannelBottomCardArt!.Id! : (Spr)Manifest.ChannelTopCardArt!.Id!;
		return new CardData()
		{
			cost = 1,
			floppable = upgrade != Upgrade.None,
			buoyant = upgrade == Upgrade.B,
			retain = upgrade == Upgrade.B,
			infinite = true,
			art = art
		};
	}

	public override List<CardAction> GetActions(State s, Combat c)
	{
		switch(upgrade)
		{
			case Upgrade.None:
				return new List<CardAction> {
					new AStatus {
						status = Status.shield,
						statusAmount = 1,
						targetPlayer = true
					},
					new ADrawCard {
						count = 1
					}
				};
			case Upgrade.A:
				return new List<CardAction> {
					new AStatus {
						status = Status.shield,
						statusAmount = 1,
						targetPlayer = true,
						disabled = flipped
					},
					new ADrawCard {
						count = 1,
						disabled = flipped
					},
					new ADummyAction(),
					new AAttack {
						damage = GetDmg(s, 1),
						disabled = !flipped
					},
					new ADrawCard {
						count = 1,
						disabled = !flipped
					}
				};
			case Upgrade.B:
				return new List<CardAction> {
					new AStatus {
						status = Status.shield,
						statusAmount = 1,
						targetPlayer = true,
						disabled = flipped
					},
					new ADummyAction(),
					new ADrawCard {
						count = 1,
						disabled = !flipped
					}

				};
			default:
				return new List<CardAction>();
		}
	}
}




[CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
public class ChargeCannons : Card
{
	public override string Name() => "Charge Cannons";

	public override CardData GetData(State state)
	{
		string description = upgrade switch {
			Upgrade.A => "X = <c=energy>ENERGY</c>\nAdd X <c=card>Surge As</c> to your draw pile, lose all <c=energy>ENERGY</c>.",
			Upgrade.B => "X = <c=energy>ENERGY</c>\nAdd X <c=card>Surge Bs</c> to your draw pile, lose all <c=energy>ENERGY</c>.",
			_ => "X = <c=energy>ENERGY</c>\nAdd X <c=card>Surges</c> to your draw pile, lose all <c=energy>ENERGY</c>.",
		};
		return new CardData
		{
			cost = 0,
			exhaust = true,
			description = description,
			art = StableSpr.cards_EndlessMagazine
		};
	}

	public override List<CardAction> GetActions(State s, Combat c)
	{
		List<CardAction> result = new List<CardAction>();

		var currentCost = this.GetCurrentCostNoRecursion(s);
		result.Add(new AVariableHintEnergy
		{
			setAmount = Manifest.GetEnergyAmount(s, c, this) - currentCost
		});

		int amount = Manifest.GetEnergyAmount(s, c, this) - currentCost;
		result.Add(new AAddCardAdjusted
		{
			card = new Surge {
				upgrade = upgrade
			},
			amount = amount,
			amountDisplayAdjustment = -currentCost,
			destination = CardDestination.Deck,
			xHint = 1
		});
		
		result.Add(new AEnergySet {
			setTo = 0
		});

		return result;
	}
}




[CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
public class ChargeShields : Card
{
	public override string Name() => "Charge Shields";

	public override CardData GetData(State state)
	{
		return new CardData
		{
			cost = 0,
			exhaust = true,
			art = StableSpr.cards_BoostCapacitors
		};
	}

	public override List<CardAction> GetActions(State s, Combat c)
	{
		var currentCost = this.GetCurrentCostNoRecursion(s);
		return new List<CardAction>
		{
			new AVariableHintEnergy
			{
				setAmount = Manifest.GetEnergyAmount(s, c, this) - currentCost,
			},
			new AStatusAdjusted
			{
				targetPlayer = true,
				status = upgrade == Upgrade.B ? Status.maxShield : Status.tempShield,
				statusAmount = Manifest.GetEnergyAmount(s, c, this),
				amountDisplayAdjustment = -currentCost,
				xHint = 1
			},
			new AStatusAdjusted
			{
				targetPlayer = true,
				status = Status.shield,
				statusAmount = Manifest.GetEnergyAmount(s, c, this),
				amountDisplayAdjustment = -currentCost,
				xHint = 1
			},
			new AEnergySet {
				setTo = upgrade == Upgrade.A ? 1 : 0
			}
		};
	}
}




[CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
public class ChargeThrusters : Card
{
	public override string Name() => "Charge Thrusters";

	public override CardData GetData(State state)
	{
		return new CardData
		{
			cost = 1,
			exhaust = true,
			art = StableSpr.cards_CombustionEngine
		};
	}

	public override List<CardAction> GetActions(State s, Combat c)
	{
		List<CardAction> result = new();

		var currentCost = this.GetCurrentCostNoRecursion(s);
		result.Add(new AVariableHintEnergy
		{
			setAmount = Manifest.GetEnergyAmount(s, c, this) - currentCost,
		});

		int multiplier = upgrade == Upgrade.None ? 1 : 2;
		result.Add(new AStatusAdjusted
		{
			targetPlayer = true,
			status = Status.evade,
			statusAmount = multiplier * Manifest.GetEnergyAmount(s, c, this),
			amountDisplayAdjustment = -multiplier * currentCost,
			xHint = multiplier
		});
		
		result.Add(new AEnergySet {
			setTo = upgrade == Upgrade.A ? 0 : 1
		});
		
		if (upgrade == Upgrade.B)
		{
			result.Add(new AStatus
			{
				targetPlayer = true,
				status = Status.loseEvadeNextTurn,
				statusAmount = 1
			});
		}

		return result;
	}
}




[CardMeta(rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
public class Circuit : Card
{
	public override string Name() => "Circuit";

	public override CardData GetData(State state)
	{
		return new CardData
		{
			cost = upgrade switch
			{
				Upgrade.None => 3,
				Upgrade.A => 2,
				Upgrade.B => 4,
				_ => 3
			},
			exhaust = true,
		};
	}

	public override List<CardAction> GetActions(State s, Combat c)
	{
		return new List<CardAction>
		{
			new AStatus
			{
				status = (Status)(Manifest.CircuitStatus?.Id ?? throw new Exception("Missing CircuitStatus")),
				statusAmount = upgrade == Upgrade.B ? 2 : 1,
				targetPlayer = true
			}
		};
	}
}




[CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
public class EnergyBolt : Card
{
	public override string Name() => "Energy Bolt";

	public override CardData GetData(State state)
	{
		return new CardData()
		{
			cost = 1,
			art = StableSpr.cards_BlockerBurnout
		};
	}

	public override List<CardAction> GetActions(State s, Combat c)
	{
		switch(upgrade) {
			case Upgrade.None:
				return new List<CardAction> {
					new AAttack {
						damage = GetDmg(s, 2),
						piercing = true,
						status = Status.tempShield,
						statusAmount = 1
					}
				};
			case Upgrade.A:
				return new List<CardAction> {
					new AAttack {
						damage = GetDmg(s, 3),
						piercing = true,
						status = Status.tempShield,
						statusAmount = 1
					}
				};
			case Upgrade.B:
				return new List<CardAction> {
					new AAttack {
						damage = GetDmg(s, 4),
						piercing = true,
						status = Status.shield,
						statusAmount = 2
					}
				};
			default:
				return new List<CardAction>();
		}
	}
}




[CardMeta(rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
public class GammaRay : Card
{
	public override string Name() => "Gamma Ray";

	public override CardData GetData(State state)
	{
		return new CardData
		{
			cost = upgrade == Upgrade.B ? 5 : 4,
			exhaust = true,
			retain = upgrade == Upgrade.A
		};
	}

	public override List<CardAction> GetActions(State s, Combat c)
	{
		return new List<CardAction>
		{
			new AAttack
			{
				damage = GetDmg(s, upgrade == Upgrade.B ? 13 : 9),
				piercing = true,
				dialogueSelector = ".GammaRay"
			}
		};
	}
}




[CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
public class GarageSale : Card
{
	public override string Name() => "Garage Sale";

	public override CardData GetData(State state)
	{
		int cost = 2;
		if (upgrade == Upgrade.A) cost = 1;
		else if (upgrade == Upgrade.B) cost = 3;
		return new CardData
		{
			cost = cost,
			exhaust = true
		};
	}

	public override List<CardAction> GetActions(State s, Combat c)
	{
		List<CardAction> result = new()
		{
			new ADiscountHand
			{
				discountAmount = -1
			}
		};

		if (upgrade != Upgrade.B)
		{
			result.Add(new AEndTurn());
		}
		return result;
	}
}




[CardMeta(rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
public class Innovation : Card
{
	public override string Name() => "Innovation";

	public override CardData GetData(State state)
	{
		string description = upgrade switch {
			Upgrade.None => "Choose a card in hand. It costs 0 <c=energy>ENERGY</c> once per turn.",
			Upgrade.A => "Choose a card in hand. It costs 0 <c=energy>ENERGY</c> once per turn.",
			Upgrade.B => "<c=downside>Discard</c> a card in hand. It costs 0 <c=energy>ENERGY</c> once per turn.",
			_ => ""
		};
		return new CardData
		{
			cost = upgrade == Upgrade.B ? 0 : upgrade == Upgrade.A ? 2 : 3,
			exhaust = true,
			description = description
		};
	}

	public override List<CardAction> GetActions(State s, Combat c)
	{
		switch (upgrade) {
			case Upgrade.None:
			case Upgrade.A:
				return new List<CardAction> {
					new ADelay{
						time = -0.5
					},
					new ACardSelect {
						browseAction = new AChooseCardMakeFreeOncePerTurn(),
						browseSource = CardBrowse.Source.Hand
					}
				};
			case Upgrade.B:
				return new List<CardAction> {
					new ADelay{
						time = -0.5
					},
					new ACardSelect {
						browseAction = new AChooseCardMakeFreeOncePerTurnAndDiscard(),
						browseSource = CardBrowse.Source.Hand
					}
				};
			default:
				return new List<CardAction>();
		}
	}
}




[CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
public class Interference : Card
{
	public override string Name() => "Interference";

	public override CardData GetData(State state)
	{
		return new CardData()
		{
			cost = 1,
			infinite = true,
			flippable = upgrade == Upgrade.A,
			art = flipped ? StableSpr.cards_ScootLeft : StableSpr.cards_ScootRight
		};
	}

	public override List<CardAction> GetActions(State s, Combat c)
	{	
		return new List<CardAction> {
			new AMoveImproved
			{
				dir = upgrade == Upgrade.B ? 2 : 1,
				targetPlayer = false,
				ignoreHermes = true
			}
		};
	}
}




[CardMeta(rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
public class Jumpstart : Card
{
	public override string Name() => "Jumpstart";

	public override CardData GetData(State state)
	{
		return new CardData
		{
			cost = 0,
			buoyant = upgrade == Upgrade.B,
			exhaust = true
		};
	}

	public override List<CardAction> GetActions(State s, Combat c)
	{
		List<CardAction> result = new();

		int cost = this.GetCurrentCostNoRecursion(s);
		AVariableHintEnergy hint = new()
		{
			setAmount = Manifest.GetEnergyAmount(s, c, this) - cost
		};
		result.Add(hint);

		result.Add(new ADrawCardAdjusted {
			count = Manifest.GetEnergyAmount(s, c, this),
			countDisplayAdjustment = -cost,
			xHint = 1
		});

		if (upgrade == Upgrade.A)
			result.Add(new ADrawCard
			{
				count = 2
			});

		return result;
	}
}




[CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
public class PowerCell : CheapCard
{
	public override string Name() => "Power Cell";

	public override int GetCheapDiscount()
	{
		if (upgrade == Upgrade.A)
			return -1;
		return 0;
	}

	public override CardData GetData(State state)
	{
		base.GetData(state);
		return new CardData()
		{
			cost = upgrade == Upgrade.B ? 2 : 1,
			art = StableSpr.cards_GoatDrone
		};
	}

	public override List<CardAction> GetActions(State s, Combat c)
	{
		switch (upgrade)
		{
			case Upgrade.None:
				return new List<CardAction>
				{
					new ASpawn
					{
						thing = new Midrow.PowerCell
						{
							yAnimation = 0.0
						}
					}
				};
			case Upgrade.A:
				return new List<CardAction>
				{
					new ASpawn
					{
						thing = new Midrow.PowerCell
						{
							yAnimation = 0.0,
							bubbleShield = true
						}
					}
				};
			case Upgrade.B:
				return new List<CardAction>
				{
					new ASpawn
					{
						thing = new Midrow.PowerCell
						{
							yAnimation = 0.0
						}
					},
					new AMove
					{
						dir = -1,
						targetPlayer = true
					},
					new ASpawn
					{
						thing = new Midrow.PowerCell
						{
							yAnimation = 0.0
						}
					}
				};
			default:
				return new List<CardAction>();
		}
	}
}




[CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
public class PowerNap : CheapCard
{
	public override string Name() => "Power Nap";

	public override int GetCheapDiscount()
	{
		if (upgrade == Upgrade.A)
			return -1;
		return 0;
	}
	
	public override CardData GetData(State state)
	{
		base.GetData(state);
		return new CardData
		{
			cost = 1,
			exhaust = upgrade == Upgrade.B,
			floppable = true,
			art = flipped ? (Spr)Manifest.PowerNapBottomCardArt.Id! : (Spr)Manifest.PowerNapTopCardArt.Id!,
			artTint = "ffffff"
		};
	}

	public override List<CardAction> GetActions(State s, Combat c)
	{
		return new List<CardAction>
		{
			new AStatus {
				status = Status.energyNextTurn,
				statusAmount = upgrade == Upgrade.B ? 2 : 1,
				targetPlayer = true,
				disabled = flipped
			},
			new AStatus {
				status = Status.drawNextTurn,
				statusAmount = upgrade == Upgrade.B ? 2 : 1,
				targetPlayer = true,
				disabled = flipped,
				dialogueSelector = ".PowerNapNap"
			},
			new ADummyAction(),
			new AEnergy {
				changeAmount = upgrade == Upgrade.B ? 2 : 1,
				disabled = !flipped
			},
			new ADrawCard {
				count = upgrade == Upgrade.B ? 2 : 1,
				disabled = !flipped,
				dialogueSelector = ".PowerNapAwake"
			}
		};
	}
}




[CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
public class PowerSink : Card
{
	public override string Name() => "Power Sink";

	public override CardData GetData(State state)
	{
		return new CardData()
		{
			cost = upgrade == Upgrade.A ? 0 : 1,
			exhaust = upgrade == Upgrade.B,
			art = StableSpr.cards_MultiBlast
		};
	}

	public override List<CardAction> GetActions(State s, Combat c)
	{
		List<CardAction> result = new();

		int currentCost = this.GetCurrentCostNoRecursion(s);
		AVariableHintEnergy hint = new()
		{
			setAmount = Manifest.GetEnergyAmount(s, c, this) - currentCost,
		};
		result.Add(hint);

		int multiplier = upgrade == Upgrade.B ? 3 : 2;
		result.Add(new AAttackAdjusted {
			damage = GetDmg(s, multiplier * Manifest.GetEnergyAmount(s, c, this)),
			damageDisplayAdjustment = -currentCost * multiplier,
			xHint = multiplier
		});
		
		AEnergySet energy = new()
		{
			setTo = 0
		};

		result.Add(energy);

		return result;
	}
}




[CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
public class RefundShot : CheapCard
{
	public override string Name() => "Refund Shot";

	public override int GetCheapDiscount()
	{
		if (upgrade == Upgrade.A)
			return -1;
		return 0;
	}

	public override CardData GetData(State state)
	{
		base.GetData(state);
		return new CardData
		{
			cost = upgrade == Upgrade.B ? 3 : 1
		};
	}

	public override List<CardAction> GetActions(State s, Combat c)
	{
		return upgrade switch
		{
			Upgrade.None or Upgrade.A => new List<CardAction> {
					new AAttack {
						damage = GetDmg(s, 1)
					},
					new AHurtAndHealLater
					{
						targetPlayer = true,
						hurtAmount = 1
					},
					new AEnergy {
						changeAmount = 1
					}
				},
			Upgrade.B => new List<CardAction> {
					new AAttack {
						damage = GetDmg(s, 2)
					},
					new AHurtAndHealLater
					{
						targetPlayer = true,
						hurtAmount = 1
					},
					new AEnergy {
						changeAmount = 3
					}
				},
			_ => new List<CardAction>(),
		};
	}
}




[CardMeta(rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
public class RenewableResource : Card
{
	public override string Name() => "Renewable Resource";

	public override CardData GetData(State state)
	{
		return new CardData
		{
			cost = upgrade == Upgrade.A ? 0 : 1,
			exhaust = upgrade != Upgrade.B,
			description = "Rightmost non-<c=cardtrait>infinite</c> card gains <c=cardtrait>infinite</c> and <c=cardtrait>short-circuit</c>."
		};
	}

	public override List<CardAction> GetActions(State s, Combat c)
	{
		return new List<CardAction>
		{
			new AAddShortCircuitToRightmostCard {
				skipInfinite = true
			},
			new AAddInfiniteToRightmostCard {
				skipInfinite = true
			}
		};
	}

	public override void HilightOtherCards(State s, Combat c)
	{
		Card? card = c.hand.Where((Card c) => c != this && !c.GetDataWithOverrides(s).infinite).FirstOrDefault();
		if (card != null)
		{   
			c.hilightedCards.Add(card.uuid);
		}
	}
}




[CardMeta(dontOffer = true, rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
public class ReverseEngineer : Card
{
	public override string Name() => "ReverseEngineer";

	public override CardData GetData(State state)
	{
		// var default_desc = "<c=cardtrait>Exhaust</c> the leftmost non-<c=cardtrait>infinite</c> card and gain its cost as <c=energy>ENERGY</c>.";
		// var description = upgrade switch
		// {
		//     Upgrade.None => default_desc,
		//     Upgrade.A => default_desc,
		//     Upgrade.B => "Choose a card. Discard it and gain its cost as <c=energy>ENERGY</c>.",
		//     _         => ""
		// };
		return new CardData
		{
			description = upgrade switch
			{
				Upgrade.A => "Choose a card in hand. Discard it and gain its cost as <c=energy>ENERGY</c>.",
				Upgrade.B => "Choose a card in hand. Exhaust it and gain its cost as <c=energy>ENERGY</c>.",
				_ => "Choose a card in hand. Exhaust it and gain its cost as <c=energy>ENERGY</c>."
			},
			retain = true,
			cost = 0,
			exhaust = upgrade != Upgrade.B,
			temporary = true,
			art = StableSpr.cards_CorruptedCore
		};
	}

	public override List<CardAction> GetActions(State s, Combat c)
	{
		return upgrade switch {
			Upgrade.A => new List<CardAction> {
				new ADelay
				{
					time = -0.5
				},
				new ACardSelect
				{
					browseAction = new AGetEnergyFromChosenCard(),
					browseSource = CardBrowse.Source.Hand
				}
			},
			_ => new List<CardAction> {
				new ADelay
				{
					time = -0.5
				},
				new ACardSelect
				{
					browseAction = new AGetEnergyFromChosenCard
					{
						exhaustThisCardAfterwards = true
					},
					browseSource = CardBrowse.Source.Hand
				}
			},
		};
	}
}




[CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
public class Rummage : Card
{
	public override string Name() => "Rummage";

	public override CardData GetData(State state)
	{
		return new CardData
		{
			cost = 1,
			infinite = true,
			art = StableSpr.cards_QuickThinking
		};
	}

	public override List<CardAction> GetActions(State s, Combat c)
	{
		return upgrade switch
		{
			Upgrade.None => new List<CardAction>
				{
					new ADrawCard
					{
						count = 2
					}
				},
			Upgrade.A => new List<CardAction>
				{
					new ADrawCard
					{
						count = 3
					}
				},
			Upgrade.B => new List<CardAction>
				{
					new ADrawCard
					{
						count = 4
					},
					new ADiscard
					{
						count = 2
					}
				},
			_ => new List<CardAction>(),
		};
	}
}




[CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
public class ShortTermSolution : CheapCard
{
	public override string Name() => "Short-Term Solution";

	public override int GetCheapDiscount()
	{
		return -1;
	}

	public override CardData GetData(State state)
	{
		base.GetData(state);
		return new CardData
		{
			cost = 2,
			art = StableSpr.cards_ColorlessTrash,
			flippable = true
		};
	}

	public override List<CardAction> GetActions(State s, Combat c)
	{
		return upgrade switch
		{
			Upgrade.None => new List<CardAction> {
					new AMoveImproved
					{
						dir = 3,
						targetPlayer = false,
						ignoreHermes = true
					}
					// new AAttack {
					// 	damage = GetDmg(s, 3)
					// },
				},
			Upgrade.A => new List<CardAction> {
					new AMoveImproved
					{
						dir = 3,
						targetPlayer = false,
						ignoreHermes = true
					},
					// new AAttack {
					// 	damage = GetDmg(s, 3)
					// },
					new AStatus {
						status = Status.tempShield,
						statusAmount = 2,
						targetPlayer = true
					},
				},
			Upgrade.B => new List<CardAction> {
					new AMoveImproved
					{
						dir = 5,
						targetPlayer = false,
						ignoreHermes = true
					},
					new AStatus
					{
						status = Status.overdrive,
						statusAmount = 2,
						targetPlayer = false
					}
					// new AAttack {
					// 	damage = GetDmg(s, 6)
					// }
				},
			_ => new List<CardAction>(),
		};
	}
}




[CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
public class SolarSailing : Card
{
    public override string Name() => "Solar Sailing";

    public override CardData GetData(State state)
    {
        return new CardData
        {
            cost = 0,
            flippable = upgrade == Upgrade.B,
            retain = upgrade == Upgrade.A,
			art = StableSpr.cards_SolarBreeze
        };
    }

    public override List<CardAction> GetActions(State s, Combat c)
    {
        return new List<CardAction>
        {
            new AMove
            {
                dir = 1,
                targetPlayer = true
            },
            new ADrawCard
            {
                count = 1
            }
        };
    }
}




[CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B }, dontOffer = true)]
public class Surge : Card
{
    public override string Name() => "Surge";

    public override CardData GetData(State state)
    {
        return new CardData()
        {
            cost = 1,
            exhaust = true,
            temporary = true,
            art = StableSpr.cards_Overdrive
        };
    }

    public override List<CardAction> GetActions(State s, Combat c)
    {
		return upgrade switch
		{
			Upgrade.None => new List<CardAction>
				{
					new AAttack {
						damage = GetDmg(s, 2)
					},
					new AStatus {
						status = Status.overdrive,
						statusAmount = 1,
						targetPlayer = true
					}
				},
			Upgrade.A => new List<CardAction>
				{
					new AAttack {
						damage = GetDmg(s, 3)
					},
					new AStatus {
						status = Status.overdrive,
						statusAmount = 1,
						targetPlayer = true
					}
				},
			Upgrade.B => new List<CardAction>
				{
					new AAttack {
						damage = GetDmg(s, 1),
						stunEnemy = true
					},
					new AStatus {
						status = Status.overdrive,
						statusAmount = 1,
						targetPlayer = true
					}
				},
			_ => new List<CardAction>(),
		};
	}
}




[CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
public class EddieExe : Card
{
    public override string Name() => "Eddie.EXE";

    public override CardData GetData(State state)
    {
        return new CardData()
        {
            cost = upgrade == Upgrade.A ? 0 : 1,
			exhaust = true,
			description = ColorlessLoc.GetDesc(state, upgrade == Upgrade.B ? 3 : 2, (Deck)Manifest.EddieDeck.Id!)
        };
    }

    public override List<CardAction> GetActions(State s, Combat c)
    {
		return upgrade switch
		{
			Upgrade.B => new()  {
				new ACardOffering
				{
					amount = 3,
					limitDeck = (Deck)Manifest.EddieDeck.Id!,
					makeAllCardsTemporary = true,
					overrideUpgradeChances = false,
					canSkip = false,
					inCombat = true,
					discount = -1,
					dialogueSelector = ".summonEddie"
				}
			},
			_ => new() {
				new ACardOffering
				{
					amount = 2,
					limitDeck = (Deck)Manifest.EddieDeck.Id!,
					makeAllCardsTemporary = true,
					overrideUpgradeChances = false,
					canSkip = false,
					inCombat = true,
					discount = -1,
					dialogueSelector = ".summonEddie"
				}
			},
		};
	}
}
