using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eddie.Cards;

[CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B }, dontOffer = true)]
public class Surge : Card
{
    private static readonly Lazy<Spr> art = new(() => Enum.Parse<Spr>("cards_Overdrive"));

    public override string Name() => "Surge";

    public override CardData GetData(State state)
    {
        return new CardData()
        {
            cost = 1,
            exhaust = true,
            temporary = true,
            art = art.Value
        };
    }

    public override List<CardAction> GetActions(State s, Combat c)
    {
        switch (upgrade)
        {
            case Upgrade.None:
                return new List<CardAction>
                {
                    new AAttack {
                        damage = GetDmg(s, 2)
                    },
                    new AStatus {
                        status = Status.overdrive,
                        statusAmount = 1,
                        targetPlayer = true
                    }
                };
            case Upgrade.A:
                return new List<CardAction>
                {
                    new AAttack {
                        damage = GetDmg(s, 3)
                    },
                    new AStatus {
                        status = Status.overdrive,
                        statusAmount = 1,
                        targetPlayer = true
                    }
                };
            case Upgrade.B:
                return new List<CardAction>
                {
                    new AAttack {
                        damage = GetDmg(s, 1),
                        stunEnemy = true
                    },
                    new AStatus {
                        status = Status.overdrive,
                        statusAmount = 1,
                        targetPlayer = true
                    }
                };
            default:
                return new List<CardAction>();
        }
    }
}
