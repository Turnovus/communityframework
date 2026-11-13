using System.Collections.Generic;
using Verse;

namespace RockSettings;

public class GameComponent_Rocks : GameComponent
{
	public class TileRockSettings : IExposable
	{
		public List<ThingDef> rocks;

		public TileRockSettings()
		{
		}

		public TileRockSettings(List<ThingDef> rocks)
		{
			this.rocks = rocks;
		}

		public void ExposeData()
		{
			Scribe_Collections.Look(ref rocks, "rocks", LookMode.Def);
		}
	}

	public Dictionary<int, TileRockSettings> rocksDict = new Dictionary<int, TileRockSettings>();

	public GameComponent_Rocks()
	{
	}

	public GameComponent_Rocks(Game game)
	{
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Collections.Look(ref rocksDict, "rocksDict", LookMode.Value, LookMode.Deep);
	}
}
