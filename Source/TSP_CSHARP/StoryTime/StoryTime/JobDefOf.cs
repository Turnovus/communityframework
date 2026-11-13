using RimWorld;
using Verse;

namespace StoryTime;

[DefOf]
public static class JobDefOf
{
	public static JobDef FrogLegRipping;

	public static JobDef TomatoCutting;

	public static JobDef woodFrogHarvesting;

	public static JobDef DeliverFood;

	public static JobDef FeedPatient;

	public static JobDef Ingest;

	static JobDefOf()
	{
		DefOfHelper.EnsureInitializedInCtor(typeof(JobDefOf));
	}
}
