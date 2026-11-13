using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace StoryTime;

public class JobDriver_TradeWith : JobDriver
{
	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return ReservationUtility.Reserve(pawn, (LocalTargetInfo)base.TargetThingA, job, 1, -1, (ReservationLayerDef)null, errorOnFailed);
	}

	protected override IEnumerable<Toil> MakeNewToils()
	{
		this.FailOnDespawnedOrNull(TargetIndex.A);
		yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
		Toil trade = new Toil();
		trade.initAction = delegate
		{
			Find.WindowStack.Add(new Dialog_Trade(trade.actor, base.TargetThingA.TryGetComp<CompTradingPost>().tradingPost));
		};
		yield return trade;
	}
}
