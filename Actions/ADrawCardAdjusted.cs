using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eddie.Actions
{
    public class ADrawCardAdjusted : ADrawCard
    {
        public int countAdjustment = 0;
        public int countDisplayAdjustment = 0;

        public override void Begin(G g, State s, Combat c)
	    {
            count += countAdjustment;
            base.Begin(g, s, c);
            count -= countAdjustment;
        }

        public override Icon? GetIcon(State s)
        {
            count += countDisplayAdjustment;
            Icon? icon = base.GetIcon(s);
            count -= countDisplayAdjustment;
            return icon;   
        }

        public override List<Tooltip> GetTooltips(State s)
        {
            count += countDisplayAdjustment;
            List<Tooltip> tooltips = base.GetTooltips(s);
            count -= countDisplayAdjustment;
            return tooltips;
        }
    }
}