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
        /// <summary>
        /// The additional mass that this creature can carry when part of a
        /// caravan.
        /// </summary>
        public static StatDef CF_CaravanCapacity;

        /// <summary>
        /// How frequently someone will suffer a mental break while their mood is below their mental break threshold.
        /// This does not affect mental breaks from other sources.
        /// </summary>
        public static StatDef CF_RandomBreakFrequency;
#pragma warning restore CS0649

        static CF_StatDefOf() =>
            DefOfHelper.EnsureInitializedInCtor(typeof(CF_StatDefOf));
    }
}
