using System.Collections.Generic;
using Verse;
using Verse.Sound;
using RimWorld;

namespace CF
{
    /// <summary>
    /// Static helper class, contains helpful methods to apply to buildings.
    /// </summary>
    public static class CommunityBuildingUtility
    {
        /// <summary>
        /// The vanilla <c>SoundDef</c> played when a building is
        /// minified/uninstalled. Stored here as a public member for
        /// consistency.
        /// </summary>
        public static readonly SoundDef minifySound =
            SoundDef.Named("ThingUninstalled");

        /// <summary>
        /// Minifies a <c>Thing</c> on the spot if its <c>ThingDef</c> says
        /// that it can be minified, and destroys it if not.
        /// </summary>
        /// <param name="thing">The <c>Thing</c> to minify/destroy.</param>
        /// <param name="destroyMode">
        /// The destroy mode to pass to <c>thing.Destroy</c>. It's not
        /// recommended to set this unless you have a good reason to do so.
        /// </param>
        public static void MinifyOrDestroy(
            this Thing thing,
            DestroyMode destroyMode = DestroyMode.KillFinalize
        )
        {
            if (thing.def.Minifiable)
            {
                Map map = thing.Map;
                MinifiedThing package = MinifyUtility.MakeMinified(thing);
                GenPlace.TryPlaceThing(
                    package, thing.Position, map, ThingPlaceMode.Near);
                minifySound.PlayOneShot(new TargetInfo(thing.Position, map));
            }
            else
            {
                thing.Destroy(destroyMode);
            }
        }

        /// <summary>
        /// Checks whether the building is a wall, namely that it holds a roof,
        /// blocks light, and covers the floor.
        /// </summary>
        /// <param name="building">The building to check.</param>
        /// <returns>
        /// Whether the building is a wall by the above criteria.
        /// </returns>
        // By CuproPanda.
        public static bool IsWall(this Building building)
        {
            ThingDef def = building?.def;
            return def != null &&
                def.holdsRoof && def.blockLight && def.coversFloor;
        }

        /// <summary>
        /// Returns whether an identical <c>Thing</c> exists in the specified
        /// cell, namely that its <c>Def</c> matches and it's facing in the
        /// same direction.
        /// </summary>
        /// <param name="buildableDef">
        /// The <c>BuildableDef</c> a <c>PlaceWorker</c> is trying to place.
        /// </param>
        /// <param name="cell">The cell it's trying to be placed in.</param>
        /// <param name="rot">
        /// The rotation of the current thing which is attempting to be placed.
        /// </param>
        /// <param name="map">The map it's trying to be placed in.</param>
        /// <returns>
        /// <c>True</c>, if the same building exists in the same spot with the
        /// same rotation.
        /// </returns>
        // By CuproPanda
        public static bool ConflictingThing(
            this IntVec3 cell,
            BuildableDef buildableDef,
            Rot4 rot,
            Map map
        )
        {
            List<Thing> things = map.thingGrid.ThingsListAtFast(cell);
            foreach (Thing t in things)
            {
                if (t.def == buildableDef && t.Rotation == rot)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// If the given <see cref="BuildableDef"/> defines an incomplete
        /// building (a blueprint or building frame), then this method will
        /// give the <see cref="BuildableDef"/> of the finished building.
        /// </summary>
        /// <param name="building">
        /// The <see cref="BuildableDef"/> to be evaluated.
        /// </param>
        /// <param name="stage">
        /// The construction stage that this <see cref="BuildableDef"/>
        /// defines.
        /// </param>
        /// <returns>
        /// The <see cref="BuildableDef"/> of the building when it is fully
        /// constructed. If the building is already fully constructed, then
        /// the method will return the same reference as the <c>building</c>
        /// parameter.
        /// </returns>
        public static BuildableDef GetFullyConstructedDefOf(
            BuildableDef building,
            out EBuildableDefStage stage
        )
        {
            if (!(building is ThingDef thingDef))
            {
                stage = EBuildableDefStage.Building;
                return building;
            }

            if (thingDef.IsBlueprint)
            {
                stage = EBuildableDefStage.Blueprint;
                return thingDef.entityDefToBuild;
            }
            if (thingDef.IsFrame)
            {
                stage = EBuildableDefStage.Frame;
                return thingDef.entityDefToBuild;
            }
            stage = EBuildableDefStage.Building;
            return building;
        }

        /// <summary>
        /// An enumerator that contains all of the stages of building
        /// construction, including finished and unfinished stages.
        /// </summary>
        public enum EBuildableDefStage
        {
            /// <summary>
            /// The building has been planned out by the player, but
            /// construction has not yet begun, and no resources have been
            /// delivered.
            /// </summary>
            Blueprint,
            /// <summary>
            /// Colonists have begun working on this building. This represents
            /// any time between the first resource being delivered, and all of
            /// the required work being completed.
            /// </summary>
            Frame,
            /// <summary>
            /// The building is fully completed. It requires no additional
            /// resources or work, and is behaving exactly as is defined in its
            /// <see cref="ThingDef"/>.
            /// </summary>
            Building,
        }
    }
}
