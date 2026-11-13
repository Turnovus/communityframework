using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;

namespace MechanoidModificationsLWS;

[HarmonyPatch(typeof(Pawn), "GetDisabledWorkTypes")]
internal class GetDisabledWorkTypes_Patch
{
	private static void Postfix(ref List<WorkTypeDef> __result, Pawn __instance)
	{
		if (__instance.RaceProps.IsMechanoid)
		{
			if (__instance.health.hediffSet.HasHediff(MechanoidModificationsLWHediffDefOf.GEGEMM_CleaningModule))
			{
				__result.Remove(VanillaUndeclaredWorkTypeDefOf.Cleaning);
			}
			if (__instance.health.hediffSet.HasHediff(MechanoidModificationsLWHediffDefOf.GEGEMM_HaulingModule))
			{
				__result.Remove(WorkTypeDefOf.Hauling);
			}
			if (__instance.health.hediffSet.HasHediff(MechanoidModificationsLWHediffDefOf.GEGEMM_ConstructionModule))
			{
				__result.Remove(WorkTypeDefOf.Construction);
			}
			if (__instance.health.hediffSet.HasHediff(MechanoidModificationsLWHediffDefOf.GEGEMM_SowingModule))
			{
				__result.Remove(WorkTypeDefOf.Growing);
			}
			if (__instance.health.hediffSet.HasHediff(MechanoidModificationsLWHediffDefOf.GEGEMM_PlantCuttingModule))
			{
				__result.Remove(WorkTypeDefOf.PlantCutting);
			}
			if (__instance.health.hediffSet.HasHediff(MechanoidModificationsLWHediffDefOf.GEGEMM_MiningModule))
			{
				__result.Remove(WorkTypeDefOf.Mining);
			}
			if (__instance.health.hediffSet.HasHediff(MechanoidModificationsLWHediffDefOf.GEGEMM_DoctorModule))
			{
				__result.Remove(WorkTypeDefOf.Doctor);
			}
			if (__instance.health.hediffSet.HasHediff(MechanoidModificationsLWHediffDefOf.GEGEMM_FirefighterModule))
			{
				__result.Remove(WorkTypeDefOf.Firefighter);
			}
			if (__instance.health.hediffSet.HasHediff(MechanoidModificationsLWHediffDefOf.GEGEMM_CraftingModule))
			{
				__result.Remove(WorkTypeDefOf.Crafting);
			}
			if (__instance.health.hediffSet.HasHediff(MechanoidModificationsLWHediffDefOf.GEGEMM_SmithingModule))
			{
				__result.Remove(WorkTypeDefOf.Smithing);
			}
			if (__instance.health.hediffSet.HasHediff(MechanoidModificationsLWHediffDefOf.GEGEMM_TailoringModule))
			{
				__result.Remove(VanillaUndeclaredWorkTypeDefOf.Tailoring);
			}
			if (__instance.health.hediffSet.HasHediff(MechanoidModificationsLWHediffDefOf.GEGEMM_CookingModule))
			{
				__result.Remove(VanillaUndeclaredWorkTypeDefOf.Cooking);
			}
			if (__instance.health.hediffSet.HasHediff(MechanoidModificationsLWHediffDefOf.GEGEMM_BasicModule))
			{
				__result.Remove(VanillaUndeclaredWorkTypeDefOf.BasicWorker);
			}
			if (__instance.health.hediffSet.HasHediff(MechanoidModificationsLWHediffDefOf.GEGEMM_HuntingModule))
			{
				__result.Remove(WorkTypeDefOf.Hunting);
			}
			if (__instance.health.hediffSet.HasHediff(MechanoidModificationsLWHediffDefOf.GEGEMM_ArtModule))
			{
				__result.Remove(VanillaUndeclaredWorkTypeDefOf.Art);
			}
			if (ModFinder.AlphaBees && __instance.health.hediffSet.HasHediff(MechanoidModificationsLWModdedHediffDefOf.GEGEMM_BeekeepingModule))
			{
				__result.Remove(ModdedWorkTypeDefOf.RB_BeekeepingWork);
			}
			if (ModFinder.VanillaGeneticsExpanded && __instance.health.hediffSet.HasHediff(MechanoidModificationsLWModdedHediffDefOf.GEGEMM_GeneticsModule))
			{
				__result.Remove(ModdedWorkTypeDefOf.GR_Genetics);
			}
			if (ModFinder.VanillaBooksExpanded && __instance.health.hediffSet.HasHediff(MechanoidModificationsLWModdedHediffDefOf.GEGEMM_BooksModule))
			{
				__result.Remove(ModdedWorkTypeDefOf.VBE_Writing);
			}
			if (ModFinder.VanillaFishingExpanded && __instance.health.hediffSet.HasHediff(MechanoidModificationsLWModdedHediffDefOf.GEGEMM_FishingModule))
			{
				__result.Remove(ModdedWorkTypeDefOf.VCEF_Fishing);
			}
			if (ModFinder.DubsRimatomics && __instance.health.hediffSet.HasHediff(MechanoidModificationsLWModdedHediffDefOf.GEGEMM_NuclearWorkModule))
			{
				__result.Remove(ModdedWorkTypeDefOf.NuclearWork);
			}
		}
	}
}
