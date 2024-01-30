using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheJazMaster.Eddie.Actions
{
    public class AMakeCardTemporarilyFreeOncePerTurn : CardAction
    {
        
        public int howManyCards = 1;
        public bool skipInfinite = true;
        public bool permanent = false;

        public override void Begin(G g, State s, Combat c)
        {
            for (int num = 0; num < c.hand.Count; num++)
            {
                Card card = c.hand[num];
                if (!(skipInfinite && card.GetDataWithOverrides(s).infinite))
                {
                    if (howManyCards <= 0)
                    {
                        break;
                    }
                    howManyCards--;
                    
                    Cheap.SetFree(card, true, true, permanent ? true : null);
                    // Cheap.free.AddOrUpdate(card, true);
                    // Cheap.free_once_per_turn.AddOrUpdate(card, true);
                    // if (permanent)
                    //     Cheap.free_permanent.AddOrUpdate(card, true);
                }
            }
        }

        public override Icon? GetIcon(State s)
        {
            return new Icon(Enum.Parse<Spr>("icons_discount"), null, Colors.textMain);
        }
    }
}