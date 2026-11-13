using System.Collections.Generic;
using RimWorld;
using Verse;

namespace MechanoidModificationsLWS;

public static class SkillsHandler
{
	public static int Level(Pawn pawn)
	{
		return pawn.RaceProps.mechFixedSkillLevel + (int)pawn.GetStatValue(MechanoidModificationsLWStatDefOf.MechAdditionalSkillLevel);
	}

	public static List<WorkTypeDef> Work(Pawn pawn)
	{
		List<WorkTypeDef> list = new List<WorkTypeDef>();
		foreach (WorkTypeDef mechEnabledWorkType in pawn.RaceProps.mechEnabledWorkTypes)
		{
			list.Add(mechEnabledWorkType);
		}
		if (pawn.health.hediffSet.HasHediff(MechanoidModificationsLWHediffDefOf.GEGEMM_CleaningModule) && !list.Contains(VanillaUndeclaredWorkTypeDefOf.Cleaning))
		{
			list.Add(VanillaUndeclaredWorkTypeDefOf.Cleaning);
		}
		if (pawn.health.hediffSet.HasHediff(MechanoidModificationsLWHediffDefOf.GEGEMM_HaulingModule) && !list.Contains(WorkTypeDefOf.Hauling))
		{
			list.Add(WorkTypeDefOf.Hauling);
		}
		if (pawn.health.hediffSet.HasHediff(MechanoidModificationsLWHediffDefOf.GEGEMM_ConstructionModule) && !list.Contains(WorkTypeDefOf.Construction))
		{
			list.Add(WorkTypeDefOf.Construction);
		}
		if (pawn.health.hediffSet.HasHediff(MechanoidModificationsLWHediffDefOf.GEGEMM_SowingModule) && !list.Contains(WorkTypeDefOf.Growing))
		{
			list.Add(WorkTypeDefOf.Growing);
		}
		if (pawn.health.hediffSet.HasHediff(MechanoidModificationsLWHediffDefOf.GEGEMM_PlantCuttingModule) && !list.Contains(WorkTypeDefOf.PlantCutting))
		{
			list.Add(WorkTypeDefOf.PlantCutting);
		}
		if (pawn.health.hediffSet.HasHediff(MechanoidModificationsLWHediffDefOf.GEGEMM_MiningModule) && !list.Contains(WorkTypeDefOf.Mining))
		{
			list.Add(WorkTypeDefOf.Mining);
		}
		if (pawn.health.hediffSet.HasHediff(MechanoidModificationsLWHediffDefOf.GEGEMM_DoctorModule) && !list.Contains(WorkTypeDefOf.Doctor))
		{
			list.Add(WorkTypeDefOf.Doctor);
		}
		if (pawn.health.hediffSet.HasHediff(MechanoidModificationsLWHediffDefOf.GEGEMM_FirefighterModule) && !list.Contains(WorkTypeDefOf.Firefighter))
		{
			list.Add(WorkTypeDefOf.Firefighter);
		}
		if (pawn.health.hediffSet.HasHediff(MechanoidModificationsLWHediffDefOf.GEGEMM_CraftingModule) && !list.Contains(WorkTypeDefOf.Crafting))
		{
			list.Add(WorkTypeDefOf.Crafting);
		}
		if (pawn.health.hediffSet.HasHediff(MechanoidModificationsLWHediffDefOf.GEGEMM_SmithingModule) && !list.Contains(WorkTypeDefOf.Smithing))
		{
			list.Add(WorkTypeDefOf.Smithing);
		}
		if (pawn.health.hediffSet.HasHediff(MechanoidModificationsLWHediffDefOf.GEGEMM_TailoringModule) && !list.Contains(VanillaUndeclaredWorkTypeDefOf.Tailoring))
		{
			list.Add(VanillaUndeclaredWorkTypeDefOf.Tailoring);
		}
		if (pawn.health.hediffSet.HasHediff(MechanoidModificationsLWHediffDefOf.GEGEMM_CookingModule) && !list.Contains(VanillaUndeclaredWorkTypeDefOf.Cooking))
		{
			list.Add(VanillaUndeclaredWorkTypeDefOf.Cooking);
		}
		if (pawn.health.hediffSet.HasHediff(MechanoidModificationsLWHediffDefOf.GEGEMM_BasicModule) && !list.Contains(VanillaUndeclaredWorkTypeDefOf.BasicWorker))
		{
			list.Add(VanillaUndeclaredWorkTypeDefOf.BasicWorker);
		}
		if (pawn.health.hediffSet.HasHediff(MechanoidModificationsLWHediffDefOf.GEGEMM_HuntingModule) && !list.Contains(WorkTypeDefOf.Hunting))
		{
			list.Add(WorkTypeDefOf.Hunting);
		}
		if (pawn.health.hediffSet.HasHediff(MechanoidModificationsLWHediffDefOf.GEGEMM_ArtModule) && !list.Contains(VanillaUndeclaredWorkTypeDefOf.Art))
		{
			list.Add(VanillaUndeclaredWorkTypeDefOf.Art);
		}
		if (ModFinder.AlphaBees && pawn.health.hediffSet.HasHediff(MechanoidModificationsLWModdedHediffDefOf.GEGEMM_BeekeepingModule) && !list.Contains(ModdedWorkTypeDefOf.RB_BeekeepingWork))
		{
			list.Add(ModdedWorkTypeDefOf.RB_BeekeepingWork);
		}
		if (ModFinder.VanillaGeneticsExpanded && pawn.health.hediffSet.HasHediff(MechanoidModificationsLWModdedHediffDefOf.GEGEMM_GeneticsModule) && !list.Contains(ModdedWorkTypeDefOf.GR_Genetics))
		{
			list.Add(ModdedWorkTypeDefOf.GR_Genetics);
		}
		if (ModFinder.VanillaBooksExpanded && pawn.health.hediffSet.HasHediff(MechanoidModificationsLWModdedHediffDefOf.GEGEMM_BooksModule) && !list.Contains(ModdedWorkTypeDefOf.VBE_Writing))
		{
			list.Add(ModdedWorkTypeDefOf.VBE_Writing);
		}
		if (ModFinder.VanillaFishingExpanded && pawn.health.hediffSet.HasHediff(MechanoidModificationsLWModdedHediffDefOf.GEGEMM_FishingModule) && !list.Contains(ModdedWorkTypeDefOf.VCEF_Fishing))
		{
			list.Add(ModdedWorkTypeDefOf.VCEF_Fishing);
		}
		if (ModFinder.DubsRimatomics && pawn.health.hediffSet.HasHediff(MechanoidModificationsLWModdedHediffDefOf.GEGEMM_NuclearWorkModule) && !list.Contains(ModdedWorkTypeDefOf.NuclearWork))
		{
			list.Add(ModdedWorkTypeDefOf.NuclearWork);
		}
		return list;
	}
}
