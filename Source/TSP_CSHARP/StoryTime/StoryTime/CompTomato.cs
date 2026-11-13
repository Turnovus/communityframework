using RimWorld;
using Verse;

namespace StoryTime;

public class CompTomato : CompHasGatherableBodyResource
{
	protected override int GatherResourcesIntervalDays => Props.tomatoIntervalDays;

	protected override int ResourceAmount => Props.tomatoAmount;

	protected override ThingDef ResourceDef => Props.tomatoDef;

	protected override string SaveKey => "woolGrowth";

	public CompProperties_Tomato Props => (CompProperties_Tomato)props;

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
		return Active ? ((string)("tomatoGrowth".Translate() + ": " + base.Fullness.ToStringPercent())) : null;
	}
}
