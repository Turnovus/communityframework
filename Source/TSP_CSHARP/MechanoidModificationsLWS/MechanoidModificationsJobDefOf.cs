using RimWorld;
using Verse;

namespace MechanoidModificationsLWS;

[DefOf]
public static class MechanoidModificationsJobDefOf
{
	public static JobDef RemoveModification;

	static MechanoidModificationsJobDefOf()
	{
		DefOfHelper.EnsureInitializedInCtor(typeof(MechanoidModificationsJobDefOf));
	}
}
