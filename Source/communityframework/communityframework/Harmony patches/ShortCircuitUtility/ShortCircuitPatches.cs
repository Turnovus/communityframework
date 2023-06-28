﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Verse;
using RimWorld;
using HarmonyLib;

namespace CF
{
    /// <summary>
    /// A collection of patches that affect the short circuit incident.
    /// </summary>
    [ClassWithPatches("ShortCircuitPatches")]
    public static class ShortCircuitPatches
    {
        /// <summary>
        /// This patch modifies the behaviors of batteries with <see cref="BatteryExtension"/>
        /// during a short circuit.
        /// </summary>
        [HarmonyPatch(typeof(ShortCircuitUtility))]
        [HarmonyPatch("DrainBatteriesAndCauseExplosion")] // Private method named by string
        public static class ShortCircuitPatches_Batteries
        {
            private static readonly MethodInfo M_CanShortCircuit =
                typeof(ShortCircuitPatches_Batteries).GetMethod(nameof(ShortCircuitPatches_Batteries.CanShortCircuit),
                    BindingFlags.Public | BindingFlags.Static);

            [HarmonyTranspiler]
            static IEnumerable<CodeInstruction> CheckBatteryExtension(
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

            /// <summary>
            /// 
            /// </summary>
            /// <param name="comp"></param>
            /// <returns></returns>
            public static bool CanShortCircuit(CompPowerBattery comp)
            {
                BatteryExtension extension = comp?.parent.def.GetModExtension<BatteryExtension>();
                return !extension?.neverShortCircuit ?? true;
            }
        }

        /// <summary>
        /// This patch allows custom buildings to act as short circuit sources, like vanilla
        /// conduits.
        /// </summary>
        [HarmonyPatch]
        public static class ShortCircuitPatches_ConduitPatch
        {
            private static readonly MethodInfo M_AddExtraBuildings =
                typeof(ShortCircuitPatches_ConduitPatch).GetMethod(
                    nameof(ShortCircuitPatches_ConduitPatch.AddExtraBuildings), BindingFlags.Static | BindingFlags.Public);
            private static readonly MethodInfo M_ListerThings_ThingsOfDef =
                typeof(ListerThings).GetMethod(
                    nameof(ListerThings.ThingsOfDef), BindingFlags.Public | BindingFlags.Instance);

            private static Type TargetInnerClass => AccessTools.FirstInner(
                    typeof(ShortCircuitUtility),
                    inner => inner.Name.Contains("<GetShortCircuitablePowerConduits>"));

            static MethodBase TargetMethod()
            {
                return AccessTools.FirstMethod(TargetInnerClass, method => method.Name.Contains("MoveNext"));
            }

            [HarmonyTranspiler]
            static IEnumerable<CodeInstruction> IncludeOtherShortSources(
                IEnumerable<CodeInstruction> instructions
            )
            {
                FieldInfo mapField = AccessTools.Field(TargetInnerClass, "map");

                bool patched = false;

                foreach (CodeInstruction instruction in instructions)
                {
                    yield return instruction;

                    if (patched || instruction.operand as MethodInfo != M_ListerThings_ThingsOfDef)
                        continue;

                    patched = true;

                    // Currently on stack: a list of conduits on the map

                    // Grab the map
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Ldfld, mapField);
                    // Call the method. Pops the list of things and the map, then pushes the list
                    // back in.
                    yield return new CodeInstruction(OpCodes.Call, M_AddExtraBuildings);
                }

                if (!patched)
                    ULog.Error("Patch " + nameof(ShortCircuitPatches_ConduitPatch.IncludeOtherShortSources) + " failed.");
            }

            /// <summary>
            /// Helper method for adding custom short circuit sources to the list of conduits found
            /// inside of the incident worker for the short circuit incident.
            /// </summary>
            /// <param name="list">The original list of conduits.</param>
            /// <param name="map">The map that the short circuit is occuring on.</param>
            /// <returns>
            /// A copy of <c>list</c> with additional short-circuitable buildings in <c>map</c>.
            /// </returns>
            public static List<Thing> AddExtraBuildings(List<Thing> list, Map map)
            {
                List<Thing> resultList = new List<Thing>(list);

                if (!StartupUtil.ExtraShortCircuitSources.Any())
                    return resultList;

                foreach (ThingDef def in StartupUtil.ExtraShortCircuitSources)
                {
                    List<Thing> extraThings = map.listerThings.ThingsOfDef(def);
                    if (!extraThings.NullOrEmpty())
                        resultList.AddRange(extraThings);
                }
                    
                return resultList;
            }
        }
    }
}
