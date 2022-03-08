using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Verse;
using RimWorld;

namespace CF
{
    /// <summary>
    /// A class to load data from existing classes on startup. Intended for <c>RangedShieldBelt</c> such that it copies the existing <c>ShieldBelt</c> class
    /// (with minor changes), in order to automatically keep up with updates.
    /// </summary>
    [StaticConstructorOnStartup]
    static class ResourceLoader
    {
        static ResourceLoader()
        {
            Type shieldBeltType = typeof(ShieldBelt);
            Type rangedShieldBeltType = typeof(RangedShieldBelt);
            foreach(MethodInfo originalMi in shieldBeltType.GetMethods().Where(x => x.DeclaringType == shieldBeltType))
            {
                Log.Message($"Method:\n\tOriginal: {originalMi.Name} {originalMi.DeclaringType}");
                MethodInfo newMi = rangedShieldBeltType.GetMethod(originalMi.Name);
                Log.Message($"\tNew     : {newMi.Name} {newMi.DeclaringType}");
            }
        }
    }
}
