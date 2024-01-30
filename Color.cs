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

namespace TheJazMaster.Eddie
{
	public static class EddieColor
	{
		private static bool LookupColor(ref uint? __result, string key)
		{
			if (key == "Eddie.EddieDeck") {
				__result = ToInt(Manifest.Eddie_PrimaryColor);
				return false;
			}
			return true;
		}

		private static uint ToInt(System.Drawing.Color color)
		{
			return (uint)((Mutil.Clamp((int)(color.A), 0, 255) << 24) | (Mutil.Clamp((int)(color.R), 0, 255) << 16) | (Mutil.Clamp((int)(color.G), 0, 255) << 8) | Mutil.Clamp((int)(color.B), 0, 255));
		}
	}
}