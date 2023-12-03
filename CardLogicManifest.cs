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
using Eddie.Cards;
using Eddie.Actions;

namespace Eddie
{
    public partial class Manifest
    {

        // private static ICustomEventHub? _eventHub;

        // internal static ICustomEventHub EventHub { get => _eventHub ?? throw MakeInformativeException(Logger, ); set => _eventHub = value; }

        // public void LoadManifest(ICustomEventHub eventHub)
        // {
        //     // //assign for local consumption
        //     // _eventHub = eventHub;
        //     // //distance, target_player, from_evade, combat, state
        //     // eventHub.MakeEvent<Tuple<int, bool, bool, Combat, State>>("Eddie.ShipMoved");

        //     var harmony = new Harmony("Eddie.Cards");
        //     // {
        //     //     var avariablehint_get_icon_method = typeof(AVariableHint).GetMethod("GetIcon") ?? throw MakeInformativeException(Logger, "AVariableHint.GetIcon method not found.");

        //     //     var avariablehint_get_icon_prefix = typeof(Manifest).GetMethod("AVariableHint_Get_Icon_Pre", BindingFlags.Static | BindingFlags.NonPublic) ?? throw MakeInformativeException(Logger, "Manifest.AVariableHint_Get_Icon_Pre method not found.");

        //     //     harmony.Patch(avariablehint_get_icon_method, prefix: new HarmonyMethod(avariablehint_get_icon_prefix));
        //     // }

        //     {
        //         var aenergy_get_icon_method = typeof(AEnergy).GetMethod("GetIcon") ?? throw MakeInformativeException(Logger, "AEnergy.GetIcon method not found.");

        //         var aenergy_get_icon_prefix = typeof(Manifest).GetMethod("AEnergy_Get_Icon_Pre", BindingFlags.Static | BindingFlags.NonPublic) ?? throw MakeInformativeException(Logger, "Manifest.AEnergy_Get_Icon_Pre method not found.");

        //         harmony.Patch(aenergy_get_icon_method, prefix: new HarmonyMethod(aenergy_get_icon_prefix));
        //     }
        //     {
        //         var aenergy_begin_method = typeof(AEnergy).GetMethod("Begin") ?? throw MakeInformativeException(Logger, "AEnergy.Begin method not found.");

        //         var aenergy_begin_prefix = typeof(Manifest).GetMethod("AEnergy_Begin_Pre", BindingFlags.Static | BindingFlags.NonPublic) ?? throw MakeInformativeException(Logger, "Manifest.AEnergy_Begin_Pre method not found.");

        //         harmony.Patch(aenergy_begin_method, prefix: new HarmonyMethod(aenergy_begin_method));
        //     }
        // }

        // Methods for "X = energy"
        // private static bool AVariableHint_Get_Icon_Pre(AVariableHint __instance, Icon? __result, State s)
        // {
        //     StructRef<bool>? data = null; 
        //     if (isEnergy.TryGetValue(__instance, out data) && data.Value)
        //     {
        //         __result = new Icon(Spr.combat_energy, null, Colors.textMain);
        //         return false;
        //     }
        //     return true;
        // }

        // Methods for "energy = X"
        // private static bool AEnergy_Get_Icon_Pre(AEnergy __instance, Icon? __result, State s)
        // {
        //     StructRef<AStatusMode>? data = null; 
        //     if (aEnergyMode.TryGetValue(__instance, out data) && data.Value == AStatusMode.Set)
        //     {
        //         __result = new Icon(Spr.icons_energy, null, Colors.textMain);
        //         return false;
        //     }
        //     return true;
        // }
        // private static void AEnergy_Begin_Pre(AEnergy __instance, State s, Combat c)
        // {
        //     StructRef<AStatusMode>? data = null; 
        //     if (aEnergyMode.TryGetValue(__instance, out data) && data.Value == AStatusMode.Set) {
        //         c.energy = 0;
        //     }
        // }


        public static int getEnergyAmount(State s, Combat c, Card? card)
        {
            if (s.route is Combat combat)
            {
                return c.energy;// - (card == null ? 0 : card.GetCurrentCost(s));
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

            int cost = card.GetCurrentCost(s);

            c.Queue(new AEnergy
            {
                changeAmount = cost
            });
        }
    }

}
