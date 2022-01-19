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
    /// Applies a specified hediff at a specified interval when the parent, which must be <c>Apparel</c>, is worn.
    /// </summary>
    /// <remarks>
    /// There unfortunately isn't a method which is run when apparel is equipped, so there will be a delay of max <c>tickInterval</c> before the hediff is applied.
    /// 
    /// For performance reasons, the interval isn't used when the apparel's <c>tickerType</c> is Rare.
    /// 
    /// If you want custom behavior with severity, e.g. increasing severity when worn, use a HediffComp for that; applied hediffs will be of severity <c>initialSeverity</c> to start.
    /// </remarks>
    class CompApplyHediffWhenWorn : CompWithCheapHashInterval
    {
        public Apparel Apparel => base.parent as Apparel;
        public CompProperties_ApplyHediffWhenWorn Props => (CompProperties_ApplyHediffWhenWorn)base.props;

        public void ApplyHediffs()
        {
            foreach(HediffDef hd in Props.hediffsToApply) Apparel.Wearer.health.AddHediff(hd, null, null, null);
        }

        public override void CompTick()
        {
            base.CompTick();
            if (IsCheapIntervalTick(Props.tickInterval)) ApplyHediffs();
        }
        public override void CompTickRare()
        {
            base.CompTickRare();
            ApplyHediffs();
        }
    }
    public class CompProperties_ApplyHediffWhenWorn : CompProperties
    {
#pragma warning disable CS0649
        public int tickInterval = 250;
        public List<HediffDef> hediffsToApply;
#pragma warning restore CS0649

        public CompProperties_ApplyHediffWhenWorn()
        {
            base.compClass = typeof(CompApplyHediffWhenWorn);
        }

        public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
        {
            foreach (string str in base.ConfigErrors(parentDef)) yield return str;
            if (!parentDef.thingClass.IsAssignableFrom(typeof(Apparel))) yield return "CompApplyHediffWhenWorn must be on a Thing with thingClass Apparel!";
        }
    }
}
