using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace MechanoidModificationsLWS;

public class JobDriver_RemoveModification : JobDriver
{
	private const TargetIndex CorpseInd = TargetIndex.A;

	public const int RemoveTicks = 5000;

	protected Corpse Corpse => (Corpse)job.GetTarget(TargetIndex.A).Thing;

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return ReservationUtility.Reserve(pawn, (LocalTargetInfo)Corpse, job, 1, -1, (ReservationLayerDef)null, errorOnFailed);
	}

	public override string GetReport()
	{
		return "ReportRemovingModifications".Translate();
	}

	protected override IEnumerable<Toil> MakeNewToils()
	{
		if (!ModLister.CheckBiotech("Remove modififcation"))
		{
			yield break;
		}
		this.FailOnDestroyedOrNull(TargetIndex.A);
		this.FailOn(() => !Corpse.InnerPawn.RaceProps.IsMechanoid);
		yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
		Toil toil = Toils_General.Wait(5000, TargetIndex.A).WithProgressBarToilDelay(TargetIndex.A).WithEffect(() => EffecterDefOf.MechRepairing, TargetIndex.A)
			.PlaySustainerOrSound(SoundDefOf.RepairMech_Touch)
			.PlaySoundAtEnd(SoundDefOf.Mechlink_Removed);
		toil.handlingFacing = true;
		yield return toil;
		Toil toil2 = ToilMaker.MakeToil("MakeNewToils");
		toil2.initAction = delegate
		{
			Pawn innerPawn = Corpse.InnerPawn;
			List<Hediff> hediffs = innerPawn.health.hediffSet.hediffs;
			for (int num = hediffs.Count - 1; num >= 0; num--)
			{
				if (hediffs[num].def.defName.Contains("GEGEMM_"))
				{
					Thing thing = ThingMaker.MakeThing(ThingDef.Named(hediffs[num].def.spawnThingOnRemoved.defName));
					innerPawn.health.RemoveHediff(hediffs[num]);
					GenPlace.TryPlaceThing(thing, Corpse.Position, base.Map, ThingPlaceMode.Near);
				}
			}
		};
		toil2.defaultCompleteMode = ToilCompleteMode.Instant;
		yield return toil2;
	}
}
