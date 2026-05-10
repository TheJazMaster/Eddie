using FSPRO;

namespace TheJazMaster.Eddie.Actions
{
    public class AEnergySet : AStatus
    {
        public int setTo = 0;

        public override void Begin(G g, State s, Combat c)
        {
            int changeAmount = setTo - c.energy;
            if (changeAmount != 0) {
                c.energy = Math.Max(setTo, 0);
                Audio.Play((changeAmount > 0) ? Event.Status_PowerUp : Event.Status_PowerDown);
                if (changeAmount < 0)
                {
                    c.pulseEnergyBad = 0.5;
                }
                else if (changeAmount > 0)
                {
                    c.pulseEnergyGood = 0.5;
                }
            }
            else {
                timer = 0;
            }
        }

        public override Icon? GetIcon(State s) {
            statusAmount = setTo;
            mode = AStatusMode.Set;
            targetPlayer = true;
            return new Icon(ModEntry.Instance.EnergyIcon, null, Colors.textMain);
        }

        public override List<Tooltip> GetTooltips(State s) => [];
    }
}