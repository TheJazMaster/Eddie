using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eddie.Cards
{
    public abstract class CheapCard : Card
    {
        Upgrade? lastUpgrade;
        public abstract int GetCheapDiscount();

        public override CardData GetData(State state)
        {
            if (upgrade != lastUpgrade)
            {
                discount = GetCheapDiscount();
                lastUpgrade = upgrade;
            }
            return new CardData();
        }
    }
}
