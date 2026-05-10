
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using Nanoray.Shrike;
using Nanoray.Shrike.Harmony;
using TheJazMaster.Eddie.Actions;
using TheJazMaster.Eddie.Artifacts;

namespace TheJazMaster.Eddie;

[HarmonyPatch]
public class XEnergyManager {
    static ModEntry Instance => ModEntry.Instance;

    public static int GetEnergyAmount(State s, Combat c, Card? card)
    {
        if (s.route is Combat)
        {
            return c.energy;
        }
        return s.ship.baseEnergy;
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

    [HarmonyTranspiler]
    [HarmonyPatch(typeof(Card), nameof(Card.RenderAction))]
    private static IEnumerable<CodeInstruction> Card_RenderAction_Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il, MethodBase originalMethod)
    {
        return new SequenceBlockMatcher<CodeInstruction>(instructions)
			.Find(
				ILMatches.AnyLdloc,
                ILMatches.Ldfld("action"),
                ILMatches.Isinst<AVariableHint>(),
                ILMatches.Stloc<AVariableHint>(originalMethod).CreateLdlocInstruction(out var ldLoc),
                ILMatches.Ldloc<AVariableHint>(originalMethod),
                ILMatches.Brfalse,
                ILMatches.Ldloc<AVariableHint>(originalMethod),
                ILMatches.Ldfld("hand"),
                ILMatches.Brtrue.GetBranchTarget(out var label)
			)
			.Insert(SequenceMatcherPastBoundsDirection.After, SequenceMatcherInsertionResultingBounds.JustInsertion, [
                ldLoc,
                new CodeInstruction(OpCodes.Isinst, typeof(AVariableHintEnergy)),
                new CodeInstruction(OpCodes.Brtrue_S, label.Value)
            ])
            .AllElements();
	}
}


static class EnergyExtensions
{

    static int recursionLevel = 0;
	public static int GetCurrentCostNoRecursion(this Card card, State s)
	{
		int result;
		recursionLevel++;
		if (recursionLevel < 2)
			result = card.GetCurrentCost(s);
		else
			result = card.GetData(s).cost;
		recursionLevel = 0;
		return result;
	}
}