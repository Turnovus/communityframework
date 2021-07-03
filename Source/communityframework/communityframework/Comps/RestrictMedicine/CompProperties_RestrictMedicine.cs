using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace CF
{
    class CompProperties_RestrictMedicine : CompProperties
    {
        public CompProperties_RestrictMedicine()
        {
            this.compClass = typeof(CompRestrictMedicine);
        }

        public List<PawnKindDef> restrictTo;
    }
}
