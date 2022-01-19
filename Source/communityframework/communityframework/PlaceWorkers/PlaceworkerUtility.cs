using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace D9Framework
{
    /// <summary>
    /// Class holding utility functions for the various included <c>PlaceWorkers</c>.
    /// </summary>
    class PlaceWorkerUtility
    {
        /// <summary>
        /// Checks whether the building is a wall, namely that it holds a roof, blocks light, and covers the floor.
        /// </summary>
        /// <param name="building">The building to check.</param>
        /// <returns>Whether the building is a wall by the above criteria.</returns>
        public static bool IsWall(Building building)
        {
            ThingDef def = building?.def;
            return def != null && (def.holdsRoof && def.blockLight && def.coversFloor);
        }
        /// <summary>
        /// Returns whether an identical <c>Thing</c> exists in the specified cell, namely that its <c>Def</c> matches and it's facing in the same direction.
        /// </summary>
        /// <param name="buildableDef">The <c>BuildableDef</c> a <c>PlaceWorker</c> is trying to place.</param>
        /// <param name="cell">The cell it's trying to be placed in.</param>
        /// <param name="rot">The rotation of the current thing which is attempting to be placed.</param>
        /// <param name="map">The map it's trying to be placed in.</param>
        /// <returns></returns>
        public static bool ConflictingThing(BuildableDef buildableDef, IntVec3 cell, Rot4 rot, Map map)
        {
            List<Thing> things = map.thingGrid.ThingsListAtFast(cell);            
            foreach (Thing t in things) if (t.def as BuildableDef == buildableDef && t.Rotation == rot) return true; 
            return false;
        }
    }
}
