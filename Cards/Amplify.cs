using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eddie.Cards
{
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Amplify : Card
    {
        public override string Name() => "Amplify";

        public override CardData GetData(State state)
        {
            return new CardData
            {
                cost = upgrade == Upgrade.B ? 3 : 2,
                exhaust = true
            };
        }

        public override List<CardAction> GetActions(State s, Combat c)
        {
            Status lose_energy_status = (Status)(Manifest.LoseEnergyEveryTurnStatus?.Id ?? throw new Exception("Missing Lose Energy Status"));
            List<CardAction> result = new List<CardAction>
            {
                new AStatus
                {
                    targetPlayer = true,
                    status = Status.powerdrive,
                    statusAmount = upgrade == Upgrade.B ? 2: 1
                },
                new AStatus
                {
                    targetPlayer = true,
                    status = lose_energy_status,
                    statusAmount = upgrade == Upgrade.B ? 2: 1
                }
            };

            if (upgrade == Upgrade.A)
                result.Add(new AStatus
                {
                    targetPlayer = true,
                    status = Status.energyNextTurn,
                    statusAmount = 1
                });

            return result;
        }
    }
}
