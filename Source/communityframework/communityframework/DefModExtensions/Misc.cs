using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace CF
{
    class IgnoreNeed : DefModExtension
    {
        public List<NeedDef> needs;
    }
    /// <summary>
    /// <c>DefModExtension</c> which flags the parent plant <c>ThingDef</c> as using negative fertility. Specifies minimum and maximum fertility values within which the final fertility is clamped.
    /// </summary>
    /// <remarks>
    /// See <see cref="CF.NegativeFertilityPatch"/> for implementation details.
    /// </remarks>
    public class UseNegativeFertility : DefModExtension
    {
        public float minFertility = 0.05f, maxFertility = 1.4f;
    }
}
