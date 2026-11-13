using RimWorld;
using RimWorld.Planet;
using Verse;

namespace StoryTime;

public class RuneBiomeWorker : BiomeWorker
{
	public override float GetScore(BiomeDef biome, Tile tile, PlanetTile planetTile)
	{
		double num = 610.0;
		double num2 = -10.0;
		double num3 = 0.007;
		return tile.WaterCovered ? (-100f) : (((double)tile.temperature > num2 || (double)tile.rainfall > num || (double)Rand.Value > num3) ? 0f : ((float)(70.0 + ((double)tile.temperature - 7.0) + ((double)tile.rainfall - num) / 180.0)));
	}
}
