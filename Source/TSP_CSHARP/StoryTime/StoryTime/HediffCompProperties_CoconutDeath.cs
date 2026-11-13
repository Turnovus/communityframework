using Verse;

namespace StoryTime;

public class HediffCompProperties_CoconutDeath : HediffCompProperties
{
	public string targetCoconut = "";

	public HediffCompProperties_CoconutDeath()
	{
		compClass = typeof(HediffComp_CoconutDeath);
	}
}
