using Nickel;
using TheJazMaster.Eddie.Actions;

namespace TheJazMaster.Eddie.Cards;

[CardMeta(rarity = Rarity.uncommon, upgradesTo = [Upgrade.A, Upgrade.B])]
public class Amplify : Card
{
	public override string Name() => "Amplify";

	public override CardData GetData(State state)
	{
		return new CardData
		{
			cost = 3,
			exhaust = upgrade != Upgrade.B,
			art = StableSpr.cards_Overclock
		};
	}

	public override List<CardAction> GetActions(State s, Combat c) => upgrade switch {
		Upgrade.A => [
			new AStatus
			{
				targetPlayer = true,
				status = (Status)Manifest.GainEnergyEveryTurnStatus.Id!,
				statusAmount = 1
			}
		],
		_ => [
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
				statusAmount = 1
			}
		]
	};
}



[CardMeta(rarity = Rarity.common, upgradesTo = [Upgrade.A, Upgrade.B])]
public class Borrow : Card
{
	public override string Name() => "Borrow";

	public override CardData GetData(State state)
	{
		return new CardData
		{
			cost = 0,
			art = StableSpr.cards_ExtraBattery,
		};
	}

	public override List<CardAction> GetActions(State s, Combat c)
	{
		return upgrade switch
		{
			Upgrade.None =>
				[
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
						hurtAmount = 2
					}
				],
			Upgrade.B =>
				[
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
						count = 2
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
						hurtAmount = 2
					}
				],
			Upgrade.A =>
				[
					new AEnergy
					{
						changeAmount = 2
					},
					new AHurtAndHealLater
					{
						targetPlayer = true,
						hurtAmount = 2
					}
				],
			_ => [],
		};
	}
}



[CardMeta(rarity = Rarity.common, upgradesTo = [Upgrade.A, Upgrade.B])]
public class Channel : Card
{
	public override string Name() => "Channel";

	public override CardData GetData(State state)
	{
		return new CardData()
		{
			cost = 1,
			infinite = true
		};
	}

	public override List<CardAction> GetActions(State s, Combat c)
	{
		return upgrade switch
		{
			Upgrade.None => [
				new AStatus {
					status = Status.shield,
					statusAmount = 1,
					targetPlayer = true
				},
				new ADrawCard {
					count = 1
				}
			],
			Upgrade.A => [
				new AStatus {
					status = Status.shield,
					statusAmount = 1,
					targetPlayer = true
				},
				new ADrawCard {
					count = 2
				}
			],
			Upgrade.B => [
				new AAttack {
					damage = GetDmg(s, 2)
				},
				new ADrawCard {
					count = 1
				},
			],
			_ => [],
		};
	}
}




[CardMeta(rarity = Rarity.uncommon, upgradesTo = [Upgrade.A, Upgrade.B])]
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
		List<CardAction> result = [];

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




[CardMeta(rarity = Rarity.uncommon, upgradesTo = [Upgrade.A, Upgrade.B])]
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
		return
		[
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
		];
	}
}




[CardMeta(rarity = Rarity.uncommon, upgradesTo = [Upgrade.A, Upgrade.B])]
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
		List<CardAction> result = [];

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




[CardMeta(rarity = Rarity.rare, upgradesTo = [Upgrade.A, Upgrade.B])]
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




[CardMeta(rarity = Rarity.common, upgradesTo = [Upgrade.A, Upgrade.B])]
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
		return upgrade switch
		{
			Upgrade.None => [
					new AAttack {
						damage = GetDmg(s, 2),
						piercing = true,
						status = Status.tempShield,
						statusAmount = 1
					}
				],
			Upgrade.A => [
					new AAttack {
						damage = GetDmg(s, 3),
						piercing = true,
						status = Status.tempShield,
						statusAmount = 1
					}
				],
			Upgrade.B => [
					new AAttack {
						damage = GetDmg(s, 4),
						piercing = true,
						status = Status.shield,
						statusAmount = 2
					}
				],
			_ => [],
		};
	}
}




[CardMeta(rarity = Rarity.rare, upgradesTo = [Upgrade.A, Upgrade.B])]
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
		return
		[
			new AAttack
			{
				damage = GetDmg(s, upgrade == Upgrade.B ? 13 : 9),
				piercing = true,
				dialogueSelector = ".GammaRay"
			}
		];
	}
}




[CardMeta(rarity = Rarity.uncommon, upgradesTo = [Upgrade.A, Upgrade.B])]
public class GarageSale : Card
{
	public override string Name() => "Garage Sale";

	public override CardData GetData(State state)
	{
		int cost = 1;
		if (upgrade == Upgrade.A) cost = 0;
		else if (upgrade == Upgrade.B) cost = 2;
		return new CardData
		{
			cost = cost,
			exhaust = true
		};
	}

