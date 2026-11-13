using Verse;

namespace BorpaEventsAndBehaviors;

public class CompProperties_KachorpaShedding : CompProperties
{
	public int shedIntervalDays;

	public int steelAmount;

	public ThingDef steelDef;

	public CompProperties_KachorpaShedding()
	{
		compClass = typeof(CompKachorpaShedding);
	}
}
