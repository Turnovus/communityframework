using System;
using System.Reflection;
using Verse;
using RimWorld;

namespace CF
{
    /// <summary>
    /// Static helper utility that contains methods pertaining to starting,
    /// doing, and completing bills and recipes.
    /// </summary>
    [StaticConstructorOnStartup]
    static class CommunityRecipeUtility
    {
        /// <summary>
        /// An empty delegate to define the method signature used by
        /// <c>Verse.GenRecipe.PostProcessProduct</c>.
        /// </summary>
        private delegate Thing PostProcessProductSignature(
            Thing product,
            RecipeDef recipeDef,
            Pawn worker,
            Precept_ThingStyle precept=null,
            ThingStyleDef style=null,
            int? overrideGraphicIndex=null);

        /// <summary>
        /// This delegate refers to the private method
        /// <c>Verse.GenRecipe.PostProcessProduct</c>.
        /// </summary>
        private readonly static Delegate postProcessProductDelegate;

        /// <summary>
        /// Sets up delegates refering to private methods.
        /// </summary>
        static CommunityRecipeUtility()
        {
            postProcessProductDelegate = typeof(GenRecipe).GetMethod(
                "PostProcessProduct",
                BindingFlags.NonPublic | BindingFlags.Static
            ).CreateDelegate(typeof(PostProcessProductSignature));
        }

        /// <summary>
        /// Calls the vanilla private method used to finalize crafted items.
        /// This method will set up <c>CompQuality</c> and <c>CompArt</c>,
        /// apply any ideo styles, and will minify the product if possible.
        /// </summary>
        /// <remarks>
        /// This method doesn't do anything other than call a private method
        /// from the vanilla API. Normally, we shouldn't be doing this.
        /// However, this method has no reason to be private in the first
        /// place; it is static and completely stateless.
        /// </remarks>
        /// <param name="product">The crafting product to finalize</param>
        /// <param name="recipeDef">The recipe that created the product</param>
        /// <param name="worker">The pawn doing the recipe</param>
        /// <param name="precept">The pawn's ideo style precept</param>
        /// <param name="style">The style that will be applied to the product.</param>
        /// <param name="overrideGraphicIndex">
        /// Index override for the graphic.
        /// </param>
        /// <returns>A reference to <c>product</c>.</returns>
        public static Thing PostProcessProduct(
            Thing product,
            RecipeDef recipeDef,
            Pawn worker,
            Precept_ThingStyle precept = null,
            ThingStyleDef style=null,
            int? overrideGraphicIndex=null
        )
            => postProcessProductDelegate.DynamicInvoke(
                new object[] {
                    product, recipeDef, worker, precept, style,
                    overrideGraphicIndex
                }
            ) as Thing;
    }
}
