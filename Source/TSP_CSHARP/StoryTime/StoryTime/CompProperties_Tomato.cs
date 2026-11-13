using Verse;

namespace StoryTime;

public class CompProperties_Tomato : CompProperties
{
	public int tomatoIntervalDays;

	public int tomatoAmount = 1;

	public ThingDef tomatoDef;

	public CompProperties_Tomato()
	{
		compClass = typeof(CompTomato);
	}
}
