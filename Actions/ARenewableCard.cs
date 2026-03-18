using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheJazMaster.Eddie.Actions
{
    public class ARenewableCard : CardAction
    {
        public bool permanent;

        public override void Begin(G g, State s, Combat c)
        {
            Card? card = selectedCard;
			if (card != null)
			{
                ShortCircuit.SetShortCircuit(s, card, true, permanent);
                Manifest.Helper.Content.Cards.SetCardTraitOverride(s, card, Manifest.Helper.Content.Cards.InfiniteCardTrait, true, permanent);
            }
        }

        public override string? GetCardSelectText(State s) => Manifest.MakeRenewableGlossary?.Head;
        
        public override Icon? GetIcon(State s)
        {
            return new Icon(StableSpr.icons_infinite, null, Colors.textMain);
        }
    }
}