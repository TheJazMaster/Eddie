using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eddie.Actions
{
    public class QueueCardActions : CardAction
    {
		public QueueCardActions() {
			timer = 0.0;
		}

        public override void Begin(G g, State s, Combat c)
        {
            Card? card = selectedCard;
			if (card != null)
            	c.QueueImmediate(card.GetActionsOverridden(s, c));
        }
    }
}