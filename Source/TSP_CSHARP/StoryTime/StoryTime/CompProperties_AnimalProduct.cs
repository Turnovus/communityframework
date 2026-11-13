using System.Collections.Generic;
using Verse;

namespace StoryTime;

public class CompProperties_AnimalProduct : CompProperties
{
	public int gatheringIntervalDays = 1;

	public int resourceAmount = 1;

	public ThingDef resourceDef;

	public string customResourceString = "";

	public bool isRandom;

	public List<string> randomItems;

	public List<string> seasonalItems;

	public bool hasAditional;

	public int additionalItemsProb = 1;

	public int additionalItemsNumber = 1;

	public List<string> additionalItems;
}
