using CobaltCoreModding.Definitions.ExternalItems;
using CobaltCoreModding.Definitions.ModContactPoints;
using CobaltCoreModding.Definitions.ModManifests;

using HarmonyLib;

namespace Eddie
{
    public partial class Manifest : IStatusManifest
    {
        public static ExternalStatus? CircuitStatus { get; private set; }
        public static ExternalStatus? ClosedCircuitStatus { get; private set; }
        public static ExternalStatus? LoseEnergyEveryTurnStatus { get; private set; }

        public void LoadManifest(IStatusRegistry statusRegistry)
        {
            //patch in logic for our statuses
            var harmony = new Harmony("Eddie.Status");
            // CircuitStatusLogic(harmony);
            // ClosedCircuitStatusLogic(harmony);
            LoseEnergyEveryTurnStatusLogic(harmony);

            //create status objects
            CircuitStatus = new ExternalStatus("Eddie.Status.Circuit", true, Eddie_PrimaryColor, null, CircuitIcon ?? throw MakeInformativeException(Logger, "Missing Circuit Icon for status"), true);
            CircuitStatus.AddLocalisation("Circuit", "Gain {0} Closed Circuit at the start of your turn.");
            statusRegistry.RegisterStatus(CircuitStatus);

            ClosedCircuitStatus = new ExternalStatus("Eddie.Status.ClosedCircuit", true, Eddie_PrimaryColor, null, ClosedCircuitIcon ?? throw MakeInformativeException(Logger, "Missing Closed Circuit Icon for status"), true);
            ClosedCircuitStatus.AddLocalisation("Closed Circuit", "When a card would be discarded, lose 1 Closed Circuit instead.");
            statusRegistry.RegisterStatus(ClosedCircuitStatus);

            LoseEnergyEveryTurnStatus = new ExternalStatus("Eddie.Status.LoseEnergyEveryTurn", true, Eddie_PrimaryColor, null, LoseEnergyEveryTurnIcon ?? throw MakeInformativeException(Logger, "Missing Lose Energy Icon for status"), true);
            LoseEnergyEveryTurnStatus.AddLocalisation("Lose Energy Every Turn", "Lose {0} energy at the start of every turn.");
            statusRegistry.RegisterStatus(LoseEnergyEveryTurnStatus);
        }

        private void LoseEnergyEveryTurnStatusLogic(Harmony harmony)
        {

            var start_turn_method = typeof(Ship).GetMethod("OnBeginTurn") ?? throw MakeInformativeException(Logger, "Couldn't find Ship.OnBeginTurn method");
            var start_turn_post = typeof(Manifest).GetMethod("LoseEnergyEveryTurn", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic) ?? throw MakeInformativeException(Logger, "Couldnt find Manifest.LoseEnergyEveryTurn method");
            harmony.Patch(start_turn_method, postfix: new HarmonyMethod(start_turn_post));
        }

