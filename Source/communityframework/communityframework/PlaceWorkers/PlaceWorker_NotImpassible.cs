using Verse;

namespace CF
{
    /// <summary>
    /// <see cref="PlaceWorker"/> that ensures that the parent
    /// <see cref="Thing"/> is not places on the same cell as any other
    /// <see cref="Thing"/> that has a <see cref="BuildableDef.passability"/>
    /// of <see cref="Traversability.Impassable"/>.
    /// </summary>
    class PlaceWorker_NotImpassible : PlaceWorker
    {
        /// <summary>
        /// Ensures that the parent <see cref="Thing"/> is not places on the
        /// same cell as any other <see cref="Thing"/> that has a
        /// <see cref="BuildableDef.passability"/>.
        /// </summary>
        /// <param name="checkingDef">
        /// The <see cref="ThingDef"/> using the <see cref="PlaceWorker"/>
        /// </param>
        /// <param name="loc">
        /// The location that the building is being placed at.
        /// </param>
        /// <param name="rot">
        /// The rotation that the building is being placed at.
        /// </param>
        /// <param name="map">
        /// The <see cref="Map"/> that the building is being placed in.
        /// </param>
        /// <param name="thingToIgnore">Unused.</param>
        /// <param name="thing">Unused.</param>
        /// <returns>
        /// <c>false</c> if any impassible building exists in any cell occupied
        /// by the blueprint being placed. <c>true</c> if no impassible
        /// building exists, meaning that placement is permitted.
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
            BuildableDef otherThingFinished;

            foreach (IntVec3 c in GenAdj.CellsOccupiedBy(loc, rot, checkingDef.Size))
            {
                foreach (Thing otherThing in c.GetThingList(map))
                {
                    otherThingFinished = CommunityBuildingUtility
                        .GetFullyConstructedDefOf(otherThing.def,
                        out CommunityBuildingUtility.EBuildableDefStage _);

                    if (otherThingFinished.passability == Traversability.Impassable)
                        return false;
                }
            }

            return true;
        }
    }
}
