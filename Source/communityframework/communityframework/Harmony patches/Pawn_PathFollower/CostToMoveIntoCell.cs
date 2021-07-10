using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;
using Verse.AI;
using HarmonyLib;

namespace CF
{
    /// <summary>
    /// This patches the method CostToMoveIntoCell from the Pawn_PathFollower class so when a hediff has the related comp, it can move unrestricted through any terrain.
    /// </summary>
    [HarmonyPatch(typeof(Pawn_PathFollower))]
    [HarmonyPatch("CostToMoveIntoCell")]
    class CostToMoveIntoCell
    {
        static int Postfix(int ret, Pawn pawn, IntVec3 c)
        {
            if (pawn.health.hediffSet.GetAllComps().Where(hediff => { return hediff is HediffComp_IgnorePathCost; }).Any())
            {
                return (c.x != pawn.Position.x && c.z != pawn.Position.z) ? pawn.TicksPerMoveDiagonal : pawn.TicksPerMoveCardinal;
            }
            return ret;
        }
    }
}
