using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eddie.Cards
{
    [CardMeta(rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Circuit : Card
    {
        public override string Name() => "";

        public override CardData GetData(State state)
        {
            return new CardData
            {
                cost = upgrade switch
                {
                    Upgrade.None => 3,
                    Upgrade.A => 2,
                    Upgrade.B => 4,
                    _ => 3
                },
                exhaust = true
            };
        }

        public override List<CardAction> GetActions(State s, Combat c)
        {
            return new List<CardAction>
            {
                new AStatus
                {
                    status = (Status)(Manifest.CircuitStatus?.Id ?? throw new Exception("Missing CircuitStatus")),
                    statusAmount = upgrade == Upgrade.B ? 2 : 1,
                    targetPlayer = true
                }
            };
        }
    }
}
