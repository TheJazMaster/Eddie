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
                cost = upgrade == Upgrade.B ? 0 : 1,
                buoyant = upgrade == Upgrade.A,
                exhaust = true
            };
        }

        public override List<CardAction> GetActions(State s, Combat c)
        {
            List<CardAction> result = new List<CardAction>();

            AVariableHintEnergy hint = new AVariableHintEnergy
            {
                setAmount = Manifest.getEnergyAmount(s, c, this)
            };
            result.Add(hint);

            int cost = GetCurrentCost(s);
            result.Add(new ADrawCardAdjusted {
                count = Manifest.getEnergyAmount(s, c, this),
                countDisplayAdjustment = -cost,
                xHint = 1
            });

            if (upgrade != Upgrade.B)
                result.Add(new ADrawCard
                {
                    count = 1
                });

            return result;
        }
    }
}
