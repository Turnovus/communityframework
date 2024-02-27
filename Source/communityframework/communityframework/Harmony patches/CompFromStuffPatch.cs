using System;
using Verse;
using HarmonyLib;

namespace CF
{
    /// <summary>
    /// Allows modder to add comps to any items created with a certain Stuff
    /// using the <c>CompsToAddWhenStuff</c> <c>ModExtension</c>.
    /// </summary>
    [ClassWithPatches("ApplyCompFromStuffPatch")]
    static class CompFromStuffPatch
    {
        [HarmonyPatch(
            typeof(ThingWithComps),
            nameof(ThingWithComps.InitializeComps)
        )]
        class AddCompPostfix
        {
            [HarmonyPostfix]
            public static void MakeThingPostfix(ref ThingWithComps __instance)
            {
                CompsToAddWhenStuff extension = __instance.Stuff?.GetModExtension<CompsToAddWhenStuff>();
                if (extension == null || extension.comps == null || extension.comps.Count <= 0)
                    return;

                foreach (CompProperties properties in extension.comps)
                {
                    ThingComp comp = (ThingComp)Activator.CreateInstance(properties.compClass);
                    comp.parent = __instance;
                    __instance.AllComps.Add(comp);
                    comp.Initialize(properties);
                }
            }
        }
    }
}