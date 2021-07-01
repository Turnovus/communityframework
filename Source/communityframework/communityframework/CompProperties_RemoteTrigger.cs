using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace CF
{
    class CompProperties_RemoteTrigger : CompProperties
    {
        public CompProperties_RemoteTrigger()
        {
            this.compClass = typeof(CompRemoteTrigger);
        }

        public string texPath;

    }
}
