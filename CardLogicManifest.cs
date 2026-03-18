using System;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CobaltCoreModding.Definitions.ExternalItems;
using CobaltCoreModding.Definitions.ModContactPoints;
using CobaltCoreModding.Definitions.ModManifests;
using FMOD;
using FSPRO;
using HarmonyLib;
using TheJazMaster.Eddie.Cards;
using TheJazMaster.Eddie.Actions;

namespace TheJazMaster.Eddie
{
    public partial class Manifest
    {
        public static int GetEnergyAmount(State s, Combat c, Card? card)
        {
            if (s.route is Combat combat)
            {
                return c.energy;
            }
            return 0;
        }

        public static void TurnCardToEnergy(State s, Combat c, Card? card, CardAction action, bool exhaustThisCardAfterwards)
        {
            if (card == null)
                return;

            if (exhaustThisCardAfterwards)
            {
                c.Queue(new AExhaustOtherCard
                {
                    uuid = card.uuid
                });
            }
            else
            {
                c.Queue(new ADiscardPosition
                {
                    handPosition = c.hand.Contains(card) ? c.hand.IndexOf(card) : null
                });
            }
            action.timer = 0.2;

            int cost = card.GetCurrentCostNoRecursion(s);

            c.Queue(new AEnergy
            {
                changeAmount = cost
            });
        }
    }

}
