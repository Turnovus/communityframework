using Verse;

namespace StoryTime;

public class HediffComp_CoconutDeath : HediffComp
{
	public bool CoconutDeath;

	public HediffCompProperties_CoconutDeath Props => (HediffCompProperties_CoconutDeath)props;

	public virtual void Notify_PawnDied()
	{
		Map mapHeld = parent.pawn.Corpse.MapHeld;
		IntVec3 position = parent.pawn.Corpse.Position;
		if (mapHeld != null)
		{
			CoconutDeath = true;
			if (CoconutDeath)
			{
				GenSpawn.Spawn(ThingDef.Named(Props.targetCoconut), position, mapHeld);
				parent.pawn.Corpse.Destroy();
			}
		}
	}
}
