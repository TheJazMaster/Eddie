using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eddie.Cards
{
    [CardMeta(rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class GammaRay : Card
    {
        public override string Name() => "Gamma Ray";

        public override CardData GetData(State state)
        {
            return new CardData
            {
                cost = upgrade == Upgrade.B ? 5 : 4,
                exhaust = true,
                retain = upgrade == Upgrade.A
            };
        }

        public override List<CardAction> GetActions(State s, Combat c)
        {
            return new List<CardAction>
            {
                new AAttack
                {
                    damage = upgrade == Upgrade.B ? 13 : 9,
                    piercing = true
                }
            };
        }
    }
}
