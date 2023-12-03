using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FSPRO;

namespace Eddie.Actions
{
    public class ADiscardPosition : CardAction
    {
        public int? count;
        public int? handPosition;

        public override void Begin(G g, State s, Combat c)
        {
            if (handPosition == null)
            {
                Audio.Play(Event.ZeroEnergy);
                return;
            }
            State s2 = s;
            List<Card> list = new List<Card>(c.hand);
            int num = 0;
            foreach (Card item in list)
            {
                if (num < handPosition)
                {
                    num++;
                    continue;
                }
                if (num >= handPosition + count) break;
                c.hand.Remove(item);
                item.waitBeforeMoving = (double)num++ * 0.05;
                item.flipped = false;
                item.OnDiscard(s2, c);
                c.SendCardToDiscard(s2, item);
            }
            if (list.Count > 0)
            {
                Audio.Play(Event.CardHandling);
            }
        }

        public override List<Tooltip> GetTooltips(State s)
        {
            if (count.HasValue)
            {
                List<Tooltip> list = new List<Tooltip>();
                list.Add(new TTGlossary("action.discardCard", count.Value));
                return list;
            }
            return new List<Tooltip>
            {
                new TTGlossary("action.discardCard")
            };
        }

        public override Icon? GetIcon(State s)
        {
            if (count.HasValue)
            {
                return new Icon(Enum.Parse<Spr>("icons_discardCard"), count.Value, Colors.textMain);
            }
            return new Icon(Spr.icons_discardCard, null, Colors.textMain);
        }
    }
}