using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheJazMaster.Eddie.Actions
{
    public class CardSelectAddShortCircuitAndMakeCheaperForever : CardAction
    {
        int howMuch = 1;
        
        public override Route? BeginWithRoute(G g, State s, Combat c)
        {
            if (selectedCard != null)
            {
                ShortCircuit.SetShortCircuit(selectedCard, true, true);
                Cheap.SetFree(selectedCard, null, null, howMuch);
                return new ShowCards
                {
                    messageKey = "showcards.addedShortCircuit",
                    cardIds = new List<int> { selectedCard.uuid }
                };
            }
            return null;
        }

        public override string? GetCardSelectText(State s)
        {
            return "Select a card to make free but <c=downside>add <c=cardtrait>short-circuit</c> to</c>, forever.";
        }
    }
}