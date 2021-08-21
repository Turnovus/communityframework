using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace CF
{
    class HediffComp_IgnorePathCost : HediffComp
    {
		public override string CompTipStringExtra
		{
			get
			{
				//English: "Terrain path cost is ignored"
				return "CF_IgnorePathCost_StringExtra".Translate();
			}
		}
	}
}
