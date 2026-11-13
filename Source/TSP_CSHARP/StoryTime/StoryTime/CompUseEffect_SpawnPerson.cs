using RimWorld;
using Verse;

namespace StoryTime;

public class CompUseEffect_SpawnPerson : CompUseEffect
{
	public override float OrderPriority => 1000f;

	public CompProperties_SpawnPawn SpawnerProps => props as CompProperties_SpawnPawn;

	public virtual void DoSpawn(Pawn usedBy)
	{
		Pawn pawn = PawnGenerator.GeneratePawn(SpawnerProps.pawnKind, Faction.OfPlayer);
		if (pawn != null)
		{
			GenPlace.TryPlaceThing(pawn, parent.Position, parent.Map, ThingPlaceMode.Near);
			if (SpawnerProps.sendMessage)
			{
				Messages.Message("BeanSpawned".Translate(pawn.Name.ToStringFull), MessageTypeDefOf.NeutralEvent);
			}
		}
	}

	public override void DoEffect(Pawn usedBy)
	{
		base.DoEffect(usedBy);
		for (int i = 0; i < SpawnerProps.amount; i++)
		{
			DoSpawn(usedBy);
		}
	}
}
