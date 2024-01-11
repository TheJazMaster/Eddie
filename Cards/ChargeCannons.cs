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
            string description = upgrade switch {
                Upgrade.A => "X = <c=energy>ENERGY</c>\nAdd X <c=card>Surge A</c>s to your draw pile, lose all <c=energy>ENERGY</c>.",
                // Upgrade.A => "Spend all energy, add 1 <c=card>Surge A</c> to your draw pile for each.",
                Upgrade.B => "X = <c=energy>ENERGY</c>\nAdd X <c=card>Surge B</c>s to your draw pile, lose all <c=energy>ENERGY</c>.",
                // Upgrade.B => "Spend all energy, add 1 <c=card>Surge B</c> to your draw pile for each.",
                _ => "X = <c=energy>ENERGY</c>\nAdd X <c=card>Surge</c>s to your draw pile, lose all <c=energy>ENERGY</c>.",
                // _ => "Spend all energy, add 1 <c=card>Surge</c> to your draw pile for each."
            };
            return new CardData
            {
                cost = 0,
                exhaust = true,
                description = description
            };
        }

        public override List<CardAction> GetActions(State s, Combat c)
        {
            List<CardAction> result = new List<CardAction>();

            var currentCost = this.GetCurrentCostNoRecursion(s);
            result.Add(new AVariableHintEnergy
            {
                setAmount = Manifest.GetEnergyAmount(s, c, this) - currentCost
            });

            // result.Add(new AStatusAdjusted
            // {
            //     targetPlayer = true,
            //     status = Status.overdrive,
            //     statusAmount = Manifest.GetEnergyAmount(s, c, this),
            //     amountDisplayAdjustment = -currentCost,
            //     xHint = 1
            // });

            result.Add(new AAddCardAdjusted
            {
                card = new Surge {
                    upgrade = upgrade
                },
                amount = Manifest.GetEnergyAmount(s, c, this) - currentCost,
                amountDisplayAdjustment = -currentCost,
                destination = CardDestination.Deck,
                xHint = 1
            });
            
            result.Add(new AEnergySet {
                setTo = 0
            });

            return result;
        }
    }
}
