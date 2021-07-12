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
    /// This patches the method ShouldHaveNeed so it checks if a pawn has the IgnoreNeed defmodextension. If this defmodextension contains the need in it's list, ignore it.
    /// </summary>
    [HarmonyPatch(typeof(Pawn_NeedsTracker), "ShouldHaveNeed")]
    class ShouldHaveNeed
    {
        public static void Postfix(
            ref bool __result,
            NeedDef nd,
            Pawn ___pawn)
        {
            //Ensure that the pawn has the ModExtension before trying to access
            IgnoreNeed ignore = ___pawn.GetModExtension<IgnoreNeed>();
            if (ignore != null && ignore.needs.Contains(nd))
            {
                __result = false;
            }
        }
    }
}
