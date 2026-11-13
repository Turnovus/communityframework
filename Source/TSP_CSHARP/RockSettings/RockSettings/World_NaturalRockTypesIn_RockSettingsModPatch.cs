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

    private static bool Prefix(ref PlanetTile tile, World __instance, ref IEnumerable<ThingDef> __result)
    {
        // Check if the tile index is invalid or out of bounds
        if (tile < 0 || tile >= __instance.grid.TilesCount)
        {
            Log.Warning($"[RockSettings] Skipping NaturalRockTypesIn for invalid tile: {tile}");
            return true; // Let the original method handle it (to avoid breaking things)
        }

        // Check if the tile is in the dictionary (cached rock settings)
        if (GameComponent_Rocks.rocksDict.TryGetValue(tile, out var value))
        {
            __result = value.rocks;
            return false; // Skip original method
        }

        // Check if the tile has a biome
        BiomeDef biome = __instance.grid[tile].PrimaryBiome;
        if (biome == null)
        {
            Log.Warning($"[RockSettings] No biome found for tile {tile}. Possible pocket map.");
            return true; // Let the original method handle it
        }

        // Check if the biome has custom rock settings
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
                GameComponent_Rocks.rocksDict[tile] = new GameComponent_Rocks.TileRockSettings(list2);
                return false; // Skip original method
            }
        }

        return true; // Run the original method if no custom settings exist
    }
}
