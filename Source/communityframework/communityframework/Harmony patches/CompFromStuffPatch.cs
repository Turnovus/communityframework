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
            typeof(ThingMaker),
            nameof(ThingMaker.MakeThing),
            new Type[] { typeof(ThingDef), typeof(ThingDef) }
        )]
        class AddCompPostfix
        {
            [HarmonyPostfix]
            public static void MakeThingPostfix(
                ref Thing __result,
                ref ThingDef stuff
            )
            {
                if (stuff == null || !(__result is ThingWithComps thingWithComps))
                    return;

                CompsToAddWhenStuff extension = stuff.GetModExtension<CompsToAddWhenStuff>();
                if (extension == null || extension.comps == null || extension.comps.Count <= 0)
                    return;

                foreach (CompProperties properties in extension.comps)
                {
                    ThingComp comp = (ThingComp)Activator.CreateInstance(properties.compClass);
                    comp.parent = thingWithComps;
                    thingWithComps.AllComps.Add(comp);
                    comp.Initialize(properties);
                }
            }
        }
    }
}