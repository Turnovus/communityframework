using System.Collections.Generic;
using RimWorld;
using Verse;

namespace StoryTime;

public class IncidentWorker_JeubSwarm : IncidentWorker
{
	private const float PointsFactor = 1f;

	private const int AnimalsStayDurationMin = 60000;

	private const int AnimalsStayDurationMax = 120000;

	protected override bool CanFireNowSub(IncidentParms parms)
	{
		if (!base.CanFireNowSub(parms))
		{
			return false;
		}
		Map map = (Map)parms.target;
		PawnKindDef pawnKindDef = default(PawnKindDef);
		IntVec3 result;
		//return ManhunterPackIncidentUtility.TryFindManhunterAnimalKind(parms.points, map.Tile, ref pawnKindDef) && RCellFinder.TryFindRandomPawnEntryCell(out result, map, CellFinder.EdgeRoadChance_Animal);
        return AggressiveAnimalIncidentUtility.TryFindAggressiveAnimalKind(parms.points, map.Tile, out pawnKindDef) && RCellFinder.TryFindRandomPawnEntryCell(out result, map, CellFinder.EdgeRoadChance_Animal);
    }

    protected override bool TryExecuteWorker(IncidentParms parms)
	{
		Map map = (Map)parms.target;
		PawnKindDef jeub = PawnKindDefOf.Jeub;
		if ((jeub == null && !AggressiveAnimalIncidentUtility.TryFindAggressiveAnimalKind(parms.points, map.Tile, out jeub)) || AggressiveAnimalIncidentUtility.GetAnimalsCount(jeub, parms.points) == 0)
		{
			return false;
		}
		IntVec3 result = parms.spawnCenter;
		if (!result.IsValid && !RCellFinder.TryFindRandomPawnEntryCell(out result, map, CellFinder.EdgeRoadChance_Animal))
		{
			return false;
		}
		List<Pawn> list = AggressiveAnimalIncidentUtility.GenerateAnimals(jeub, map.Tile, parms.points * 3f, parms.pawnCount);
		Rot4 rot = Rot4.FromAngleFlat((map.Center - result).AngleFlat);
		for (int i = 0; i < list.Count; i++)
		{
			Pawn pawn = list[i];
			QuestUtility.AddQuestTag(GenSpawn.Spawn((Thing)pawn, CellFinder.RandomClosewalkCellNear(result, map, 10), map, rot, WipeMode.Vanish, false), parms.questTag);
			pawn.health.AddHediff(HediffDefOf.Scaria);
			pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.ManhunterPermanent, (string)null, false, false, false, (Pawn)null, false, false);
			pawn.mindState.exitMapAfterTick = Find.TickManager.TicksGame + Rand.Range(60000, 120000);
		}
		SendStandardLetter("LetterLabelJeubSwarm".Translate(), "JeubSwarm".Translate(jeub.GetLabelPlural()), LetterDefOf.ThreatBig, parms, list[0]);
		Find.TickManager.slower.SignalForceNormalSpeedShort();
		LessonAutoActivator.TeachOpportunity(ConceptDefOf.ForbiddingDoors, OpportunityType.Critical);
		LessonAutoActivator.TeachOpportunity(ConceptDefOf.AllowedAreas, OpportunityType.Important);
		return true;
	}
}
