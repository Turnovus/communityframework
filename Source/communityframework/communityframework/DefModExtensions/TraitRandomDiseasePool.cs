using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace CF
{
    /// <summary>
    /// A <c>DefModExtension</c> that allows for a trait to randomly cause
    /// diseases from a pre-defined list, regardless of the biome that the
    /// pawn is currently situated in.
    /// </summary>
    class TraitRandomDiseasePool : DefModExtension
    {
        /// <summary>
        /// Internal helper class, represents the collection of 
        /// hediff-causing incidents that can be applied by each degree of the
        /// affected trait.
        /// </summary>
        public class DiseasePool
        {
            /// <summary>
            /// The mean time, in days before a hediff-causing incident occurs.
            /// </summary>
            public float mtbDiseaseDays = 15f;
            /// <summary>
            /// A dictionary consisting of hediff-causing incidents as keys,
            /// and their randomness weight as values.
            /// </summary>
            public Dictionary<IncidentDef, int> diseases;
            /// <summary>
            /// The required trait degree for this incident pool to be used.
            /// </summary>
            public int degree = 0;

            /// <summary>
            /// Method that is called when
            /// <c>Pawn_HealthTracker.HealthTick</c> decides that it is time
            /// for a disease-causing incident to occur, based on
            /// <c>mtbDiseaseDays</c>.
            /// </summary>
            /// <param name="pawn">The pawn targeted by the incident.</param>
            public void CauseIncidentFromPool(Pawn pawn)
            {
                //Selects a random incident from our pool, using the specified
                //weights.
                IncidentDef incidentDef = diseases.Keys.
                    RandomElementByWeightWithFallback(
                        d => diseases[d]
                        );
                if (incidentDef == null)
                    return;
                //Used to store the message that is sent when the disease is
                //blocked, i.e. by penoxycyline
                string blockedInfo;
                //Make sure that the incident being caused is actually a
                //disease.
                if (incidentDef.Worker is IncidentWorker_Disease worker)
                {
                    List<Pawn> pawns = worker.ApplyToPawns(
                        Gen.YieldSingle(pawn), out blockedInfo);
                    if (!PawnUtility.ShouldSendNotificationAbout(pawn))
                        return;
                    //Check if disease was actually applied to target pawn,
                    //because the pawn may have the disease blocked by another
                    //hediff
                    if (pawns.Contains(pawn))
                        //If the pawn was affected, send the normal "incident 
                        //occured" letter.
                        Find.LetterStack.ReceiveLetter(
                            "LetterLabelTraitDisease".Translate(
                                incidentDef.diseaseIncident.label),
                            "LetterTraitDisease".Translate(
                                pawn.LabelCap,
                                incidentDef.diseaseIncident.label,
                                pawn.Named("PAWN")).AdjustedFor(pawn),
                            LetterDefOf.NegativeEvent, pawn);
                    else if (!blockedInfo.NullOrEmpty())
                        //If pawn was not affected, send a simple "blocked"
                        //notice. Will only execute if there's a "disease
                        //blocked" message to display.
                        Messages.Message(
                            blockedInfo, pawn, MessageTypeDefOf.NeutralEvent);
                }
                //Prevent non-disease events from occuring, because trying to
                //create a raid here would likely cause a lot of painful
                //errors.
                else
                    Log.Error("DefModExtension \"TraitRandomDiseasePools\" " +
                        "tried to trigger non-disease incident.");
            }
        }

        /// <summary>
        /// Contains all of the <c>TraitMtbDiseasePool</c>s specified for each
        /// trait degree.
        /// </summary>
        public List<DiseasePool> pools;
    }
}