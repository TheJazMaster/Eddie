using System.Reflection;
using Nanoray.PluginManager;
using Nickel;
using TheJazMaster.Eddie.Actions;

namespace TheJazMaster.Eddie.Cards;

public class Amplify : Card, IRegisterableCard
{
	public static void Register(Deck deck, IModHelper helper, IPluginPackage<IModManifest> package) {
		IRegisterableCard.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, ModEntry.Instance.EddieDeck,
			Rarity.uncommon,
			StableSpr.cards_Overclock
		);
	}

	public override CardData GetData(State state) => new() {
		cost = 3,
		exhaust = upgrade != Upgrade.B,
	};

	public override List<CardAction> GetActions(State s, Combat c) => upgrade switch {
		Upgrade.A => [
			new AStatus {
				targetPlayer = true,
				status = StatusManager.MoreEnergyStatus,
				statusAmount = 1
			}
		],
		_ => [
			new AStatus
			{
				targetPlayer = true,
				status = StatusManager.MoreEnergyStatus,
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



public class Borrow : Card, IRegisterableCard
{
	public static void Register(Deck deck, IModHelper helper, IPluginPackage<IModManifest> package) {
		IRegisterableCard.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, ModEntry.Instance.EddieDeck,
			Rarity.common,
			StableSpr.cards_ExtraBattery
		);
	}

	public override CardData GetData(State state) => new() {
		cost = 0,
	};

	public override List<CardAction> GetActions(State s, Combat c) => upgrade switch {
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
				ModEntry.Instance.KokoroApi.TempHull.MakeLossAction(2, true).AsCardAction
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
				ModEntry.Instance.KokoroApi.TempHull.MakeLossAction(2, true).AsCardAction
			],
		Upgrade.A =>
			[
				new AEnergy
				{
					changeAmount = 2
				},
				ModEntry.Instance.KokoroApi.TempHull.MakeLossAction(2, true).AsCardAction
			],
		_ => [],
	};
}



public class Channel : Card, IRegisterableCard, IHasCustomCardTraits
{
	public static void Register(Deck deck, IModHelper helper, IPluginPackage<IModManifest> package) {
		IRegisterableCard.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, ModEntry.Instance.EddieDeck,
			Rarity.common,
			helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile($"Sprites/card_art/channel.png")).Sprite
		);
	}

    public IReadOnlySet<ICardTraitEntry> GetInnateTraits(State state) => 
		upgrade == Upgrade.B ? new HashSet<ICardTraitEntry>() { ModEntry.Instance.KokoroApi.Heavy.Trait } : [];

	public override CardData GetData(State state) => new() {
		cost = 1,
		infinite = true
	};

	public override List<CardAction> GetActions(State s, Combat c) => upgrade switch {
		Upgrade.A => [
			new AStatus {
				status = Status.shield,
				statusAmount = 1,
				targetPlayer = true
			},
			new ADrawCard {
				count = 1
			},
			new AAttack {
				damage = GetDmg(s, 1)
			},
		],
		_ => [
			new AStatus {
				status = Status.shield,
				statusAmount = 1,
				targetPlayer = true
			},
			new ADrawCard {
				count = 1
			}
		],
	};
}



public class ChargeCannons : Card, IRegisterableCard //1.3
{
	public static void Register(Deck deck, IModHelper helper, IPluginPackage<IModManifest> package) {
		IRegisterableCard.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, ModEntry.Instance.EddieDeck,
			Rarity.uncommon,
			StableSpr.cards_EndlessMagazine
		);
	}

	public override CardData GetData(State state) => new() {
		cost = 0,
		exhaust = true,
		description = ModEntry.Instance.Localizations.Localize(["card", GetType().Name, "description", upgrade.ToString()])
	};

	public override List<CardAction> GetActions(State s, Combat c)
	{
		List<CardAction> result = [];

		var currentCost = this.GetCurrentCostNoRecursion(s);
		result.Add(new AVariableHintEnergy
		{
			setAmount = XEnergyManager.GetEnergyAmount(s, c, this) - currentCost
		});

		int amount = XEnergyManager.GetEnergyAmount(s, c, this) - currentCost;
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



public class ChargeShields : Card, IRegisterableCard
{
	public static void Register(Deck deck, IModHelper helper, IPluginPackage<IModManifest> package) {
		IRegisterableCard.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, ModEntry.Instance.EddieDeck,
			Rarity.uncommon,
			StableSpr.cards_BoostCapacitors
		);
	}

	public override CardData GetData(State state) => new() {
		cost = 0,
		exhaust = true
	};

	public override List<CardAction> GetActions(State s, Combat c)
	{
		var currentCost = this.GetCurrentCostNoRecursion(s);
		return
		[
			new AVariableHintEnergy {
				setAmount = XEnergyManager.GetEnergyAmount(s, c, this) - currentCost,
			},
			new AStatusAdjusted {
				targetPlayer = true,
				status = upgrade == Upgrade.B ? Status.maxShield : Status.tempShield,
				statusAmount = XEnergyManager.GetEnergyAmount(s, c, this),
				amountDisplayAdjustment = -currentCost,
				xHint = 1
			},
			new AStatusAdjusted {
				targetPlayer = true,
				status = Status.shield,
				statusAmount = XEnergyManager.GetEnergyAmount(s, c, this),
				amountDisplayAdjustment = -currentCost,
				xHint = 1
			},
			new AEnergySet {
				setTo = upgrade == Upgrade.A ? 1 : 0
			}
		];
	}
}



