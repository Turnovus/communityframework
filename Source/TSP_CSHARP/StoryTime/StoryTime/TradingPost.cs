using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace StoryTime;

public class TradingPost : IExposable, ITrader, IThingHolder
{
	public TraderKindDef traderKindDef;

	private ThingOwner things;

	private List<Pawn> soldPrisoners = new List<Pawn>();

	private int randomPriceFactorSeed = -1;

	public ThingWithComps tradingPost;

	public int Silver => CountHeldOf(ThingDefOf.Silver);

	public TradeCurrency TradeCurrency => TraderKind.tradeCurrency;

	public IThingHolder ParentHolder => null;

	public TraderKindDef TraderKind => traderKindDef;

	public int RandomPriceFactorSeed => randomPriceFactorSeed;

	public float TradePriceImprovementOffsetForPlayer => 0f;

	public IEnumerable<Thing> Goods
	{
		get
		{
			CompTradingPost compTradingPost = CompTradingPost;
			if (compTradingPost.Props.tradeMode == TradeMode.BuyOnly || compTradingPost.Props.tradeMode == TradeMode.Both)
			{
				int i = 0;
				while (i < things.Count)
				{
					yield return things[i];
					int num = i + 1;
					i = num;
				}
				yield break;
			}
			foreach (Thing item in things.Where((Thing x) => x.def == ThingDefOf.Silver))
			{
				yield return item;
			}
		}
	}

	public string TraderName => traderKindDef.LabelCap;

	public bool CanTradeNow => true;

	public Faction Faction => null;

	public CompTradingPost CompTradingPost => tradingPost.TryGetComp<CompTradingPost>();

	public TradingPost()
	{
		things = new ThingOwner<Thing>(this);
		randomPriceFactorSeed = Rand.RangeInclusive(1, 10000000);
	}

	public TradingPost(ThingWithComps tradingPost, TraderKindDef traderKindDef)
	{
		things = new ThingOwner<Thing>(this);
		randomPriceFactorSeed = Rand.RangeInclusive(1, 10000000);
		this.tradingPost = tradingPost;
		this.traderKindDef = traderKindDef;
	}

	public void Restock()
	{
		things.ClearAndDestroyContents();
		GenerateThings();
	}

	public IEnumerable<Thing> ColonyThingsWillingToBuy(Pawn playerNegotiator)
	{
		CompTradingPost compTradingPost = CompTradingPost;
		if (compTradingPost.Props.tradeMode == TradeMode.SellOnly || compTradingPost.Props.tradeMode == TradeMode.Both)
		{
			foreach (Thing item in playerNegotiator.Map.listerThings.AllThings.Where((Thing x) => x.def.category == ThingCategory.Item && TradeUtility.PlayerSellableNow(x, playerNegotiator) && !x.Position.Fogged(x.Map) && (playerNegotiator.Map.areaManager.Home[x.Position] || x.IsInAnyStorage()) && ReachableForTrade(playerNegotiator, x)))
			{
				yield return item;
			}
			if (playerNegotiator.GetLord() == null)
			{
				yield break;
			}
			foreach (Pawn item2 in from x in TradeUtility.AllSellableColonyPawns(playerNegotiator.Map)
				where !x.Downed
				select x)
			{
				yield return item2;
			}
			yield break;
		}
		foreach (Thing item3 in from x in playerNegotiator.Map.listerThings.ThingsOfDef(ThingDefOf.Silver)
			where !x.Position.Fogged(x.Map) && (playerNegotiator.Map.areaManager.Home[x.Position] || x.IsInAnyStorage()) && ReachableForTrade(playerNegotiator, x)
			select x)
		{
			yield return item3;
		}
	}

	private bool ReachableForTrade(Pawn playerNegotiator, Thing thing)
	{
		return playerNegotiator.Map == thing.Map && playerNegotiator.Map.reachability.CanReach(playerNegotiator.Position, thing, PathEndMode.Touch, TraverseMode.PassDoors, Danger.Some);
	}

