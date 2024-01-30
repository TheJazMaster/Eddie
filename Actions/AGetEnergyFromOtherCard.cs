using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FSPRO;

namespace TheJazMaster.Eddie.Actions
{
    public class AGetEnergyFromOtherCard : CardAction
    {
        public int handPosition;
        public bool exhaustThisCardAfterwards;
        public bool skipInfiniteCards;

        public override void Begin(G g, State s, Combat c)
        {
            int position = handPosition;
            while (true)
            {
                if (c.hand.Count > position)
                {
                    Card card = c.hand[position];
                    if (skipInfiniteCards && card.GetDataWithOverrides(s).infinite)
                    {
                        position++;
                        continue;
                    }
                    Manifest.TurnCardToEnergy(s, c, card, this, exhaustThisCardAfterwards);
                }
                else
                {
                    Audio.Play(Event.CardHandling);
                }
                break;
            }
        }

        public override List<Tooltip> GetTooltips(State s)
        {
            List<Tooltip> list = new List<Tooltip>();
            // list.Add(new TTGlossary("action.bypass"));
            if (exhaustThisCardAfterwards)
            {
                list.Add(new TTGlossary("cardtrait.exhaust"));
            }
            return list;
        }

        public override Icon? GetIcon(State s)
        {
            return new Icon((Spr)Enum.Parse<Spr>("icons_bypass"), handPosition, Colors.textMain);
        }
    }
}