public class ChargeThrusters : Card, IRegisterableCard//1.3
{
	public static void Register(Deck deck, IModHelper helper, IPluginPackage<IModManifest> package) {
		IRegisterableCard.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, ModEntry.Instance.EddieDeck,
			Rarity.uncommon,
			StableSpr.cards_CombustionEngine
		);
	}

	public override CardData GetData(State state) => new() {
		cost = 1,
		exhaust = true
	};

	public override List<CardAction> GetActions(State s, Combat c) {
		int currentCost = this.GetCurrentCostNoRecursion(s);
		return upgrade switch {
			Upgrade.A => [
				new AVariableHintEnergy {
					setAmount = XEnergyManager.GetEnergyAmount(s, c, this) - currentCost
				},
				new AStatusAdjusted {
					status = Status.evade,
					targetPlayer = true,
					statusAmount = 2 * XEnergyManager.GetEnergyAmount(s, c, this),
					amountDisplayAdjustment = -2 * currentCost,
					xHint = 2
				},
				new AEnergySet {
					setTo = 0
				}
			],
			Upgrade.B => [
				new AVariableHintEnergy {
					setAmount = XEnergyManager.GetEnergyAmount(s, c, this) - this.GetCurrentCostNoRecursion(s)
				},
				new AStatusAdjusted {
					status = Status.evade,
					targetPlayer = true,
					statusAmount = 2 * XEnergyManager.GetEnergyAmount(s, c, this),
					amountDisplayAdjustment = -2 * currentCost,
					xHint = 2
				},
				new AEnergySet {
					setTo = 1
				},
				new AStatus {
					status = Status.loseEvadeNextTurn,
					statusAmount = 1,
					targetPlayer = true
				}
			],
			_ => [
				new AVariableHintEnergy {
					setAmount = XEnergyManager.GetEnergyAmount(s, c, this) - currentCost
				},
				new AStatusAdjusted {
					status = Status.evade,
					targetPlayer = true,
					statusAmount = XEnergyManager.GetEnergyAmount(s, c, this),
					amountDisplayAdjustment = -currentCost,
					xHint = 1
				},
				new AEnergySet {
					setTo = 1
				}
			]
		};
	}
}



public class Circuit : Card, IRegisterableCard
{
	public static void Register(Deck deck, IModHelper helper, IPluginPackage<IModManifest> package) {
		IRegisterableCard.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, ModEntry.Instance.EddieDeck,
			Rarity.rare,
			helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile($"Sprites/card_art/circuit.png")).Sprite
		);
	}

	public override CardData GetData(State state) => new() {
		cost = upgrade switch {
			Upgrade.A => 2,
			Upgrade.B => 4,
			_ => 3
		},
		exhaust = true,
	};

	public override List<CardAction> GetActions(State s, Combat c) => [
		new AStatus {
			status = StatusManager.CircuitStatus,
			statusAmount = upgrade == Upgrade.B ? 2 : 1,
			targetPlayer = true
		}
	];
}



