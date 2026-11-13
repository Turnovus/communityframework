using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace StoryTime;

public class DeathActionWorker_BloodExplosion : DeathActionWorker
{
	public override void PawnDied(Corpse corpse, Lord lord)
	{
		float num = ((corpse.InnerPawn.ageTracker.CurLifeStageIndex == 0) ? 5.9f : ((corpse.InnerPawn.ageTracker.CurLifeStageIndex != 1) ? 3.9f : 2.9f));
		GenExplosion.DoExplosion(corpse.Position, corpse.Map, num, DefDatabase<DamageDef>.GetNamed("BloodExplosion"), (Thing)corpse.InnerPawn, 10, -1f, SoundDef.Named("FrogPop"), (ThingDef)null, (ThingDef)null, (Thing)null, ThingDef.Named("Filth_Blood"), 100f, 6, (GasType?)null);
	}
}
