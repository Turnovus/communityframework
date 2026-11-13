using System.Collections.Generic;
using RimWorld;
using Verse;

namespace StoryTime;

public class HediffComp_Polymorph : HediffComp
{
	public bool PolymorphDeath;

	public HediffCompProperties_Polymorph Props => (HediffCompProperties_Polymorph)props;

	public virtual void Notify_PawnDied()
	{
		Map mapHeld = parent.pawn.Corpse.MapHeld;
		IntVec3 position = parent.pawn.Corpse.Position;
		if (mapHeld != null)
		{
			PolymorphDeath = true;
			if (PolymorphDeath)
			{
				Find.LetterStack.ReceiveLetter(Props.polymorphLetterLabel.Translate(), Props.polymorphLetter.Translate(parent.pawn), Props.polymorphLetterDef, (LookTargets)null, (Faction)null, (Quest)null, (List<ThingDef>)null, (string)null);
				GenSpawn.Spawn(ThingDef.Named(Props.targetPolymorph), position, mapHeld);
				parent.pawn.Corpse.Destroy();
			}
		}
	}
}
