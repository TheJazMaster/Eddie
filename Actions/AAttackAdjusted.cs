using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eddie.Actions
{
    public class AAttackAdjusted : AAttack
    {
        public int damageAdjustment = 0;
        public int damageDisplayAdjustment = 0;

        public override void Begin(G g, State s, Combat c)
	    {
            damage += damageAdjustment;
            base.Begin(g, s, c);
            damage -= damageAdjustment;
        }
        public override Icon? GetIcon(State s)
        {
            damage += damageDisplayAdjustment;
            Icon? icon = base.GetIcon(s);
            damage -= damageDisplayAdjustment;
            return icon;   
        }
    }
}