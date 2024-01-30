// AMove
using System;
using System.Collections.Generic;
using System.Linq;
using FMOD;
using FSPRO;

namespace TheJazMaster.Eddie.Actions
{
	public class AMoveImproved : AMove
	{
		public override List<Tooltip> GetTooltips(State s)
		{
			if (!targetPlayer) {
				List<Tooltip> list = new List<Tooltip>();
				list.Add(dir > 0 ? new TTGlossary(Manifest.MoveEnemyRightGlossary!.Head, dir) : new TTGlossary(Manifest.MoveEnemyLeftGlossary!.Head, -dir));
				return list;
			} else {
				return base.GetTooltips(s);
			}
		}
	}
}