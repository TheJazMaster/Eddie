using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eddie.Cards
{
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class EnergyBolt : Card
    {
        public override string Name() => "Energy Bolt";

        public override CardData GetData(State state)
        {
            return new CardData()
            {
                cost = 1
            };
        }

        public override List<CardAction> GetActions(State s, Combat c)
        {
            switch(upgrade) {
                case Upgrade.None:
                    return new List<CardAction> {
                        new AAttack {
                            damage = GetDmg(s, 2),
                            piercing = true,
                            status = Status.tempShield,
                            statusAmount = 1
                        }
                    };
                case Upgrade.A:
                    return new List<CardAction> {
                        new AAttack {
                            damage = GetDmg(s, 3),
                            piercing = true,
                            status = Status.tempShield,
                            statusAmount = 1
                        }
                    };
                case Upgrade.B:
                    return new List<CardAction> {
                        new AAttack {
                            damage = GetDmg(s, 4),
                            piercing = true,
                            status = Status.shield,
                            statusAmount = 2
                        }
                    };
                default:
                    return new List<CardAction>();
            }
        }
    }
}
