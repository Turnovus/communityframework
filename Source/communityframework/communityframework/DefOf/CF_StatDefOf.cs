using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace CF
{
    /// <summary>
    /// <see cref="Verse.Def"/> cache containing the framework's XML
    /// <see cref="RimWorld.StatDef"/>s for easy reference in C# assemblies.
    /// </summary>
    [DefOf]
    public static class CF_StatDefOf
    {
#pragma warning disable CS0649
        public static StatDef CF_CaravanCapacity;
#pragma warning restore CS0649

        static CF_StatDefOf() =>
            DefOfHelper.EnsureInitializedInCtor(typeof(CF_StatDefOf));
    }
}
