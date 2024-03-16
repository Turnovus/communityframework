using Verse;
using RimWorld;

namespace CF
{
    class IngredientValueGetter_Mass : IngredientValueGetter
    {
        public override float ValuePerUnitOf(ThingDef t)
        {
            return t.BaseMass;
        }
        public override string BillRequirementsDescription(
            RecipeDef r,
            IngredientCount ing
        )
        {
            return "CF_BillRequiresMass".Translate(
                ing.GetBaseCount(),
                ing.filter.Summary
            );
        }
    }
}