using Eddie.CardLogicManifest
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
        public override string Name() => "Power Sink";

        public override CardData GetData(State state)
        {
            return new CardData()
            {
                cost = upgrade == Upgrade.A ? 0 : 1,
                exhaust = upgrade == Upgrade.B
            };
        }

        public override List<CardAction> GetActions(State s, Combat c)
        {
            ConditionalWeakTable isEnergy = CardLogicManifest.isEnergy;
            ConditionalWeakTable energyAmount = CardLogicManifest.energyAmount;
            ConditionalWeakTable aEnergyMode = CardLogicManifest.aEnergyMode;

            List result = new List<CardAction>();

            AVariableHint hint = new AVariableHint();
            int amount = CardLogicManifest.getEnergyAmount(s, c, this);
            isEnergy.Add(hint, new Boolean(true));
            energyAmount.Add(hint, new Integer(amount));
            result.add(hint);


            result.add(new AAttack {
                damage = GetDmg(s, (upgrade == Upgrade.B ? 3 : 2) * amount)
            });
            
            AEnergy energy = new AEnergy {
                changeAmount = 0
            };
            aEnergyMode.Add(energy, AStatusMode.Set);

            result.add(energy);
        }
    }
}
