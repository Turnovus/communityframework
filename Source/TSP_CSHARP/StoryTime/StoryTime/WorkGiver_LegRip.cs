using RimWorld;
using Verse;

namespace StoryTime;

public class WorkGiver_LegRip : WorkGiver_GatherAnimalBodyResources
{
	protected override JobDef JobDef => JobDefOf.FrogLegRipping;

	protected override CompHasGatherableBodyResource GetComp(Pawn animal)
	{
		return animal.TryGetComp<CompRippable>();
	}
}
