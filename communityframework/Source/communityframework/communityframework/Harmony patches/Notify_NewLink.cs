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
            int facilityCount = 0; // Count of similarly connected facilities
            bool alreadyContainsRecipe = false; // If the target workbench already contains a recipe from the recipe list that should be added on the link creation.

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

                    if (facilityCount == 1) // This makes sure the code below is only executed on the first facility of the same type.
                    {
                        foreach (RecipeDef recipe in props.recipes) // Check if any of the recipes already exist.
                        {
                            if (__instance.parent.def.AllRecipes.Contains(recipe))
                            {
                                alreadyContainsRecipe = true;
                            }
                        }
                        if (alreadyContainsRecipe == false)
                        {
                            __instance.parent.def.AllRecipes.AddRange(props.recipes);
                        }
                        else
                        {
                            Log.Error("One of the recipes added through CompProperties_UnlockRecipe already exists. Cancelling patch...");
                        }
                    }
                }
            }
        }
    }
}
