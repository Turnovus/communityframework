using Verse;

namespace MechanoidModificationsLWS;

public class HediffCompProperties_SkillModification : HediffCompProperties
{
	public WorkTypeDef workType;

	public HediffCompProperties_SkillModification()
	{
		compClass = typeof(HediffComp_SkillModification);
	}
}
