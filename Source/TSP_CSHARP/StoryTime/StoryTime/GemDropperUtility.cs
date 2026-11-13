using System.Collections.Generic;
using System.Linq;
using Verse;

namespace StoryTime;

public static class GemDropperUtility
{
	public static ThingDef TryGetItem(float commonGemDropRate, float uncommonGemDropRate, float rareGemDropRate)
	{
		if (Rand.Value <= commonGemDropRate)
		{
			IEnumerable<ThingDef> source = DefDatabase<ThingDef>.AllDefs.Where((ThingDef x) => x.thingCategories != null && x.thingCategories.Contains(DefDatabase<ThingCategoryDef>.GetNamed("ST_GemsUncut_Common")));
			return source.RandomElement();
		}
		if (Rand.Value <= uncommonGemDropRate)
		{
			IEnumerable<ThingDef> source2 = DefDatabase<ThingDef>.AllDefs.Where((ThingDef x) => x.thingCategories != null && x.thingCategories.Contains(DefDatabase<ThingCategoryDef>.GetNamed("ST_GemsUncut_Uncommon")));
			return source2.RandomElement();
		}
		if (Rand.Value <= rareGemDropRate)
		{
			IEnumerable<ThingDef> source3 = DefDatabase<ThingDef>.AllDefs.Where((ThingDef x) => x.thingCategories != null && x.thingCategories.Contains(DefDatabase<ThingCategoryDef>.GetNamed("ST_GemsUncut_Rare")));
			return source3.RandomElement();
		}
		return null;
	}
}
