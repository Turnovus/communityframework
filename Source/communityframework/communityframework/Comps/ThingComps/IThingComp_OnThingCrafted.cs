﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace CF
{
    /// <summary>
    /// An Interface that allows <c>ThingComp</c>s to run a method,
    /// <c>Notify_ThingCrafted</c>, whenever its parent <c>Thing</c> is
    /// involved in a recipe. Can be used on Races (crafter, surgery target), 
    /// Things (inputs and outputs), and buildings (workbenches).
    /// </summary>
    public interface IThingComp_OnThingCrafted
    {
        /// <summary>
        /// This method is called whenever the parent <c>Thing</c> is involved
        /// in a recipe. The items crafted are passed by reference, and can
        /// therefore be modified.
        /// </summary>
        /// <remarks>
        /// No distinction is made here as to whether the parent is the
        /// bill-giver, the bill-doer, or an ingredient. Such checks should
        /// therefore be made in the method body (i.e. if <c>parent != worker
        /// </c> then return).
        /// </remarks>
        /// <param name="products">A collection of <c>Thing</c>s that were
        /// crafted.</param>
        /// <param name="recipeDef">The <c>Def</c> of the crafting recipe.
        /// </param>
        /// <param name="worker">The <c>Pawn</c> that is doing the crafting.
        /// </param>
        /// <param name="ingredients">A list of <c>Thing</c>s that will be
        /// consumed by the recipe</param>
        /// <param name="dominantIngredient">The <c>Thing</c> marked as the
        /// recipe's dominant ingredient.</param>
        /// <param name="billGiver">The <c>IBillGiver</c> (usually a workbench
        /// for crafting recipes, and a <c>Pawn</c> for surgical recipes) that
        /// the recipe is being performed on.</param>
        /// <param name="precept">The style given to the resulting
        /// <c>Thing</c>s.</param>
        void Notify_ThingCrafted(
            ref IEnumerable<Thing> products,
            RecipeDef recipeDef,
            Pawn worker,
            List<Thing> ingredients,
            Thing dominantIngredient,
            IBillGiver billGiver,
            Precept_ThingStyle precept = null);
    }
}
