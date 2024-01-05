using Eddie.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eddie.Cards
{
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class ChargeCannons : Card
    {
        public override string Name() => "Charge Cannons";

        public override CardData GetData(State state)
        {
            return new CardData
            {
                cost = 1,
                exhaust = true
            };
        }

        public override List<CardAction> GetActions(State s, Combat c)
        {
            List<CardAction> result = new List<CardAction>();

            var currentCost = this.GetCurrentCostNoRecursion(s);
            result.Add(new AVariableHintEnergy
            {
                setAmount = Manifest.getEnergyAmount(s, c, this) - currentCost
            });

            result.Add(new AStatusAdjusted
            {
                targetPlayer = true,
                status = Status.overdrive,
                statusAmount = Manifest.getEnergyAmount(s, c, this),
                amountDisplayAdjustment = -currentCost,
                xHint = 1
            });
            
            result.Add(new AEnergySet {
                setTo = upgrade == Upgrade.B ? 2 : 1
            });
            
            if (upgrade == Upgrade.A)
            {
                result.Add(new AStatus
                {
                    targetPlayer = true,
                    status = Status.stunCharge,
                    statusAmount = 1
                });
            }

            return result;
        }
    }
}
