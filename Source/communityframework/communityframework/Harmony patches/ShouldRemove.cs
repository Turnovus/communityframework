using Verse;
using HarmonyLib;

namespace CF
{
    /// <summary>
    /// Harmony patch applied to the <c>Verse.Hediff_MissingPart</c> class,
    /// intended to provide functionality to <c>HediffComp_ShouldRemove</c>,
    /// even though <c>Hediff_MissingComp</c> overrides the method normally
    /// used to detect it.
    /// </summary>
    [ClassWithPatches("ApplyShouldRemovePatch")]
    static class ShouldRemovePatch
    {
        [HarmonyPatch(typeof(Hediff_MissingPart))]
        [HarmonyPatch("ShouldRemove", MethodType.Getter)]
        static class ShouldRemove
        {
            /// <summary>
            /// By default, <c>Hediff_MissingPart.ShouldRemove</c> is hard-coded to
            /// always return <c>false</c>. This patch causes it to return
            /// <c>true</c> if its comps contains <c>HediffComp_ShouldRemove</c>.
            /// A specific reference to <c>HediffComp_ShouldRemove</c> is used so
            /// that we don't break the behavior of other comps that are applied to
            /// missing parts.
            /// </summary>
            /// <remarks>
            /// Now, since we already know that the base method returns a constant
            /// value, we don't *need* to incorporate its return value. However,
            /// other modders may change this method, and we need to account for
            /// that.
            /// </remarks>
            /// <param name="__result">
            /// The original return value of
            /// <c>Hediff_MissingPart.ShouldRemove</c>.
            /// </param>
            /// <param name="__instance">
            /// The <c>Hediff_MissingPart</c> instance.
            /// </param>
            static void Postfix(ref bool __result, Hediff_MissingPart __instance)
            {
                __result = __result ||
                    __instance.TryGetComp<HediffComp_ShouldRemove>() != null;
            }
        }
    }    
}
