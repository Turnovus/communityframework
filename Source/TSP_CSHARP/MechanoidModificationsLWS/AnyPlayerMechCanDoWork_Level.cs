using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using Verse;

namespace MechanoidModificationsLWS;

[HarmonyPatch(typeof(MechanitorUtility))]
[HarmonyPatch("AnyPlayerMechCanDoWork")]
public static class AnyPlayerMechCanDoWork_Level
{
	private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
	{
		List<CodeInstruction> list = new List<CodeInstruction>(instructions);
		int num = -1;
		for (int i = 0; i < list.Count; i++)
		{
			FieldInfo field = AccessTools.Field(typeof(RaceProperties), "mechFixedSkillLevel");
			if (list[i].opcode == OpCodes.Blt_S && list[i - 2].LoadsField(field))
			{
				num = i;
			}
		}
		MethodInfo operand = AccessTools.Method(typeof(SkillsHandler), "Level");
		if (num != -1)
		{
			list[num - 3].opcode = OpCodes.Call;
			list[num - 3].operand = operand;
			list[num - 2].opcode = OpCodes.Nop;
		}
		return list;
	}
}
