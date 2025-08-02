using System.Collections.Generic;
using Verse;

namespace CF
{
    public class HediffExtension : DefModExtension
    {
#pragma warning disable CS0649
        public List<List<HediffDef>> hediffGiversCannotGiveByStage;
#pragma warning restore CS0649

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

        public bool HediffGiverCanGive(HediffDef hediffToCheck, int atParentStage)
        {
            if (hediffGiversCannotGiveByStage.NullOrEmpty())
                return true;

            return !hediffGiversCannotGiveByStage[atParentStage].Contains(hediffToCheck);
        }
    }
}