using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheJazMaster.Eddie.Actions
{
    public class AChooseCardMakeFreeOncePerTurnAndDiscard : CardAction
    {
        public int handPosition = 0;

        public override void Begin(G g, State s, Combat c)
        {
            Card? card = selectedCard;
			if (card != null)
			{
                CheapManager.SetFree(card, true, true, null);
                // Cheap.free.AddOrUpdate(card, true);
				// Cheap.free_once_per_turn.AddOrUpdate(card, true);
				
                c.hand.Remove(card);
                card.flipped = false;
                card.OnDiscard(s, c);
                c.SendCardToDiscard(s, card);
            }
        }

        public override string? GetCardSelectText(State s) => 
            ModEntry.Instance.Localizations.Localize(["card", "Innovation", "cardSelectText", "discard"]);

        public override Icon? GetIcon(State s)
        {
            return new Icon(StableSpr.icons_discount, null, Colors.textMain);
        }
    }
}