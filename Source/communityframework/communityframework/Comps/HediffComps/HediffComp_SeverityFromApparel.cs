using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace D9Framework
{
    /// <summary>
    /// Sets the parent hediff's severity based on worn apparel.
    /// </summary>
    /// <remarks>
    /// If wornSeverity is unset, uses the hediff's initial severity.
    /// If apparelDefs is unset, checks if any worn apparel have a matching <see cref="D9Framework.CompApplyHediffWhenWorn"/>.
    /// </remarks>
    class HediffComp_SeverityFromApparel : HediffComp
    {
        public HediffCompProps_SeverityFromApparel Props => (HediffCompProps_SeverityFromApparel)base.props;
        public float WornSeverity => Props.wornSeverity ?? base.parent.def.initialSeverity;

        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);
            if (IsCheapIntervalTick)
            {
                foreach (Apparel apparel in base.parent.pawn.apparel.WornApparel) if (IsMatching(apparel))
                    {
                        base.parent.Severity = WornSeverity;
                        return;
                    }
                // by default, sets it to 0, which removes the hediff
                base.parent.Severity = Props.unwornSeverity;
            }
        }

        public bool IsMatching(Apparel apparel)
        {
            if (Props.apparelDefs != null) return Props.apparelDefs.Contains(apparel.def);
            else return apparel.TryGetComp<CompApplyHediffWhenWorn>()?.Props.hediffsToApply.Contains(base.parent.def) ?? false;
        }

        #region cheap tick interval stuff
        private int hashOffset = 0;
        public bool IsCheapIntervalTick => (int)(Find.TickManager.TicksGame + hashOffset) % Props.tickInterval == 0;

        public override void CompPostMake() // todo: check whether this is called when loading a save
        {
            hashOffset = parent.pawn.thingIDNumber.HashOffset();
        }
        #endregion cheap tick interval stuff
    }
    class HediffCompProps_SeverityFromApparel : HediffCompProperties
    {
#pragma warning disable CS0649
        public float? wornSeverity;
        public float unwornSeverity = 0f;
        public List<ThingDef> apparelDefs = null;
        public int tickInterval = 250;
#pragma warning restore CS0649

        public HediffCompProps_SeverityFromApparel()
        {
            base.compClass = typeof(HediffComp_SeverityFromApparel);
        }
    }
}
