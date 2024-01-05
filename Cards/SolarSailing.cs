using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eddie.Cards;

[CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
public class SolarSailing : Card
{
    public override string Name() => "Solar Sailing";

    public override CardData GetData(State state)
    {
        return new CardData
        {
            cost = 0,
            flippable = upgrade == Upgrade.B,
            retain = upgrade == Upgrade.A
        };
    }

    public override List<CardAction> GetActions(State s, Combat c)
    {
        return new List<CardAction>
        {
            new AMove
            {
                dir = 1,
                targetPlayer = true
            },
            new ADrawCard
            {
                count = 1
            }
        };
    }
}
