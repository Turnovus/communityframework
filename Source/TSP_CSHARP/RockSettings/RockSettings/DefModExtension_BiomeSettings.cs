using System.Collections.Generic;
using Verse;

namespace RockSettings;

public class DefModExtension_BiomeSettings : DefModExtension
{
	public IntRange rockCount;

	public List<ThingDef> allowedRockDefs;
}
