using System.Collections.Generic;
using Verse;

namespace CF
{
    /// <summary>
    /// The <c>HediffProperties</c> used to provide instructions for
    /// <c>HediffComp_MultiplyOtherHediffDuration</c>. The <c>compClass</c>
    /// must be manually set to either
    /// <c>HediffComp_MultiplyOtherHediffDuration</c> or
    /// <c>HediffComp_MultiplyOtherHediffSeverity</c>.
    /// </summary>
    class HediffCompProperties_MultiplyOtherHediff :
        HediffCompProperties
    {
        /// <summary>
        /// A list of <c>HediffDef</c>s. If a sibling <c>Hediff</c> is added
        /// with one of these <c>Def</c>s, then it will have its
        /// <c>HediffComp_Disappears.ticksToDisappear</c> multiplied by
        /// <c>multiplier</c>.
        /// </summary>
        public List<HediffDef> affectedHediffs = new List<HediffDef>();
        /// <summary>
        /// The value that affected <c>Hediff</c>s will have their durations
        /// multiplied by.
        /// </summary>
        public float multiplier = 1.0f;
        /// <summary>
        /// If <c>true</c>, then the comp will affect newly-added
        /// <c>Hediff</c>s. Does not affect conditions that are already
        /// present.
        /// </summary>
        public bool affectsNewHediffs = true;
        /// <summary>
        /// If <c>true</c>, then the comp will affect <c>Hediff</c>s that were
        /// already present when the parent condition was applied. Does not
        /// affect other conditions that were added after the parent.
        /// </summary>
        public bool affectsExistingHediffs = false;
    }
}
