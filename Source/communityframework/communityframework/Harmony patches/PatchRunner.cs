using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;
using HarmonyLib;

namespace CF
{
    /// <summary>
    /// This class patches all the Harmony classes
    /// </summary>
    [StaticConstructorOnStartup]
    public class PatchRunner
    {
        static PatchRunner()
        {
            DoPatching();
        }

        public static void DoPatching()
        {
            //Start patching all harmony patches.
            var harmony = new Harmony("com.communityframework.harmonypatches");
            harmony.PatchAll();
        }

    }
}
