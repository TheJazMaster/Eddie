using Microsoft.Xna.Framework.Graphics;
using static System.Linq.Enumerable;

namespace Eddie.Midrow
{
    public class PowerCell : StuffBase
    {
        public override Spr? GetIcon()
        {
            if (Manifest.PowerCellIcon != null && Manifest.PowerCellIcon.Id != null)
                return (Spr?)Manifest.PowerCellIcon.Id.Value;
            return null;
        }

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

        public override List<Tooltip> GetTooltips()
        {
            List<Tooltip> list = new List<Tooltip> {
                new TTGlossary(Manifest.PowerCellGlossary?.Head ?? throw new Exception("Missing Power Cell glossary"))
            };
            if (bubbleShield)
            {
                list.Add(new TTGlossary("midrow.bubbleShield"));
            }
            return list;
        }

        public override void Render(G g, Vec v)
        {
            if (Manifest.PowerCellSprite != null && Manifest.PowerCellSprite.Id != null)
                DrawWithHilight(g, (Spr)Manifest.PowerCellSprite.Id.Value, v + GetOffset(g), Mutil.Rand((double)x + 0.1) > 0.5, Mutil.Rand((double)x + 0.2) > 0.5);
        }

        public override List<CardAction>? GetActionsOnDestroyed(State s, Combat c, bool wasPlayer, int worldX)
        {
            return new List<CardAction>
            {
                new AEnergy
                {
                    changeAmount = 1
                }
            };
        }
    }
}