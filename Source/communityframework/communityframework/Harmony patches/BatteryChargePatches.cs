using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using RimWorld;
using HarmonyLib;

namespace CF
{
    /// <summary>
    /// A collection of patches that alter the behaviors of batteries, allowing for more control
    /// over custom batteries.
    /// </summary>
    [ClassWithPatches("ApplyBatteryPatches")]
    public static class BatteryChargePatches
    {
        private static readonly MethodInfo M_CanDischarge =
                typeof(BatteryChargePatches).GetMethod(nameof(BatteryChargePatches.CanDischarge),
                    BindingFlags.Public | BindingFlags.Static);

        /// <summary>
        /// Checks if the battery is allowed to discharge by <see cref="BatteryExtension"/>.
        /// </summary>
        /// <param name="battery">
        /// The <see cref="CompPowerBattery"/> of the battery being checked.
        /// </param>
        /// <returns>
        /// <c>true</c> if the battery's <see cref="Verse.ThingDef"/> does not have a
        /// <see cref="BatteryExtension"/>, or if its <see cref="BatteryExtension.neverDischarge"/>
        /// is <c>false</c>. <c>true</c> otherwise.
        /// </returns>
        public static bool CanDischarge(CompPowerBattery battery)
        {
            BatteryExtension extension = battery.parent.def.GetModExtension<BatteryExtension>();
            return extension == null || !extension.neverDischarge;
        }

        /// <summary>
        /// This patch modifies the amount of additional energy that a battery can accept.
        /// </summary>
        [HarmonyPatch(typeof(CompPowerBattery))]
        [HarmonyPatch(nameof(CompPowerBattery.AmountCanAccept))]
        [HarmonyPatch(MethodType.Getter)]
        public static class AmountCanAccept
        {
            /// <summary>
            /// Checks the battery's <see cref="Verse.ThingDef"/> for any extension that derives
            /// <see cref="AmountCanAccept"/>, then adjusts the return value of the method
            /// accordingly.
            /// </summary>
            /// <param name="__instance">
            /// The <see cref="CompPowerBattery"/> being checked.
            /// </param>
            /// <param name="__result">
            /// The amount of additional energy that the battery will accept.
            /// </param>
            [HarmonyPostfix]
            public static void CheckForBatteryExtension(ref CompPowerBattery __instance, ref float __result)
            {
                DefModExtension_BatteryAmountCanAccept extenstion = __instance.parent.def.GetModExtension<DefModExtension_BatteryAmountCanAccept>();
                if (extenstion == null)
                    return;
                __result = extenstion.AmountCanAccept(__instance, __result);
            }
        }

        /// <summary>
        /// This patch alters whether or not a battery self-discharges the hardcoded amount of 5W
        /// per day.
        /// </summary>
        [HarmonyPatch(typeof(CompPowerBattery))]
        [HarmonyPatch(nameof(CompPowerBattery.CompTick))]
        public static class BatteryDischargeCheckExtension
        {
            /// <summary>
            /// Injects a condition into <see cref="CompPowerBattery.CompTick"/> that checks if the
            /// parent's <see cref="Verse.ThingDef"/> contains a <see cref="BatteryExtension"/>,
            /// then checks if <see cref="BatteryExtension.neverDischarge"/> is <c>true</c>. If the
            /// battery has been configured never to discharge, then it will skip over the
            /// instructions for self-discharge.
            /// </summary>
            /// <param name="instructions">The instructions given to the transpiler.</param>
            /// <param name="generator"><see cref="ILGenerator"/> used for creating labels</param>
            /// <returns>A patched list of instructions</returns>
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

        /// <summary>
        /// This patch prevents batteries from showing the "self-discharging" line on the
        /// inspection pane if the battery has been configured not to self-discharge.
        /// </summary>
        [HarmonyPatch(typeof(CompPowerBattery))]
        [HarmonyPatch(nameof(CompPowerBattery.CompInspectStringExtra))]
        public static class BatteryInspectStringCheckExtension
        {
            private static readonly FieldInfo F_StoredEnergy = // Private field named by string
                AccessTools.Field(typeof(CompPowerBattery), "storedEnergy");

            /// <summary>
            /// Injects a condition into <see cref="CompPowerBattery.CompInspectStringExtra"/> that
            /// checks if the parent's <see cref="Verse.ThingDef"/> contains a
            /// <see cref="BatteryExtension"/>, then checks if
            /// <see cref="BatteryExtension.neverDischarge"/> is <c>true</c>. If the battery has
            /// been configured never to discharge, then it will skip over the instructions to
            /// include the string "Self-discharging: 5W".
            /// </summary>
            /// <param name="instructions">The instructions given to the transpiler.</param>
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
