using RimWorld;
using UnityEngine;
using Verse;

namespace StoryTime;

internal class NightFrogIncidentWorker : IncidentWorker
{
	private static readonly FloatRange CountPerColonistRange = new FloatRange(0.75f, 1.25f);

	private const int MinCount = 1;

	private const int MaxCount = 10;

	protected override bool CanFireNowSub(IncidentParms parms)
	{
		if (!base.CanFireNowSub(parms))
		{
			return false;
		}
		Map map = (Map)parms.target;
		IntVec3 result;
		return Find.World.tileTemperatures.SeasonAndOutdoorTemperatureAcceptableFor(map.Tile, PawnKindDefOf.NightFrog.race) && RCellFinder.TryFindRandomPawnEntryCell(out result, map, CellFinder.EdgeRoadChance_Animal);
	}

	protected override bool TryExecuteWorker(IncidentParms parms)
	{
		Map map = (Map)parms.target;
		PawnKindDef nightFrog = PawnKindDefOf.NightFrog;
		if (!RCellFinder.TryFindRandomPawnEntryCell(out var result, map, CellFinder.EdgeRoadChance_Animal))
		{
			return false;
		}
		int num = Mathf.Clamp(GenMath.RoundRandom((float)map.mapPawns.FreeColonistsCount * CountPerColonistRange.RandomInRange), 1, 10);
		for (int i = 0; i < num; i++)
		{
			IntVec3 loc = CellFinder.RandomClosewalkCellNear(result, map, 10);
			((Pawn)GenSpawn.Spawn(PawnGenerator.GeneratePawn(nightFrog), loc, map)).needs.food.CurLevelPercentage = 1f;
		}
		SendStandardLetter("LetterLabelNightFrogsArrived".Translate(), "NightFrogsArrived".Translate(), LetterDefOf.ThreatSmall, parms, new TargetInfo(result, map));
		return true;
	}
}
