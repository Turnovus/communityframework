using HarmonyLib;
using RimWorld;
using Verse;
using System;

namespace StoryTime;

//[HarmonyPatch(typeof(Mineable))]
//[HarmonyPatch("TrySpawnYield")]
[HarmonyPatch(typeof(Mineable), "TrySpawnYield", new Type[] { typeof(Map), typeof(bool), typeof(Pawn) })]

[StaticConstructorOnStartup]
public class Mineable_TrySpawnYield_Patch
{
	public static void Postfix(Mineable __instance, Map __0, Pawn __2)
	{
		CompGemDropper compGemDropper = __instance.TryGetComp<CompGemDropper>();
		if (compGemDropper == null)
		{
			return;
		}
		ThingDef thingDef = GemDropperUtility.TryGetItem(compGemDropper.commonGemDropRate, compGemDropper.uncommonGemDropRate, compGemDropper.rareGemDropRate);
		if (thingDef != null)
		{
			Thing thing2 = ThingMaker.MakeThing(thingDef);
			thing2.stackCount = 1;
			GenPlace.TryPlaceThing(thing2, __instance.Position, __0, ThingPlaceMode.Near, ForbidIfNecessary);
			if (__2 != null && __2.Faction == Faction.OfPlayer)
			{
				Messages.Message("ST_FoundGem".Translate(__2.LabelShortCap, thing2.Label), thing2, MessageTypeDefOf.PositiveEvent);
			}
		}
		void ForbidIfNecessary(Thing thing, int count)
		{
			if ((__2 == null || !__2.IsColonist) && thing.def.EverHaulable && !thing.def.designateHaulable)
			{
				thing.SetForbidden(value: true, warnOnFail: false);
			}
		}
	}
}
