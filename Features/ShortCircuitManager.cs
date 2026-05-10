using TheJazMaster.Eddie.Actions;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using Nanoray.Shrike;
using Nanoray.Shrike.Harmony;
using Nickel;

namespace TheJazMaster.Eddie;

[HarmonyPatch]
public class ShortCircuitManager
{
    static ModEntry Instance => ModEntry.Instance;

	internal static ICardTraitEntry ShortCircuitTrait = null!;

	public ShortCircuitManager() {
		Spr ShortCircuitIcon = Instance.Helper.Content.Sprites.RegisterSprite(Instance.Package.PackageRoot.GetRelativeFile("Sprites/icons/short_circuit.png")).Sprite;

        ShortCircuitTrait = Instance.Helper.Content.Cards.RegisterTrait("ShortCircuit", new()
        {
            Icon = (state, card) => ShortCircuitIcon,
            Name = (_) =>  Instance.Localizations.Localize(["trait", "ShortCircuit", "name"]),
            Tooltips = (state, card) => [
				new GlossaryTooltip($"trait.{GetType().Namespace!}::ShortCircuit") {
					Icon = ShortCircuitIcon,
					TitleColor = Colors.cardtrait,
					Title = Instance.Localizations.Localize(["trait", "ShortCircuit", "name"]),
                    Description = Instance.Localizations.Localize(["trait", "ShortCircuit", "description"]),
				}
            ]
        });
	}

	public static void SetShortCircuit(State s, Card card, bool? overrideValue = null, bool permanentValue = false) {
		Instance.Helper.Content.Cards.SetCardTraitOverride(s, card, ShortCircuitTrait, overrideValue, permanentValue);
	}

	public static bool IsShortCircuitActive(State s, Card card)
	{
		return Instance.Helper.Content.Cards.IsCardTraitActive(s, card, ShortCircuitTrait);
	}

	private static void ShortCircuitCheck(State s, Combat c, Card card, int cost)
	{
		if (IsShortCircuitActive(s, card)) {
			c.Queue(new AShortCircuit {
				startEnergy = c.energy + cost
			});
		}
	}

	[HarmonyTranspiler]
	[HarmonyPatch(typeof(Combat), nameof(Combat.TryPlayCard))]
	[HarmonyBefore(["Shockah.Soggins"])]
	private static IEnumerable<CodeInstruction> Combat_TryPlayCard_Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il, MethodBase originalMethod)
	{
		return new SequenceBlockMatcher<CodeInstruction>(instructions)
			.Find(
				ILMatches.Stloc<int>(originalMethod).CreateLdlocInstruction(out var instr)
			)
            .Find(
                ILMatches.Ldarg(0),
				ILMatches.Ldloc<List<CardAction>>(originalMethod),
				ILMatches.Call("Queue")
            )
            .Insert(SequenceMatcherPastBoundsDirection.After, SequenceMatcherInsertionResultingBounds.IncludingInsertion, [
                new(OpCodes.Ldarg_1),
				new(OpCodes.Ldarg_0),
				new(OpCodes.Ldarg_2),
				new(instr.Value),
				new(OpCodes.Call, AccessTools.DeclaredMethod(typeof(ShortCircuitManager), nameof(ShortCircuitCheck)))
            ])
            .AllElements();
	}

	[HarmonyPostfix]
	[HarmonyPatch(typeof(Combat), nameof(Combat.RenderBehindCockpit))]
	private static void Combat_RenderBehindCockpit_Postfix(G g, Combat __instance) {
		if (Instance.Helper.ModData.TryGetModData(g.state.ship, "ShortCircuitOverlay", out double data)) {
			Draw.Sprite(ModEntry.Instance.ShortCircuitOverlaySprite, -25.0, -25.0, blend: BlendMode.Screen, color: Colors.white.gain(data / 1.0 * 0.25));
			g.state.flash = new Color(1, 1, 0).gain(data / 4.0);

			double newData = data - g.dt;
			if (newData <= 0)
				Instance.Helper.ModData.RemoveModData(g.state.ship, "ShortCircuitOverlay");
			else
				Instance.Helper.ModData.SetModData(g.state.ship, "ShortCircuitOverlay", newData);
		}
	}
}