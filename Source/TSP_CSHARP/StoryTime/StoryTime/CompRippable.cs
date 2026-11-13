using RimWorld;
using Verse;

namespace StoryTime;

public class CompRippable : CompHasGatherableBodyResource
{
	protected override int GatherResourcesIntervalDays => Props.ripIntervalDays;

	protected override int ResourceAmount => Props.legAmount;

	protected override ThingDef ResourceDef => Props.legDef;

	protected override string SaveKey => "woolGrowth";

	public CompProperties_Rippable Props => (CompProperties_Rippable)props;

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
		return Active ? ((string)("legGrowth".Translate() + ": " + base.Fullness.ToStringPercent())) : null;
	}
}
