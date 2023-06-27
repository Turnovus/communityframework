using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;
using HarmonyLib;

namespace CF
{
    [ClassWithPatches("ApplyAmountCanAcceptPatch")]
    public static class AmountCanAccept_CheckExtension
    {
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
    }
}
