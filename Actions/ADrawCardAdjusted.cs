using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheJazMaster.Eddie.Actions
{
    public class ADrawCardAdjusted : ADrawCard
    {
        public int countAdjustment = 0;
        public int countDisplayAdjustment = 0;

        public override void Begin(G g, State s, Combat c)
	    {
            var oldCount = count;
            count = Math.Max(0, count + countAdjustment);
            base.Begin(g, s, c);
            count = oldCount;
        }

        public override Icon? GetIcon(State s)
        {
            var oldCount = count;
            count = Math.Max(0, count + countDisplayAdjustment);
            Icon? icon = base.GetIcon(s);
            count = oldCount;
            return icon;   
        }

        public override List<Tooltip> GetTooltips(State s)
        {
            var oldCount = count;
            count = Math.Max(0, count + countDisplayAdjustment);
            List<Tooltip> tooltips = base.GetTooltips(s);
            count = oldCount;
            return tooltips;
        }
    }
}