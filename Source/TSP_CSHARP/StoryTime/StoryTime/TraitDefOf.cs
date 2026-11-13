using RimWorld;

namespace StoryTime;

[DefOf]
public static class TraitDefOf
{
	public static TraitDef Buglover;

	public static TraitDef Kind;

	public static TraitDef RareGenius;

	static TraitDefOf()
	{
		DefOfHelper.EnsureInitializedInCtor(typeof(ThingDefOf));
	}
}
