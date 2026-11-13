using RimWorld;
using Verse;

namespace StoryTime;

public class WorkGiver_woodFrog : WorkGiver_GatherAnimalBodyResources
{
	protected override JobDef JobDef => JobDefOf.woodFrogHarvesting;

	protected override CompHasGatherableBodyResource GetComp(Pawn animal)
	{
		return animal.TryGetComp<CompWoodFrog>();
	}
}
