using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace CF
{
    [StaticConstructorOnStartup]
    class StartupUtil
    {
        public static readonly List<ThingDef> ExtraShortCircuitSources = new List<ThingDef>();

        static StartupUtil()
        {
            foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefsListForReading)
            {
                PowerExtension powerExtension = thingDef.GetModExtension<PowerExtension>();

                if (powerExtension != null)
                {
                    if (powerExtension.shortCircuitSource)
                        ExtraShortCircuitSources.Add(thingDef);
                }
            }
        }
    }
}
