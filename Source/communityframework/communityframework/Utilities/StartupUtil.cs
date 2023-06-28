using System.Collections.Generic;
using Verse;

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
