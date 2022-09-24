using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Verse;
using RimWorld;

namespace CF
{
    [StaticConstructorOnStartup]
    static class CommunityRecipeUtility
    {
        private static Delegate postProcessProductDelegate;

        static CommunityRecipeUtility()
        {
            postProcessProductDelegate = typeof(GenRecipe).GetMethod(
                "PostProcessProduct",
                BindingFlags.NonPublic | BindingFlags.Static
            ).CreateDelegate(typeof(Delegate));
        }

        public static Thing PostProcessProduct(
            Thing product,
            RecipeDef recipeDef,
            Pawn worker,
            Precept_ThingStyle precept = null
        )
            => postProcessProductDelegate.DynamicInvoke(
                new object[] { product, recipeDef, worker, precept }
            ) as Thing;
            //=> postProcessProductInfo.Invoke(
            //    null, new object[] { product, recipeDef, worker, precept }
            //) as Thing;

    }
}
