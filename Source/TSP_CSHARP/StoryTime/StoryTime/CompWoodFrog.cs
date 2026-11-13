using RimWorld;
using Verse;

namespace StoryTime;

public class CompWoodFrog : CompHasGatherableBodyResource
{
	protected override int GatherResourcesIntervalDays => Props.woodHarvestIntervalDays;

	protected override int ResourceAmount => Props.woodAmount;

	protected override ThingDef ResourceDef => Props.woodDef;

	protected override string SaveKey => "woolGrowth";

	public CompProperties_Wood Props => (CompProperties_Wood)props;

	protected override bool Active
	{
		get
		{
			if (!base.Active)
			{
				return false;
			}
			return !(parent is Pawn pawn) || pawn.ageTracker.CurLifeStage.shearable;
		}
	}

	public override string CompInspectStringExtra()
	{
		return Active ? ((string)("woodGrowth".Translate() + ": " + base.Fullness.ToStringPercent())) : null;
	}
}
