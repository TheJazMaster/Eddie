using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eddie.Actions
{
    public class AAddInfiniteToLeftmostCard : CardAction
    {
        public int howManyCards = 1;
        public bool permanent;
        public bool skipInfinite = true;

        
        public override void Begin(G g, State s, Combat c)
        {
            for (int num = 0; num < c.hand.Count; num++)
            {
                if (!(skipInfinite && c.hand[num].GetDataWithOverrides(s).infinite))
                {
                    if (howManyCards <= 0)
                    {
                        break;
                    }
                    howManyCards--;
                    InfiniteOverride.infinite_override.Add(c.hand[num], true);
                    if (permanent)
                    {
                        InfiniteOverride.infinite_override_is_permanent.Add(c.hand[num], true);
                    }
                }
            }
        }

        private string GetTooltipText()
        {
            if (permanent)
            {
                return "forever";
            }
            return "for the rest of the combat";
        }

        public override List<Tooltip> GetTooltips(State s)
        {
            List<Tooltip> list = new List<Tooltip>();
            list.Add(new TTGlossary(Manifest.AddInfiniteGlossary?.Head ?? throw new Exception("Missing Add Infinite glossary"), GetTooltipText()));
            return list;
        }

        public override Icon? GetIcon(State s)
        {
            return new Icon(Enum.Parse<Spr>("icons_infinite"), null, Colors.textMain);
        }
    }
}