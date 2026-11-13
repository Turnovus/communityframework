using System.Collections.Generic;
using RimWorld;
using Verse;

namespace BorpaEventsAndBehaviors;

public class IncidentWorker_JohnBorpa : IncidentWorker_ThrumboPasses
{
	protected override bool CanFireNowSub(IncidentParms parms)
	{
		Map map = (Map)parms.target;
		if (map.mapTemperature.SeasonAndOutdoorTemperatureAcceptableFor(ThingDef.Named("BORPA_JohnBorpa")) && TryFindEntryCell(map, out var _))
		{
            //return LoadedModManager.GetMod<BorpaMod>().GetSettings<BorpaSettings>().JohnBorpaEventEnabled;
            return true;
        }
		return false;
	}

	private bool TryFindEntryCell(Map map, out IntVec3 cell)
	{
		return RCellFinder.TryFindRandomPawnEntryCell(out cell, map, CellFinder.EdgeRoadChance_Animal + 0.2f);
	}

	protected override bool TryExecuteWorker(IncidentParms parms)
	{
		Map map = (Map)parms.target;
		PawnKindDef kindDef = PawnKindDef.Named("BORPA_JohnBorpa");
		if (!RCellFinder.TryFindRandomPawnEntryCell(out var result, map, CellFinder.EdgeRoadChance_Animal))
		{
			return false;
		}
		Rot4.FromAngleFlat((map.Center - result).AngleFlat);
		ThingDef named = DefDatabase<ThingDef>.GetNamed("BORPA_MeleeWeapon_ScrapingSpear");
		IntVec3 loc = CellFinder.RandomClosewalkCellNear(result, map, 1);
		Pawn pawn = PawnGenerator.GeneratePawn(kindDef);
		pawn.gender = Gender.Male;
		pawn.health.RemoveAllHediffs();
		Pawn obj = (Pawn)GenSpawn.Spawn(pawn, loc, map);
		Thing thing = ThingMaker.MakeThing(named);
		obj.equipment.AddEquipment((ThingWithComps)thing);
		LetterDef named2 = DefDatabase<LetterDef>.GetNamed("BORPA_JohnBorpaLetter");
		obj.mindState.exitMapAfterTick = Find.TickManager.TicksGame + Rand.Range(18000, 50000);
		obj.mindState.mentalStateHandler.TryStartMentalState(DefDatabase<MentalStateDef>.GetNamed("ManhunterPermanent"), (string)null, true, false, false, (Pawn)null, false, false);
		obj.HostileTo(Faction.OfPlayer);
		Find.LetterStack.ReceiveLetter("LetterLabelJohnBorpa".Translate(), "LetterJohnBorpa".Translate(), named2, (LookTargets)pawn, (Faction)null, (Quest)null, (List<ThingDef>)null, (string)null);
		return true;
	}
}
