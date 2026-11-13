using Verse;

namespace StoryTime;

public class CompProperties_Rippable : CompProperties
{
	public int ripIntervalDays;

	public int legAmount = 1;

	public ThingDef legDef;

	public CompProperties_Rippable()
	{
		compClass = typeof(CompRippable);
	}
}
