using Eddie.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eddie.Cards
{
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Interference : Card
    {
        public override string Name() => "Interference";

        public override CardData GetData(State state)
        {
            return new CardData()
            {
                cost = upgrade == Upgrade.B ? 0 : 1,
                infinite = upgrade != Upgrade.B,
                recycle = upgrade == Upgrade.B,
                flippable = upgrade == Upgrade.A
            };
        }

        public override List<CardAction> GetActions(State s, Combat c)
        {
            switch (upgrade) {
                case Upgrade.None:
                case Upgrade.A:
                    return new List<CardAction> {
                        new AMoveImproved
                        {
                            dir = 1,
                            targetPlayer = false
                        }
                    };
                // case Upgrade.A:
                //     return new List<CardAction> {
                //         new AMoveImproved
                //         {
                //             dir = 2,
                //             targetPlayer = false
                //         }
                //     };
                case Upgrade.B:
                    return new List<CardAction> {
                        new AMoveImproved
                        {
                            dir = 2,
                            targetPlayer = false
                        },
                        new AStatus
                        {
                            status = Status.overdrive,
                            statusAmount = 1,
                            targetPlayer = false
                        }
                    };
                default:
                    return new List<CardAction>();
            }
        }
    }
}
