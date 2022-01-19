using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace D9Framework
{
    /// <summary>
    /// <c>ModSettings</c> class for D9 Framework. Mainly handles which Harmony patches should be applied and saves all specified settings.
    /// </summary>
    /// <remarks>Static for convenience.</remarks>
    public class D9FModSettings : ModSettings
    {        
        public static bool DEBUG = false; //for release set false by default
        public static bool PrintPatchedMethods => DEBUG && printPatchedMethods;
        public static bool printPatchedMethods = false;

        // I don't want to touch CMF because it has custom patching, so retain old settings setup.
        public static bool ApplyCarryMassFramework => !DEBUG || applyCMF;
        public static bool applyCMF = true;

        // despite being public, please don't fuck with these. Access patch application settings with ShouldPatch.
        // They're only public so I can use them in the mod settings screen.
        public static Dictionary<string, PatchInfo> Patches = new Dictionary<string, PatchInfo>();

        public class PatchInfo : IExposable
        {
            public bool apply;
            public string saveKey, labelKey, descKey;

            // The game requires a no-arg constructor
            public PatchInfo() { }

            public PatchInfo(string s, bool a, string l, string d)
            {
                saveKey = s;
                apply = a;
                labelKey = l;
                descKey = d;
            }

            public override string ToString()
            {
                return "<{" + saveKey + ": " + apply + "}>";
            }

            public void ExposeData()
            {
                Scribe_Values.Look(ref saveKey, "saveKey");
                Scribe_Values.Look(ref apply, "apply");
                Scribe_Values.Look(ref labelKey, "labelKey");
                Scribe_Values.Look(ref descKey, "descKey");
            }
        } 

        public override void ExposeData()
        {
            base.ExposeData();
            List<PatchInfo> savePatches = new List<PatchInfo>();
            if (Scribe.mode == LoadSaveMode.Saving)
            {
                savePatches = SerializePatches();     
            }
            Scribe_Collections.Look(ref savePatches, "Patches", LookMode.Deep);
            if(Scribe.mode != LoadSaveMode.Saving)
            {
                DeserializePatches(savePatches);
            }
            Scribe_Values.Look(ref applyCMF, "ApplyCarryMassFramework", true);
            Scribe_Values.Look(ref DEBUG, "debug", false);
            Scribe_Values.Look(ref printPatchedMethods, "printPatchedMethods");
        }

        public static bool ShouldPatch(string patchkey)
        {
            if (!DEBUG) return true;
            if (!Patches.ContainsKey(patchkey))
            {
                ULog.Warning("ShouldPatch called for non-initialized patchkey.");
                return true;
            }
            return Patches[patchkey].apply;
        }

        public static List<PatchInfo> SerializePatches()
        {
            List<PatchInfo> ret = new List<PatchInfo>();
            foreach (string key in Patches.Keys.ToList())
            {
                ret.Add(new PatchInfo(key,
                    Patches[key].apply,
                    Patches[key].labelKey,
                    Patches[key].descKey));
            }
            return ret;
        }

        public static void DeserializePatches(List<PatchInfo> list)
        {
            foreach (PatchInfo pi in list)
            {
                Patches[pi.saveKey] = pi;
            }
        }
    }
    /// <summary>
    /// <c>Mod</c> class for D9 Framework. Mainly handles the settings screen.
    /// </summary>
    public class D9FrameworkMod : Mod
    {
        D9FModSettings settings;
        public D9FrameworkMod(ModContentPack con) : base(con)
        {
            this.settings = GetSettings<D9FModSettings>();
            new HarmonyLoader();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listing = new Listing_Standard();
            listing.Begin(inRect);
            listing.CheckboxLabeled("D9FSettingsDebug".Translate(), ref D9FModSettings.DEBUG, "D9FSettingsDebugTooltip".Translate());
            if (D9FModSettings.DEBUG)
            {
                listing.CheckboxLabeled("D9FSettingsPPM".Translate(), ref D9FModSettings.printPatchedMethods, "D9FSettingsPPMTooltip".Translate());
                listing.Label("D9FSettingsApplyAtOwnRisk".Translate());
                listing.Label("D9FSettingsRestartToApply".Translate());
                listing.Label("D9FSettingsDebugModeRequired".Translate());
                List<D9FModSettings.PatchInfo> patches = D9FModSettings.SerializePatches();
                foreach(D9FModSettings.PatchInfo pi in patches)
                {
                    listing.CheckboxLabeled(pi.labelKey.Translate(), ref pi.apply, pi.descKey.Translate());
                }
                D9FModSettings.DeserializePatches(patches);
                listing.CheckboxLabeled("D9FSettingsApplyCMF".Translate(), ref D9FModSettings.applyCMF, "D9FSettingsApplyCMFTooltip".Translate());
            }
            listing.End();
            base.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return "D9FSettingsCategory".Translate();
        }
    }
}
