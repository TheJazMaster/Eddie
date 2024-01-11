using Eddie.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eddie.Cards
{
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class ChargeShields : Card
    {
        public override string Name() => "Charge Shields";

        public override CardData GetData(State state)
        {
            return new CardData
            {
                cost = upgrade == Upgrade.B ? 0 : 1,
                exhaust = true
            };
        }

        public override List<CardAction> GetActions(State s, Combat c)
        {
            var currentCost = this.GetCurrentCostNoRecursion(s);
            return new List<CardAction>
            {
                new AVariableHintEnergy
                {
                    setAmount = Manifest.GetEnergyAmount(s, c, this) - currentCost,
                },
                new AStatusAdjusted
                {
                    targetPlayer = true,
                    status = upgrade == Upgrade.A ? Status.maxShield : Status.tempShield,
                    statusAmount = Manifest.GetEnergyAmount(s, c, this),
                    amountDisplayAdjustment = -currentCost,
                    xHint = 1
                },
                new AStatusAdjusted
                {
                    targetPlayer = true,
                    status = Status.shield,
                    statusAmount = Manifest.GetEnergyAmount(s, c, this),
                    amountDisplayAdjustment = -currentCost,
                    xHint = 1
                },
                new AEnergySet {
                    setTo = 0
                }
            };
        }
    }
}
