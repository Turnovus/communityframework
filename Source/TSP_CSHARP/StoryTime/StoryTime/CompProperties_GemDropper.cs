using Verse;

namespace StoryTime;

public class CompProperties_GemDropper : CompProperties
{
	public float commonGemDropRate;

	public float uncommonGemDropRate;

	public float rareGemDropRate;

	public CompProperties_GemDropper()
	{
		compClass = typeof(CompGemDropper);
	}
}
