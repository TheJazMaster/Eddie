using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheJazMaster.Eddie.Actions
{
    public class AAddInfiniteToRightmostCard : CardAction
    {
        public int howManyCards = 1;
        public bool permanent;
        public bool skipInfinite = true;

        
        public override void Begin(G g, State s, Combat c)
        {
            for (int num = c.hand.Count-1; num >= 0; num--)
            {
                var card = c.hand[num];
                if (!(skipInfinite && card.GetDataWithOverrides(s).infinite))
                {
                    if (howManyCards <= 0)
                    {
                        break;
                    }
                    howManyCards--;
                    InfiniteOverride.SetInfiniteOverride(card, true, permanent);
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
            return new Icon((Spr)Manifest.ApplyInfiniteIcon!.Id!, null, Colors.textMain);
        }
    }
}