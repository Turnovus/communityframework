using System.Collections.Generic;
using System.Linq;
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

                // Okay, so, LINQ is really weird. The contents of __result are things that were
                // made with the method ThingMaker.MakeThing. MakeThing is called inside of the
                // method we're patching - that's how all the Things got into __result to begin
                // with. But MakeThing technically hasn't been *run* yet. It gets run when the
                // contents of the list are first accessed, not when they are first appended.
                //
                // Because of some weird voodoo that I can't quite discern, that means we can't
                // actually directly  modify the Things inside of __result here. All of the MakeThings
                // inside of __result get evaluated when we enumerate over it here, but then they
                // get evaluated again after the patch when the crafter pawn gets the job to haul
                // the crafted Things to storage.
                //
                // I have no idea what's going on, but I think that if we don't assign a value to
                // __result, then any changes we make to its contents won't stick. So we copy the
                // contents of __result to a new IEnumerable, specifically using ToList to force
                // MakeThing to run. Then, at the end of the patch, we assign our copy of the
                // original IEnumerable, to the original IEnumerable. This ensures that *our*
                // MakeThing evaluations are the final ones.
                //
                // We're also doing this after checking that the RecipeDef is actually using
                // OutputWorkers. Because even though I don't know if this behavior counts as
                // invasive, I still feel safer this way.
                //
                // I don't know, and I give up on trying to understand or to care. just read this:
                // https://stackoverflow.com/questions/1168944/how-to-tell-if-an-ienumerablet-is-subject-to-deferred-execution
                IEnumerable<Thing> products = __result.ToList();

                // Run each post-craft method, then finalize any Things that
                // they produce before adding them to the list of products.
                foreach (OutputWorker o in ext.outputWorkers)
                {
                    newProducts = o.PostCraft(
                        products,
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
                        products.AddItem(t);
                    }
                }

                __result = products;
            }
        }
    }
}
