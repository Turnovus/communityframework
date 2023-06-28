using System.Reflection;
using Verse;

namespace CF
{
    class HarmonyUtils
    {
        public static readonly MethodInfo M_ThingComp_CompTick =
                typeof(ThingComp).GetMethod(nameof(ThingComp.CompTick),
                    BindingFlags.Public | BindingFlags.Instance);
    }
}
