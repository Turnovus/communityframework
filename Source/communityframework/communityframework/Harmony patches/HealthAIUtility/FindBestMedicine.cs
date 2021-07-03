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
    /// This patches the method FindBestMedicine so it checks for the RestrictMedicine comp. If it's there, check if the pawn is in the RestrictTo list.
    /// </summary>
    [HarmonyPatch(typeof(HealthAIUtility), "FindBestMedicine")]
    class FindBestMedicine
    {
        static Thing Postfix(Thing ret, Pawn healer, Pawn patient)
        {
            if (ret.TryGetComp<CompRestrictMedicine>() != null)
            {
                CompRestrictMedicine compRestrictMedicine = ret.TryGetComp<CompRestrictMedicine>();
                CompProperties_RestrictMedicine props = compRestrictMedicine.Props;

                if (props.restrictTo.Contains(patient.kindDef))
                {

                }
            }
        }
    }
}
}
