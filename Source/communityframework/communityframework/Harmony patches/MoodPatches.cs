using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace CF
{
    [ClassWithPatches("ApplyMoodPatch")]
    static class MoodPatches
    {
        [HarmonyPatch(typeof(MentalBreaker))]
        class MentalBreakPatch
        {
            private static readonly MethodInfo M_AdjustBreakFrequency = AccessTools.Method(
                typeof(MentalBreakPatch), nameof(MentalBreakPatch.AdjustBreakFrequency));
            
            private static readonly FieldInfo F_MentalBreaker_Pawn =
                AccessTools.Field(typeof(MentalBreaker), "pawn");
            
            [HarmonyTranspiler]
            [HarmonyPatch("TestMoodMentalBreak")] // Private method named by string
            public static IEnumerable<CodeInstruction> InjectBreakFrequencyStat(
                IEnumerable<CodeInstruction> instructions)
            {
                // We want to inject our instructions this many times. If this variable is non-zero
                // at the end of the patch, something has gone wrong.
                int injectCount = 3;
                foreach (CodeInstruction instruction in instructions)
                {
                    // If we find the length of a day, we know we're looking at the parameters of
                    // an MTB method. We want to intercept the first parameter of each call (3
                    // calls total) with our own method.
                    if (instruction.opcode == OpCodes.Ldc_R4 &&
                        instruction.operand as float? == GenDate.TicksPerDay)
                    {
                        yield return new CodeInstruction(OpCodes.Ldarg_0); // this
                        yield return new CodeInstruction(OpCodes.Ldfld, F_MentalBreaker_Pawn);
                        yield return new CodeInstruction(OpCodes.Call, M_AdjustBreakFrequency);

                        injectCount--;
                    }
                    
                    yield return instruction;
                }

                if (injectCount != 0)
                    ULog.Error("MentalBreakPatch.InjectBreakFrequencyStat failed with flag " + injectCount);
            }

            public static float AdjustBreakFrequency(float factor, Pawn pawn)
            {
                return factor / pawn.GetStatValue(CF_StatDefOf.CF_RandomBreakFrequency);
            }
        }
    }
}