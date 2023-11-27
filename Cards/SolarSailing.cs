using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eddie.Cards
{
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class SolarSailing : Card
    {
        public override string Name() => "Solar Sailing";

        public override CardData GetData(State state)
        {
            return new CardData
            {
                cost = 0,
                flippable = upgrade == Upgrade.A
            };
        }

        public override List<CardAction> GetActions(State s, Combat c)
        {
            switch (upgrade)
            {
                case Upgrade.None:
                case Upgrade.A:
                    return new List<CardAction>
                    {
                        new AMove
                        {
                            dir = 1,
                            targetPlayer = true
                        },
                        new ADraw
                        {
                            amount = 1
                        }
                    };
                case Upgrade.B:
                    return new List<CardAction>
                    {
                        new AMove
                        {
                            dir = 2,
                            isRandom = true,
                            targetPlayer = true
                        },
                        new ADraw
                        {
                            amount = 1
                        }
                    };
            }
            return result;
        }
    }
}
