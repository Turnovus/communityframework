using System.Reflection;
using HarmonyLib;
using Verse;

namespace StoryTime;

[StaticConstructorOnStartup]
public class MenuPatch
{
	static MenuPatch()
	{
		new Harmony("com.jonah.rimworld.mod.storytime").PatchAll(Assembly.GetExecutingAssembly());
	}
}
