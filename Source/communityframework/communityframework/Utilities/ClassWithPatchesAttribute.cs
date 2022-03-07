using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF
{
    /// <summary>
    /// Used to make adding new Harmony patches internally easier. Not intended
    /// for other mods' use.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    internal class ClassWithPatchesAttribute : Attribute
    {
        public string NameKey, SaveKey, DescKey;
        public ClassWithPatchesAttribute(string saveKey)
        {
            SaveKey = saveKey;
            NameKey = $"{CFSettings.KeyPrefix}{saveKey}_Name";
            DescKey = $"{CFSettings.KeyPrefix}{saveKey}_Desc";
        }
    }
}
