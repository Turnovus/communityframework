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
            int facilityCount = 0; // Count of similarly connected facilities

            if (__instance.parent.GetComp<CompUnlocksRecipe>() != null)
            {
                CompUnlocksRecipe compUnlocksRecipe = __instance.parent.GetComp<CompUnlocksRecipe>();
                CompProperties_UnlocksRecipe props = compUnlocksRecipe.Props;

                CompAffectedByFacilities compAffectedByFacilities = __instance.parent.GetComp<CompAffectedByFacilities>();

                List<Thing> connectedFacilities = compAffectedByFacilities.LinkedFacilitiesListForReading;

                if (connectedFacilities != null)
                {
                    foreach (Thing singleFacility in connectedFacilities)
                    {
                        if (props.targetFacility.defName == singleFacility.def.defName) // If the defName of the facility is equal to the defName of the target defName from the XML, add 1.
                        {
                            facilityCount++;
                        }
                    }
                    if (facilityCount < 1) // This code is executed after the facility is removed, meaning the facilityCount should be 0 if it was the only facility of that type.
                    {
                        foreach (RecipeDef recipe in props.recipes)
                        {
                            __instance.parent.def.AllRecipes.Remove(recipe);
                        }
                    }
                }
            }
        }
    }
}

