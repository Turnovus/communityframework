using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using Verse;

namespace MechanoidModificationsLWS;

[HarmonyPatch(typeof(QualityUtility))]
[HarmonyPatch("GenerateQualityCreatedByPawn")]
[HarmonyPatch(new Type[]
{
	typeof(Pawn),
	typeof(SkillDef),
	typeof(bool)
})]
public static class GenerateQualityCreatedByPawn_Level
{
	private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
	{
		List<CodeInstruction> list = new List<CodeInstruction>(instructions);
		int num = -1;
		for (int i = 0; i < list.Count; i++)
		{
			FieldInfo field = AccessTools.Field(typeof(RaceProperties), "mechFixedSkillLevel");
			if (list[i].opcode == OpCodes.Ldfld && list[i].LoadsField(field))
			{
				num = i;
			}
		}
		MethodInfo operand = AccessTools.Method(typeof(SkillsHandler), "Level");
		if (num != -1)
		{
			list[num - 1].opcode = OpCodes.Call;
			list[num - 1].operand = operand;
			list[num].opcode = OpCodes.Nop;
		}
		return list;
	}
}
