namespace TheJazMaster.Eddie.Actions
{
    public class AChooseCardMakeFreeOncePerTurn : CardAction
    {
        public int handPosition = 0;

        public override void Begin(G g, State s, Combat c)
        {
			if (selectedCard != null) {
                CheapManager.SetFree(selectedCard, true, true, null);
            }
        }

        public override string? GetCardSelectText(State s) => 
            ModEntry.Instance.Localizations.Localize(["card", "Innovation", "cardSelectText", "normal"]);

        public override Icon? GetIcon(State s)
        {
            return new Icon(StableSpr.icons_discount, null, Colors.textMain);
        }
    }
}