using Verse;

namespace CF
{
    class PlaceWorker_NotImpassible : PlaceWorker
    {

        public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
        {
            if (!base.AllowsPlacing(checkingDef, loc, rot, map, thingToIgnore, thing))
                return false;

            BuildableDef otherThingFinished;

            foreach (IntVec3 c in GenAdj.CellsOccupiedBy(loc, rot, checkingDef.Size))
            {
                foreach (Thing otherThing in c.GetThingList(map))
                {
                    otherThingFinished = CommunityBuildingUtility
                        .GetFullyConstructedDefOf(otherThing.def,
                        out CommunityBuildingUtility.EBuildStage _);

                    if (otherThingFinished.passability == Traversability.Impassable)
                        return false;
                }
            }

            return true;
        }
    }
}
