using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using Nanoray.Shrike;
using Nanoray.Shrike.Harmony;

namespace TheJazMaster.Eddie;

[HarmonyPatch]
public class ArtifactInterfaceManager {

    [HarmonyPostfix]
    [HarmonyPatch(typeof(AMove), nameof(AMove.Begin))]
	private static void AMove_Begin_Postfix(AMove __instance, Combat c, State s)
    {
        foreach (Artifact item in s.EnumerateAllArtifacts()) {
            if (item is IOnMoveArtifact artifact) {
                artifact.OnMove(s, c, __instance);
            }
        }
    }

    public interface IOnMoveArtifact {
        void OnMove(State s, Combat c, AMove move);
    }



    [HarmonyPostfix]
    [HarmonyPatch(typeof(Combat), nameof(Combat.SendCardToExhaust))]
    private static void TriggerExhaustArtifacts(Combat __instance, State s, Card card) {
        if (!__instance.exhausted.Contains(card)) return;

        foreach (Artifact item in s.EnumerateAllArtifacts()) {
            if (item is IOnExhaustArtifact artifact)  {
                artifact.OnExhaustCard(s, __instance, card);
            }     
        }
    }

    public interface IOnExhaustArtifact {
        void OnExhaustCard(State s, Combat c, Card card);
    }



    [HarmonyPrefix]
    [HarmonyPatch(typeof(AStatus), nameof(AStatus.Begin))]
    private static void AStatus_Begin_Prefix(G g, State s, Combat c, AStatus __instance, ref int __state) {
        Ship ship = __instance.targetPlayer ? s.ship : c.otherShip;
        __state = ship.Get(__instance.status);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(AStatus), nameof(AStatus.Begin))]
    private static void AStatus_Begin_Postfix(G g, State s, Combat c, AStatus __instance, ref int __state) {
        if (__instance.status != Status.shield) return;

        Ship ship = __instance.targetPlayer ? s.ship : c.otherShip;
        int after = ship.Get(Status.shield);

        if (after < ship.GetMaxShield()) return;

        int supposed = __state;
        if (__instance.mode == AStatusMode.Add) {
			supposed += __instance.statusAmount;
		}
		else if (__instance.mode == AStatusMode.Set) {
			supposed = __instance.statusAmount;
		}
		else if (__instance.mode == AStatusMode.Mult) {
			supposed *= __instance.statusAmount;
		}
        var overshield = Math.Max(supposed - after, 0);
        foreach (Artifact item in s.EnumerateAllArtifacts()) {
            if (item is IOvershieldArtifact artifact) 
                artifact.OnOvershield(s, c, overshield, __instance.targetPlayer);
        }
    }
    
    public interface IOvershieldArtifact {
        void OnOvershield(State s, Combat c, int overshield, bool targetPlayer);
    }
}