        private static void LoseEnergyEveryTurn(Ship __instance, State s, Combat c)
        {
            if (LoseEnergyEveryTurnStatus?.Id == null)
                return;
            var status = (Status)LoseEnergyEveryTurnStatus.Id;
            
            c.energy -= __instance.Get(status);
        }


//         private void SmartExplosiveLogic(Harmony harmony)
//         {
//             //patch start turn to decrease status by 1.

//             var start_turn_method = typeof(Ship).GetMethod("OnBeginTurn") ?? throw MakeInformativeException(Logger, "Couldnt find Ship.OnBeginTurn method");
//             var start_turn_post = typeof(Manifest).GetMethod("SmartExplosiveTurnStart", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic) ?? throw MakeInformativeException(Logger, "Couldnt find Manifest.SmartExplosiveTurnStart method");
//             harmony.Patch(start_turn_method, postfix: new HarmonyMethod(start_turn_post));

//             //patch regular missile.begin to not fire if the owner has smart explosive

//             var a_missile_hit_update = typeof(AMissileHit).GetMethod("Update") ?? throw MakeInformativeException(Logger, "Couldnt find AMissileHit.Update method");

//             var a_missile_hit_prefix = typeof(Manifest).GetMethod("SmartExplosiveMissileLock", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic) ?? throw MakeInformativeException(Logger, "Couldnt find Manifest.SmartExplosiveMissileLock method");
//             harmony.Patch(a_missile_hit_update, prefix: new HarmonyMethod(a_missile_hit_prefix));
//         }

//         private static bool SmartExplosiveMissileLock(State s, Combat c, AMissileHit __instance)
//         {
//             if (SmartExplosiveStatus?.Id == null)
//                 return true;
//             if (!c.stuff.TryGetValue(__instance.worldX, out var stuffBase) || stuffBase is not Missile missile)
//                 return true;

//             var ship = missile.fromPlayer ? s.ship : c.otherShip;
//             if (ship.Get((Status)SmartExplosiveStatus.Id) <= 0)
//                 return true;
//             //check if seeker missile
//             if (missile.missileType == MissileType.seeker)
//                 return true;
//             //check if missile will hit
//             var target_ship = missile.targetPlayer ? s.ship : c.otherShip;
//             if (target_ship.HasNonEmptyPartAtWorldX(__instance.worldX))
//                 return true;
//             //if not kill action immediately and let update not run.
//             __instance.timer = -1;
//             return false;
//         }

//         private static void SmartExplosiveTurnStart(Ship __instance)
//         {
//             if (SmartExplosiveStatus?.Id == null)
//                 return;
//             __instance.Add((Status)SmartExplosiveStatus.Id, -1);
//         }

//         private void RocketSiloStatusLogic(Harmony harmony)
//         {
//             // patch turn start of player to generate card in hand.
//             var a_start_player_turn_begin_method = typeof(AStartPlayerTurn).GetMethod("Begin") ?? throw MakeInformativeException(Logger, "Couldnt find AStartPlayerTurn.Begin method");
//             var a_start_player_turn_begin_postfix = typeof(Manifest).GetMethod("RocketSiloTurnStart", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic) ?? throw MakeInformativeException(Logger, "Couldnt find Manifest.RocketSiloTurnStart method");
//             harmony.Patch(a_start_player_turn_begin_method, postfix: new HarmonyMethod(a_start_player_turn_begin_postfix));
//         }

//         private static void RocketSiloTurnStart(State s, Combat c)
//         {
//             if (RocketSiloStatus?.Id == null)
//                 return;
//             var status = (Status)RocketSiloStatus.Id;
//             var ammount = s.ship.Get(status);
//             if (ammount <= 0)
//                 return;
//             s.ship.PulseStatus(status);
//             c.QueueImmediate(new AAddCard()
//             {
//                 amount = ammount,
//                 card = new Cards.MicroMissiles(),
//                 handPosition = 0,
//                 destination = CardDestination.Hand
//             });
//         }

//         private void PopBubblesStatusLogic(Harmony harmony)
//         {
//             //patch turn start to decrease this value by 1 and destroy all midrow bubbles.
//             var start_turn_method = typeof(Ship).GetMethod("OnBeginTurn") ?? throw MakeInformativeException(Logger, "Couldnt find Ship.OnBeginTurn method");
//             var start_turn_post = typeof(Manifest).GetMethod("PopBubblesTurnStart", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic) ?? throw MakeInformativeException(Logger, "Couldnt find Manifest.PopBubblesTurnStart method");
//             harmony.Patch(start_turn_method, postfix: new HarmonyMethod(start_turn_post));
//         }

//         private static void PopBubblesTurnStart(Combat c, Ship __instance)
//         {
//             if (PopBubblesStatus?.Id == null)
//                 return;
//             var status = (Status)PopBubblesStatus.Id;
//             if (__instance.Get(status) <= 0)
//                 return;
//             Audio.Play(FSPRO.Event.Hits_ShieldPop);
//             foreach (var stuff in c.stuff.Values)
//             {
//                 stuff.bubbleShield = false;
//             }
//             // __instance.Add(status, -1);
//             __instance.Set(status, 0);
//         }

    }
}
