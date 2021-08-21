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
    /// A base for other <c>HediffComp</c>s to derive from. It is functionally
    /// identical to <c>Verse.HediffComp</c>, except that it has an additional
    /// method, <c>OnHediffAdded</c>, which is called whenever a sibling
    /// <c>Hediff</c> is added to the parent's <c>Pawn</c>.
    /// </summary>
    public abstract class HediffComp_OnHediffAdded : HediffComp
    {
        /// <summary>
        /// Empty method meant to be overriden. This method will run whenever
        /// a sibling <c>Hediff</c> is added to the parent <c>Hediff</c>'s
        /// <c>Pawn</c>.
        /// </summary>
        /// <param name="hediff">The <c>Hediff</c> that was just added.</param>
        public virtual void OnHediffAdded (ref Hediff hediff)
        {
        }
    }
}
