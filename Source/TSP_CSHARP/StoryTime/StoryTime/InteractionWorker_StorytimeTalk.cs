using RimWorld;
using Verse;

namespace StoryTime;

public class InteractionWorker_StorytimeTalk : InteractionWorker
{
	public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
	{
		return 0.33f;
	}
}
