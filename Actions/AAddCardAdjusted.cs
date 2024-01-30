using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheJazMaster.Eddie.Actions
{
    public class AAddCardAdjusted : AAddCard
    {
        public int amountAdjustment = 0;
        public int amountDisplayAdjustment = 0;

        public override void Begin(G g, State s, Combat c)
	    {
            var oldAmount = amount;
            amount = Math.Max(0, amount + amountAdjustment);
            base.Begin(g, s, c);
            amount = oldAmount;
        }
        public override Icon? GetIcon(State s)
        {
            var oldAmount = amount;
            amount = Math.Max(0, amount + amountDisplayAdjustment);
            Icon? icon = base.GetIcon(s);
            amount = oldAmount;
            return icon;   
        }
    }
}