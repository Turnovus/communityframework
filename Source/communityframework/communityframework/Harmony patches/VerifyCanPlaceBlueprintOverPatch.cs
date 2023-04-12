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
    /// <summary>
    /// This patch modifies the player's ability to place blueprints over other
    /// <see cref="Thing"/>s that occupy any of the same cells as the
    /// blueprint.
    /// </summary>
    [ClassWithPatches("ApplyCanPlaceBlueprintOverPatch")]
    static class VerifyCanPlaceBlueprintOverPatch
    {
        /// <summary>
        /// This patch modifies the player's ability to place blueprints over
        /// other <see cref="Thing"/>s that occupy any of the same cells as the
        /// blueprint.
        /// </summary>
        [HarmonyPatch(typeof(GenConstruct))]
        [HarmonyPatch(nameof(GenConstruct.CanPlaceBlueprintOver))]
        class CanPlaceBlueprintOver
        {
            /// <summary>
            /// After all of the vanilla checks have passed successfully, this
            /// postfix checks that any additional conditions added by the
            /// framework have also been met.
            /// </summary>
            /// <param name="__result">
            /// Initially stores the result of the original method. If this
            /// value is <c>true</c>, but the new conditions defined by the
            /// framework are not met, then it will be changed to <c>false</c>.
            /// </param>
            /// <param name="newDef">
            /// The <see cref="ThingDef"/> of the building that the player is
            /// attempting to place.
            /// </param>
            /// <param name="oldDef">
            /// The <see cref="ThingDef"/> that the game is checking if
            /// <c>newDef</c> can be placed on top of.
            /// </param>
            [HarmonyPostfix]
            public static void ApplyAdditionalConditions(
                ref bool __result,
                BuildableDef newDef,
                ThingDef oldDef
            )
            {
                // If any value is null, stop
                if (newDef == null || oldDef == null || newDef == oldDef)
                    return;

                // If the vanilla overlap checks failed, stop
                if (__result == false)
                    return;


                BuildableDef toBuild =
                    CommunityBuildingUtility.GetFullyConstructedDefOf(
                        newDef,
                        out CommunityBuildingUtility.EBuildableDefStage _);

                BuildableDef builtOn =
                    CommunityBuildingUtility.GetFullyConstructedDefOf(
                        oldDef,
                        out CommunityBuildingUtility
                            .EBuildableDefStage buildStage);

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
