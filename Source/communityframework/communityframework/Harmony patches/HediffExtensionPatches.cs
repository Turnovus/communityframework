using HarmonyLib;
using Verse;

namespace CF
{
    [ClassWithPatches("ApplyHediffExtensionPatches")]
    public class HediffExtensionPatches
    {
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
    }
}