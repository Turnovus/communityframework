using Verse;

namespace CF
{
    /// <summary>
    /// The base <c>HediffProperties</c> used to provide instructions for
    /// <c>HediffComp_PainFromSeverity</c>
    /// </summary>
    class HediffCompProperties_SeverityFromPain : HediffCompProperties
    {
        /// <summary>
        /// If this is set to <c>true</c>, then the <c>Pawn</c>'s current pain
        /// will be divided by their maximum pain shock threshold. If
        /// <c>false</c>, it will always be the exact value.
        /// </summary>
        public bool usePainThreshold = true;
        /// <summary>
        /// The minimum severity that can be applied to the parent <c>Hediff</c>. This prevents the
        /// hediff from being auto-culled when updated.
        /// </summary>
        public float minSeverity = 0.001f;

        /// <summary>
        /// Boilerplate constructor, sets the comp's class to
        /// <c>HediffComp_SeverityFromPain</c>.
        /// </summary>
        public HediffCompProperties_SeverityFromPain() =>
            compClass = typeof(HediffComp_SeverityFromPain);
    }
}
