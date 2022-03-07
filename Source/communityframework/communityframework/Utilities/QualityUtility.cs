using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Verse;
using RimWorld;

namespace CF
{
    [StaticConstructorOnStartup]
    public static class QualityUtility
    {
        public static FieldInfo QualityInt;

        static QualityUtility()
        {
            QualityInt = typeof(CompQuality).GetField(
                "qualityInt", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        public static void SetQualitySilent(
            this CompQuality comp,
            QualityCategory quality)
        {
            QualityInt.SetValue(comp, quality);
        }
    }
}
