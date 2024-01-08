using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eddie.Actions
{
    public class AAddShortCircuitToRightmostCard : CardAction
    {
        public int howManyCards = 1;
        public bool permanent;
        public bool skipInfinite = true;

        
        public override void Begin(G g, State s, Combat c)
        {
            for (int num = c.hand.Count-1; num >= 0; num--)
            {
                if (!(skipInfinite && c.hand[num].GetDataWithOverrides(s).infinite))
                {
                    if (howManyCards <= 0)
                    {
                        break;
                    }
                    howManyCards--;

                    ShortCircuit.SetShortCircuit(c.hand[num], true, permanent ? true : null);
                    // ShortCircuit.short_circuit_override.AddOrUpdate(c.hand[num], true);
                    // if (permanent)
                    // {
                    //     ShortCircuit.short_circuit_override_is_permanent.AddOrUpdate(c.hand[num], true);
                    // }
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
            return new Icon((Spr)Manifest.ApplyShortCircuitIcon!.Id!, null, Colors.textMain);
        }
    }
}