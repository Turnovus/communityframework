using RimWorld;
using Verse;

namespace MechanoidModificationsLWS;

[DefOf]
public static class MechanoidModificationsLWModdedHediffDefOf
{
	[MayRequire("sarg.rimbees")]
	public static HediffDef GEGEMM_BeekeepingModule;

	[MayRequire("VanillaExpanded.VGeneticsE")]
	public static HediffDef GEGEMM_GeneticsModule;

	[MayRequire("VanillaExpanded.VBooksE")]
	public static HediffDef GEGEMM_BooksModule;

	[MayRequire("VanillaExpanded.VCEF")]
	public static HediffDef GEGEMM_FishingModule;

	[MayRequire("Dubwise.Rimatomics")]
	public static HediffDef GEGEMM_NuclearWorkModule;

	static MechanoidModificationsLWModdedHediffDefOf()
	{
		DefOfHelper.EnsureInitializedInCtor(typeof(MechanoidModificationsLWModdedHediffDefOf));
	}
}
