using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eddie.Actions
{
    public class CardSelectAddShortCircuitForever : CardAction
    {
        public override Route? BeginWithRoute(G g, State s, Combat c)
        {
            Card? card = selectedCard;
            if (card != null)
            {
                ShortCircuit.short_circuit_override_is_permanent.Add(card, true);
                ShortCircuit.short_circuit_override.Add(card, true);
                return new ShowCards
                {
                    messageKey = "showcards.addedRetain",
                    cardIds = new List<int> { card.uuid }
                };
            }
            return null;
        }

        public override string? GetCardSelectText(State s)
        {
            return "Select a card to add <c=cardtrait>short-circuit</c> to, forever.\n(<c=cardtrait>Short-circuit</c> cards cost 1 more energy, but their actions happen twice!)";
        }
    }
}