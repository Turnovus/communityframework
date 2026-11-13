using System;
using Verse;
using RimWorld;

namespace MonolynAlgaeBall
{
    public class CompProperties_AlgaeBall : CompProperties
    {
        public ThoughtDef badThought;         // granted to non-Monolyn (-5, "I ate an algae ball!")
        public ThoughtDef monolynGoodThought; // granted to Monolyn (e.g. AteFineMeal for the pickle)

        public CompProperties_AlgaeBall()
        {
            compClass = typeof(CompAlgaeBall);
        }
    }

    // Grants a race-conditional thought when the algae ball (or its pickle) is eaten.
    public class CompAlgaeBall : ThingComp
    {
        private static GeneDef monolynGene;

        private static GeneDef MonolynGene
        {
            get
            {
                if (monolynGene == null)
                    monolynGene = DefDatabase<GeneDef>.GetNamedSilentFail("BlackBlood");
                return monolynGene;
            }
        }

        public CompProperties_AlgaeBall Props
        {
            get { return (CompProperties_AlgaeBall)props; }
        }

        public override void PostIngested(Pawn ingester)
        {
            base.PostIngested(ingester);
            if (ingester == null)
                return;

            if (IsMonolyn(ingester))
            {
                if (Props.monolynGoodThought != null)
                    ingester.needs.mood.thoughts.memories.TryGainMemory(Props.monolynGoodThought);
            }
            else
            {
                if (Props.badThought != null)
                    ingester.needs.mood.thoughts.memories.TryGainMemory(Props.badThought);
            }
        }

        private static bool IsMonolyn(Pawn p)
        {
            GeneDef g = MonolynGene;
            return g != null && p.genes != null && p.genes.HasActiveGene(g);
        }
    }
}
