using System;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FSPRO;

namespace TheJazMaster.Eddie.Actions;

public class AShortCircuit : CardAction
{
    public int startEnergy;

    public override void Begin(G g, State s, Combat c)
    {
        if (c.energy < startEnergy) {
            timer = 0;
            return;
        }

        int count = 2;
        int ccStatus = s.ship.Get((Status)Manifest.ClosedCircuitStatus.Id!);
        if (ccStatus > 0) {
            int difference = Math.Min(count, ccStatus);
            count -= difference;
            s.ship.Add((Status)Manifest.ClosedCircuitStatus.Id!, -difference);
        }

        List<Card> list = [];
        int num = 0;
        for (int i = 0; i < count && i < c.hand.Count; i++)
        {
            list.Add(c.hand[i]);
        }
        foreach (Card item in list)
        {
            c.hand.Remove(item);
            item.waitBeforeMoving = ++num * 0.16;
            item.OnDiscard(s, c);
            c.SendCardToDiscard(s, item);
        }
        if (list.Count > 0)
        {
            Manifest.Instance.KokoroApi.SetExtensionData<double>(s.ship, "ShortCircuitOverlay", 1); 
            Audio.Play(Event.CardHandling);
        }
    }

    public override List<Tooltip> GetTooltips(State s) => [new TTGlossary(Manifest.DiscardLeftmostGlossary?.Head!, 2)];
}