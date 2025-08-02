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
            
        }
    }
}