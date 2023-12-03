using Eddie.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eddie.Cards
{
    [CardMeta(rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class RenewableResource : Card
    {
        public override string Name() => "";

        public override CardData GetData(State state)
        {
            return new CardData
            {
                cost = upgrade == Upgrade.A ? 0 : 1,
                exhaust = upgrade != Upgrade.B,
                description = "Leftmost non-<c=cardtrait>infinite</c> card gains <c=cardtrait>infinite</c> and <c=cardtrait>short-circuit</c>."
            };
        }

        public override List<CardAction> GetActions(State s, Combat c)
        {
            return new List<CardAction>
            {
                new AAddShortCircuitToLeftmostCard {
                    skipInfinite = true
                },
                new AAddInfiniteToLeftmostCard {
                    skipInfinite = true
                }
            };
        }

        public override void HilightOtherCards(State s, Combat c)
        {
            Card? card = c.hand.Where((Card c) => c != this && !c.GetDataWithOverrides(s).infinite).FirstOrDefault();
            if (card != null)
            {   
                c.hilightedCards.Add(card.uuid);
            }
        }
    }
}
