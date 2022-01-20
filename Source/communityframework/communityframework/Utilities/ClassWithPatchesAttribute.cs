using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF
{
    /// <summary>
    /// Used to make adding new Harmony patches internally easier. Not intended for other mods' use.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    internal class ClassWithPatchesAttribute : Attribute
    {
        public string PlainName, SaveKey, Description;

        public ClassWithPatchesAttribute(string plainName, string saveKey, string description)
        {
            PlainName = plainName;
            SaveKey = saveKey;
            Description = description;
        }
    }
}
