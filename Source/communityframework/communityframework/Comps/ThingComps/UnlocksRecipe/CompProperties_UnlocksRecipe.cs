using System.Collections.Generic;
using RimWorld;
using Verse;

namespace CF
{
    /// <summary>
    /// The properties class for <see cref="CompUnlocksRecipe"/>. Used to
    /// define which recipes are unlocked, and by what facilities.
    /// </summary>
    public class CompProperties_UnlocksRecipe : CompProperties
    {
        /// <summary>
        /// Used to store a single facility that is able to unlock the listed
        /// recipes, and the minimum quality of the facility required for the
        /// recipe to be unlocked.
        /// </summary>
        public class LinkableFacilities
        {
            /// <summary>
            /// Thing <see cref="ThingDef"/> of the facility that unlocks the
            /// given list of recipes.
            /// </summary>
            public ThingDef targetFacility;
            /// <summary>
            /// The minimum quality that <see cref="targetFacility"/> must be
            /// for it to be able to unlock the listed recipes.
            /// </summary>
            public QualityCategory minQuality;
        }
        /// <summary>
        /// The list of facilities that unlock the recipes defined in
        /// <see cref="recipes"/>, and the minimum
        /// quality that the facility must be to do so.
        /// </summary>
        public List<LinkableFacilities> linkableFacilities;
        /// <summary>
        /// The recipes that will become available when one of the facility
        /// requirements defined by <see cref="linkableFacilities"/> is met.
        /// </summary>
        public List<RecipeDef> recipes;

        /// <summary>
        /// Constructor, automatically initializes <c>compClass</c> to
        /// <see cref="CompUnlocksRecipe"/>.
        /// </summary>
        public CompProperties_UnlocksRecipe() => compClass = typeof(CompUnlocksRecipe);
        
    }
}
