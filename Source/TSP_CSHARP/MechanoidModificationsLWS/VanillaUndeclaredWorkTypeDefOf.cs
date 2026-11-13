using RimWorld;
using Verse;

namespace MechanoidModificationsLWS;

[DefOf]
public static class VanillaUndeclaredWorkTypeDefOf
{
	public static WorkTypeDef Cleaning;

	public static WorkTypeDef Tailoring;

	public static WorkTypeDef Cooking;

	public static WorkTypeDef BasicWorker;

	public static WorkTypeDef Art;

	static VanillaUndeclaredWorkTypeDefOf()
	{
		DefOfHelper.EnsureInitializedInCtor(typeof(VanillaUndeclaredWorkTypeDefOf));
	}
}
