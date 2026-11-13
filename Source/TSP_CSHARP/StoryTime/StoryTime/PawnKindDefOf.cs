using RimWorld;
using Verse;

namespace StoryTime;

[DefOf]
public static class PawnKindDefOf
{
	public static PawnKindDef Jeub;

	public static PawnKindDef Cartographer;

	public static PawnKindDef MoonGibbon;

	public static PawnKindDef NightFrog;

	public static PawnKindDef ManFrog;

	static PawnKindDefOf()
	{
		DefOfHelper.EnsureInitializedInCtor(typeof(PawnKindDefOf));
	}
}
