using RimWorld;
using Verse;

namespace MechanoidModificationsLWS;

[DefOf]
public static class ModdedWorkTypeDefOf
{
	[MayRequire("sarg.rimbees")]
	public static WorkTypeDef RB_BeekeepingWork;

	[MayRequire("VanillaExpanded.VGeneticsE")]
	public static WorkTypeDef GR_Genetics;

	[MayRequire("VanillaExpanded.VBooksE")]
	public static WorkTypeDef VBE_Writing;

	[MayRequire("VanillaExpanded.VCEF")]
	public static WorkTypeDef VCEF_Fishing;

	[MayRequire("Dubwise.Rimatomics")]
	public static WorkTypeDef NuclearWork;	

    static ModdedWorkTypeDefOf()
	{
		DefOfHelper.EnsureInitializedInCtor(typeof(ModdedWorkTypeDefOf));
	}
}
