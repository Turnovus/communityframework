using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Verse;
using RimWorld;

namespace CF
{
    /// <summary>
    /// A Harmony patch class which copies classes out of the original <c>ShieldBelt</c> class so <c>RangedShieldBelt</c> can use them. 
    /// </summary>
    /// <remarks>
    /// See <see href="https://harmony.pardeike.net/articles/reverse-patching.html" langword="the original article"/> for more info.
    /// </remarks>
    [HarmonyPatch]
    public static class ShieldBeltStubs
    {
        private const string ERROR_TEXT = "A reverse patch in Community Framework copying RimWorld.ShieldBelt.{0} into CF.RangedShieldBelt.{0} apparently failed.";
        [HarmonyReversePatch]
        [HarmonyPatch(typeof(ShieldBelt), nameof(ShieldBelt.ExposeData))]
        public static void ExposeData(object inst) { throw new NotImplementedException(ERROR_TEXT.Replace("{0}", "ExposeData()"));  }
        [HarmonyReversePatch]
        [HarmonyPatch(typeof(ShieldBelt), nameof(ShieldBelt.GetWornGizmos))]
        public static IEnumerable<Gizmo> GetWornGizmos(object inst) { throw new NotImplementedException(ERROR_TEXT.Replace("{0}", "GetWornGizmos()"));  }
        [HarmonyReversePatch]
        [HarmonyPatch(typeof(ShieldBelt), nameof(ShieldBelt.GetSpecialApparelScoreOffset))]
        public static float GetSpecialApparelScoreOffset(object inst) { throw new NotImplementedException(ERROR_TEXT.Replace("{0}", "GetSpecialApparelScoreOffset()"));  }
        [HarmonyReversePatch]
        [HarmonyPatch(typeof(ShieldBelt), nameof(ShieldBelt.Tick))]
        public static void Tick(object inst) { throw new NotImplementedException(ERROR_TEXT.Replace("{0}", "Tick()"));  }
        [HarmonyReversePatch]
        [HarmonyPatch(typeof(ShieldBelt), nameof(ShieldBelt.CheckPreAbsorbDamage))]
        public static bool CheckPreAbsorbDamage(object inst, DamageInfo dinfo) { throw new NotImplementedException(ERROR_TEXT.Replace("{0}", "CheckPreAbsorbDamage()"));  }
        [HarmonyReversePatch]
        [HarmonyPatch(typeof(ShieldBelt), nameof(ShieldBelt.KeepDisplaying))]
        public static void KeepDisplaying(object inst) { throw new NotImplementedException(ERROR_TEXT.Replace("{0}", "KeepDisplaying()"));  }
        [HarmonyReversePatch]
        [HarmonyPatch(typeof(ShieldBelt), "AbsorbedDamage")] // AbsorbedDamage is private
        public static void AbsorbedDamage(object inst, DamageInfo dinfo) { throw new NotImplementedException(ERROR_TEXT.Replace("{0}", "AbsorbedDamage()"));  }
        [HarmonyReversePatch]
        [HarmonyPatch(typeof(ShieldBelt), "Break")] // also private
        public static void Break(object inst) { throw new NotImplementedException(ERROR_TEXT.Replace("{0}", "Break()"));  }
        [HarmonyReversePatch]
        [HarmonyPatch(typeof(ShieldBelt), "Reset")] // also private
        public static void Reset(object inst) { throw new NotImplementedException(ERROR_TEXT.Replace("{0}", "Reset()"));  }
        [HarmonyReversePatch]
        [HarmonyPatch(typeof(ShieldBelt), nameof(ShieldBelt.DrawWornExtras))]
        public static void DrawWornExtras(object inst) { throw new NotImplementedException(ERROR_TEXT.Replace("{0}", "DrawWornExtras()"));  }
    }
}
