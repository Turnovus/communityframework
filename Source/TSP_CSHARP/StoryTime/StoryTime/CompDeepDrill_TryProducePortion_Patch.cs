using HarmonyLib;
using RimWorld;
using Verse;

namespace StoryTime;

[HarmonyPatch(typeof(CompDeepDrill))]
[HarmonyPatch("TryProducePortion")]
public class CompDeepDrill_TryProducePortion_Patch
{
	public static void Postfix(CompDeepDrill __instance)
	{
		CompGemDropper compGemDropper = __instance.parent.TryGetComp<CompGemDropper>();
		if (compGemDropper == null)
		{
			return;
		}
		ThingDef thingDef = GemDropperUtility.TryGetItem(compGemDropper.commonGemDropRate, compGemDropper.uncommonGemDropRate, compGemDropper.rareGemDropRate);
		if (thingDef != null)
		{
			Thing thing = ThingMaker.MakeThing(thingDef);
			thing.stackCount = 1;
			GenPlace.TryPlaceThing(thing, __instance.parent.Position, __instance.parent.Map, ThingPlaceMode.Near);
			Building building = __instance.parent as Building;
			Pawn firstPawn = building.InteractionCell.GetFirstPawn(building.Map);
			if (firstPawn != null && firstPawn.Faction == Faction.OfPlayer)
			{
				Messages.Message("ST_FoundGem".Translate(firstPawn.LabelShortCap, thing.Label), thing, MessageTypeDefOf.PositiveEvent);
			}
		}
	}
}
