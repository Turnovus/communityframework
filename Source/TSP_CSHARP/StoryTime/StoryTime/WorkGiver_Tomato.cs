using RimWorld;
using Verse;

namespace StoryTime;

public class WorkGiver_Tomato : WorkGiver_GatherAnimalBodyResources
{
	protected override JobDef JobDef => JobDefOf.TomatoCutting;

	protected override CompHasGatherableBodyResource GetComp(Pawn animal)
	{
		return animal.TryGetComp<CompTomato>();
	}
}
