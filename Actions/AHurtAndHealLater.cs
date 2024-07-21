using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheJazMaster.Eddie.Actions
{
    public class AHurtAndHealLater : CardAction
    {
		public int hurtAmount;

		public bool targetPlayer;

        public override void Begin(G g, State s, Combat c)
	    {
            Manifest.Instance.KokoroApi.SetExtensionData(targetPlayer ? s.ship : c.otherShip, Manifest.NoHealThisTurnKey, true);
            c.Queue(new AHurt
			{
				hurtAmount = hurtAmount,
				hurtShieldsFirst = false,
				targetPlayer = targetPlayer
			});
            c.Queue(new AStatus
			{
				status = (Status)(Manifest.HealNextTurnStatus?.Id ?? throw new Exception("Missing heal next turn status")),
				statusAmount = hurtAmount,
				targetPlayer = targetPlayer
			});
        }
        public override Icon? GetIcon(State s)
        {
            if (Manifest.TemporaryHurtIcon?.Id != null)
                return new Icon((Spr)Manifest.TemporaryHurtIcon.Id, hurtAmount, Colors.textMain);
            return null;
        }

        public override List<Tooltip> GetTooltips(State s)
        {
            List<Tooltip> list = new List<Tooltip>();

			list.Add(new TTGlossary(Manifest.TemporaryHurtGlossary?.Head ?? throw new Exception("Missing temporary hurt glossary"), hurtAmount));

            return list;
        }
    }
}