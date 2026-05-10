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
                ShortCircuitManager.SetShortCircuit(s, card, true, permanent);
                ModEntry.Instance.Helper.Content.Cards.SetCardTraitOverride(s, card, ModEntry.Instance.Helper.Content.Cards.InfiniteCardTrait, true, permanent);
            }
        }

        public override string? GetCardSelectText(State s) => ModEntry.Instance.Localizations.Localize(["card", "RenewableResource", "cardSelectText"]);
    }
}