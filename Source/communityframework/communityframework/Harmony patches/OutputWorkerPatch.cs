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
    [ClassWithPatches("ApplyOutputWorkerPatch")]
    static class OutputWorkerPatch
    {
        [HarmonyPatch(typeof(GenRecipe))]
        [HarmonyPatch(nameof(GenRecipe.MakeRecipeProducts))]
        class MakeRecipeProducts
        {
            public static void Prefix(
                RecipeDef recipeDef,
                Pawn worker,
                List<Thing> ingredients,
                IBillGiver billGiver,
                Precept_ThingStyle precept
            )
            {
                UseOutputWorkers ext =
                    recipeDef.GetModExtension<UseOutputWorkers>();
                if (ext == null) return;

                foreach (OutputWorker o in ext.ActiveWorkers)
                    o.PreCraft(
                        recipeDef,
                        worker,
                        ingredients,
                        billGiver,
                        precept
                    );
            }

            public static void Postfix(
                ref IEnumerable<Thing> __result,
                RecipeDef recipeDef,
                Pawn worker,
                List<Thing> ingredients,
                IBillGiver billGiver,
                Precept_ThingStyle precept
            )
            {
                IEnumerable<Thing> newProducts;

                UseOutputWorkers ext =
                    recipeDef.GetModExtension<UseOutputWorkers>();
                if (ext == null) return;

                foreach (OutputWorker o in ext.ActiveWorkers)
                {
                    newProducts = o.PostCraft(
                        __result,
                        recipeDef,
                        worker,
                        ingredients,
                        billGiver,
                        precept
                    );

                    foreach (Thing t in newProducts)
                    {
                        CommunityRecipeUtility.PostProcessProduct(
                            t,
                            recipeDef,
                            worker,
                            precept
                        );
                        __result.AddItem(t);
                    }
                }
            }
        }
    }
}