public class EnergyBolt : Card, IRegisterableCard
{
	public static void Register(Deck deck, IModHelper helper, IPluginPackage<IModManifest> package) {
		IRegisterableCard.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, ModEntry.Instance.EddieDeck,
			Rarity.common,
			StableSpr.cards_BlockerBurnout
		);
	}

	public override CardData GetData(State state) => new() {
		cost = 1,
	};

	public override List<CardAction> GetActions(State s, Combat c) => upgrade switch {
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


public class GammaRay : Card, IRegisterableCard
{
	public static void Register(Deck deck, IModHelper helper, IPluginPackage<IModManifest> package) {
		IRegisterableCard.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, ModEntry.Instance.EddieDeck,
			Rarity.rare,
			helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile($"Sprites/card_art/gamma_ray.png")).Sprite
		);
	}
	public override CardData GetData(State state) => new() {
		cost = upgrade == Upgrade.B ? 5 : 4,
		exhaust = true,
		retain = upgrade == Upgrade.A
	};

	public override List<CardAction> GetActions(State s, Combat c) => [
		new AAttack {
			damage = GetDmg(s, upgrade == Upgrade.B ? 13 : 9),
			piercing = true,
			dialogueSelector = ".GammaRay"
		}
	];
}



public class GarageSale : Card, IRegisterableCard
{
	public static void Register(Deck deck, IModHelper helper, IPluginPackage<IModManifest> package) {
		IRegisterableCard.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, ModEntry.Instance.EddieDeck,
			Rarity.uncommon,
			helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile($"Sprites/card_art/garage_sale.png")).Sprite
		);
	}

	public override CardData GetData(State state) => new() {
		cost = upgrade switch {
			Upgrade.A => 1,
			_ => 2
		},
		exhaust = true
	};

	public override List<CardAction> GetActions(State s, Combat c) => upgrade switch {
		Upgrade.B => [
			new ADiscountHand(),
		],
		_ => [
			new ADiscountHand(),
			new ADiscountHand(),
			new AEndTurn()
		]
	};
}




public class Hyperfocus : Card, IRegisterableCard
{
	public static void Register(Deck deck, IModHelper helper, IPluginPackage<IModManifest> package) {
		// IRegisterableCard.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, ModEntry.Instance.EddieDeck,
		// 	Rarity.rare,
		// 	helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile($"Sprites/card_art/hyperfocus.png")).Sprite
		// );
	}

	public override CardData GetData(State state)
	{
		return new CardData
		{
			cost = 0,
			exhaust = upgrade != Upgrade.B
		};
	}

	// public override List<CardAction> GetActions(State s, Combat c)
	// {
	// 	List<CardAction> result =
	// 	[
	// 		new ADiscard {
	// 			count = 1
	// 		}
	// 	];

	// 	if (upgrade != Upgrade.B)
	// 	{
	// 		result.Add(new AEndTurn());
	// 	}
	// 	return result;
	// }
}




public class Innovation : Card, IRegisterableCard
{
	public static void Register(Deck deck, IModHelper helper, IPluginPackage<IModManifest> package) {
		IRegisterableCard.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, ModEntry.Instance.EddieDeck,
			Rarity.rare,
			helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile($"Sprites/card_art/innovation.png")).Sprite
		);
	}

	public override CardData GetData(State state) => new() {
		cost = upgrade == Upgrade.B ? 0 : upgrade == Upgrade.A ? 2 : 3,
		exhaust = true,
		description = ModEntry.Instance.Localizations.Localize(["card", GetType().Name, "description", upgrade.ToString()]),
	};

	public override List<CardAction> GetActions(State s, Combat c) => upgrade switch {
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




public class Interference : Card, IRegisterableCard
{
	public static void Register(Deck deck, IModHelper helper, IPluginPackage<IModManifest> package) {
		IRegisterableCard.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, ModEntry.Instance.EddieDeck,
			Rarity.common,
			null
		);
	}

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

	public override List<CardAction> GetActions(State s, Combat c) => [
		new AMove {
			dir = upgrade == Upgrade.B ? 2 : 1,
			targetPlayer = false
		}
	];
}




public class Jumpstart : Card, IRegisterableCard
{
	public static void Register(Deck deck, IModHelper helper, IPluginPackage<IModManifest> package) {
		IRegisterableCard.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, ModEntry.Instance.EddieDeck,
			Rarity.rare,
			helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile($"Sprites/card_art/jumpstart.png")).Sprite
		);
	}

	public override CardData GetData(State state) => new() {
		cost = upgrade == Upgrade.B ? 0 : 3,
		buoyant = upgrade == Upgrade.A,
		exhaust = true
	};

	public override List<CardAction> GetActions(State s, Combat c) {
		if (upgrade == Upgrade.B) {
			int cost = this.GetCurrentCostNoRecursion(s);

			return [
				new AVariableHintEnergy {
					setAmount = XEnergyManager.GetEnergyAmount(s, c, this) - cost
				},
				new ADrawCardAdjusted {
					count = XEnergyManager.GetEnergyAmount(s, c, this),
					countDisplayAdjustment = -cost,
					xHint = 1
				}
			];
		}
		return [
			new ADrawCard {
				count = 3
			},
			new AEnergy {
				changeAmount = 3
			}
		];
	}
}