	public IEnumerable<Pawn> AllSellableColonyPawns(Pawn negotiator)
	{
		foreach (Pawn pawn2 in negotiator.Map.mapPawns.PrisonersOfColonySpawned)
		{
			if (pawn2.guest.PrisonerIsSecure)
			{
				pawn2.guest.joinStatus = JoinStatus.JoinAsColonist;
				yield return pawn2;
			}
		}
		foreach (Pawn pawn in negotiator.Map.mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer))
		{
			if (negotiator != pawn && ((pawn.IsColonistPlayerControlled && pawn.HostFaction == null && !pawn.InMentalState && !pawn.Downed) || pawn.IsSlave))
			{
				pawn.guest.joinStatus = JoinStatus.JoinAsColonist;
				yield return pawn;
			}
		}
	}

	public void GenerateThings()
	{
		things.TryAddRangeOrTransfer(ThingSetMakerDefOf.TraderStock.root.Generate(new ThingSetMakerParams
		{
			traderDef = TraderKind,
			tile = tradingPost.Map.Tile
		}));
	}

	public void TraderTick()
	{
		for (int num = things.Count - 1; num >= 0; num--)
		{
			if (things[num] is Pawn pawn)
			{
				//pawn.Tick();
				if (pawn.Dead)
				{
					things.Remove(pawn);
				}
			}
		}
	}

	public void ExposeData()
	{
		Scribe_Deep.Look(ref things, "things", this);
		Scribe_Collections.Look(ref soldPrisoners, "soldPrisoners", LookMode.Reference);
		Scribe_Values.Look(ref randomPriceFactorSeed, "randomPriceFactorSeed", 0);
		Scribe_References.Look(ref tradingPost, "tradingPost");
		Scribe_Defs.Look(ref traderKindDef, "traderKindDef");
		if (Scribe.mode == LoadSaveMode.PostLoadInit)
		{
			soldPrisoners.RemoveAll((Pawn x) => x == null);
		}
	}

	public int CountHeldOf(ThingDef thingDef, ThingDef stuffDef = null)
	{
		return HeldThingMatching(thingDef, stuffDef)?.stackCount ?? 0;
	}

	public void GiveSoldThingToTrader(Thing toGive, int countToGive, Pawn playerNegotiator)
	{
		Thing thing = toGive.SplitOff(countToGive);
		thing.PreTraded(TradeAction.None, playerNegotiator, this);
		Thing thing2 = TradeUtility.ThingFromStockToMergeWith(this, thing);
		if (thing2 != null)
		{
			if (!thing2.TryAbsorbStack(thing, respectStackLimit: false))
			{
				thing.Destroy();
			}
			return;
		}
		if (thing is Pawn pawn && pawn.RaceProps.Humanlike)
		{
			soldPrisoners.Add(pawn);
		}
		things.TryAddOrTransfer(thing, canMergeWithExistingStacks: false);
	}

	public void GiveSoldThingToPlayer(Thing toGive, int countToGive, Pawn playerNegotiator)
	{
		Thing thing = toGive.SplitOff(countToGive);
		thing.PreTraded(TradeAction.PlayerBuys, playerNegotiator, this);
		if (thing is Pawn item)
		{
			soldPrisoners.Remove(item);
		}
		if (CompTradingPost.Props.cargoPodsDelivery)
		{
			TradeUtility.SpawnDropPod(DropCellFinder.TradeDropSpot(playerNegotiator.Map), playerNegotiator.Map, thing);
		}
		else
		{
			GenPlace.TryPlaceThing(thing, tradingPost.Position, tradingPost.Map, ThingPlaceMode.Near);
		}
	}

	private Thing HeldThingMatching(ThingDef thingDef, ThingDef stuffDef)
	{
		for (int i = 0; i < things.Count; i++)
		{
			if (things[i].def == thingDef && things[i].Stuff == stuffDef)
			{
				return things[i];
			}
		}
		return null;
	}

	public void ChangeCountHeldOf(ThingDef thingDef, ThingDef stuffDef, int count)
	{
		Thing thing = HeldThingMatching(thingDef, stuffDef);
		if (thing == null)
		{
			Log.Error("Changing count of thing trader doesn't have: " + thingDef);
		}
		thing.stackCount += count;
	}

	public ThingOwner GetDirectlyHeldThings()
	{
		return things;
	}

	public void GetChildHolders(List<IThingHolder> outChildren)
	{
		ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, GetDirectlyHeldThings());
	}
}
