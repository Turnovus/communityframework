using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace CF
{
    public abstract class DefModExtension_CanBlockPlacement : DefModExtension
    {
        public abstract bool BlocksPlacementOf(
            BuildableDef otherThing,
            CommunityBuildingUtility.EBuildStage existingBuildingStage
        );
    }

    public class BlocksImpassibleBuildings : DefModExtension_CanBlockPlacement
    {
        public override bool BlocksPlacementOf(
            BuildableDef otherThing,
            CommunityBuildingUtility.EBuildStage _
        ) =>
            otherThing.passability == Traversability.Impassable;
    }
}
