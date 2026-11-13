using HarmonyLib;
using Verse;

namespace RockSettings;

[StaticConstructorOnStartup]
public static class Start
{
	static Start()
	{
		new Harmony("DimonSever000.RockSettings").PatchAll();
	}
}
