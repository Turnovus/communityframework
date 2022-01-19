using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D9Framework
{
    /// <summary>
    /// Used to make adding new Harmony patches internally easier. Not intended for other mods' use.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    internal class ClassWithPatchesAttribute : Attribute
    {
        public string PlainName, SaveKey, LabelKey, DescKey;

        public ClassWithPatchesAttribute(string plainName, string saveKey)
        {
            PlainName = plainName;
            SaveKey = saveKey;
            LabelKey = "D9FSettingsApply" + saveKey;
            DescKey = LabelKey + "Tooltip";
        }

        public ClassWithPatchesAttribute(string plainName, string saveKey, string labelKey)
        {
            PlainName = plainName;
            SaveKey = saveKey;
            LabelKey = labelKey;
            DescKey = labelKey + "Tooltip";
        }

        public ClassWithPatchesAttribute(string plainName, string saveKey, string labelKey, string descKey)
        {
            PlainName = plainName;
            SaveKey = saveKey;
            LabelKey = labelKey;
            DescKey = descKey;
        }
    }
}
