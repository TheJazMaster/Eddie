using System.Reflection;
using System.Reflection.Emit;

using HarmonyLib;
using Nickel;
using Nanoray.PluginManager;
using Shockah.Kokoro;

namespace TheJazMaster.Eddie;

[HarmonyPatch]
public class StatusManager : IKokoroApi.IV2.IStatusLogicApi.IHook, IKokoroApi.IV2.IStatusRenderingApi.IHook
{
    static ModEntry Instance => ModEntry.Instance;
    static IModHelper Helper => Instance.Helper;
    static IPluginPackage<IModManifest> Package => Instance.Package;

	internal static Status CircuitStatus;
	internal static Status ClosedCircuitStatus;
    internal static Status MoreEnergyStatus;


    public StatusManager() {
        Instance.KokoroApi.StatusLogic.RegisterHook(this);
        Instance.KokoroApi.StatusRendering.RegisterHook(this);

        CircuitStatus = Helper.Content.Statuses.RegisterStatus("circuit", new()
		{
			Definition = new()
			{
				icon = Helper.Content.Sprites.RegisterSprite(Package.PackageRoot.GetRelativeFile("Sprites/icons/circuit.png")).Sprite,
				color = new("00802d"),
				isGood = true
			},	
			Name = Instance.AnyLocalizations.Bind(["status", "Circuit", "name"]).Localize,
			Description = Instance.AnyLocalizations.Bind(["status", "Circuit", "description"]).Localize
		}).Status;

        ClosedCircuitStatus = Helper.Content.Statuses.RegisterStatus("ClosedCircuit", new()
		{
			Definition = new()
			{
				icon = Helper.Content.Sprites.RegisterSprite(Package.PackageRoot.GetRelativeFile("Sprites/icons/closed_circuit.png")).Sprite,
				color = new("f2ca00"),
				isGood = true
			},
			Name = Instance.AnyLocalizations.Bind(["status", "ClosedCircuit", "name"]).Localize,
			Description = Instance.AnyLocalizations.Bind(["status", "ClosedCircuit", "description"]).Localize
		}).Status;

        MoreEnergyStatus = Helper.Content.Statuses.RegisterStatus("MoreEnergy", new()
		{
			Definition = new()
			{
				icon = Helper.Content.Sprites.RegisterSprite(Package.PackageRoot.GetRelativeFile("Sprites/icons/energy_more_every_turn.png")).Sprite,
				color = new("18529c"),
                isGood = true
			},
			Name = Instance.AnyLocalizations.Bind(["status", "MoreEnergy", "name"]).Localize,
			Description = Instance.AnyLocalizations.Bind(["status", "MoreEnergy", "description"]).Localize
		}).Status;

        Instance.DraculaApi?.RegisterBloodTapOptionProvider(CircuitStatus, (_, _, status) => [
            new AHurt { targetPlayer = true, hurtAmount = 2 },
            new AStatus { targetPlayer = true, status = status, statusAmount = 1 },
            new AStatus { targetPlayer = true, status = ClosedCircuitStatus, statusAmount = 1 },
        ]);
        Instance.DraculaApi?.RegisterBloodTapOptionProvider(ClosedCircuitStatus, (_, _, status) => [
            new AHurt { targetPlayer = true, hurtAmount = 1 },
            new AStatus { targetPlayer = true, status = status, statusAmount = 4 }
        ]);
    }
	public void OnStatusTurnTrigger(IKokoroApi.IV2.IStatusLogicApi.IHook.IOnStatusTurnTriggerArgs args) {
        if (args.Timing != IKokoroApi.IV2.IStatusLogicApi.StatusTurnTriggerTiming.TurnStart) return;

        if (args.Status == CircuitStatus) args.Ship.Add(ClosedCircuitStatus, args.OldAmount);
        if (args.Status == MoreEnergyStatus) args.Combat.energy += args.OldAmount;
    }

    public IReadOnlyList<Tooltip> OverrideStatusTooltips(IKokoroApi.IV2.IStatusRenderingApi.IHook.IOverrideStatusTooltipsArgs args) {
        if (args.Status == CircuitStatus) 
            return [.. args.Tooltips, .. StatusMeta.GetTooltips(ClosedCircuitStatus, args.Amount)];
        return args.Tooltips;
    }

    private static List<int> previousHandUuids = [];

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Combat), nameof(Combat.Update))]
    private static void Combat_Update_Prefix(G g, Combat __instance) {
        previousHandUuids = [.. __instance.hand.Select(card => card.uuid)];
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Combat), nameof(Combat.SendCardToDiscard))]
    private static bool Combat_SendCardToDiscard_Prefix(State s, Combat __instance, Card card) {
        if (s.ship.Get(ClosedCircuitStatus) > 0 && previousHandUuids.Contains(card.uuid)) {
            List<Card> hand = __instance.hand;
            int j = 0;
            for (int i = 0; i < previousHandUuids.Count && j < hand.Count; i++) {
                if (previousHandUuids[i] == card.uuid) break;
                if (previousHandUuids[i] == hand[j].uuid) j++;
            }
            __instance.hand.Insert(j, card);
            s.ship.Add(ClosedCircuitStatus, -1);
            s.ship.PulseStatus(ClosedCircuitStatus);
            card.flipAnim = 1;
            return false;
        }
        return true;
    }

}