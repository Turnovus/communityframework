using System.Collections.Generic;
using System.Linq;
using Verse;

namespace CF
{
    /// <summary>
    /// <c>PlaceWorker</c> requiring that the parent <c>Thing</c> be placed
    /// under a roof. <seealso cref="CF.CompValidator"/>
    /// </summary>
    /// <remarks>
    /// Originally by CuproPanda, for Additional Joy Objects.
    /// </remarks>
    public class PlaceWorker_Roofed : PlaceWorker
    {
        /// <summary>
        /// Checks that the parent <see cref="Thing"/> is placed under a roof.
        /// </summary>
        /// <param name="checkingDef">
        /// <inheritdoc cref="PlaceWorker_NotImpassible.AllowsPlacing"
        /// path="/param[@name='checkingDef']"/>
        /// </param>
        /// <param name="loc">
        /// <inheritdoc cref="PlaceWorker_NotImpassible.AllowsPlacing"
        /// path="/param[@name='loc']"/>
        /// </param>
        /// <param name="rot">
        /// <inheritdoc cref="PlaceWorker_NotImpassible.AllowsPlacing"
        /// path="/param[@name='rot']"/>
        /// </param>
        /// <param name="map">
        /// <inheritdoc cref="PlaceWorker_NotImpassible.AllowsPlacing"
        /// path="/param[@name='map']"/>
        /// </param>
        /// <param name="thingToIgnore">
        /// <inheritdoc cref="PlaceWorker_NotImpassible.AllowsPlacing"
        /// path="/param[@name='thingToIgnore']"/>
        /// </param>
        /// <param name="thing">
        /// <inheritdoc cref="PlaceWorker_NotImpassible.AllowsPlacing"
        /// path="/param[@name='thing']"/>
        /// </param>
        /// <returns>
        /// <c>false</c> if the blueprint is being placed out in the open
        /// without a roof over it. <c>true</c> if every cell of the blueprint
        /// being placed is roofed, meaning that placement is permitted.
        /// </returns>
        public override AcceptanceReport AllowsPlacing(
            BuildableDef checkingDef,
            IntVec3 loc,
            Rot4 rot,
            Map map,
            Thing thingToIgnore = null,
            Thing thing = null
        )
        {
            if (!map.roofGrid.Roofed(loc))
            {
                return new AcceptanceReport(
                    "CF_Roofed_NeedsRoof".Translate(checkingDef.label)
                );
            }
            return true;
        }
    }
    /// <summary>
    /// <c>PlaceWorker</c> requiring that the parent <c>Thing</c> be placed
    /// under a roof and not over another <c>Thing</c> which is too tall.
    /// <seealso cref="CF.CompValidator"/>
    /// </summary>
    /// <remarks>
    /// Originally by CuproPanda, for Additional Joy Objects.
    /// </remarks>
    public class PlaceWorker_RoofHanger : PlaceWorker_Roofed
    {
        /// <summary>
        /// Ensures that the parent <see cref="Thing"/> is placed under a roof,
        /// and not over any other <see cref="Thing"/> that is too tall.
        /// </summary>
        /// <param name="checkingDef">
        /// <inheritdoc cref="PlaceWorker_NotImpassible.AllowsPlacing"
        /// path="/param[@name='checkingDef']"/>
        /// </param>
        /// <param name="loc">
        /// <inheritdoc cref="PlaceWorker_NotImpassible.AllowsPlacing"
        /// path="/param[@name='loc']"/>
        /// </param>
        /// <param name="rot">
        /// <inheritdoc cref="PlaceWorker_NotImpassible.AllowsPlacing"
        /// path="/param[@name='rot']"/>
        /// </param>
        /// <param name="map">
        /// <inheritdoc cref="PlaceWorker_NotImpassible.AllowsPlacing"
        /// path="/param[@name='map']"/>
        /// </param>
        /// <param name="thingToIgnore">
        /// <inheritdoc cref="PlaceWorker_NotImpassible.AllowsPlacing"
        /// path="/param[@name='thingToIgnore']"/>
        /// </param>
        /// <param name="thing">
        /// <inheritdoc cref="PlaceWorker_NotImpassible.AllowsPlacing"
        /// path="/param[@name='thing']"/>
        /// </param>
        /// <returns>
        /// <c>false</c> if the blueprint's area is not fully roofed, or if
        /// there are any other <see cref="Thing"/>s occupying the same space
        /// as the blueprint which are too tall. <c>true</c> if none of the
        /// prior conditions are violated, meaning that placement is permitted.
        /// </returns>
		public override AcceptanceReport AllowsPlacing(
            BuildableDef checkingDef,
            IntVec3 loc,
            Rot4 rot,
            Map map,
            Thing thingToIgnore = null,
            Thing thing = null
        )
        {
            //check if tile is roofed
            AcceptanceReport roofedReport =
                base.AllowsPlacing(checkingDef, loc, rot, map, thingToIgnore);
            if (!roofedReport.Accepted) return roofedReport;

            // Don't allow placing on big things
            foreach (
                IntVec3 c in GenAdj.CellsOccupiedBy(loc, rot, checkingDef.Size)
            ) 
            {
                if (c.GetEdifice(map) != null)
                {
                    if (
                        c.GetEdifice(map).def.blockWind == true ||
                        c.GetEdifice(map).def.holdsRoof == true
                    )
                    {
                        return new AcceptanceReport(
                            "CF_Chandelier_TooTall".Translate(
                                c.GetEdifice(map).LabelCap,
                                checkingDef.LabelCap
                            )
                        );
                    }
                }

                IEnumerable<Thing> things = c.GetThingList(map);
                // don't hang if there's already a chandelier here
                if (things.Where(
                    x => x.def.placeWorkers != null
                    && x.def.placeWorkers.Where(
                        y => y.GetType() ==
                            typeof(PlaceWorker_RoofHanger))
                    .Any()
                ).Any())
                {
                    return new AcceptanceReport(
                        "IdenticalThingExists".Translate()
                    );
                }
            }
            // Otherwise, accept placing
            return true;
        }
    }
}
