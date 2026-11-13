using HarmonyLib;
using Verse;

namespace MonolynAlgaeBall
{
    // Applies all [HarmonyPatch] classes in this assembly on game startup.
    [StaticConstructorOnStartup]
    public static class HarmonyPatcher
    {
        static HarmonyPatcher()
        {
            new Harmony("MonolynAlgaeBall").PatchAll();
        }
    }
}
