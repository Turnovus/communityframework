using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace CF
{
    class CompUnlocksRecipe : CompAffectedByFacilities
    {
        public CompProperties_UnlocksRecipe Props
        {
            get
            {
                return (CompProperties_UnlocksRecipe)this.props;
            }
        }
    }   
}
