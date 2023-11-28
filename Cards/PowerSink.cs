using Eddie.Actions;
using System;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eddie.Cards
{
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class PowerSink : Card
    {
        // int cardsPlayedBefore = 0;
        public override string Name() => "Power Sink";

        public override CardData GetData(State state)
        {
            return new CardData()
            {
                cost = upgrade == Upgrade.B ? 2 : upgrade == Upgrade.A ? 0 : 1,
                exhaust = upgrade == Upgrade.B
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

            int multiplier = (upgrade == Upgrade.B ? 3 : 2);
            int cost = GetDataWithOverrides(s).cost;
            result.Add(new AAttackAdjusted {
                damage = GetDmg(s, multiplier * Manifest.getEnergyAmount(s, c, this)),
                damageDisplayAdjustment = -cost * multiplier,
                xHint = multiplier
            });
            
            AEnergySet energy = new AEnergySet {
                setTo = 0
            };

            result.Add(energy);

            return result;
        }
    }
}
