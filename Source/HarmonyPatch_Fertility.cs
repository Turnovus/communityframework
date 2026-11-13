using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;

namespace MonolynAlgaeBall
{
    // FertilityGrid.FertilityAt returns a cached value read from def.fertility,
    // which is fixed. This postfix replaces it with the cultivator's dynamic
    // fertility: 0 when unpowered (growth stops), 1 unfueled, 5 fueled.
    [HarmonyPatch(typeof(FertilityGrid), "FertilityAt")]
    public static class Patch_FertilityAt
    {
        public static void Postfix(FertilityGrid __instance, IntVec3 loc, ref float __result)
        {
            Map map = Traverse.Create(__instance).Field("map").GetValue<Map>();
            List<Thing> things = map.thingGrid.ThingsListAt(loc);
            for (int i = 0; i < things.Count; i++)
            {
                Building_AlgaeBallCultivator cultivator = things[i] as Building_AlgaeBallCultivator;
                if (cultivator != null)
                {
                    __result = cultivator.Fertility;
                    return;
                }
            }
        }
    }
}
