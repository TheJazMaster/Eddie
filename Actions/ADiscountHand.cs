using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eddie.Actions
{
    public class ADiscountHand : CardAction
    {
        public int discountAmount = -1;
        
        public override void Begin(G g, State s, Combat c)
        {
            // timer = 0.0;
            foreach (Card item in c.hand)
            {
                c.Queue(new ADiscountCard
                {
                    discountAmount = discountAmount,
                    uuid = item.uuid
                });
            }
        }

        public override Icon? GetIcon(State s)
        {
            return new Icon(Enum.Parse<Spr>("icons_discount"), null, Colors.textMain);
        }

        public override List<Tooltip> GetTooltips(State s)
        {
			List<Tooltip> list = new List<Tooltip>();
            if (discountAmount < 0) {
                list.Add(new TTGlossary((Manifest.DiscountHandGlossary?.Head) ?? throw new Exception("Missing discount hand glossary"), Math.Abs(discountAmount)));
                list.Add(new TTGlossary("cardtrait.discount", Math.Abs(discountAmount)));
            } else {
                list.Add(new TTGlossary((Manifest.ExpensiveHandGlossary?.Head) ?? throw new Exception("Missing expensive hand glossary"), discountAmount));
                list.Add(new TTGlossary("cardtrait.expensive", discountAmount));
            }
			return list;
		}
    }
}