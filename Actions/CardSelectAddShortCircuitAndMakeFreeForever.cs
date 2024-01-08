using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eddie.Actions
{
    public class CardSelectAddShortCircuitAndMakeFreeForever : CardAction
    {
        public override Route? BeginWithRoute(G g, State s, Combat c)
        {
            if (selectedCard != null)
            {
                // ShortCircuit.short_circuit_override_is_permanent.AddOrUpdate(selectedCard, true);
                // ShortCircuit.short_circuit_override.AddOrUpdate(selectedCard, true);
                ShortCircuit.SetShortCircuit(selectedCard, true, true);
                Cheap.SetFree(selectedCard, true, null, true);
                // Cheap.free.AddOrUpdate(selectedCard, true);
                // Cheap.free_permanent.AddOrUpdate(selectedCard, true);
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