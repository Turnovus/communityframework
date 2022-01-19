using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace CF
{
    /// <summary>
    /// A <c>ThingComp</c> which implements tick distribution to reduce lag spikes and microstuttering when a large number of instances exist.
    /// </summary>
    /// <remarks>
    /// Only really necessary for comps which are expected to be on large numbers of <c>Thing</c>s, for example stuffed buildings.
    /// </remarks>
    public abstract class CompWithCheapHashInterval : ThingComp
    {       
        private int hashOffset = 0;
        public bool IsCheapIntervalTick(int interval) => (int)(Find.TickManager.TicksGame + hashOffset) % interval == 0;

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            hashOffset = parent.thingIDNumber.HashOffset();
        }
    }
}
