using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace StoryTime;

public class CompTradingPost : ThingComp
{
	public int lastRestockTick;

	public TradingPost tradingPost;

	public CompProperties_TradingPost Props => props as CompProperties_TradingPost;

	public override void PostSpawnSetup(bool respawningAfterLoad)
	{
		base.PostSpawnSetup(respawningAfterLoad);
		if (!respawningAfterLoad)
		{
			Restock();
		}
	}

	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		foreach (Gizmo item in base.CompGetGizmosExtra())
		{
			yield return item;
		}
		yield return new Designator_ZoneAddStockpile_Resources();
	}

	public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
	{
		CompTradingPost compTradingPost = this;
		foreach (FloatMenuOption item in base.CompFloatMenuOptions(selPawn))
		{
			yield return item;
		}
		if (!selPawn.CanReach(compTradingPost.parent, PathEndMode.OnCell, Danger.Deadly))
		{
			yield return new FloatMenuOption("CannotTrade".Translate() + ": " + "NoPath".Translate().CapitalizeFirst(), null);
		}
		else if (selPawn.skills.GetSkill(SkillDefOf.Social).TotallyDisabled)
		{
			yield return new FloatMenuOption("CannotPrioritizeWorkTypeDisabled".Translate(SkillDefOf.Social.LabelCap), null);
		}
		yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("TradeWith".Translate(compTradingPost.parent.LabelShort + ", " + compTradingPost.tradingPost.TraderKind.label), delegate
		{
			Job job = JobMaker.MakeJob(TP_DefOf.TP_TradeWith, parent);
			job.playerForced = true;
			selPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.InteractingWithTraders, KnowledgeAmount.Total);
		}, MenuOptionPriority.InitiateSocial, null, compTradingPost.parent), selPawn, compTradingPost.parent);
	}

	private void Restock()
	{
		tradingPost = new TradingPost(parent, Props.traderKinds.RandomElement());
		tradingPost.Restock();
		lastRestockTick = Find.TickManager.TicksGame;
		if (Props.alerts)
		{
			Find.LetterStack.ReceiveLetter("TP.TradesUpdate".Translate(), "TP.TradesUpdateDesc".Translate(), LetterDefOf.NeutralEvent, (LookTargets)parent, (Faction)null, (Quest)null, (List<ThingDef>)null, (string)null);
		}
	}

	public override void CompTick()
	{
		base.CompTick();
		if (Find.TickManager.TicksGame > lastRestockTick + Props.refreshTraderTicks)
		{
			Restock();
		}
	}

	public override void PostExposeData()
	{
		base.PostExposeData();
		Scribe_Values.Look(ref lastRestockTick, "lastRestockTick", 0);
		Scribe_Deep.Look(ref tradingPost, "tradingPost");
	}
}