public class PowerCell : Card, IRegisterableCard, IHasCustomCardTraits
{
	public static void Register(Deck deck, IModHelper helper, IPluginPackage<IModManifest> package) {
		IRegisterableCard.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, ModEntry.Instance.EddieDeck,
			Rarity.common,
			StableSpr.cards_GoatDrone
		);
	}

	public override CardData GetData(State state) => new() {
		cost = upgrade == Upgrade.B ? 2 : 1
	};

	public override List<CardAction> GetActions(State s, Combat c)
	{
		return upgrade switch
		{
			Upgrade.None =>
				[
					new ASpawn
					{
						thing = new Features.PowerCell
						{
							yAnimation = 0.0
						}
					}
				],
			Upgrade.A =>
				[
					new ASpawn
					{
						thing = new Features.PowerCell
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
						thing = new Features.PowerCell
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
						thing = new Features.PowerCell
						{
							yAnimation = 0.0
						}
					}
				],
			_ => [],
		};
	}

	public IReadOnlySet<ICardTraitEntry> GetInnateTraits(State state) =>
		upgrade == Upgrade.A ? new HashSet<ICardTraitEntry>() { CheapManager.CheapTrait } : [];
}




public class PowerNap : Card, IHasCustomCardTraits
{
	static Spr topArt;
	static Spr bottomArt;
	public static void Register(Deck deck, IModHelper helper, IPluginPackage<IModManifest> package) {
		topArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile($"Sprites/card_art/power_nap_top.png")).Sprite;
		bottomArt = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile($"Sprites/card_art/power_nap_bottom.png")).Sprite;

		IRegisterableCard.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, ModEntry.Instance.EddieDeck,
			Rarity.common,
			null
		);
	}

	public override CardData GetData(State state) => new() {
		cost = 1,
		exhaust = upgrade == Upgrade.B,
		floppable = true,
		art = flipped ? bottomArt : topArt,
		artTint = "ffffff"
	};

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
		upgrade == Upgrade.A ? new HashSet<ICardTraitEntry>() { CheapManager.CheapTrait } : [];
}




public class PowerSink : Card
{
	public static void Register(Deck deck, IModHelper helper, IPluginPackage<IModManifest> package) {
		IRegisterableCard.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, ModEntry.Instance.EddieDeck,
			Rarity.uncommon,
			StableSpr.cards_MultiBlast
		);
	}

	public override CardData GetData(State state) => new() {
		cost = upgrade == Upgrade.B ? 1 : 0,
		exhaust = upgrade != Upgrade.A
	};

	public override List<CardAction> GetActions(State s, Combat c)
	{
		List<CardAction> result = [];

		int currentCost = this.GetCurrentCostNoRecursion(s);
		AVariableHintEnergy hint = new()
		{
			setAmount = XEnergyManager.GetEnergyAmount(s, c, this) - currentCost,
		};
		result.Add(hint);

		int multiplier = upgrade == Upgrade.B ? 3 : 2;
		result.Add(new AAttackAdjusted {
			damage = GetDmg(s, multiplier * XEnergyManager.GetEnergyAmount(s, c, this)),
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




public class RefundShot : Card, IRegisterableCard, IHasCustomCardTraits
{
	public static void Register(Deck deck, IModHelper helper, IPluginPackage<IModManifest> package) {
		IRegisterableCard.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, ModEntry.Instance.EddieDeck,
			Rarity.common,
			helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile($"Sprites/card_art/refund_shot.png")).Sprite
		);
	}

	public override CardData GetData(State state) => new() {
		cost = upgrade == Upgrade.B ? 3 : 1
	};

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
						damage = GetDmg(s, 3)
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
		upgrade == Upgrade.A ? new HashSet<ICardTraitEntry>() { CheapManager.CheapTrait } : [];
}




public class RenewableResource : Card
{
	public static void Register(Deck deck, IModHelper helper, IPluginPackage<IModManifest> package) {
		IRegisterableCard.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, ModEntry.Instance.EddieDeck,
			Rarity.rare,
			helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile($"Sprites/card_art/renewable_resource.png")).Sprite
		);
	}

	public override CardData GetData(State state) => new() {
		cost = upgrade == Upgrade.A ? 0 : 1,
		exhaust = true,
		description = ModEntry.Instance.Localizations.Localize(["card", GetType().Name, "description", upgrade.ToString()]),
	};

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




