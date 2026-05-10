using Microsoft.Xna.Framework.Graphics;
using Nickel;
using static System.Linq.Enumerable;

namespace TheJazMaster.Eddie.Features
{
    public class PowerCell : StuffBase
    {
        public override Spr? GetIcon() => ModEntry.Instance.PowerCellIcon;

        public override string GetDialogueTag()
        {
            return "powerCell";
        }

        public override double GetWiggleAmount()
        {
            return 1.0;
        }

        public override double GetWiggleRate()
        {
            return 1.0;
        }

        public override List<Tooltip> GetTooltips() => [
            new GlossaryTooltip($"midrow.{GetType().Namespace}::PowerCell") {
                Icon = GetIcon(),
                TitleColor = Colors.midrow,
                Title = ModEntry.Instance.Localizations.Localize(["midrow", "PowerCell", "name"]),
                Description = ModEntry.Instance.Localizations.Localize(["midrow", "PowerCell", "description"])
            }
        ];

        public override void Render(G g, Vec v)
        {
            DrawWithHilight(g, ModEntry.Instance.PowerCellSprite, v + GetOffset(g), Mutil.Rand(x + 0.1) > 0.5, Mutil.Rand(x + 0.2) > 0.5);
        }

        public override List<CardAction>? GetActionsOnDestroyed(State s, Combat c, bool wasPlayer, int worldX) => [
            new AEnergy {
                changeAmount = 1
            }
        ];
    }
}