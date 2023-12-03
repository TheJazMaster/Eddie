using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eddie.Actions
{
    public class ADiscountHand : CardAction
    {
        public int discountAmount = 0;
        
        public override void Begin(G g, State s, Combat c)
        {
            // timer = 0.0;
            foreach (Card item in c.hand)
            {
                c.Queue(new ADiscountCard
                {
                    discountAmount = discountAmount,
                    uuid = item.uuid
                });
            }
        }
    }
}