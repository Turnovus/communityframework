using Verse;

namespace CF
{
    /// <summary>
    /// Represents a side-effect that can be applied upon regeneration,
    /// including the <c>Hediff</c> to apply, the frequency at which it occurs,
    /// how its severity should be calculated, and whether it should be applied
    /// to the target part or whole body.
    /// </summary>
    public class RegenSideEffect
    {
        /// <summary>
        /// The <c>Hediff</c> that this side-effect will apply.
        /// </summary>
        public HediffDef hediffDef = null;
        /// <summary>
        /// The chance as a decimal percent that this side-effect will be
        /// applied upon successful regeneration.
        /// </summary>
        public float percentChance = 1.0f;
        /// <summary>
        /// Range for the <c>Hediff</c>'s random starting severity.
        /// </summary>
        public FloatRange severity = new FloatRange(0f, 1f);
        /// /// <summary>
        /// If <c>true</c>, the <c>Hediff</c>'s severity will be multiplied by
        /// the cured <c>Hediff</c>'s severity. If it is an injury, it will be
        /// multiplied by the <c>Hediff</c>'s severity as a percent of the base
        /// health.
        /// </summary>
        public bool useInjurySeverityMult = false;
        /// <summary>
        /// If <c>true</c>, the <c>Hediff</c> will be applied to the whole 
        /// body, instead of the cured part.
        /// </summary>
        public bool isGlobalHediff = false;
    }
}
