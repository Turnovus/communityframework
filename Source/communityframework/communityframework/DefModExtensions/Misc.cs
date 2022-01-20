using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace CF
{
    /// <summary>
    /// <c>DefModExtension</c> for use with <see cref="CF.ShouldHaveNeedPatch"/>. Needs listed here will be ignored by pawns with this <c>DefModExtension</c>.
    /// </summary>
    class IgnoreNeed : DefModExtension
    {
#pragma warning disable CS0649
        public List<NeedDef> needs;
#pragma warning restore CS0649
    }
    /// <summary>
    /// <c>DefModExtension</c> which flags the parent plant <c>ThingDef</c> as using negative fertility. 
    /// Specifies minimum and maximum fertility values within which the final fertility is clamped.
    /// </summary>
    /// <remarks>
    /// See <see cref="CF.NegativeFertilityPatch"/> for implementation details.
    /// </remarks>
    public class UseNegativeFertility : DefModExtension
    {
        public float minFertility = 0.05f, maxFertility = 1.4f;
    }
    /// <summary>
    /// <c>DefModExtension</c> for use with <see cref="CF.CompFromStuffPatch"/>. Specifies the <c>ThingComps</c>, by their <c>CompProperties</c>, 
    /// which should be added to newly-generated items made from the specified Stuff.
    /// </summary>
    class CompsToAddWhenStuff : DefModExtension
    {
# pragma warning disable CS0649 //disable the warning that this field is never assigned to, as the game handles that
        public List<CompProperties> comps;
#pragma warning restore CS0649
    }
}
