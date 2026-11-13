using HarmonyLib;
using Verse;

namespace StoryTime;

[StaticConstructorOnStartup]
internal class Main
{
	static Main()
	{
		Harmony harmony = new Harmony("com.Rimworld.StoryTime");
		harmony.PatchAll();
	}
}
