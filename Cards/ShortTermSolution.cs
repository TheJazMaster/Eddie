using Eddie.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eddie.Cards
{
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class ShortTermSolution : CheapCard
    {
        public override string Name() => "Short-Term Solution";

        public override int GetCheapDiscount()
        {
            return -1;
        }

        public override CardData GetData(State state)
        {
            base.GetData(state);
            return new CardData
            {
                cost = upgrade == Upgrade.A ? 3 : 2
            };
        }

        public override List<CardAction> GetActions(State s, Combat c)
        {
            switch (upgrade)
            {
                case Upgrade.None:
                    return new List<CardAction> {
                        new AAttack {
                            damage = GetDmg(s, 3)
                        },
                    };
                case Upgrade.A:
                    return new List<CardAction> {
                        new AAttack {
                            damage = GetDmg(s, 5)
                        },
                    };
                case Upgrade.B:
                    return new List<CardAction> {
                        new AAttack {
                            damage = GetDmg(s, 3),
                            disabled = flipped
                        },
                        new AAddCard {
                            card = new ShortTermSolution {
                                temporaryOverride = true
                            },
                            destination = CardDestination.Deck
                        }
                    };
                default:
                    return new List<CardAction>();
            }
        }
    }
}
