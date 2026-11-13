using Verse;

namespace StoryTime;

public class HediffCompProperties_Polymorph : HediffCompProperties
{
	public string targetPolymorph = "";

	public string polymorphLetterLabel = "";

	public string polymorphLetter = "";

	public LetterDef polymorphLetterDef;

	public HediffCompProperties_Polymorph()
	{
		compClass = typeof(HediffComp_Polymorph);
	}
}
