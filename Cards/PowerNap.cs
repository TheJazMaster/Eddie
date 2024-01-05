using Eddie.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eddie.Cards
{
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class PowerNap : CheapCard
    {
        public override string Name() => "Power Nap";

        public override int GetCheapDiscount()
        {
            if (upgrade == Upgrade.A)
                return -1;
            return 0;
        }
        
        public override CardData GetData(State state)
        {
            base.GetData(state);
            return new CardData
            {
                cost = 1,
                exhaust = upgrade == Upgrade.B,
                floppable = true,
		        art = (flipped ? (Spr)Manifest.PowerNapBottomCardArt!.Id! : (Spr)Manifest.PowerNapTopCardArt!.Id!),
		        artTint = "ffffff"
            };
        }

        public override List<CardAction> GetActions(State s, Combat c)
        {
            switch (upgrade)
            {
                case Upgrade.None:
                case Upgrade.A:
                    return new List<CardAction>
                    {
                        new AStatus {
                            status = Status.energyNextTurn,
                            statusAmount = 1,
                            targetPlayer = true,
                            disabled = flipped
                        },
                        new AStatus {
                            status = Status.drawNextTurn,
                            statusAmount = 1,
                            targetPlayer = true,
                            disabled = flipped
                        },
                        new ADummyAction(),
                        new AEnergy {
                            changeAmount = 1,
                            disabled = !flipped
                        },
                        new ADrawCard {
                            count = 1,
                            disabled = !flipped
                        }
                    };
                case Upgrade.B:
                    return new List<CardAction>
                    {
                        new AStatus {
                            status = Status.energyNextTurn,
                            statusAmount = 2,
                            targetPlayer = true,
                            disabled = flipped
                        },
                        new AStatus {
                            status = Status.drawNextTurn,
                            statusAmount = 2,
                            targetPlayer = true,
                            disabled = flipped
                        },
                        new ADummyAction(),
                        new AEnergy {
                            changeAmount = 2,
                            disabled = !flipped
                        },
                        new ADrawCard {
                            count = 2,
                            disabled = !flipped
                        }
                    };
                default:
                    return new List<CardAction>();
            }
        }
    }
}
