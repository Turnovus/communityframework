using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace D9Framework
{
    /// <summary>
    /// Standard RimWorld <c>DefOf</c> class for use in various location within the mod. Not intended for external use, but can be used by others.
    /// </summary>
    [DefOf]
    public static class D9FrameworkDefOf
    {
#pragma warning disable CS0649
        #region StatDefs
        public static StatDef HealingRateFactor;
        public static StatDef BleedRateFactor;
        #endregion StatDefs
#pragma warning restore CS0649

        static D9FrameworkDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(D9FrameworkDefOf));
        }
    }
}
