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
                Cheap.SetFree(card, true, true, false);
                // Cheap.free.AddOrUpdate(card, true);
				// Cheap.free_once_per_turn.AddOrUpdate(card, true);
				
                c.hand.Remove(card);
                card.flipped = false;
                card.OnDiscard(s, c);
                c.SendCardToDiscard(s, card);
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