using Eddie.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eddie.Cards
{
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class ChargeThrusters : Card
    {
        public override string Name() => "Charge Thrusters";

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
                setAmount = Manifest.GetEnergyAmount(s, c, this) - currentCost,
            });

            int multiplier = upgrade == Upgrade.None ? 1 : 2;
            result.Add(new AStatusAdjusted
            {
                targetPlayer = true,
                status = Status.evade,
                statusAmount = multiplier * Manifest.GetEnergyAmount(s, c, this),
                amountDisplayAdjustment = -multiplier * currentCost,
                xHint = multiplier
            });
            
            result.Add(new AEnergySet {
                setTo = upgrade == Upgrade.A ? 0 : 1
            });
            
            if (upgrade == Upgrade.B)
            {
                result.Add(new AStatus
                {
                    targetPlayer = true,
                    status = Status.loseEvadeNextTurn,
                    statusAmount = 1
                });
            }

            return result;
        }
    }
}
