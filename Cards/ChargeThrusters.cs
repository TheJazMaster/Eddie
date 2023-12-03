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

            result.Add(new AVariableHintEnergy
            {
                setAmount = Manifest.getEnergyAmount(s, c, this)
            });

            result.Add(new AStatusAdjusted
            {
                targetPlayer = true,
                status = Status.overdrive,
                statusAmount = Manifest.getEnergyAmount(s, c, this),
                amountDisplayAdjustment = -GetCurrentCost(s),
                xHint = 1
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
