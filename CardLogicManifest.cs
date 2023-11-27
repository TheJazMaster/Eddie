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
using HarmonyLib;
using Eddie.Cards;

namespace Eddie
{
    public partial class Manifest
    {
        // Logic for "X = energy"
        public static ConditionalWeakTable isEnergy = new ConditionalWeakTable<AVariableHint, Boolean>
        public static ConditionalWeakTable energyAmount = new ConditionalWeakTable<AVariableHint, Integer>

        // Logic for "energy = X"
        public static ConditionalWeakTable aEnergyMode = new ConditionalWeakTable<AEnergy, AStatusMode>

        // private static ICustomEventHub? _eventHub;

        // internal static ICustomEventHub EventHub { get => _eventHub ?? throw new Exception(); set => _eventHub = value; }

        public void LoadManifest(ICustomEventHub eventHub)
        {
            // //assign for local consumption
            // _eventHub = eventHub;
            // //distance, target_player, from_evade, combat, state
            // eventHub.MakeEvent<Tuple<int, bool, bool, Combat, State>>("Eddie.ShipMoved");

            var harmony = new Harmony("Eddie.Cards");
            {
                var avariablehint_get_icon_method = typeof(AVariableHint).GetMethod("GetIcon") ?? throw new Exception("AVariableHint.GetIcon method not found.");

                var avariablehint_get_icon_prefix = GetMethod("AVariableHint_Get_Icon_Pre", BindingFlags.Static | BindingFlags.NonPublic) ?? throw new Exception("Manifest.AVariableHint_Get_Icon_Pre method not found.");

                harmony.Patch(avariablehint_get_icon_method, postfix: null, prefix: new HarmonyMethod(avariablehint_get_icon_prefix));
            }

            {
                var aenergy_get_icon_method = typeof(AEnergy).GetMethod("GetIcon") ?? throw new Exception("AEnergy.GetIcon method not found.");

                var aenergy_get_icon_prefix = GetMethod("AEnergy_Get_Icon_Pre", BindingFlags.Static | BindingFlags.NonPublic) ?? throw new Exception("Manifest.AEnergy_Get_Icon_Pre method not found.");

                harmony.Patch(avariablehint_get_icon_method, postfix: null, prefix: new HarmonyMethod(aenergy_get_icon_prefix));
            }
            {
                var aenergy_begin_method = typeof(AEnergy).GetMethod("Begin") ?? throw new Exception("AEnergy.Begin method not found.");

                var aenergy_begin_prefix = GetMethod("AEnergy_Begin_Pre", BindingFlags.Static | BindingFlags.NonPublic) ?? throw new Exception("Manifest.AEnergy_Begin_Pre method not found.");

                harmony.Patch(avariablehint_get_icon_method, postfix: null, prefix: new HarmonyMethod(avariablehint_begin_prefix));
            }
        }

        // Methods for "X = energy"
        private static void AVariableHint_Get_Icon_Pre(AVariableHint __instance, State s)
        {
            if (isEnergy.TryGetValue(__instance))
            {
                return new Icon(Spr.combat_energy, null, Colors.textMain);
            }
        }

        // Methods for "energy = X"
        private static void AEnergy_Get_Icon_Pre(AEnergy __instance, State s)
        {
            if (aEnergyMode.TryGetValue(__instance) == AStatusMode.Set)
            {
                return new Icon(Spr.icons_energy, null, Colors.textMain);
            }
        }
        private static void AEnergy_Begin_Pre(AEnergy __instance, State s, Combat c)
        {
            if (aEnergyMode.TryGetValue(__instance) == AStatusMode.Set) {
                c.energy = 0;
            }
        }


        public static int getEnergyAmount(State s, Combat c, Card c)
        {
            return c.energy - c.cost
        }
    }

}
