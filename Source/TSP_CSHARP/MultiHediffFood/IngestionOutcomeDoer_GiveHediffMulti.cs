using System.Collections.Generic;
using JetBrains.Annotations;
using RimWorld;
using Verse;

[UsedImplicitly]
public class IngestionOutcomeDoer_GiveHediffMulti : IngestionOutcomeDoer
{
	public class HediffNode
	{
		public HediffDef hediffDef;

		public ChemicalDef toleranceChemical;

		public float weight;

		public float severity;

		public bool divideByBodySize;
	}

	public List<HediffNode> hediffs = new List<HediffNode>();

	protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested, int ingestedCount)
	{
		if (hediffs.Count == 0)
		{
			return;
		}
		HediffNode hediffNode = hediffs.RandomElementByWeight((HediffNode d) => d.weight);
		if (hediffNode == null)
		{
			return;
		}
		if (hediffNode.hediffDef == null)
		{
			Log.Error("Missing hediff def in this IngestionOutcomeDoer_GiveHediffMulti. Missing mod or misspelled def name?");
			return;
		}
		Hediff hediff = HediffMaker.MakeHediff(hediffNode.hediffDef, pawn);
		float num = ((hediffNode.severity > 0f) ? hediffNode.severity : hediffNode.hediffDef.initialSeverity);
		if (hediffNode.divideByBodySize)
		{
			num /= pawn.BodySize;
		}
		if (hediffNode.toleranceChemical != null)
		{
			AddictionUtility.ModifyChemicalEffectForToleranceAndBodySize(pawn, hediffNode.toleranceChemical, ref num, true);
		}
		hediff.Severity = num;
		pawn.health.AddHediff(hediff);
	}

	public override IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef)
	{
		if (!parentDef.IsDrug || !(chance >= 1f))
		{
			yield break;
		}
		foreach (HediffNode item in hediffs)
		{
			if (item.hediffDef == null)
			{
				continue;
			}
			foreach (StatDrawEntry item2 in item.hediffDef.SpecialDisplayStats(StatRequest.ForEmpty()))
			{
				yield return item2;
			}
		}
	}
}
