using Verse;

namespace CF
{
    /// <summary>
    /// A <see cref="DefModExtension"/> that, when applied to a building's
    /// <see cref="ThingDef"/>, can allow that building to conditionally block
    /// the placement of other buildings.
    /// </summary>
    public abstract class DefModExtension_CanBlockPlacement : DefModExtension
    {
        /// <summary>
        /// Determines whether or not another building is blocked by the
        /// presence of this building in the same tile.
        /// </summary>
        /// <remarks>
        /// This method does know which building stage the parent building is
        /// in (blueprint, frame, or finished). For the other building, it can
        /// be assumed that it always a blueprint.
        /// </remarks>
        /// <param name="otherThing">
        /// The <see cref="BuildableDef"/>, most likely a
        /// <see cref="ThingDef"/>, of the building that the player is trying
        /// to place over the parent building.
        /// </param>
        /// <param name="existingBuildingStage">
        /// The building stage of the existing building, meaning the one that
        /// has this extension attached to it.
        /// It is not recommended to use this parameter without good reason, as
        /// changing the acceptance of overlapping buildings based on building
        /// phase can be very inconsistent. However, the ability exists, should
        /// you need it.
        /// </param>
        /// <returns>
        /// <c>true</c>, if the placement is blocked, and the new building is
        /// not allowed to be blaced over the existing one. <c>false</c>
        /// if the building is not directly blocked, and is allowed to be
        /// placed as long as all other external conditions for building 
        /// placement are met.
        /// </returns>
        public abstract bool BlocksPlacementOf(
            BuildableDef otherThing,
            CommunityBuildingUtility.EBuildableDefStage existingBuildingStage
        );
    }

    /// <summary>
    /// A <see cref="DefModExtension"/> that will prevent impassable structures
    /// like walls, vents, and coolers from being manually placed on top of
    /// whatever building has the extension, even if it would normally be
    /// possible for such overlap to happen otherwise (i.e. with conduits and
    /// floor coverings).
    /// </summary>
    public class BlocksImpassibleBuildings : DefModExtension_CanBlockPlacement
    {
        /// <inheritdoc/>
        public override bool BlocksPlacementOf(
            BuildableDef otherThing,
            CommunityBuildingUtility.EBuildableDefStage _
        ) =>
            otherThing.passability == Traversability.Impassable;
    }
}
