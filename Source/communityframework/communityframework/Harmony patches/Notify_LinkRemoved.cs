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
    /// This patches the method Notify_LinkRemoved so the CompUnlocksRecipe specific code is executed after the regular Notify_LinkRemoved code is run.
    /// CompUnlocksRecipe in this case checks when link is removed, if the removed facility was the only one of that type, and if so, remove the recipes from the target workbench. 
    /// </summary>
    [HarmonyPatch(typeof(CompAffectedByFacilities), "Notify_LinkRemoved")]
    class Notify_LinkRemoved
    {
        public static void Postfix(Thing thing, CompAffectedByFacilities __instance)
        {
            CompUnlocksRecipe.RemoveRecipe(thing, __instance);
        }
    }
}

