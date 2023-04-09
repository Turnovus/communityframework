using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;
using HarmonyLib;

namespace CF
{
    [ClassWithPatches("ApplyCanPlaceBlueprintOverPatch")]
    static class VerifyCanPlaceBlueprintOverPatch
    {
        [HarmonyPatch(typeof(GenConstruct))]
        [HarmonyPatch(nameof(GenConstruct.CanPlaceBlueprintOver))]
        class CanPlaceBlueprintOver
        {
            [HarmonyPostfix]
            public static void ApplyAdditionalConditions(
                ref bool __result,
                BuildableDef newDef,
                ThingDef oldDef
            )
            {
                // FIXME: Remove logging
                // Log.Message((oldDef.defName ?? "null") + " " + __result.ToString());

                if (newDef == null || oldDef == null || newDef == oldDef)
                    return;

                if (__result == false)
                    return;

                CommunityBuildingUtility.EBuildStage buildStage;

                BuildableDef toBuild =
                    CommunityBuildingUtility.GetFullyConstructedDefOf(
                        newDef, out CommunityBuildingUtility.EBuildStage _);

                BuildableDef builtOn =
                    CommunityBuildingUtility.GetFullyConstructedDefOf(
                        oldDef, out buildStage);

                if (builtOn.modExtensions.NullOrEmpty())
                    return;

                foreach (DefModExtension ext in builtOn.modExtensions)
                {
                    if (
                        ext is DefModExtension_CanBlockPlacement blocker &&
                        blocker.BlocksPlacementOf(toBuild, buildStage)
                    )
                    {
                        __result = false;
                        break;
                    }
                }
            }
        }
    }
}
