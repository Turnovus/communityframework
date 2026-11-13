using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using Verse;

namespace MechanoidModificationsLWS;

[HarmonyPatch(typeof(GenConstruct))]
[HarmonyPatch("CanConstruct")]
[HarmonyPatch(new Type[]
{
	typeof(Thing),
	typeof(Pawn),
	typeof(bool),
	typeof(bool),
	typeof(JobDef),
})]
public static class CanConstruct_Level_Construction
{
	private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
	{
		List<CodeInstruction> list = new List<CodeInstruction>(instructions);
		int num = -1;
		for (int i = 0; i < list.Count; i++)
		{
			FieldInfo field = AccessTools.Field(typeof(RaceProperties), "mechFixedSkillLevel");
			FieldInfo field2 = AccessTools.Field(typeof(BuildableDef), "constructionSkillPrerequisite");
			if (list[i].opcode == OpCodes.Bge_S && list[i - 4].LoadsField(field) && list[i - 1].LoadsField(field2))
			{
				num = i;
			}
		}
		MethodInfo operand = AccessTools.Method(typeof(SkillsHandler), "Level");
		if (num != -1)
		{
			list[num - 5].opcode = OpCodes.Call;
			list[num - 5].operand = operand;
			list[num - 4].opcode = OpCodes.Nop;
		}
		return list;
	}
}
