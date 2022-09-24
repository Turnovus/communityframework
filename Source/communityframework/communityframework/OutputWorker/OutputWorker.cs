using System.Collections.Generic;
using Verse;
using RimWorld;

namespace CF
{
   abstract class OutputWorker
    {
        public abstract void PreCraft(
            RecipeDef recipeDef,
            Pawn worker,
            IEnumerable<Thing> ingredients,
            IBillGiver billGiver,
            Precept_ThingStyle precept
        );

        public abstract IEnumerable<Thing> PostCraft(
            IEnumerable<Thing> products,
            RecipeDef recipeDef,
            Pawn worker,
            IEnumerable<Thing> ingredients,
            IBillGiver billGiver,
            Precept_ThingStyle precept
        );
    }
}
