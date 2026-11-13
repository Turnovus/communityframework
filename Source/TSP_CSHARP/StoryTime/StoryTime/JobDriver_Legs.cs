using RimWorld;
using Verse;

namespace StoryTime;

public class JobDriver_Legs : JobDriver_GatherAnimalBodyResources
{
	protected override float WorkTotal => 400f;

	protected override CompHasGatherableBodyResource GetComp(Pawn animal)
	{
		return animal.TryGetComp<CompRippable>();
	}
}
