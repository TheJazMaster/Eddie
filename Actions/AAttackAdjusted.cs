using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheJazMaster.Eddie.Actions
{
    public class AAttackAdjusted : AAttack
    {
        public int damageAdjustment = 0;
        public int damageDisplayAdjustment = 0;

        public override void Begin(G g, State s, Combat c)
	    {
            var oldDamage = damage;
            damage = Math.Max(0, damage + damageAdjustment);
            base.Begin(g, s, c);
            damage = oldDamage;
        }
        public override Icon? GetIcon(State s)
        {
            var oldDamage = damage;
            damage = Math.Max(0, damage + damageDisplayAdjustment);
            Icon? icon = base.GetIcon(s);
            damage = oldDamage;
            return icon;   
        }
    }
}