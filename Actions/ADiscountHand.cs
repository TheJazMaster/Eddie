using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nickel;

namespace TheJazMaster.Eddie.Actions
{
    public class ADiscountHand : DynamicWidthCardAction
    {   
        public override void Begin(G g, State s, Combat c)
        {
            foreach (Card item in c.hand)
            {
                c.QueueImmediate(new ADiscountCard
                {
                    discountAmount = -1,
                    uuid = item.uuid
                });
            }
        }

        public override Icon? GetIcon(State s) => new Icon(ModEntry.Instance.DiscountHandIcon, null, Colors.textMain);
        
        public override List<Tooltip> GetTooltips(State s) => [
			new GlossaryTooltip($"action.{GetType().Namespace}::{GetType().Name}") {
                Icon = ModEntry.Instance.DiscountHandIcon,
                IsWideIcon = true,
                TitleColor = Colors.action,
                Title = ModEntry.Instance.Localizations.Localize(["action", GetType().Name, "name"]),
				Description = ModEntry.Instance.Localizations.Localize(["action", GetType().Name, "description"])
			}
		];
    }
}