using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace CF
{
    /// <summary>
    /// Utility Log. Convenient logging methods which automatically prefix themselves for identifiability, plus debug-only messages.
    /// </summary>
    class ULog
    {
        public static bool DEBUG => CommunityFrameworkModSettings.DEBUG;
        public static string modid = "Community Framework";
        public static string Prefix => "[" + modid + "] ";

        public static void Message(string s)
        {
            Log.Message(Prefix + s);
        }

        public static void Warning(string s)
        {
            Log.Warning(Prefix + s);
        }

        public static void Error(string s)
        {
            Log.Error(Prefix + s);
        }

        public static void DebugMessage(String s, bool addPrefix = true)
        {
            if (DEBUG) Log.Message((addPrefix ? Prefix : "") + s);
        }
    }
}
