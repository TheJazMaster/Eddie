using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eddie.Cards
{
    [CardMeta(rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Innovation : Card
    {
        public override string Name() => "";

        public override CardData GetData(State state)
        {
            return new CardData
            {
            };
        }

        public override List<CardAction> GetActions(State s, Combat c)
        {
            return new List<CardAction>();
        }
    }
}
