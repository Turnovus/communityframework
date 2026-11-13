using System.Collections.Generic;
using RimWorld;
using Verse;

namespace StoryTime;

public class CompProperties_TradingPost : CompProperties
{
	public List<TraderKindDef> traderKinds;

	public int refreshTraderTicks;

	public bool alerts;

	public bool cargoPodsDelivery;

	public TradeMode tradeMode;

	public CompProperties_TradingPost()
	{
		compClass = typeof(CompTradingPost);
	}
}
