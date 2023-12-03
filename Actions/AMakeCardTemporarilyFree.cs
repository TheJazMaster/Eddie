using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eddie.Actions
{
    public class AMakeCardTemporarilyFree : CardAction
    {
        public int handPosition = 0;

        public override void Begin(G g, State s, Combat c)
        {
            if (c.hand.Count > handPosition)
            {
                Card card = c.hand[handPosition];
                Cheap.free.Add(card, true);
            }
        }

        private string GetTooltipText()
        {
            return "for the rest of the combat";
        }

        public override List<Tooltip> GetTooltips(State s)
        {
            List<Tooltip> list = new List<Tooltip>();
            list.Add(new TTGlossary(Manifest.MakeFreeGlossary?.Head ?? throw new Exception("Missing Make Free glossary"), GetTooltipText()));
            return list;
        }

        public override Icon? GetIcon(State s)
        {
            return new Icon(Enum.Parse<Spr>("icons_discount"), null, Colors.textMain);
        }
    }
}