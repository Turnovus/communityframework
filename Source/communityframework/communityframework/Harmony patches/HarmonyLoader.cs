using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Verse;
using HarmonyLib;

namespace CF
{
    /// <summary>
    /// Loads included Harmony patches only if they're enabled in the mod settings.
    /// </summary>
    class HarmonyLoader
    {
        static HarmonyLoader()
        {
            ULog.Message("Applying Harmony patches...");
            var harmony = new Harmony("com.dninemfive.D9Framework");
            // https://stackoverflow.com/questions/2639418/use-reflection-to-get-a-list-of-static-classes
            foreach(Type type in typeof(HarmonyLoader).Assembly.GetTypes().Where(t => t.IsClass && t.IsSealed && t.IsAbstract))
            {
                ClassWithPatchesAttribute attr;
                if ((attr = type.TryGetAttribute<ClassWithPatchesAttribute>()) != null)
                {
                    if (!CommunityFrameworkModSettings.Patches.ContainsKey(attr.SaveKey))
                    {
                        CommunityFrameworkModSettings.Patches[attr.SaveKey] = new CommunityFrameworkModSettings.PatchInfo(attr.SaveKey, true, attr.LabelKey, attr.DescKey);                        
                    }
                    else
                    {
                        CommunityFrameworkModSettings.Patches[attr.SaveKey].apply = CommunityFrameworkModSettings.ShouldPatch(attr.SaveKey);
                    }
                    if (CommunityFrameworkModSettings.ShouldPatch(attr.SaveKey))
                    {
                        PatchAll(harmony, type);
                        ULog.DebugMessage("\t" + attr.PlainName + " enabled.", false);
                    }                    
                }
            }
            if (CommunityFrameworkModSettings.PrintPatchedMethods)
            {
                ULog.Message("The following methods were successfully patched:");
                foreach (MethodBase mb in harmony.GetPatchedMethods()) Log.Message("\t" + mb.DeclaringType.Name + "." + mb.Name);
            }
        }
        // thanks to lbmaian
        public static void PatchAll(Harmony harmony, Type parentType)
        {
            foreach (var type in parentType.GetNestedTypes(AccessTools.all))
            {
                new PatchClassProcessor(harmony, type).Patch();
            }
        }
    }
}
