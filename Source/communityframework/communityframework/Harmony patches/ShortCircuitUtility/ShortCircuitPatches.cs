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
    [ClassWithPatches("ShortCircuitPatches")]
    public static class ShortCircuitPatches
    {
        [HarmonyPatch(typeof(ShortCircuitUtility))]
        [HarmonyPatch("DrainBatteriesAndCauseExplosion")] // Private method named by string
        public static class ShortCircuitPatches_Batteries
        {
            private static readonly MethodInfo M_CanShortCircuit =
                typeof(ShortCircuitPatches_Batteries).GetMethod(nameof(ShortCircuitPatches_Batteries.CanShortCircuit),
                    BindingFlags.Public | BindingFlags.Static);

            [HarmonyTranspiler]
            public static IEnumerable<CodeInstruction> CheckBatteryExtension(
                IEnumerable<CodeInstruction> instructions,
                ILGenerator generator
            )
            {
                // Flag:
                // 0 - Waiting for stloc.1, to make sure the local var batteryComp is initialized.
                //     Inject evaluation method.
                // 1 - Looking for the next ldloc.0 (index) to inject label.
                // 2 - patch done.
                int flag = 0;
                Label label = generator.DefineLabel();

                foreach (CodeInstruction instruction in instructions)
                {
                    yield return instruction;
                    if (flag == 2)
                        continue;

                    if (flag == 0 && instruction.opcode == OpCodes.Stloc_1)
                    {
                        // Grab the local variable that stores the battery comp
                        yield return new CodeInstruction(OpCodes.Ldloc_1);
                        // Do the comparison, push result to buffer
                        yield return new CodeInstruction(OpCodes.Call, M_CanShortCircuit);
                        // If false, advance the foreach loop
                        yield return new CodeInstruction(OpCodes.Brfalse_S, label);

                        flag = 1;
                    }

                    if (flag == 1 && instruction.opcode == OpCodes.Ldloc_0)
                    {
                        instruction.labels.Add(label);
                        flag = 2;
                    }
                }

                if (flag != 2)
                    ULog.Error("Patch " + nameof(CheckBatteryExtension) + " failed. Flag: " + flag.ToString());
            }

            public static bool CanShortCircuit(CompPowerBattery comp)
            {
                BatteryExtension extension = comp?.parent.def.GetModExtension<BatteryExtension>();
                return !extension?.neverShortCircuit ?? true;
            }
        }
    }
}
