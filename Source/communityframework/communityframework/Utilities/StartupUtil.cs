using System.Collections.Generic;
using Verse;

namespace CF
{
    /// <summary>
    /// A class that, on startup, does any def modifications and caches any values neccessary.
    /// </summary>
    [StaticConstructorOnStartup]
    public class StartupUtil
    {
        /// <summary>
        /// A list of buildings that have <see cref="PowerExtension"/>, with
        /// <see cref="PowerExtension.shortCircuitSource"/> set to <c>true</c>. Cached here on
        /// startup so that we don't have to go looking through the entire <see cref="ThingDef"/>
        /// database anytime a short circuit occurs.
        /// </summary>
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
