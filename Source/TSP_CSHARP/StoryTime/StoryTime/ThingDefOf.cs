using RimWorld;
using Verse;

namespace StoryTime;

[DefOf]
public static class ThingDefOf
{
	public static ThingDef Cartographer;

	public static ThingDef Silver;

	public static ThingDef Filth_Blood;

	static ThingDefOf()
	{
		DefOfHelper.EnsureInitializedInCtor(typeof(ThingDefOf));
	}
}
