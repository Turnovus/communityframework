using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace CF
{
    /// <summary>
    /// <c>ModSettings</c> class for Community Framework. Mainly handles which
    /// Harmony patches should be applied and saves all specified settings.
    /// </summary>
    /// <remarks>Static for convenience.</remarks>
    public class CFSettings : ModSettings
    {
        /// <summary>
        /// This is manually prefixed onto every mod settings string key
        /// that is defined in the framework's language XML, to avoid the
        /// possibility of a translation key collision.
        /// </summary>
        public static readonly string KeyPrefix = "CFSettings_";
        /// <summary>
        /// This is manually affixed onto every string key for a mod setting's
        /// title, in the framework's language XML.
        /// </summary>
        public static readonly string NamePostfix = "_Name";
        /// <summary>
        /// This is manually affixed onto every string key for a mod setting's
        /// description, in the framework's language XML.
        /// </summary>
        public static readonly string DescPostfix = "_Desc";
        /// <summary>
        /// If <c>true</c>, then the framework is running in debug mode, and
        /// debug logging and features are currently active.
        /// </summary>
        public static bool DEBUG = false; //for release set false by default
        /// <summary>
        /// If the framework should log which methods it has patched on
        /// startup.
        /// </summary>
        public static bool PrintPatchedMethods => DEBUG && printPatchedMethods;
        /// <summary>
        /// If the framework should log which methods it has patched on
        /// startup.
        /// </summary>
        public static bool printPatchedMethods = false;

        /// <summary>
        /// A list of Harmony patches to apply on startup.
        /// </summary>
        /// <remarks>
        /// despite being public, please don't access these. Access patch
        /// application settings with ShouldPatch.
        /// They're only public so they can be used in the mod settings screen.
        /// </remarks>
        public static Dictionary<string, PatchSave> Patches =
            new Dictionary<string, PatchSave>();

        /// <summary>
        /// Represents a single one of the framework's Harmony patches, and
        /// whether or not it should be applied.
        /// </summary>
        public class PatchSave : IExposable
        {
            /// <summary>
            /// If the patch should be applied.
            /// </summary>
            public bool apply;
            /// <summary>
            /// Identifier used to associate this patch save with the specific
            /// Harmony patch that it represents.
            /// </summary>
            public string saveKey;

            /// <summary>
            /// A no-arg constructor, required by the vanilla API. Does
            /// nothing.
            /// </summary>
            public PatchSave() { }

            /// <summary>
            /// Constructor with parameters for initializing fields.
            /// </summary>
            /// <param name="s">
            /// The save key, used to associate this patch save with the
            /// specific Harmony patch that it represents.
            /// </param>
            /// <param name="a">
            /// If the patch should be applied.
            /// </param>
            public PatchSave(string s, bool a)
            {
                saveKey = s;
                apply = a;
            }

            /// <summary>
            /// Returns a string containing the <c>saveKey</c> of the patch,
            /// and whether or not the patch should be applied.
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return "<{" + saveKey + ": " + apply + "}>";
            }

            /// <summary>
            /// Saves and/or loads the fields of this <c>PatchSave</c>, so that
            /// patch settings are persistent between game sessions.
            /// </summary>
            public void ExposeData()
            {
                Scribe_Values.Look(ref saveKey, "saveKey");
                Scribe_Values.Look(ref apply, "apply");
            }
        } 

        /// <summary>
        /// Saves and/or loads the fields of each <see cref="PatchSave"/>
        /// stored, so that patch settings are persistent between game
        /// sessions.
        /// </summary>
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

        /// <summary>
        /// Determines whether or not the given patch should be run for this
        /// session.
        /// </summary>
        /// <param name="patchkey">
        /// The identifier of the patch being checked.
        /// </param>
        /// <returns>
        /// <c>true</c> if the patch should be applied, <c>false</c> if the
        /// player has chosen to disable it.
        /// </returns>
        public static bool ShouldPatch(string patchkey)
        {
            if (!DEBUG) return true;
            if (!Patches.ContainsKey(patchkey))
            {
                ULog.Warning(
                    "ShouldPatch called for non-initialized patchkey.");
                return true;
            }
            return Patches[patchkey].apply;
        }

        /// <summary></summary>
        /// <returns></returns>
        public static List<PatchSave> SerializePatches()
        {
            List<PatchSave> ret = new List<PatchSave>();
            foreach (string key in Patches.Keys.ToList())
            {
                if (HarmonyLoader.SaveKeyExists(key))
                    ret.Add(new PatchSave(key, Patches[key].apply));
            }
            return ret;
        }

        /// <summary></summary>
        /// <param name="list"></param>
        public static void DeserializePatches(List<PatchSave> list)
        {
            foreach (PatchSave pi in list)
            {
                if (HarmonyLoader.SaveKeyExists(pi.saveKey))
                    Patches[pi.saveKey] = pi;
            }
        }
    }
    /// <summary>
    /// <c>Mod</c> class for Community Framework. Mainly handles the settings
    /// screen.
    /// </summary>
    public class CFMod : Mod
    {
        CFSettings settings;
        /// <summary>
        /// Constructor for the Framework's mod data. Initializes the
        /// framework's mod settings, and loads all of the patch data.
        /// </summary>
        /// <param name="con">Unused in this override.</param>
        public CFMod(ModContentPack con) : base(con)
        {
            settings = GetSettings<CFSettings>();
            HarmonyLoader.DoAllPatches();
        }

        /// <summary>
        /// Draws the contents of the framework's mod settings window.
        /// </summary>
        /// <param name="inRect">
        /// The <see cref="Rect"/> to draw the settings window onto.
        /// </param>
        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listing = new Listing_Standard();
            listing.Begin(inRect);
            listing.CheckboxLabeled(
                $"{CFSettings.KeyPrefix}Debug".Translate(), 
                ref CFSettings.DEBUG,
                $"{CFSettings.KeyPrefix}DebugTooltip".Translate()
            );
            if (CFSettings.DEBUG)
            {
                listing.CheckboxLabeled(
                    $"{CFSettings.KeyPrefix}PPM".Translate(),
                    ref CFSettings.printPatchedMethods, 
                    $"{CFSettings.KeyPrefix}PPMTooltip".Translate());
                listing.Label(
                    $"{CFSettings.KeyPrefix}ApplyAtOwnRisk".Translate());
                listing.Label(
                    $"{CFSettings.KeyPrefix}RestartToApply".Translate());
                listing.Label(
                    $"{CFSettings.KeyPrefix}DebugModeRequired".Translate());

                List<CFSettings.PatchSave> patches =
                    CFSettings.SerializePatches();
                foreach(CFSettings.PatchSave pi in patches)
                {
                    listing.CheckboxLabeled(
                        $"{CFSettings.KeyPrefix}ApplyPatch".Translate(
                            NameKeyOf(pi.saveKey).Translate()),
                        ref pi.apply,
                        $"{CFSettings.KeyPrefix}ApplyPatchTooltip".Translate(
                            NameKeyOf(pi.saveKey).Translate(),
                            DescKeyOf(pi.saveKey).Translate()
                        )
                    );
                }
                CFSettings.DeserializePatches(patches);
            }
            listing.End();
            base.DoSettingsWindowContents(inRect);
        }

        /// <summary>
        /// The name of the framework's listing in the game's list of available
        /// mod settings menus.
        /// </summary>
        /// <returns>
        /// <inheritdoc cref="CFMod.SettingsCategory" path="/summary"/>
        /// </returns>
        public override string SettingsCategory()
        {
            return "CFSettings_Category".Translate();
        }

        private static string NameKeyOf(string patchKey) =>
            CFSettings.KeyPrefix + patchKey + CFSettings.NamePostfix;

        private static string DescKeyOf(string patchKey) =>
            CFSettings.KeyPrefix + patchKey + CFSettings.DescPostfix;
    }
}
