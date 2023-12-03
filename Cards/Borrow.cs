using Eddie.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eddie.Cards
{
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Borrow : Card
    {
        public override string Name() => "";

        public override CardData GetData(State state)
        {
            return new CardData
            {
                cost = 0,
                floppable = upgrade == Upgrade.B
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
}
