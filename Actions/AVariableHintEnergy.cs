using Nickel;

namespace TheJazMaster.Eddie.Actions
{
    public class AVariableHintEnergy : AVariableHint
    {
        public int setAmount = 0;
        public override Icon? GetIcon(State s) => new Icon(ModEntry.Instance.EnergyIcon, null, Colors.textMain);

        public override List<Tooltip> GetTooltips(State s) => [
			new GlossaryTooltip($"action.{GetType().Namespace}::AVariableHintEnergy") {
				Description = ModEntry.Instance.Localizations.Localize(["action", GetType().Name, "description"], new {
					Parentheses = (s.route is Combat c) ? $" </c>(<c=keyword>{setAmount}</c>)" : ""
				})
			}
		];
    }
}