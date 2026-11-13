using Verse;
using Verse.AI.Group;


namespace BorpaEventsAndBehaviors;

public class DeathActionWorker_BorpaRemoveOnDeath : DeathActionWorker
{
	public override void PawnDied(Corpse c, Lord lord)
	{
		c.Destroy();
	}
}
