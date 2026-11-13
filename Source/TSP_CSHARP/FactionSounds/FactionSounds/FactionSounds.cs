using HarmonyLib;
using Verse;

namespace FactionSounds;

[StaticConstructorOnStartup]
internal static class FactionSounds
{
	static FactionSounds()
	{
		Harmony harmony = new Harmony("rimworld.factionsounds");
		harmony.PatchAll();
		Log.Message("Faction Sounds initialized");
	}
}
