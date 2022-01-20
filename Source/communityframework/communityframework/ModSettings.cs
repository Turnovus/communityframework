using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace CF
{
    /// <summary>
    /// <c>ModSettings</c> class for Community Framework. Mainly handles which Harmony patches should be applied and saves all specified settings.
    /// </summary>
    /// <remarks>Static for convenience.</remarks>
    public class CommunityFrameworkModSettings : ModSettings
    {        
        public static bool DEBUG = false; //for release set false by default
        public static bool PrintPatchedMethods => DEBUG && printPatchedMethods;
        public static bool printPatchedMethods = false;

        // despite being public, please don't fuck with these. Access patch application settings with ShouldPatch.
        // They're only public so I can use them in the mod settings screen.
        public static Dictionary<string, PatchSave> Patches = new Dictionary<string, PatchSave>();

        public class PatchSave : IExposable
        {
            public bool apply;
            public string saveKey, plainName, description;

            // The game requires a no-arg constructor
            public PatchSave() { }

            public PatchSave(string s, bool a)
            {
                saveKey = s;
                apply = a;
            }

            public override string ToString()
            {
                return "<{" + saveKey + ": " + apply + "}>";
            }

            public void ExposeData()
            {
                Scribe_Values.Look(ref saveKey, "saveKey");
                Scribe_Values.Look(ref apply, "apply");
            }
        } 

        public override void ExposeData()
        {
            base.ExposeData();
            List<PatchSave> savePatches = new List<PatchSave>();
            if (Scribe.mode == LoadSaveMode.Saving)
            {
                savePatches = SerializePatches();     
            }
            Scribe_Collections.Look(ref savePatches, "Patches", LookMode.Deep);
            if(Scribe.mode != LoadSaveMode.Saving)
            {
                DeserializePatches(savePatches);
            }
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

        public static List<PatchSave> SerializePatches()
        {
            List<PatchSave> ret = new List<PatchSave>();
            foreach (string key in Patches.Keys.ToList())
            {
                ret.Add(new PatchSave(key, Patches[key].apply));
            }
            return ret;
        }

        public static void DeserializePatches(List<PatchSave> list)
        {
            foreach (PatchSave pi in list)
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
        CommunityFrameworkModSettings settings;
        public D9FrameworkMod(ModContentPack con) : base(con)
        {
            this.settings = GetSettings<CommunityFrameworkModSettings>();
            new HarmonyLoader();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listing = new Listing_Standard();
            listing.Begin(inRect);
            listing.CheckboxLabeled("CFSettings_Debug".Translate(), ref CommunityFrameworkModSettings.DEBUG, "CFSettings_DebugTooltip".Translate());
            if (CommunityFrameworkModSettings.DEBUG)
            {
                listing.CheckboxLabeled("CFSettings_PPM".Translate(), ref CommunityFrameworkModSettings.printPatchedMethods, "CFSettings_PPMTooltip".Translate());
                listing.Label("CFSettings_ApplyAtOwnRisk".Translate());
                listing.Label("CFSettings_RestartToApply".Translate());
                listing.Label("CFSettings_DebugModeRequired".Translate());
                List<CommunityFrameworkModSettings.PatchSave> patches = CommunityFrameworkModSettings.SerializePatches();
                foreach(CommunityFrameworkModSettings.PatchSave pi in patches)
                {
                    listing.CheckboxLabeled("CFSettingsApplyPatch".Translate(pi.plainName),
                                            ref pi.apply,
                                            "CFSettingsApplyPatchTooltip".Translate(pi.plainName, pi.description));
                }
                CommunityFrameworkModSettings.DeserializePatches(patches);
            }
            listing.End();
            base.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return "CFSettings_Category".Translate();
        }
    }
}
