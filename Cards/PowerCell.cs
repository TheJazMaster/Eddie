using Eddie.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eddie.Cards
{
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class DoubleHook : Card
    {
        public override string Name() => "Power Cell";

        public override CardData GetData(State state)
        {
            return new CardData()
            {
				cost = upgrade == Upgrade.B ? 2 : 1,
                discount = upgrade == Upgrade.A ? 1 : 0
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
                        new ASpawn
                        {
                            thing = new Midrow.PowerCell
                            {
                                yAnimation = 0.0
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
                                yAnimation = 0.0,
                                bubbleShield = true
                            }
                        }
                    };
            }

            return list;
        }
    }
}
