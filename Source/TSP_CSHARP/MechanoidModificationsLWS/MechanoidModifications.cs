using HarmonyLib;
using Verse;

namespace MechanoidModificationsLWS;

[StaticConstructorOnStartup]
public static class MechanoidModifications
{
	static MechanoidModifications()
	{
		Harmony harmony = new Harmony("MechanoidModifications");
		harmony.PatchAll();
	}
}
