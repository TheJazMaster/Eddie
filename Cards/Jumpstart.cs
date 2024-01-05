using Eddie.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eddie.Cards
{
    [CardMeta(rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Jumpstart : Card
    {
        public override string Name() => "";

        public override CardData GetData(State state)
        {
            return new CardData
            {
                cost = 0,
                buoyant = upgrade == Upgrade.B,
                exhaust = true
            };
        }

        public override List<CardAction> GetActions(State s, Combat c)
        {
            List<CardAction> result = new List<CardAction>();

            int cost = this.GetCurrentCostNoRecursion(s);
            AVariableHintEnergy hint = new AVariableHintEnergy
            {
                setAmount = Manifest.getEnergyAmount(s, c, this) - cost
            };
            result.Add(hint);

            result.Add(new ADrawCardAdjusted {
                count = Manifest.getEnergyAmount(s, c, this),
                countDisplayAdjustment = -cost,
                xHint = 1
            });

            if (upgrade == Upgrade.A)
                result.Add(new ADrawCard
                {
                    count = 2
                });

            return result;
        }
    }
}
