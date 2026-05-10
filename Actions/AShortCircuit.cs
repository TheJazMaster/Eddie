using FSPRO;

namespace TheJazMaster.Eddie.Actions;

public class AShortCircuit : CardAction
{
    public int amount = 2;
    public int startEnergy;

    public override void Begin(G g, State s, Combat c)
    {
        if (c.energy < startEnergy) {
            timer = 0;
            return;
        }

        int num = 0;
        foreach (Card item in c.hand.Take(amount))
        {
            c.hand.Remove(item);
            item.waitBeforeMoving = ++num * 0.16;
            item.OnDiscard(s, c);
            c.SendCardToDiscard(s, item);
        }
        if (num > 0)
        {
            ModEntry.Instance.Helper.ModData.SetModData<double>(s.ship, "ShortCircuitOverlay", 1); 
            Audio.Play(Event.CardHandling);
        }
    }
}