using System;
using RimWorld;
using Verse;
using Verse.AI;

namespace MechanoidModificationsLWS
{
    /// <summary>
    /// Adds "Extract modifications" when the player right‑clicks a mechanoid corpse.
    /// </summary>
    public class FloatMenuOptionProvider_ExtractModifications : FloatMenuOptionProvider
    {
        /* ────────────────────  required policy flags  ─────────────────── */

        // Available to drafted and undrafted pawns.
        protected override bool Drafted => true;
        protected override bool Undrafted => true;

        // We only make one option, so multiselect isn’t meaningful.
        protected override bool Multiselect => false;

        // Pawn must have Manipulation capability (matches old code path).
        protected override bool RequiresManipulation => true;

        /* ──────────────────────  option generation  ───────────────────── */

        protected override FloatMenuOption GetSingleOptionFor(
            Thing clickedThing, FloatMenuContext context)
        {
            // 1)  Is the clicked thing a mechanoid corpse?
            if (clickedThing is not Corpse corpse) return null;
            if (!ModsConfig.BiotechActive) return null;
            if (corpse.InnerPawn?.RaceProps?.IsMechanoid != true) return null;

            // 2)  Will the selected pawn (first in the list) be able to do the work?
            Pawn pawn = context.FirstSelectedPawn;
            if (pawn == null) return null;
            if (!pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation)) return null;

            // 3)  Build the float‑menu option.
            string label = "ExtractModifications".Translate();
            Action action = () =>
            {
                Job job = JobMaker.MakeJob(MechanoidModificationsJobDefOf.RemoveModification,
                                            corpse);
                pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
            };

            return FloatMenuUtility.DecoratePrioritizedTask(
                       new FloatMenuOption(label, action),
                       pawn,
                       corpse);
        }
    }
}
