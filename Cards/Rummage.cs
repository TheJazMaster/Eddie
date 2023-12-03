using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eddie.Cards
{
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Rummage : Card
    {
        public override string Name() => "";

        public override CardData GetData(State state)
        {
            return new CardData
            {
                cost = 1,
                infinite = true
            };
        }

        public override List<CardAction> GetActions(State s, Combat c)
        {
            switch (upgrade) {
                case Upgrade.None:
                    return new List<CardAction>
                    {
                        new ADrawCard
                        {
                            count = 2
                        }
                    };
                case Upgrade.A:
                    return new List<CardAction>
                    {
                        new ADrawCard
                        {
                            count = 3
                        }
                    };
                case Upgrade.B:
                    return new List<CardAction>
                    {
                        new ADrawCard
                        {
                            count = 4
                        },
                        new ADiscard
                        {
                            count = 2
                        }
                    };
                default:
                    return new List<CardAction>();
            }
            
        }
    }
}
