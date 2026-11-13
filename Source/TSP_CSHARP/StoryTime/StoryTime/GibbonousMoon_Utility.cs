using RimWorld;
using UnityEngine;
using Verse;

namespace StoryTime;

internal class GibbonousMoon_Utility
{
	public static void SpawnMoonGibbon(Map map)
	{
		int num = Mathf.Clamp(GenMath.RoundRandom(StorytellerUtility.DefaultThreatPointsNow(map) / 100f), 1, Rand.RangeInclusive(1, 5));
		int num2 = Rand.RangeInclusive(45000, 60000);
		for (int i = 0; i < num; i++)
		{
			if (!RCellFinder.TryFindRandomPawnEntryCell(out var result, map, CellFinder.EdgeRoadChance_Animal))
			{
				break;
			}
			IntVec3 intVec = CellFinder.RandomClosewalkCellNear(result, map, 10);
			Pawn pawn = PawnGenerator.GeneratePawn(PawnKindDefOf.MoonGibbon);
			GenSpawn.Spawn((Thing)pawn, intVec, map, Rot4.Random, WipeMode.Vanish, false);
			pawn.mindState.exitMapAfterTick = Find.TickManager.TicksGame + num2;
		}
	}
}
