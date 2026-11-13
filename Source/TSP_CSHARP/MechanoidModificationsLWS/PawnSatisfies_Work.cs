using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using Verse;

namespace MechanoidModificationsLWS;

[HarmonyPatch(typeof(SkillRequirement))]
[HarmonyPatch("PawnSatisfies")]
public static class PawnSatisfies_Work
{
	private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
	{
		List<CodeInstruction> list = new List<CodeInstruction>(instructions);
		int num = -1;
		for (int i = 0; i < list.Count; i++)
		{
			FieldInfo field = AccessTools.Field(typeof(RaceProperties), "mechEnabledWorkTypes");
			if (list[i].opcode == OpCodes.Ldarg_0 && list[i - 1].LoadsField(field))
			{
				num = i;
			}
		}
		MethodInfo operand = AccessTools.Method(typeof(SkillsHandler), "Work");
		if (num != -1)
		{
			list[num - 2].opcode = OpCodes.Call;
			list[num - 2].operand = operand;
			list[num - 1].opcode = OpCodes.Nop;
		}
		return list;
	}
}
