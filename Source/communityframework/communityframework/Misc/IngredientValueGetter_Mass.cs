using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace D9Framework
{
    class IngredientValueGetter_Mass : IngredientValueGetter
    {
        public override float ValuePerUnitOf(ThingDef t)
        {
            return t.BaseMass;
        }
        public override string BillRequirementsDescription(RecipeDef r, IngredientCount ing)
        {
            return "D9F_BillRequiresMass".Translate(ing.GetBaseCount(), ing.filter.Summary);
        }
    }
}