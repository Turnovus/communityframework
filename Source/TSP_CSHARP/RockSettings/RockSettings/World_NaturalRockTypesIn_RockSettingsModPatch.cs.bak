using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RockSettings;

[HarmonyPatch(typeof(World))]
[HarmonyPatch("NaturalRockTypesIn")]
public class World_NaturalRockTypesIn_RockSettingsModPatch
{
	private static GameComponent_Rocks GameComponent_Rocks => Current.Game.GetComponent<GameComponent_Rocks>();

	private static bool Prefix(ref int tile, World __instance, ref IEnumerable<ThingDef> __result)
	{
		if (GameComponent_Rocks.rocksDict.TryGetValue(tile, out var value))
		{
			__result = value.rocks;
			return false;
		}
		BiomeDef biome = __instance.grid[tile].biome;
		if (biome != null)
		{
			DefModExtension_BiomeSettings modExtension = biome.GetModExtension<DefModExtension_BiomeSettings>();
			if (modExtension != null && !modExtension.allowedRockDefs.NullOrEmpty())
			{
				int num = Mathf.Clamp(modExtension.rockCount.RandomInRange, 0, modExtension.allowedRockDefs.Count);
				if (num > 0)
				{
					List<ThingDef> list = modExtension.allowedRockDefs.InRandomOrder().ToList();
					List<ThingDef> list2 = new List<ThingDef>();
					for (int i = 0; i < num; i++)
					{
						list2.Add(list[i]);
					}
					__result = list2;
					GameComponent_Rocks.rocksDict.Add(tile, new GameComponent_Rocks.TileRockSettings(list2));
					return false;
				}
			}
		}
		return true;
	}
}
