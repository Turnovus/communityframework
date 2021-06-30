using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;
using HarmonyLib;

namespace CF
{
    /// <summary>
    /// This patches the method Notify_NewLink so the CompUnlocksRecipe specific code is executed after the regular Notify_NewLink code is run.
    /// CompUnlocksRecipe in this case checks when a new link is created. If this new link is one with a new unique facility, and the added recipes are also not yet added to the workbench, they will be added.
    /// </summary>
    [HarmonyPatch(typeof(CompAffectedByFacilities), "Notify_NewLink")]
    class Notify_NewLink
    {
        public static void Postfix(Thing facility, CompAffectedByFacilities __instance)
        {
            CompUnlocksRecipe.AddRecipe(facility, __instance);
        }
    }
}