	public override List<CardAction> GetActions(State s, Combat c)
	{
		List<CardAction> result =
		[
			new ADiscountHand
			{
				discountAmount = -1
			}
		];

		if (upgrade != Upgrade.B)
		{
			result.Add(new AEndTurn());
		}
		return result;
	}
}




[CardMeta(rarity = Rarity.rare, upgradesTo = [Upgrade.A, Upgrade.B])]
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
		return upgrade switch
		{
			Upgrade.None or Upgrade.A => [
					new ADelay{
						time = -0.5
					},
					new ACardSelect {
						browseAction = new AChooseCardMakeFreeOncePerTurn(),
						browseSource = CardBrowse.Source.Hand
					}
				],
			Upgrade.B => [
					new ADelay{
						time = -0.5
					},
					new ACardSelect {
						browseAction = new AChooseCardMakeFreeOncePerTurnAndDiscard(),
						browseSource = CardBrowse.Source.Hand
					}
				],
			_ => [],
		};
	}
}




[CardMeta(rarity = Rarity.common, upgradesTo = [Upgrade.A, Upgrade.B])]
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
		return [
			new AMoveImproved
			{
				dir = upgrade == Upgrade.B ? 2 : 1,
				targetPlayer = false
			}
		];
	}
}




[CardMeta(rarity = Rarity.rare, upgradesTo = [Upgrade.A, Upgrade.B])]
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
		List<CardAction> result = [];

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




[CardMeta(rarity = Rarity.common, upgradesTo = [Upgrade.A, Upgrade.B])]
public class PowerCell : Card, IHasCustomCardTraits
{
	public override string Name() => "Power Cell";

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
		return upgrade switch
		{
			Upgrade.None =>
				[
					new ASpawn
					{
						thing = new Midrow.PowerCell
						{
							yAnimation = 0.0
						}
					}
				],
			Upgrade.A =>
				[
					new ASpawn
					{
						thing = new Midrow.PowerCell
						{
							yAnimation = 0.0,
							bubbleShield = true
						}
					}
				],
			Upgrade.B =>
				[
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
				],
			_ => [],
		};
	}

	public IReadOnlySet<ICardTraitEntry> GetInnateTraits(State state) =>
		upgrade == Upgrade.A ? new HashSet<ICardTraitEntry>() { Manifest.CheapTrait } : [];
}




[CardMeta(rarity = Rarity.common, upgradesTo = [Upgrade.A, Upgrade.B])]
public class PowerNap : Card, IHasCustomCardTraits
{
	public override string Name() => "Power Nap";

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
		return
		[
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
		];
	}

	public IReadOnlySet<ICardTraitEntry> GetInnateTraits(State state) =>
		upgrade == Upgrade.A ? new HashSet<ICardTraitEntry>() { Manifest.CheapTrait } : [];
}




[CardMeta(rarity = Rarity.uncommon, upgradesTo = [Upgrade.A, Upgrade.B])]
public class PowerSink : Card
{
	public override string Name() => "Power Sink";

	public override CardData GetData(State state)
	{
		return new CardData()
		{
			cost = upgrade == Upgrade.B ? 1 : 0,
			exhaust = upgrade != Upgrade.A,
			art = StableSpr.cards_MultiBlast
		};
	}

	public override List<CardAction> GetActions(State s, Combat c)
	{
		List<CardAction> result = [];

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




[CardMeta(rarity = Rarity.common, upgradesTo = [Upgrade.A, Upgrade.B])]
public class RefundShot : Card, IHasCustomCardTraits
{
	public override string Name() => "Refund Shot";

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
			Upgrade.None or Upgrade.A => [
					new AAttack {
						damage = GetDmg(s, 1)
					},
					// new AHurtAndHealLater
					// {
					// 	targetPlayer = true,
					// 	hurtAmount = 1
					// },
					new AEnergy {
						changeAmount = 1
					}
				],
			Upgrade.B => [
					new AAttack {
						damage = GetDmg(s, 2)
					},
					// new AHurtAndHealLater
					// {
					// 	targetPlayer = true,
					// 	hurtAmount = 1
					// },
					new AEnergy {
						changeAmount = 3
					}
				],
			_ => [],
		};
	}

	public IReadOnlySet<ICardTraitEntry> GetInnateTraits(State state) =>
		upgrade == Upgrade.A ? new HashSet<ICardTraitEntry>() { Manifest.CheapTrait } : [];
}




[CardMeta(rarity = Rarity.rare, upgradesTo = [Upgrade.A, Upgrade.B])]
public class RenewableResource : Card
{
	public override string Name() => "Renewable Resource";

	public override CardData GetData(State state)
	{
		return new CardData
		{
			cost = upgrade == Upgrade.A ? 0 : 1,
			exhaust = true,
			description = upgrade == Upgrade.B ? "Choose two cards in <c=keyword>hand</c>. They gain <c=cardtrait>infinite</c> and <c=cardtrait>short-circuit</c>."
				: "Choose a card in <c=keyword>hand</c>. It gains <c=cardtrait>infinite</c> and <c=cardtrait>short-circuit</c>."
		};
	}

