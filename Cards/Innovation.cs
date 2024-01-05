using Eddie.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eddie.Cards
{
    [CardMeta(rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Innovation : Card
    {
        public override string Name() => "Innovation";

        public override CardData GetData(State state)
        {
            string description = upgrade switch {
                Upgrade.None => "Leftmost non-<c=cardtrait>infinite</c> card costs 0 <c=energy>ENERGY</c> once per turn.",
                Upgrade.A => "Leftmost non-<c=cardtrait>infinite</c> card costs 0 <c=energy>ENERGY</c> once per turn.",
                Upgrade.B => "Non-<c=cardtrait>infinite</c> card costs 0 <c=energy>ENERGY</c> once per turn, discard it",
                // Upgrade.None => "Leftmost non-<c=cardtrait>infinite</c> card costs 0 <c=energy>ENERGY</c> for the rest of the combat.",
                // Upgrade.A => "Leftmost non-<c=cardtrait>infinite</c> card costs 0 <c=energy>ENERGY</c> for the rest of the combat.",
                // Upgrade.B => "Non-<c=cardtrait>infinite</c> card costs 0 <c=energy>ENERGY</c> for the rest of combat, discard it.",
                _ => ""
            };
            return new CardData
            {
                cost = upgrade == Upgrade.B ? 0 : upgrade == Upgrade.A ? 2 : 3,
                exhaust = true,
                description = description
            };
        }

        public override List<CardAction> GetActions(State s, Combat c)
        {
            switch (upgrade) {
                case Upgrade.None:
                case Upgrade.A:
                    return new List<CardAction>{
                        new AMakeCardTemporarilyFreeOncePerTurn()
                    };
                case Upgrade.B:
                    return new List<CardAction>{
                        new ADelay{
                            time = -0.5
                        },
                        new ACardSelect {
                            browseAction = new AChooseCardMakeFreeOncePerTurnAndDiscard(),
                            browseSource = CardBrowse.Source.Hand
                        }
                    };
                default:
                    return new List<CardAction>();
            }
        }
    }
}
