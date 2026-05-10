
namespace TheJazMaster.Eddie.Actions
{
    public class CardSelectAddShortCircuitAndMakeCheaperForever : CardAction
    {
        public int howMuch = 1;
        
        public override Route? BeginWithRoute(G g, State s, Combat c)
        {
            if (selectedCard != null)
            {
                ShortCircuitManager.SetShortCircuit(s, selectedCard, true, true);
                CheapManager.SetFree(selectedCard, null, null, howMuch);
                return new CustomShowCards
                {
                    message = ModEntry.Instance.Localizations.Localize(["action", GetType().Name, "showCards"]),
                    cardIds = [selectedCard.uuid]
                };
            }
            return null;
        }

        public override string? GetCardSelectText(State s) => ModEntry.Instance.Localizations.Localize(["action", GetType().Name, "cardSelect"]);
    }
}