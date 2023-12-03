using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eddie.Cards
{
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Channel : Card
    {
        public override string Name() => "Channel";

        public override CardData GetData(State state)
        {
            return new CardData()
            {
                cost = 1,
                // discount = upgrade == Upgrade.A ? 1 : 0,
                floppable = upgrade == Upgrade.B,
                buoyant = upgrade == Upgrade.B,
                retain = upgrade == Upgrade.B,
                infinite = true,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c)
        {
            switch(upgrade)
            {
                case Upgrade.None:
                    return new List<CardAction> {
                        // new AAttack {
                        //     damage = GetDmg(s, 1)
                        // },
                        new AStatus {
                            status = Status.shield,
                            statusAmount = 1,
                            targetPlayer = true,
                            disabled = flipped
                        },
                        new ADrawCard {
                            count = 1
                        }
                    };
                case Upgrade.A:
                    return new List<CardAction> {
                        // new AAttack {
                        //     damage = GetDmg(s, 1)
                        // },
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
                        // new AAttack {
                        //     damage = GetDmg(s, 1),
                        //     disabled = flipped
                        // },
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
}
