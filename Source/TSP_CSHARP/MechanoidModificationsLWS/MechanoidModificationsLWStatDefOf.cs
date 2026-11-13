using RimWorld;

namespace MechanoidModificationsLWS;

[DefOf]
public static class MechanoidModificationsLWStatDefOf
{
	public static StatDef MechAdditionalSkillLevel;

	static MechanoidModificationsLWStatDefOf()
	{
		DefOfHelper.EnsureInitializedInCtor(typeof(MechanoidModificationsLWStatDefOf));
	}
}
