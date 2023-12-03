using Eddie.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eddie.Cards
{
    [CardMeta(dontOffer = true, rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Repurpose : Card
    {
        public override string Name() => "Repurpose";

        public override CardData GetData(State state)
        {
            var default_desc = "<c=cardtrait>Exhaust</c> the leftmost non-<c=cardtrait>infinite</c> card and gain its cost as <c=energy>ENERGY</c>.";
            var description = upgrade switch
            {
                Upgrade.None => default_desc,
                Upgrade.A => default_desc,
                Upgrade.B => "Choose a card. Discard it and gain its cost as <c=energy>ENERGY</c>.",
                _         => ""
            };
            return new CardData
            {
                description = description,
                retain = upgrade == Upgrade.A,
                cost = upgrade == Upgrade.B ? 1 : 0,
                exhaust = true
            };
        }

        public override List<CardAction> GetActions(State s, Combat c)
        {
            switch (upgrade)
            {
                case Upgrade.None:
                    return new List<CardAction>
                    {
                        new ADelay
                        {
                            time = -0.5
                        },
                        new AGetEnergyFromOtherCard
                        {
                            handPosition = 0,
                            timer = 0.5,
                            skipInfiniteCards = true,
                            exhaustThisCardAfterwards = false//true
                        }
                    };
                case Upgrade.A:
                    return new List<CardAction>
                    {
                        new ADelay
                        {
                            time = -0.5
                        },
                        new AGetEnergyFromOtherCard
                        {
                            handPosition = 0,
                            timer = 0.5,
                            skipInfiniteCards = true,
                            exhaustThisCardAfterwards = false//true
                        }
                    };
                case Upgrade.B:
                    return new List<CardAction>
                    {
                        new ADelay
                        {
                            time = -0.5
                        },
                        new ACardSelect
                        {
                            browseAction = new AGetEnergyFromChosenCard
                            {
                                exhaustThisCardAfterwards = false
                            },
                            browseSource = CardBrowse.Source.Hand
                        }
                    };
                default:
                    return new List<CardAction>();
            }
        }

        public override void HilightOtherCards(State s, Combat c)
        {
            Card? card = c.hand.Where((Card c) => c != this).FirstOrDefault();
            if (card != null)
            {
                c.hilightedCards.Add(card.uuid);
            }
        }
    }
}
