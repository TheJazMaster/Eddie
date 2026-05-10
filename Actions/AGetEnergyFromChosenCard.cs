namespace TheJazMaster.Eddie.Actions
{
    public class AGetEnergyFromChosenCard : CardAction
    {
        public bool exhaustThisCardAfterwards;

        public override void Begin(G g, State s, Combat c)
        {
            Card? card = selectedCard;
            XEnergyManager.TurnCardToEnergy(s, c, card, this, exhaustThisCardAfterwards);
        }

        public override List<Tooltip> GetTooltips(State s)
        {
            List<Tooltip> list = [];
            // list.Add(new TTGlossary("action.bypass"));
            if (exhaustThisCardAfterwards)
            {
                list.Add(new TTGlossary("cardtrait.exhaust"));
            }
            return list;
        }

        public override Icon? GetIcon(State s)
        {
            return new Icon(StableSpr.icons_bypass, null, Colors.textMain);
        }
    }
}