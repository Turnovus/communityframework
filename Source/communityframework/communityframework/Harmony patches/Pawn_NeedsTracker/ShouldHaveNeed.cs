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
        public static bool Postfix(ref bool __result, NeedDef _needDef, Pawn _pawn)
        {
            if (_pawn.GetModExtension<IgnoreNeed>().needs.Contains(_needDef))
            {
                return false;
            }
            return __result;
        }
    }
}
