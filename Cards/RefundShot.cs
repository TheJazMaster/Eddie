namespace Eddie.Cards
{
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class RefundShot : CheapCard
    {
        public override string Name() => "Refund Shot";

        public override int GetCheapDiscount()
        {
            if (upgrade == Upgrade.A)
                return 1;
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
            switch (upgrade)
            {
                case Upgrade.None:
                case Upgrade.A:
                    return new List<CardAction> {
                        new AAttack {
                            damage = GetDmg(s, 1),
                            disabled = flipped
                        },
                        new ADiscard {
                            count = 1
                        },
                        new AEnergy {
                            changeAmount = 1
                        }
                    };
                case Upgrade.B:
                    return new List<CardAction> {
                        new AAttack {
                            damage = GetDmg(s, 2),
                            disabled = flipped
                        },
                        new ADiscard {
                            count = 1
                        },
                        new AEnergy {
                            changeAmount = 3
                        }
                    };
                default:
                    return new List<CardAction>();
            }
        }
    }
}