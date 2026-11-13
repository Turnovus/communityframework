using RimWorld;
using UnityEngine;
using Verse;

namespace StoryTime;

internal class GameCondition_GibbonousMoon : GameCondition
{
	private SkyColorSet GibbonMoonSkyColors = new SkyColorSet(new Color(0.403f, 0.6f, 0.8f), Color.white, new Color(0.6f, 0.6f, 0.6f), 1f);

	private int currentTicks = 295;

	private int ticksPerEvent = 3000;

	private bool gibbonMoonBegun = false;

	public override int TransitionTicks => 300;

	public override float SkyGazeChanceFactor(Map map)
	{
		return 8f;
	}

	public override float SkyGazeJoyGainFactor(Map map)
	{
		return 5f;
	}

	public override float SkyTargetLerpFactor(Map map)
	{
		return GameConditionUtility.LerpInOutValue(this, TransitionTicks);
	}

	public override SkyTarget? SkyTarget(Map map)
	{
		return new SkyTarget(0.8f, GibbonMoonSkyColors, 1f, 0f);
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref currentTicks, "currentTicks", 0);
		Scribe_Values.Look(ref ticksPerEvent, "ticksPerEvent", 3000);
		Scribe_Values.Look(ref gibbonMoonBegun, "gibbonMoonBegun", defaultValue: false);
	}

	public override void GameConditionTick()
	{
		if (!gibbonMoonBegun && currentTicks <= TransitionTicks)
		{
			foreach (Map affectedMap in base.AffectedMaps)
			{
				GibbonousMoon_Utility.SpawnMoonGibbon(affectedMap);
			}
		}
		currentTicks++;
	}

	public override void End()
	{
		base.End();
		if (base.AffectedMaps.NullOrEmpty())
		{
		}
	}
}