	public override List<CardAction> GetActions(State s, Combat c)
	{
		if (upgrade == Upgrade.B)
			return [
				new ACardSelect {
					browseAction = new ARenewableCard(),
					browseSource = CardBrowse.Source.Hand,
				},
				new ACardSelect {
					browseAction = new ARenewableCard(),
					browseSource = CardBrowse.Source.Hand,
				}
			];
		return [
			new ACardSelect {
				browseAction = new ARenewableCard(),
				browseSource = CardBrowse.Source.Hand,
			}
		];
	}

	// public override void HilightOtherCards(State s, Combat c)
	// {
	// 	Card? card = c.hand.Where((Card c) => c != this && !c.GetDataWithOverrides(s).infinite).FirstOrDefault();
	// 	if (card != null)
	// 	{   
	// 		c.hilightedCards.Add(card.uuid);
	// 	}
	// }
}




[CardMeta(dontOffer = true, rarity = Rarity.uncommon, upgradesTo = [Upgrade.A, Upgrade.B])]
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
			Upgrade.A => [
				new ADelay
				{
					time = -0.5
				},
				new ACardSelect
				{
					browseAction = new AGetEnergyFromChosenCard(),
					browseSource = CardBrowse.Source.Hand
				}
			],
			_ => [
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
			],
		};
	}
}




[CardMeta(rarity = Rarity.common, upgradesTo = [Upgrade.A, Upgrade.B])]
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
			Upgrade.None =>
				[
					new AReverseHand(),
					new ADrawCard
					{
						count = 2
					}
				],
			Upgrade.A =>
				[
					new AReverseHand(),
					new ADrawCard
					{
						count = 3
					}
				],
			Upgrade.B =>
				[
					new ADiscard
					{
						count = 2
					},
					new ADrawCard
					{
						count = 5
					}
				],
			_ => [],
		};
	}
}




[CardMeta(rarity = Rarity.uncommon, upgradesTo = [Upgrade.A, Upgrade.B])]
public class ShortTermSolution : Card, IHasCustomCardTraits
{
	public override string Name() => "Short-Term Solution";

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
			Upgrade.None => [
					new AMoveImproved
					{
						dir = 3,
						targetPlayer = false
					}
				],
			Upgrade.A => [
					new AMoveImproved
					{
						dir = 3,
						targetPlayer = false
					},
					new AStatus {
						status = Status.tempShield,
						statusAmount = 2,
						targetPlayer = true
					},
				],
			Upgrade.B => [
					new AMoveImproved
					{
						dir = 5,
						targetPlayer = false
					},
					new AStatus
					{
						status = Status.overdrive,
						statusAmount = 2,
						targetPlayer = false
					}
				],
			_ => [],
		};
	}

	public IReadOnlySet<ICardTraitEntry> GetInnateTraits(State state) =>
		new HashSet<ICardTraitEntry>() { Manifest.CheapTrait };
}




[CardMeta(rarity = Rarity.common, upgradesTo = [Upgrade.A, Upgrade.B])]
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
        return
		[
			new AMove
            {
                dir = 1,
                targetPlayer = true
            },
            new ADrawCard
            {
                count = 1
            }
        ];
    }
}




[CardMeta(rarity = Rarity.common, upgradesTo = [Upgrade.A, Upgrade.B], dontOffer = true)]
public class Surge : Card
{
    public override string Name() => "Surge";

    public override CardData GetData(State state)
    {
        return new CardData()
        {
            cost = 0,
            exhaust = true,
            temporary = true,
            art = StableSpr.cards_Overdrive
        };
    }

    public override List<CardAction> GetActions(State s, Combat c)
    {
		return upgrade switch
		{
			Upgrade.None =>
				[
					new AAttack {
						damage = GetDmg(s, 2)
					},
					new AStatus {
						status = Status.overdrive,
						statusAmount = 1,
						targetPlayer = true
					}
				],
			Upgrade.A =>
				[
					new AAttack {
						damage = GetDmg(s, 3)
					},
					new AStatus {
						status = Status.overdrive,
						statusAmount = 1,
						targetPlayer = true
					}
				],
			Upgrade.B =>
				[
					new AAttack {
						damage = GetDmg(s, 1),
						stunEnemy = true
					},
					new AStatus {
						status = Status.overdrive,
						statusAmount = 1,
						targetPlayer = true
					}
				],
			_ => [],
		};
	}
}




[CardMeta(rarity = Rarity.common, upgradesTo = [Upgrade.A, Upgrade.B])]
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
			Upgrade.B => [
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
			],
			_ => [
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
			],
		};
	}
}
