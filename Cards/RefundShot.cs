namespace Eddie.Cards
{
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class ClusterRocket : Card
    {
        public override string Name() => "Refund Shot";

        public override CardData GetData(State state)
        {
            return new CardData
            {
                cost = upgrade == Upgrade.B ? 2 : 1,
                discount = upgrade == Upgrade.A ? 1 : 0
            };
        }

        public override List<CardAction> GetActions(State s, Combat c)
        {
            var result = new List<CardAction>();
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
                            amount = 1
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
                            amount = 1
                        },
                        new AEnergy {
                            changeAmount = 2
                        }
                    };
            }
            return result;
        }
    }
}