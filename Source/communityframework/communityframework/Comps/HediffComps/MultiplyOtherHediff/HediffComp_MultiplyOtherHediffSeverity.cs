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
    /// When a sibling <c>Hediff</c> is applied, this comp with multiply the
    /// sibling's <c>Severity</c> by a set amount. Can be used to artificially
    /// increase or decrease the magnitude or duration of certain
    /// <c>Hediff</c>s.
    /// </summary>
    class HediffComp_MultiplyOtherHediffSeverity : 
        HediffComp, IHediffComp_OnHediffAdded
    {
        /// <summary>
        /// Pre-cast reference to this comp's corresponding properties,
        /// <c>HediffCompProperties_MultiplyOtherHediff</c>.
        /// </summary>
        public HediffCompProperties_MultiplyOtherHediff Props =>
            (HediffCompProperties_MultiplyOtherHediff)props;

        /// <summary>
        /// Run when another <c>Hediff</c> is added to the parent's
        /// <c>Pawn</c>. Multiplies the other <c>Hediff</c>'s severity by a set
        /// amount, if the other <c>Hediff</c>'s <c>Def</c> is in
        /// <c>Props.affectedHediffs</c>, and if <c>Props.affectsNewHediffs</c>
        /// is <c>true</c>.
        /// </summary>
        /// <param name="hediff">The <c>Hediff</c> being added.</param>
        public void OnHediffAdded(ref Hediff hediff)
        {
            if (Props.affectsNewHediffs)
                TryAdjustOtherHediff(hediff);
        }

        /// <summary>
        /// Run when the parent <see cref="Hediff"/> is applied to a
        /// <see cref="Pawn"/>. If <c>Props.affectsExistingHediff</c> is
        /// <c>true</c>, then this override will go through each health
        /// condition already present on the target <c>Pawn</c>, and, if the
        /// condition's <see cref="HediffDef"/> is in
        /// <c>Props.affectedHediffs</c>, its severity will be multiplied by
        /// <c>Props.multiplier</c>.
        /// </summary>
        /// <param name="dinfo">
        /// The <see cref="DamageInfo"/> that caused the parent condition to be
        /// applied. Unused here.
        /// </param>
        public override void CompPostPostAdd(DamageInfo? dinfo)
        {
            base.CompPostPostAdd(dinfo);

            if (!Props.affectsExistingHediffs)
                return;

            foreach (Hediff otherHediff in Pawn.health.hediffSet.hediffs)
                TryAdjustOtherHediff(otherHediff);
        }

        /// <summary>
        /// Internal helper method used to adjust the severity of other
        /// <see cref="Hediff"/>s where applicable. This method ensures that
        /// the targeted health condition is not the comp's parent condition,
        /// and that the condition's <see cref="HediffDef"/> is contained in
        /// <c>Props.affectedHediff</c> before taking effect, so doing those
        /// comparisons externally is unnecessary.
        /// </summary>
        /// <param name="other">
        /// The <see cref="Hediff"/> to possibly adjust.
        /// </param>
        private void TryAdjustOtherHediff(Hediff other)
        {
            if (other == parent)
                return;
            if (Props.affectedHediffs.Contains(other.def))
                other.Severity =
                    (int)Math.Ceiling(other.Severity * Props.multiplier);
        }
    }
}
