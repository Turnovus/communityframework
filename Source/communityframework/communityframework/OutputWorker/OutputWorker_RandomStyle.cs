using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace CF
{
    public abstract class OutputWorker_RandomStyle : OutputWorker
    {
#pragma warning disable CS0649
        public bool ideoPreceptOverrides = false;
        public float noStyleChance = 0f;
#pragma warning restore CS0649

        public abstract IEnumerable<ThingStyleChance> StylesFor(RecipeDef recipe);

        public override void PreCraft(
            RecipeDef recipeDef,
            Pawn worker,
            IEnumerable<Thing> ingredients,
            IBillGiver billGiver,
            ref Precept_ThingStyle precept,
            ref ThingStyleDef style,
            ref int? overrideGraphicIndex
        )
        {
            IEnumerable<ThingStyleChance> availableStyles = StylesFor(recipeDef);

            if (availableStyles.EnumerableNullOrEmpty())
                return;

            if (ideoPreceptOverrides && precept != null && style != null)
                return;

            if (Rand.Chance(noStyleChance))
                return;

            precept = null;
            style = availableStyles.RandomElementByWeight(s => s.Chance).StyleDef;
        }
    }

    public class OutputWorker_RandomStyleFromList : OutputWorker_RandomStyle
    {
#pragma warning disable CS0649
        public IEnumerable<ThingStyleChance> randomStyles;
#pragma warning restore CS0649

        public override IEnumerable<ThingStyleChance> StylesFor(RecipeDef recipe) => randomStyles;
    }

    public class OutputWorker_RandomStyleFromDef : OutputWorker_RandomStyle
    {
        public override IEnumerable<ThingStyleChance> StylesFor(RecipeDef recipe)
        {
            return recipe?.ProducedThingDef?.randomStyle;
        }
    }
}
