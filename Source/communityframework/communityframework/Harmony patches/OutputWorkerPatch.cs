using System.Collections.Generic;
using Verse;
using RimWorld;
using HarmonyLib;

namespace CF
{
    /// <summary>
    /// Adds patches that will run before and after any recipe is complete.
    /// Each patch will check the recipe for a <see cref="UseOutputWorkers"/>
    /// extension, and will run the relevant method from each
    /// <see cref="OutputWorker"/> found.
    /// </summary>
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
                ref Precept_ThingStyle precept,
                ref ThingStyleDef style,
                ref int? overrideGraphicIndex
            )
            {
                // Get the extension, quit if none found
                UseOutputWorkers ext =
                    recipeDef.GetModExtension<UseOutputWorkers>();
                if (ext == null || ext.outputWorkers.EnumerableNullOrEmpty())
                    return;

                // Run every pre-craft method
                foreach (OutputWorker o in ext.outputWorkers)
                    o.PreCraft(
                        recipeDef,
                        worker,
                        ingredients,
                        billGiver,
                        ref precept,
                        ref style,
                        ref overrideGraphicIndex
                    );
            }

            public static void Postfix(
                ref IEnumerable<Thing> __result,
                RecipeDef recipeDef,
                Pawn worker,
                List<Thing> ingredients,
                IBillGiver billGiver,
                Precept_ThingStyle precept,
                ThingStyleDef style,
                int? overrideGraphicIndex
            )
            {
                // Stores any new products that each OutputWorker produces, so
                // that they can be finalized later.
                IEnumerable<Thing> newProducts;

                // Get the extension, quit if none found
                UseOutputWorkers ext =
                    recipeDef.GetModExtension<UseOutputWorkers>();
                if (ext == null || ext.outputWorkers.EnumerableNullOrEmpty())
                    return;

                // Run each post-craft method, then finalize any Things that
                // they produce before adding them to the list of products.
                foreach (OutputWorker o in ext.outputWorkers)
                {
                    newProducts = o.PostCraft(
                        __result,
                        recipeDef,
                        worker,
                        ingredients,
                        billGiver,
                        precept,
                        style,
                        overrideGraphicIndex
                    );

                    // If no new products added, move on to the next worker.
                    if (newProducts.EnumerableNullOrEmpty())
                        continue;

                    foreach (Thing t in newProducts)
                    {
                        CommunityRecipeUtility.PostProcessProduct(
                            t,
                            recipeDef,
                            worker,
                            precept,
                            style,
                            overrideGraphicIndex
                        );
                        __result.AddItem(t);
                    }
                }
            }
        }
    }
}
