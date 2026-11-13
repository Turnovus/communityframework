using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace StoryTime;

[StaticConstructorOnStartup]
[HarmonyPatch]
public static class MainMenuPatch
{
	private static readonly Texture2D storytimemenu = ContentFinder<Texture2D>.Get("UI/Menu/StorytimeMenu");

	private static IEnumerable<MethodBase> TargetMethods()
	{
		yield return AccessTools.Method(typeof(MainMenuDrawer), "MainMenuOnGUI");
	}

	private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
	{
		foreach (CodeInstruction codeInstruction in new List<CodeInstruction>(instructions))
		{
			if (codeInstruction.opcode == OpCodes.Ldsfld && (FieldInfo)codeInstruction.operand == AccessTools.Field(typeof(MainMenuDrawer), "TexTitle"))
			{
				codeInstruction.operand = AccessTools.Field(typeof(MainMenuPatch), "storytimemenu");
			}
			yield return codeInstruction;
		}
	}
}
