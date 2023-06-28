using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace CF
{
    /// <summary>
    /// A base <see cref="OutputWorker"/> for making recipes that randomize the style of crafted
    /// products.
    /// </summary>
    public abstract class OutputWorker_RandomStyle : OutputWorker
    {
#pragma warning disable CS0649
        /// <summary>
        /// If <c>true</c>, then any ideological styles that the crafter has will override the
        /// randomly-assigned style, assuming that any such style exists for the crafted product.
        /// </summary>
        public bool ideoPreceptOverrides = false;
        /// <summary>
        /// A percent chance that the crafted product will have its default appearence, instead of
        /// being given one randomly.
        /// </summary>
        public float noStyleChance = 0f;
#pragma warning restore CS0649

        /// <summary>
        /// A list of styles to be chosen from when randomly applying a style to the product of a
        /// given recipe.
        /// </summary>
        /// <param name="recipe">
        /// The recipe being performed.
        /// </param>
        /// <returns>
        /// A list of styles that may be randomly applied to the products of <c>recipe</c>
        /// </returns>
        public abstract IEnumerable<ThingStyleChance> StylesFor(RecipeDef recipe);

        /// <inheritdoc/>
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

    /// <summary>
    /// An <see cref="OutputWorker"/> that gives a recipe's products a random style from a
    /// predefined list.
    /// </summary>
    public class OutputWorker_RandomStyleFromList : OutputWorker_RandomStyle
    {
#pragma warning disable CS0649
        /// <summary>
        /// A list of styles to be chosen from randomly.
        /// </summary>
        public IEnumerable<ThingStyleChance> randomStyles;
#pragma warning restore CS0649

        /// <inheritdoc/>
        public override IEnumerable<ThingStyleChance> StylesFor(RecipeDef recipe) => randomStyles;
    }

    /// <summary>
    /// An <see cref="OutputWorker"/> that gives a recipe's products a random style the product's
    /// own list of randomly-selected styles.
    /// </summary>
    public class OutputWorker_RandomStyleFromDef : OutputWorker_RandomStyle
    {
        /// <inheritdoc/>
        public override IEnumerable<ThingStyleChance> StylesFor(RecipeDef recipe)
        {
            return recipe?.ProducedThingDef?.randomStyle;
        }
    }
}
