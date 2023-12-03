using Eddie.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eddie.Cards
{
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class GarageSale : Card
    {
        public override string Name() => "";

        public override CardData GetData(State state)
        {
            int cost = 2;
            if (upgrade == Upgrade.A) cost = 1;
            else if (upgrade == Upgrade.B) cost = 3;
            return new CardData
            {
                cost = cost,
                exhaust = true
            };
        }

        public override List<CardAction> GetActions(State s, Combat c)
        {
            List<CardAction> result = new List<CardAction>();
            
            result.Add(new ADiscountHand
            {
                discountAmount = -1
            });

            if (upgrade != Upgrade.B)
            {
                result.Add(new AEndTurn());
            }
            return result;
        }
    }
}
