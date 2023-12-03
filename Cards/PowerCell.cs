using Eddie.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eddie.Cards
{
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class PowerCell : CheapCard
    {
        public override string Name() => "Power Cell";

        public override int GetCheapDiscount()
        {
            if (upgrade == Upgrade.A)
                return -1;
            return 0;
        }

        public override CardData GetData(State state)
        {
            base.GetData(state);
            return new CardData()
            {
				cost = upgrade == Upgrade.B ? 2 : 1
            };
        }

        public override List<CardAction> GetActions(State s, Combat c)
        {
            switch (upgrade)
            {
                case Upgrade.None:
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
                case Upgrade.A:
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
                case Upgrade.B:
                    return new List<CardAction>
                    {
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
                    };
                default:
                    return new List<CardAction>();
            }
        }
    }
}
