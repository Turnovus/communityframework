using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;

namespace CF
{
    [ClassWithPatches("ApplyHediffExtensionPatches")]
    static class HediffExtensionPatches
    {
        // Use this to store context that HediffStage does not have access to
        private static HediffDef viewingDef;
        
        [HarmonyPatch(typeof(HediffGiver))]
        [HarmonyPatch(nameof(HediffGiver.TryApply))]
        public static class TryApply
        {
            [HarmonyPrefix]
            public static bool PreventAddingIfBlocked(ref bool __result, HediffGiver __instance, Pawn pawn)
            {
                foreach (Hediff hediff in pawn.health.hediffSet.hediffs)
                {
                    HediffExtension extension = hediff.def.GetModExtension<HediffExtension>();
                    if (extension == null)
                        continue;

                    if (!extension.HediffGiverCanGive(__instance.hediff, hediff.CurStageIndex))
                    {
                        __result = false;
                        return false;
                    }
                }
                
                return true;
            }
        }
        
        /// <summary>
        /// This patch stores the HediffDef for which HediffDef.SpecialDisplayStats is being invoked, so that we can
        /// reference it in the HediffStage.SpecialDisplayStats patch. This cache is NOT cleared, and must be cleared
        /// by the consumer for safety.
        /// </summary>
        [HarmonyPatch(typeof(HediffDef))]
        [HarmonyPatch(nameof(HediffDef.SpecialDisplayStats))]
        static class HediffDefDisplayStats
        {
            [HarmonyPrefix]
            public static void StoreContext(ref HediffDef __instance) => viewingDef = __instance;
        }
        
        [HarmonyPatch(typeof(HediffStatsUtility))]
        [HarmonyPatch(nameof(HediffStatsUtility.SpecialDisplayStats))]
        static class HediffStageDisplayStats
        {
            [HarmonyPostfix]
            public static IEnumerable<StatDrawEntry> AddBlockedGiverHediffs(IEnumerable<StatDrawEntry> values, Hediff instance)
            {
                
                if (values != null)
                    foreach (StatDrawEntry entry in values)
                        yield return entry;
                
                // This matches vanilla logic of not displaying 
                HediffDef hediffDef = instance?.def ?? viewingDef;
                HediffExtension extension = hediffDef?.GetModExtension<HediffExtension>();
                
                // Clear the viewingDef cache now that we've had a chance to store its value locally
                viewingDef = null;
                
                if (extension?.hediffGiversCannotGiveByStage == null)
                    yield break;
                
                // We don't need to know the stage of the active hediff because vanilla only runs the base method for
                // Hediffs that have exactly one stage. An index of 0 is most vanilla-accurate here.
                List<HediffDef> blockedHediffs = extension.hediffGiversCannotGiveByStage[0];
                if (!blockedHediffs.NullOrEmpty())
                {
                    yield return new StatDrawEntry(
                        StatCategoryDefOf.CapacityEffects,
                        "CF_PreventsHediffGiver".Translate(),
                        blockedHediffs.Select(im => im.label).ToCommaList().CapitalizeFirst(),
                        "CF_PreventsHediffGiver_Desc".Translate(),
                        4050);
                }
            }
        }
    }
}