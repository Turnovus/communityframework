using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;
using HarmonyLib;

namespace CF
{
    /// <summary>
    /// This patches the method Capacity from the MassUtility class so when CarryingCapacity is added as an equippedStatOffset, its weight is also added when setting up caravans.
    /// This is currently not the case in vanilla. 
    /// </summary>
    [ClassWithPatches("ApplyCapacityPatch")]
    static class CapacityPatch
    {
        [HarmonyPatch(typeof(MassUtility))]
        [HarmonyPatch("Capacity")]
        class Capacity
        {
            public static void Postfix(Pawn p, ref StringBuilder explanation, ref float __result)
            {
                try
                {
                    if (p?.apparel?.WornApparel?.Count > 0)
                    {
                        foreach (var app in p.apparel.WornApparel)
                        {
                            var stat = app?.def?.equippedStatOffsets?.FirstOrDefault(x => x?.stat == StatDefOf.CarryingCapacity);
                            if (stat == null) continue;
                            float val = stat?.value ?? 0f;
                            {
                                __result += val;
                            }
                        }

                        if (explanation == null) return;
                        if (explanation?.Length > 0)
                        {
                            explanation.AppendLine();
                        }

                        explanation.Append("  - " + (p?.LabelShortCap ?? p?.def?.defName ?? "Error") + ": " + __result.ToStringMassOffset());
                    }
                }
                catch (Exception e)
                {
                    Log.ErrorOnce(e.ToString(), 13131313);
                }
            }
        }
    }    
}