public class ReverseEngineer : Card, IRegisterableCard
{
	public static void Register(Deck deck, IModHelper helper, IPluginPackage<IModManifest> package) {
		IRegisterableCard.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, ModEntry.Instance.EddieDeck,
			Rarity.uncommon,
			StableSpr.cards_CorruptedCore,
			true
		);
	}

	public override CardData GetData(State state) => new() {
		description = ModEntry.Instance.Localizations.Localize(["card", GetType().Name, "description", upgrade.ToString()]),
		retain = true,
		cost = 0,
		temporary = true,
		exhaust = upgrade != Upgrade.B
	};

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
					browseSource = CardBrowse.Source.Hand,
					omitFromTooltips = true
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
					browseSource = CardBrowse.Source.Hand,
					omitFromTooltips = true
				}
			],
		};
	}
}




public class Rummage : Card
{
	public static void Register(Deck deck, IModHelper helper, IPluginPackage<IModManifest> package) {
		IRegisterableCard.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, ModEntry.Instance.EddieDeck,
			Rarity.common,
			StableSpr.cards_QuickThinking
		);
	}

	public override CardData GetData(State state) => new() {
		cost = 1,
		infinite = true
	};

	public override List<CardAction> GetActions(State s, Combat c) => upgrade switch {
		Upgrade.None => [
			new ADrawCard
			{
				count = 2
			}
		],
		Upgrade.A =>
			[
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




public class ShortTermSolution : Card, IHasCustomCardTraits
{
	public static void Register(Deck deck, IModHelper helper, IPluginPackage<IModManifest> package) {
		IRegisterableCard.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, ModEntry.Instance.EddieDeck,
			Rarity.uncommon,
			StableSpr.cards_ColorlessTrash
		);
	}
	public override CardData GetData(State state) => new() {
		cost = 2,
		flippable = true
	};

	public override List<CardAction> GetActions(State s, Combat c) => upgrade switch {
		Upgrade.A => [
			new AMove {
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
			new AMove {
				dir = 5,
				targetPlayer = false
			},
			new AStatus {
				status = Status.overdrive,
				statusAmount = 2,
				targetPlayer = false
			}
		],
		_ => [
			new AMove {
				dir = 3,
				targetPlayer = false
			}
		],
	};

	public IReadOnlySet<ICardTraitEntry> GetInnateTraits(State state) =>
		new HashSet<ICardTraitEntry>() { CheapManager.CheapTrait };
}




public class SolarSailing : Card
{
	public static void Register(Deck deck, IModHelper helper, IPluginPackage<IModManifest> package) {
		IRegisterableCard.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, ModEntry.Instance.EddieDeck,
			Rarity.common,
			StableSpr.cards_SolarBreeze
		);
	}

    public override CardData GetData(State state) => new() {
		cost = 0,
		flippable = upgrade == Upgrade.B,
		retain = upgrade == Upgrade.A
	};

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



public class Surge : Card
{
	public static void Register(Deck deck, IModHelper helper, IPluginPackage<IModManifest> package) {
		IRegisterableCard.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, ModEntry.Instance.EddieDeck,
			Rarity.common,
			StableSpr.cards_Overdrive,
			true
		);
	}

    public override CardData GetData(State state) => new() {
		cost = 0,
		exhaust = true,
		temporary = true
	};

    public override List<CardAction> GetActions(State s, Combat c)
    {
		return upgrade switch
		{
			Upgrade.None =>
				[
					new AAttack {
						damage = GetDmg(s, 1)
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
						damage = GetDmg(s, 2)
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
						damage = GetDmg(s, 0),
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




public class EddieExe : Card
{
	public static void Register(Deck deck, IModHelper helper, IPluginPackage<IModManifest> package) {
		IRegisterableCard.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, Deck.colorless,
			Rarity.common,
			helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile($"Sprites/card_art/exe.png")).Sprite
		);
	}

    public override CardData GetData(State state) => new() {
		cost = upgrade == Upgrade.A ? 0 : 1,
		exhaust = true,
		description = ColorlessLoc.GetDesc(state, upgrade == Upgrade.B ? 3 : 2, ModEntry.Instance.EddieDeck)
	};

    public override List<CardAction> GetActions(State s, Combat c) => [
		new ACardOffering {
			amount = upgrade == Upgrade.B ? 3 : 2,
			limitDeck = ModEntry.Instance.EddieDeck,
			makeAllCardsTemporary = true,
			overrideUpgradeChances = false,
			canSkip = false,
			inCombat = true,
			discount = -1,
			dialogueSelector = ".summonEddie"
		}
	];
}
