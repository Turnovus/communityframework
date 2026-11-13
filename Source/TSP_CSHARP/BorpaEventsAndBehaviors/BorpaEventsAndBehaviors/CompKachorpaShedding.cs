using RimWorld;
using Verse;

namespace BorpaEventsAndBehaviors;

public class CompKachorpaShedding : CompHasGatherableBodyResource
{
	protected override int GatherResourcesIntervalDays => Props.shedIntervalDays;

	protected override int ResourceAmount => Props.steelAmount;

	protected override ThingDef ResourceDef => Props.steelDef;

	protected override string SaveKey => "kachorpaSteelGrowth";

	public CompProperties_KachorpaShedding Props => (CompProperties_KachorpaShedding)props;

	protected override bool Active
	{
		get
		{
			if (!base.Active)
			{
				return false;
			}
			if (parent is Pawn pawn)
			{
				return pawn.ageTracker.CurLifeStage.shearable;
			}
			return true;
		}
	}

	public override string CompInspectStringExtra()
	{
		if (!Active)
		{
			return null;
		}
		return "KachorpaSteelGrowth".Translate() + ": " + base.Fullness.ToStringPercent();
	}
}
