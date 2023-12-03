using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eddie.Actions
{
    public class AAddShortCircuitToLeftmostCard : CardAction
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
                    ShortCircuit.short_circuit_override.Add(c.hand[num], true);
                    if (permanent)
                    {
                        ShortCircuit.short_circuit_override_is_permanent.Add(c.hand[num], true);
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
            list.Add(new TTGlossary(Manifest.AddShortCircuitGlossary?.Head ?? throw new Exception("Missing Add Short-Circuit glossary"), GetTooltipText()));
            list.Add(new TTGlossary(Manifest.ShortCircuitGlossary?.Head ?? throw new Exception("Missing Short-Circuit glossary"), GetTooltipText()));
            return list;
        }

        public override Icon? GetIcon(State s)
        {
            return new Icon((Spr)(Manifest.ShortCircuitIcon?.Id ?? throw new Exception("Missing Short-Circuit Icon")), null, Colors.textMain);
        }
    }
}