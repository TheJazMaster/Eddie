using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eddie.Actions
{
    public class AStatusAdjusted : AStatus
    {
        public int amountAdjustment = 0;
        public int amountDisplayAdjustment = 0;

        public override void Begin(G g, State s, Combat c)
	    {
            statusAmount += amountAdjustment;
            base.Begin(g, s, c);
            statusAmount -= amountAdjustment;
        }
        public override Icon? GetIcon(State s)
        {
            statusAmount += amountDisplayAdjustment;
            Icon? icon = base.GetIcon(s);
            statusAmount -= amountDisplayAdjustment;
            return icon;
        }

        public override List<Tooltip> GetTooltips(State s)
        {
            var oldStatusAmount = statusAmount;
            if (xHint == null) {
                statusAmount += amountDisplayAdjustment;
            } else {
                statusAmount = 0;
            }
            List<Tooltip> tooltips = base.GetTooltips(s);
            if (xHint == null) {
                statusAmount -= amountDisplayAdjustment;
            } else {
                statusAmount += oldStatusAmount;
            }
            return tooltips;
        }
    }
}