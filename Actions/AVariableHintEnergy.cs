using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Threading.Tasks;
using FSPRO;

namespace Eddie.Actions
{
    public class AVariableHintEnergy : AVariableHint
    {
        public int setAmount = 0;
        public override Icon? GetIcon(State s)
        {
            if (Manifest.EnergyIcon?.Id != null)
                return new Icon((Spr)Manifest.EnergyIcon.Id, null, Colors.textMain);
            return null;
        }

        public override List<Tooltip> GetTooltips(State s)
        {
			List<Tooltip> list = new List<Tooltip>();
			string parentheses = "";
			if (s.route is Combat && setAmount >= 0)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(22, 1);
				defaultInterpolatedStringHandler.AppendLiteral(" </c>(<c=keyword>");
				defaultInterpolatedStringHandler.AppendFormatted(setAmount);
				defaultInterpolatedStringHandler.AppendLiteral("</c>)");
				
			    parentheses = defaultInterpolatedStringHandler.ToStringAndClear();
			}
			list.Add(new TTText(Loc.T(Manifest.XIsEnergyGlossary?.Head + ".desc" ?? throw new Exception("Missing X = energy glossary"), true, parentheses)));
			// list.Add(new TTGlossary((Manifest.XIsEnergyGlossary?.Head + ".desc") ?? throw new Exception("Missing X = energy glossary"), parentheses));
			return list;
		}
    }
}