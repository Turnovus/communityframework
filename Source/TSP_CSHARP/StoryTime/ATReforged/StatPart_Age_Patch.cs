using HarmonyLib;
using RimWorld;
using Verse;

namespace ATReforged;

internal class StatPart_Age_Patch
{
	[HarmonyPatch(typeof(StatPart_Age), "ActiveFor")]
	public class ActiveFor_Patch
	{
		[HarmonyPostfix]
		public static void Listener(Pawn pawn, ref bool __result)
		{
			if (__result && pawn.ageTracker.CurLifeStage.defName == "BeanManAdultStage")
			{
				__result = false;
			}
		}
	}
}
