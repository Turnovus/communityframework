using System.Collections.Generic;
using RimWorld;
using Verse;

namespace StoryTime;

public class IncidentWorker_WanderingCartographer : IncidentWorker
{
	protected override bool CanFireNowSub(IncidentParms parms)
	{
		if (!base.CanFireNowSub(parms))
		{
			return false;
		}
		Map map = (Map)parms.target;
		IntVec3 cell;
		return !map.gameConditionManager.ConditionIsActive(GameConditionDefOf.ToxicFallout) && map.mapTemperature.SeasonAndOutdoorTemperatureAcceptableFor(ThingDefOf.Cartographer) && TryFindEntryCell(map, out cell);
	}

	protected override bool TryExecuteWorker(IncidentParms parms)
	{
		Map map = (Map)parms.target;
		if (!TryFindEntryCell(map, out var cell))
		{
			return false;
		}
		PawnKindDef cartographer = PawnKindDefOf.Cartographer;
		int num = 1;
		int num2 = Rand.RangeInclusive(90000, 150000);
		IntVec3 result = IntVec3.Invalid;
		if (!RCellFinder.TryFindRandomCellOutsideColonyNearTheCenterOfTheMap(cell, map, 10f, out result))
		{
			result = IntVec3.Invalid;
		}
		Pawn pawn = null;
		for (int i = 0; i < num; i++)
		{
			IntVec3 intVec = CellFinder.RandomClosewalkCellNear(cell, map, 10);
			pawn = PawnGenerator.GeneratePawn(cartographer);
			GenSpawn.Spawn((Thing)pawn, intVec, map, Rot4.Random, WipeMode.Vanish, false);
			pawn.mindState.exitMapAfterTick = Find.TickManager.TicksGame + num2;
			if (result.IsValid)
			{
				pawn.mindState.forcedGotoPosition = CellFinder.RandomClosewalkCellNear(result, map, 10);
			}
		}
		Find.LetterStack.ReceiveLetter("LetterLabelWanderingCartographer".Translate(), "WanderingCartographer".Translate(), LetterDefOf.PositiveEvent, (LookTargets)pawn, (Faction)null, (Quest)null, (List<ThingDef>)null, (string)null);
		return true;
	}

	private bool TryFindEntryCell(Map map, out IntVec3 cell)
	{
		return RCellFinder.TryFindRandomPawnEntryCell(out cell, map, CellFinder.EdgeRoadChance_Animal + 0.2f);
	}
}
