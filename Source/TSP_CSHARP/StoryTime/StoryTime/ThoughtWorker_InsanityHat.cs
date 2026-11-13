using System.Collections.Generic;
using RimWorld;
using Verse;

namespace StoryTime;

public class ThoughtWorker_InsanityHat : ThoughtWorker
{
	protected override ThoughtState CurrentStateInternal(Pawn p)
	{
		List<Apparel> wornApparel = p.apparel.WornApparel;
		for (int i = 0; i < wornApparel.Count; i++)
		{
			if (wornApparel[i].def.defName == "Insanity_Hat")
			{
				return ThoughtState.ActiveAtStage(0);
			}
		}
		return ThoughtState.Inactive;
	}
}
