using TheJazMaster.Eddie.Actions;
using CobaltCoreModding.Definitions;
using CobaltCoreModding.Definitions.ExternalItems;
using CobaltCoreModding.Definitions.ModContactPoints;
using CobaltCoreModding.Definitions.ModManifests;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using HarmonyLib;
using FMOD;
using FSPRO;
using Microsoft.Xna.Framework.Input;

namespace TheJazMaster.Eddie;

public static class ShortCircuit
{

	public static void SetShortCircuit(State s, Card card, bool? overrideValue = null, bool permanentValue = false) {
		Manifest.Helper.Content.Cards.SetCardTraitOverride(s, card, Manifest.ShortCircuitTrait, overrideValue, permanentValue);
	}

	public static bool DoesShortCircuit(State s, Card card)
	{
		return Manifest.Helper.Content.Cards.IsCardTraitActive(s, card, Manifest.ShortCircuitTrait);
	}

	[HarmonyBefore(new string[] { "Shockah.Soggins" })]
	private static IEnumerable<CodeInstruction> ShortCircuitDiscardMaybe(IEnumerable<CodeInstruction> iseq, ILGenerator il, MethodBase originalMethod)
	{
		int? local_index = null;
		foreach (LocalVariableInfo info in originalMethod.GetMethodBody()!.LocalVariables)
		{
			if (info.LocalType == typeof(List<CardAction>))
			{
				local_index = info.LocalIndex;
				break;
			}
		}
		if (local_index == null)
			throw new Exception("CardAction list not found");

		int? cost_local_index = null;
		foreach (LocalVariableInfo info in originalMethod.GetMethodBody()!.LocalVariables)
		{
			if (info.LocalType == typeof(int))
			{
				cost_local_index = info.LocalIndex;
				break;
			}
		}
		if (cost_local_index == null)
			throw new Exception("cost int not found");

		using IEnumerator<CodeInstruction> iter = iseq.GetEnumerator();

		while(iter.MoveNext()) {
			yield return iter.Current;
			if(iter.Current.opcode != OpCodes.Ldarg_0) {
				continue;
			}

			if(!iter.MoveNext()) {
				break;
			}
			yield return iter.Current;
			
			if(!TranspilerUtils.IsLocalLoad(iter.Current) || TranspilerUtils.ExtractLocalIndex(iter.Current) != local_index) {
				continue;
			}

			while (iter.MoveNext() && (iter.Current.opcode != OpCodes.Call || ((MethodInfo)iter.Current.operand).Name != "Queue")) {
				yield return iter.Current;
			}
			yield return iter.Current;


			if(iter.Current.opcode != OpCodes.Call || (MethodInfo) iter.Current.operand != typeof(Combat).GetMethod("Queue", 0, new Type[] {typeof(IEnumerable<CardAction>)})) {
				continue;
			}
			
			Label end_label = il.DefineLabel();

			yield return new CodeInstruction(OpCodes.Ldarg_1);
			yield return new CodeInstruction(OpCodes.Ldarg_2);
			yield return new CodeInstruction(OpCodes.Call, typeof(ShortCircuit).GetMethod("DoesShortCircuit"));
			yield return new CodeInstruction(OpCodes.Brfalse, end_label);

			yield return new CodeInstruction(OpCodes.Ldarg_2);
			yield return new CodeInstruction(OpCodes.Ldarg_1);
			yield return new CodeInstruction(OpCodes.Ldarg_0);
			yield return new CodeInstruction(OpCodes.Ldloc, cost_local_index);
			yield return new CodeInstruction(OpCodes.Call, typeof(ShortCircuit).GetMethod("DiscardMaybe", BindingFlags.Static | BindingFlags.NonPublic));

			iter.MoveNext();
			iter.Current.labels.Add(end_label);
			yield return iter.Current;
			break;
		}
		while(iter.MoveNext())
			yield return iter.Current;
	}

	private static void ShortCircuitOverlay(G g, Combat __instance) {
		if (Manifest.Instance.KokoroApi.TryGetExtensionData(g.state.ship, "ShortCircuitOverlay", out double data)) {
			Draw.Sprite((Spr)Manifest.ShortCircuitOverlaySprite.Id!, -25.0, -25.0, blend: BlendMode.Screen, color: Colors.white.gain(data / 1.0 * 0.25));
			g.state.flash = new Color(1, 1, 0).gain(data / 4.0);

			double newData = data - g.dt;
			if (newData <= 0)
				Manifest.Instance.KokoroApi.RemoveExtensionData(g.state.ship, "ShortCircuitOverlay");
			else
				Manifest.Instance.KokoroApi.SetExtensionData(g.state.ship, "ShortCircuitOverlay", newData);
		}
	}

	private static void DiscardMaybe(Card card, State s, Combat c, int cost) {
		c.Queue(new AShortCircuit {
			startEnergy = c.energy + cost
		});
	}
}