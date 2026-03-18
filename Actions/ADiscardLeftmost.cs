using System;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FSPRO;

namespace TheJazMaster.Eddie.Actions;

public class ADiscardLeftmost : ADiscard
{
    private static readonly Lazy<Status> cc = new(() => (Status)Manifest.ClosedCircuitStatus!.Id!.Value);

    public override void Begin(G g, State s, Combat c)
    {
        int ccStatus = s.ship.Get(cc.Value);
        if (ccStatus > 0 && count.HasValue) {
            int difference = Math.Min(count.Value, ccStatus);
            count -= difference;
            s.ship.Add(cc.Value, -difference);
        }

        List<Card> list = new();
        int num = 0;
        for (int i = 0; i < count && i < c.hand.Count; i++)
        {
            list.Add(c.hand[i]);
        }
        foreach (Card item in list)
        {
            c.hand.Remove(item);
            item.waitBeforeMoving = (double)num++ * 0.05;
            item.OnDiscard(s, c);
            c.SendCardToDiscard(s, item);
        }
        if (list.Count > 0)
        {
            Audio.Play(Event.CardHandling);
        }
    }

    public override List<Tooltip> GetTooltips(State s)
    {
        List<Tooltip> list = new List<Tooltip>();
        list.Add(new TTGlossary(Manifest.DiscardLeftmostGlossary?.Head ?? throw new Exception("Missing Discard Leftmost glossary"), count ?? 0));
        return list;
    }
}