using Verse;

namespace MechanoidModificationsLWS;

public class HediffComp_SkillModification : HediffComp
{
	public HediffCompProperties_SkillModification Props => (HediffCompProperties_SkillModification)props;

	public override void CompPostPostAdd(DamageInfo? dinfo)
	{
		if (base.Pawn.IsColonyMech)
		{
			base.Pawn.Notify_DisabledWorkTypesChanged();
			base.Pawn.workSettings.SetPriority(Props.workType, 2);
		}
	}

	public override void CompPostPostRemoved()
	{
		base.Pawn.Notify_DisabledWorkTypesChanged();
	}
}
