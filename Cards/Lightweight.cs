using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eddie.Cards;

[CardMeta(rarity = Rarity.common, dontOffer = true)]
public class Lightweight : Card
{    
    public override string Name() => "Lightweight";

    public override CardData GetData(State state)
    {
        return new CardData
        {
            cost = 0,
            exhaust = true,
            singleUse = true
        };
    }

    public override List<CardAction> GetActions(State s, Combat c)
    {
        return new List<CardAction>
        {
            new AStatus
            {
                targetPlayer = true,
                status = Status.hermes,
                statusAmount = 1
            }
        };
    }
}