using Verse;

namespace MechanoidModificationsLWS;

[StaticConstructorOnStartup]
public static class ModFinder
{
	public static bool AlphaBees { get; }

	public static bool VanillaGeneticsExpanded { get; }

	public static bool VanillaBooksExpanded { get; }

	public static bool VanillaFishingExpanded { get; }

	public static bool DubsRimatomics { get; }

	static ModFinder()
	{
		AlphaBees = ModLister.HasActiveModWithName("Alpha Bees");
		VanillaGeneticsExpanded = ModLister.HasActiveModWithName("Vanilla Genetics Expanded");
		VanillaBooksExpanded = ModLister.HasActiveModWithName("Vanilla Books Expanded");
		VanillaFishingExpanded = ModLister.HasActiveModWithName("Vanilla Fishing Expanded");
		DubsRimatomics = ModLister.HasActiveModWithName("Dubs Rimatomics");
	}
}
