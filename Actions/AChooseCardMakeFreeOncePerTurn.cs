using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheJazMaster.Eddie.Actions
{
    public class AChooseCardMakeFreeOncePerTurn : CardAction
    {
        public int handPosition = 0;

        public override void Begin(G g, State s, Combat c)
        {
            Card? card = selectedCard;
			if (card != null)
			{
                Cheap.SetFree(card, true, true, null);
            }
        }

        public override string? GetCardSelectText(State s) => Manifest.MakeFreeGlossary?.Head;
        
        private string GetTooltipText()
        {
            return "for the rest of the combat";
        }

        public override Icon? GetIcon(State s)
        {
            return new Icon(StableSpr.icons_discount, null, Colors.textMain);
        }
    }
}