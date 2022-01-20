using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.Sound;
using RimWorld;

namespace CF {
    /// <summary>
    /// Minifies or destroys the parent <c>Thing</c> if it wouldn't be allowed to be placed at its current location. Must be used with one or more <c>PlaceWorker</c>s.
    /// </summary>
    public class CompValidator : CompWithCheapHashInterval
    {
        CompProperties_Validator Props => (CompProperties_Validator)base.props;
        public override void CompTick()
        {
            base.CompTick();   
            if (Props.ShouldUse && IsCheapIntervalTick(Props.tickInterval))
            {
                foreach(PlaceWorker pw in parent.def.PlaceWorkers)
                {
                    if (!pw.AllowsPlacing(parent.def, parent.Position, parent.Rotation, parent.Map).Accepted)
                    {
                        MinifyOrDestroy();
                        break;
                    }
                }
            }
        }

        public override string CompInspectStringExtra()
        {
            string ret = base.CompInspectStringExtra();
            if (Prefs.DevMode)
            {
                ret += "PlaceWorkers: (count = " + parent.def.PlaceWorkers.Count + "):";
                for (int i = 0; i < Math.Min(3, parent.def.PlaceWorkers.Count); i++) ret += "\n\t" + parent.def.PlaceWorkers.ElementAt(i).ToString();
            }
            return ret;
        }

        public virtual void MinifyOrDestroy()
        {
            if (parent.def.Minifiable)
            {
                Map map = parent.Map;
                MinifiedThing package = MinifyUtility.MakeMinified(parent);
                GenPlace.TryPlaceThing(package, parent.Position, map, ThingPlaceMode.Near);
                SoundDef.Named("ThingUninstalled").PlayOneShot(new TargetInfo(parent.Position, map));
            }
            else
            {
                parent.Destroy(DestroyMode.KillFinalize);
            }
        }
    }
    /// <summary>
    /// <c>CompProperties</c> for use with <see cref="CF.CompValidator"/>. Allows specifying the tick interval in XML, auto-assigns the appropriate class, and disables the comp if misconfigured.
    /// </summary>
    public class CompProperties_Validator : CompProperties
    {
        public int tickInterval = 250;        
        /// <value>
        /// Whether the validator should run its checks in-game. True by default, unless it's misconfigured, in which case checks will not run.
        /// </value>
        public bool ShouldUse { get; private set; }
        public CompProperties_Validator() => compClass = typeof(CompValidator);
        public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
        {
            if (parentDef.placeWorkers == null || parentDef.placeWorkers.Count < 1)
            {
                ShouldUse = false;
                yield return "CompProperties_Validator: no PlaceWorkers set!";
            }
            if (parentDef.tickerType != TickerType.Normal) yield return "CompProperties_Validator: TickerType is not Normal!";
        }
    }
}
