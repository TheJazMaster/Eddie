using System.Reflection;
using HarmonyLib;
using Microsoft.Extensions.Logging;

namespace TheJazMaster.Eddie;

public static class Extensions {
    public static MethodInfo? TryPatch(this Harmony harmony, ILogger logger, MethodBase original, HarmonyMethod? prefix = null, HarmonyMethod? postfix = null, HarmonyMethod? transpiler = null, HarmonyMethod? finalizer = null) {
		try {
			return harmony.Patch(original, prefix, postfix, transpiler, finalizer);
		} catch (Exception e) {
			logger.LogError(original.Name + " patch has failed. Reason:\n" +
				e.Message + "\n" + e.StackTrace);
			return null;
		}
	}

	public static T WithModData<T, K>(this T thing, string key, K data) {
		ModEntry.Instance.Helper.ModData.SetModData(thing!, key, data);
		return thing;
	}
}