using Verse;

namespace StoryTime;

public class CompProperties_Wood : CompProperties
{
	public int woodHarvestIntervalDays;

	public int woodAmount = 1;

	public ThingDef woodDef;

	public CompProperties_Wood()
	{
		compClass = typeof(CompWoodFrog);
	}
}
