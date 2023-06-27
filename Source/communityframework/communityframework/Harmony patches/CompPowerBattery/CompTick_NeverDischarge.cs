using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Reflection.Emit;
using Verse;
using RimWorld;
using HarmonyLib;

namespace CF
{
    [ClassWithPatches("BatterySelfDischargePatch")]
    public static class CompTick_NeverDischarge
    {
        [HarmonyPatch(typeof(CompPowerBattery))]
        public static class CompPowerBatteryPatches
        {
            private static readonly MethodInfo M_CanDischarge =
                typeof(CompPowerBatteryPatches).GetMethod(nameof(CompPowerBatteryPatches.CanDischarge),
                    BindingFlags.Public | BindingFlags.Static);

            [HarmonyPatch(nameof(CompPowerBattery.CompTick))]
            [HarmonyTranspiler]
            public static IEnumerable<CodeInstruction> DoNotDischargeIfExtension(
                IEnumerable<CodeInstruction> instructions,
                ILGenerator generator
            )
            {
                // Flag:
                // 0 - Looking for base.CompTick to inject patch
                // 1 - Injected patch, waiting one instruction to inject label
                // 2 = Patch done
                int flag = 0;
                Label label = generator.DefineLabel();

                foreach (CodeInstruction instruction in instructions)
                {
                    yield return instruction;

                    if (flag == 2)
                        continue;

                    if (flag == 1)
                    {
                        instruction.labels.Add(label);
                        flag = 2;
                        continue;
                    }

                    if (flag != 0 || !(instruction.opcode == OpCodes.Call && instruction.operand as MethodInfo == HarmonyUtils.M_ThingComp_CompTick))
                        continue;

                    flag = 1;
                    // push this
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    // pop this, push CanDischarge(this)
                    yield return new CodeInstruction(OpCodes.Call, M_CanDischarge);
                    // if true, jump over return
                    yield return new CodeInstruction(OpCodes.Brtrue_S, label);
                    // if false, return
                    yield return new CodeInstruction(OpCodes.Ret);
                }

                if (flag < 2)
                    ULog.Error("Patch " + nameof(DoNotDischargeIfExtension) + " failed.");
            }

            public static bool CanDischarge(CompPowerBattery battery)
            {
                BatteryExtension extension = battery.parent.def.GetModExtension<BatteryExtension>();
                return extension == null ? true : !extension.neverDischarge;
            }
        }
    }
}
