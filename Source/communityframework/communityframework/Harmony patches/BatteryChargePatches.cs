using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using RimWorld;
using HarmonyLib;

namespace CF
{
    [ClassWithPatches("ApplyBatteryPatches")]
    public static class BatteryChargePatches
    {
        private static readonly MethodInfo M_CanDischarge =
                typeof(BatteryChargePatches).GetMethod(nameof(BatteryChargePatches.CanDischarge),
                    BindingFlags.Public | BindingFlags.Static);

        public static bool CanDischarge(CompPowerBattery battery)
        {
            BatteryExtension extension = battery.parent.def.GetModExtension<BatteryExtension>();
            return extension == null ? true : !extension.neverDischarge;
        }

        [HarmonyPatch(typeof(CompPowerBattery))]
        [HarmonyPatch(nameof(CompPowerBattery.AmountCanAccept))]
        [HarmonyPatch(MethodType.Getter)]
        public static class AmountCanAccept
        {
            [HarmonyPostfix]
            public static void CheckForBatteryExtension(ref CompPowerBattery __instance, ref float __result)
            {
                DefModExtension_BatteryAmountCanAccept extenstion = __instance.parent.def.GetModExtension<DefModExtension_BatteryAmountCanAccept>();
                if (extenstion == null)
                    return;
                __result = extenstion.AmountCanAccept(__instance, __result);
            }
        }

        [HarmonyPatch(typeof(CompPowerBattery))]
        [HarmonyPatch(nameof(CompPowerBattery.CompTick))]
        public static class BatteryDischargeCheckExtension
        {
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
        }

        [HarmonyPatch(typeof(CompPowerBattery))]
        [HarmonyPatch(nameof(CompPowerBattery.CompInspectStringExtra))]
        public static class BatteryInspectStringCheckExtension
        {
            private static readonly FieldInfo F_StoredEnergy = // Private field named by string
                AccessTools.Field(typeof(CompPowerBattery), "storedEnergy");

            [HarmonyTranspiler]
            public static IEnumerable<CodeInstruction> SkipDischargeLineIfExtension(
                IEnumerable<CodeInstruction> instructions
            )
            {
                // Flag:
                // 0 - Waiting for CompPowerBattery.storedEnergy to make sure we're looking at the
                //     correct evaluation
                // 1 - Looking for ble.un.s to inject our extra condition
                // 2 - Patch done
                int flag = 0;

                foreach (CodeInstruction instruction in instructions)
                {
                    yield return instruction;

                    switch (flag) {
                        case 0:
                            if (instruction.operand as FieldInfo == F_StoredEnergy)
                                flag = 1;
                            break;
                        case 1:
                            if (instruction.opcode == OpCodes.Ble_Un_S)
                            {
                                flag = 2;
                                // push this
                                yield return new CodeInstruction(OpCodes.Ldarg_0);
                                // pop this, push CanDischarge(this)
                                yield return new CodeInstruction(OpCodes.Call, M_CanDischarge);
                                // if false, use jump to the same label as the vanilla conditional
                                yield return new CodeInstruction(OpCodes.Brfalse_S, instruction.operand);
                            }
                            break;
                    }
                }

                if (flag < 2)
                    ULog.Error("Patch " + nameof(SkipDischargeLineIfExtension) + " failed.");
            }
        }
    }
}
