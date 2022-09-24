using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace CF
{
    // These extensions are just for holding data, and none of these fields are
    // going to be assigned to in this namespace.
#pragma warning disable CS0649

    /// <summary>
    /// <c>DefModExtension</c> for use with
    /// <see cref="CF.ShouldHaveNeedPatch"/>. Needs listed here will be ignored
    /// by pawns with this <c>DefModExtension</c>.
    /// </summary>
    class IgnoreNeed : DefModExtension
    {
        public List<NeedDef> needs;
    }

    /// <summary>
    /// <c>DefModExtension</c> which flags the parent plant <c>ThingDef</c> as
    /// using negative fertility. Specifies minimum and maximum fertility
    /// values within which the final fertility is clamped.
    /// </summary>
    /// <remarks>
    /// See <see cref="CF.NegativeFertilityPatch"/> for implementation details.
    /// </remarks>
    public class UseNegativeFertility : DefModExtension
    {
        public float minFertility = 0.05f, maxFertility = 1.4f;
    }

    /// <summary>
    /// <c>DefModExtension</c> for use with
    /// <see cref="CF.CompFromStuffPatch"/>. Specifies the <c>ThingComps</c>,
    /// by their <c>CompProperties</c>, which should be added to newly-
    /// generated items made from the specified Stuff.
    /// </summary>
    class CompsToAddWhenStuff : DefModExtension
    {
        public List<CompProperties> comps;

    }

    /// <summary>
    /// An extension meant for use alongside
    /// <see cref="RimWorld.CompFacility"/>, it is meant to be used on
    /// buildings that link to other buildings.
    /// </summary>
    class BuildingFacilityExtension : DefModExtension
    {
        public bool facilityRequiresFuel = false;
    }

    class UseOutputWorkers : DefModExtension
    {
        public IEnumerable<Type> outputWorkers;

        [Unsaved(false)]
        private IEnumerable<OutputWorker> activeWorkers = null;

        public IEnumerable<OutputWorker> ActiveWorkers
        {
            get
            {
                if (activeWorkers != null)
                    return activeWorkers;

                activeWorkers = new List<OutputWorker>();
                foreach (Type t in outputWorkers)
                    activeWorkers.Append(
                        (OutputWorker)Activator.CreateInstance(t)
                    );

                return activeWorkers;
            }
        }
    }
#pragma warning restore CS0649
}
