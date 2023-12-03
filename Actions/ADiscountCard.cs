using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eddie.Actions
{
    public class ADiscountCard : CardAction
    {
        public int discountAmount = 0;
        public int uuid;

        private const double delayTimer = 0.17;
        
        public override void Begin(G g, State s, Combat c)
        {
            timer = 0.0;
            Card? card = s.FindCard(uuid);
            if (card != null && c.hand.Contains(card))
            {
                card.discount += discountAmount;
            }
        }
    }
}