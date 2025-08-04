using System.Collections.Generic;
using Verse;

namespace CF
{
    /// <summary>
    /// A <see cref="DefModExtension"/> that extends the functionality of <see cref="HediffDef"/>s
    /// </summary>
    public class HediffExtension : DefModExtension
    {
#pragma warning disable CS0649
        /// <summary>
        /// Collections of <see cref="HediffDef"/>s that <see cref="HediffGiver"/>s cannot give the parent of the
        /// instance <see cref="Hediff"/>, grouped by stage. If defined, the length of this list should match the
        /// number of <see cref="HediffStage"/>s defined in the parent def.
        /// </summary>
        /// <remarks>
        /// These conditions will be included in the parent's display stats, both in the health ITab tooltip and in the
        /// full stat display pane. However, this will only apply if the parent def defines exactly one
        /// <see cref="HediffStage"/>. This is based on vanilla behavior.
        /// </remarks>
        public List<List<HediffDef>> hediffGiversCannotGiveByStage;
#pragma warning restore CS0649
        
        /// <inheritdoc/>
        public override void ResolveReferences(Def parentDef)
        {
            if (!(parentDef is HediffDef hediffDef))
            {
                ULog.Error("CF.HediffExtension applied to invalid def " + parentDef);
                return;
            }
            
            if (!hediffGiversCannotGiveByStage.NullOrEmpty() && hediffGiversCannotGiveByStage.Count != hediffDef.stages.Count)
                ULog.Error("Error loading " + hediffDef +
                           ", hediffGiversCannotGiveByStage defined, but does not match length of hediffDef.stages.");
        }
        
        /// <summary>
        /// Whether the <see cref="Hediff"/> instantiated from the parent <see cref="HediffDef"/> should allow a given
        /// <see cref="HediffDef"/> to be applied via <see cref="HediffGiver"/>, or if the condition should be blocked.
        /// </summary>
        /// <param name="hediffToCheck">
        /// The <see cref="HediffDef"/> of the condition that a <see cref="HediffGiver"/> is attempting to apply.
        /// </param>
        /// <param name="atParentStage">
        /// The index of the parent <see cref="Hediff"/>'s current <see cref="HediffStage"/>.
        /// </param>
        /// <returns></returns>
        public bool HediffGiverCanGive(HediffDef hediffToCheck, int atParentStage)
        {
            if (hediffGiversCannotGiveByStage.NullOrEmpty())
                return true;

            return !hediffGiversCannotGiveByStage[atParentStage].Contains(hediffToCheck);
        }
    }
}