using HarmonyLib;
using RimWorld;
using Verse;

namespace MonolynAlgaeBall
{
    // The algae ball grows inside a sealed, self-lit cultivator, so it should not
    // be subject to the vanilla "plants rest at night" cycle. Force it to never
    // rest, so it grows around the clock.
    [HarmonyPatch(typeof(Plant), "get_Resting")]
    public static class Patch_PlantResting
    {
        public static void Postfix(Plant __instance, ref bool __result)
        {
            if (__instance.def != null && __instance.def.defName == "Plant_AlgaeBall")
                __result = false;
        }
    }
}
