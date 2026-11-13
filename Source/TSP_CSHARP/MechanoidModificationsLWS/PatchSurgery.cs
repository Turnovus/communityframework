using HarmonyLib;
using RimWorld;
using Verse;

namespace MechanoidModificationsLWS;

[HarmonyPatch(typeof(Pawn), "CurrentlyUsableForBills")]
internal class PatchSurgery
{
	private static void Postfix(Pawn __instance, ref bool __result)
	{
		if (!__result && __instance.IsColonyMech && !__instance.Drafted && __instance.GetMechWorkMode() == MechWorkModeDefOf.SelfShutdown)
		{
			__result = true;
		}
	}
}
