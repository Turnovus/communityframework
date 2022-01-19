using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using HarmonyLib;
using Verse;
using RimWorld;

namespace D9Framework
{
    /// <summary>
    /// Patches <see cref="RimWorld.CompFacility"/> to be inactive if the parent <c>Thing</c> has a <c>CompRefuelable</c> which is unfueled.
    /// </summary>
    [ClassWithPatches("Facility Require Fuel Patch", "ApplyFacilityRequireFuel", "D9FSettingsApplyFRF")]
    static class FacilityRequireFuel
    {
        [HarmonyPatch(typeof(CompFacility), nameof(CompFacility.CanBeActive), MethodType.Getter)]
        class FacilityRequireFuelPatch
        {
            [HarmonyPostfix]
            public static void CanBeActivePostfix(ref bool __result, ref CompFacility __instance)
            {
                CompRefuelable fuel = __instance.parent.TryGetComp<CompRefuelable>();
                __result &= (fuel == null || fuel.HasFuel);
            }
        }
    }
}