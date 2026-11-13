using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;

namespace StoryTime;

internal class GemDropper_Patch
{
	[HarmonyPatch(typeof(Thing))]
	[HarmonyPatch("ButcherProducts")]
	public class Thing_ButcherProducts_Patch
	{
		public static void Postfix(Thing __instance, Pawn __0, ref IEnumerable<Thing> __result)
		{
			CompGemDropper compGemDropper = __instance.TryGetComp<CompGemDropper>();
			if (compGemDropper == null)
			{
				return;
			}
			ThingDef thingDef = GemDropperUtility.TryGetItem(compGemDropper.commonGemDropRate, compGemDropper.uncommonGemDropRate, compGemDropper.rareGemDropRate);
			if (thingDef != null)
			{
				Thing thing = ThingMaker.MakeThing(thingDef);
				thing.stackCount = 1;
				List<Thing> list = __result.ToList();
				list.Add(thing);
				__result = list.AsEnumerable();
				if (__0 != null && __0.Faction == Faction.OfPlayer)
				{
					Messages.Message("ST_FoundGem".Translate(__0.LabelShortCap, thing.Label), thing, MessageTypeDefOf.PositiveEvent);
				}
			}
		}
	}
}
