using RimWorld;
using Verse;

namespace StoryTime;

public class ThoughtWorker_RareGenius : ThoughtWorker
{
	protected override ThoughtState CurrentSocialStateInternal(Pawn pawn, Pawn other)
	{
		if (!other.RaceProps.Humanlike || !RelationsUtility.PawnsKnowEachOther(pawn, other))
		{
			return false;
		}
		if (!other.story.traits.HasTrait(TraitDefOf.RareGenius))
		{
			return false;
		}
		return true;
	}
}
