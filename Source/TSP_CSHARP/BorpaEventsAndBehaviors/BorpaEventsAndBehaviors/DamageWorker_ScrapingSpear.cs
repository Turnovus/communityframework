using RimWorld;
using Verse;

namespace BorpaEventsAndBehaviors;

public class DamageWorker_ScrapingSpear : DamageWorker_Stab
{
	protected override void ApplySpecialEffectsToPart(Pawn pawn, float totalDamage, DamageInfo dinfo, DamageResult result)
	{
		//float scrapingSpearDurabilityDamage = LoadedModManager.GetMod<BorpaMod>().GetSettings<BorpaSettings>().ScrapingSpearDurabilityDamage;
		float scrapingSpearDurabilityDamage = 25f;
        bool flag = scrapingSpearDurabilityDamage != 0f;
		Pawn_ApparelTracker apparel = pawn.apparel;
		if (apparel != null && apparel.WornApparelCount > 0 && pawn.equipment != null && flag)
		{
			float num = scrapingSpearDurabilityDamage / 100f;
			foreach (Apparel item in pawn.apparel.WornApparel)
			{
				item.HitPoints -= (int)((float)item.MaxHitPoints * num);
			}
			pawn.apparel.WornApparel.FindAll((Apparel x) => x.HitPoints <= 0).ForEach(delegate(Apparel x)
			{
				x.Destroy();
			});
			foreach (ThingWithComps item2 in pawn.equipment.AllEquipmentListForReading)
			{
				item2.HitPoints -= (int)((float)item2.MaxHitPoints * num);
			}
			pawn.equipment.AllEquipmentListForReading.FindAll((ThingWithComps x) => x.HitPoints <= 0).ForEach(delegate(ThingWithComps x)
			{
				x.Destroy();
			});
		}
		else
		{
			base.ApplySpecialEffectsToPart(pawn, totalDamage, dinfo, result);
		}
	}
}
