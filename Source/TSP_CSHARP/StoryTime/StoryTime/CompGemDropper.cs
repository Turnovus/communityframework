using Verse;

namespace StoryTime;

public class CompGemDropper : ThingComp
{
	public CompProperties_GemDropper Props => (CompProperties_GemDropper)props;

	public float commonGemDropRate => Props.commonGemDropRate;

	public float uncommonGemDropRate => Props.uncommonGemDropRate;

	public float rareGemDropRate => Props.rareGemDropRate;
}
