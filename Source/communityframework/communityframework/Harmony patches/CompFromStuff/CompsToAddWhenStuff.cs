using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace CF
{
    /// <summary>
    /// <c>DefModExtension</c> for use with <see cref="CF.CompFromStuff"/>. Specifies the <c>ThingComps</c>, by their <c>CompProperties</c>, 
    /// which should be added to newly-generated items made from the specified Stuff.
    /// </summary>
    class CompsToAddWhenStuff : DefModExtension
    {
# pragma warning disable CS0649 //disable the warning that this field is never assigned to, as the game handles that
        public List<CompProperties> comps;
#pragma warning restore CS0649
    }
}
