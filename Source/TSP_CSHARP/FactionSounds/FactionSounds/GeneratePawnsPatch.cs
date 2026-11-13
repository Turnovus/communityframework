using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;
using Verse.Sound;

namespace FactionSounds;

[HarmonyPatch(typeof(PawnGroupMakerUtility), "GeneratePawns")]
public static class GeneratePawnsPatch
{
	private static void Postfix(PawnGroupMakerParms parms, ref IEnumerable<Pawn> __result)
	{
		if (__result != null && __result.Any() && parms.faction.def.GetModExtension<FactionSound>() != null)
		{
			FactionSound modExtension = parms.faction.def.GetModExtension<FactionSound>();
			if (parms.groupKind == PawnGroupKindDefOf.Combat && modExtension.combatSoundDef != null)
			{
				modExtension.combatSoundDef.PlayOneShotOnCamera();
			}
			else if (parms.groupKind == PawnGroupKindDefOf.Trader && modExtension.traderSoundDef != null)
			{
				modExtension.traderSoundDef.PlayOneShotOnCamera();
			}
			else if (parms.groupKind == PawnGroupKindDefOf.Settlement || (parms.groupKind == PawnGroupKindDefOf.Settlement_RangedOnly && modExtension.settlementSoundDef != null))
			{
				modExtension.settlementSoundDef.PlayOneShotOnCamera();
			}
			else if (modExtension.soundDef != null)
			{
				modExtension.soundDef.PlayOneShotOnCamera();
			}
		}
	}
}
