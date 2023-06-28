using Verse;
using RimWorld;

namespace CF
{
    public abstract class DefModExtension_BatteryAmountCanAccept : DefModExtension
    {
        public abstract float AmountCanAccept(CompPowerBattery comp, float original);
    }

    public class NeverRecharge : DefModExtension_BatteryAmountCanAccept
    {
        public override float AmountCanAccept(CompPowerBattery comp, float original) => 0f;
    }

    public class BatteryExtension : DefModExtension, IExtensionPostMake
    {
        /// <summary>
        /// If <c>true</c>, batteries with this extension will not lose 5W of energy per day, as is
        /// hardcoded in vanilla.
        /// </summary>
        public bool neverDischarge = false;
        /// <summary>
        /// If <c>true</c>, then the battery will start with a full charge when it is crafted,
        /// built, or otherwise initially created.
        /// </summary>
        public bool startCharged = false;
        /// <summary>
        /// If <c>true</c>, batteries with this extension will not lose power during the short
        /// circuit event, and will not contribute to the size of the short circuit explosion.
        /// </summary>
        public bool neverShortCircuit = false;

        /// <inheritdoc/>
        public void PostMake(Thing thing) { }

        /// <summary>
        /// After a <see cref="Thing"/> with the parent <see cref="ThingDef"/> is initially
        /// created, this method will fully charge it if is a battery and if
        /// <see cref="startCharged"/> is <c>true</c>
        /// </summary>
        /// <param name="thing"></param>
        public void PostPostMake(Thing thing)
        {
            if (!startCharged)
                return;

            CompPowerBattery battery = thing.TryGetComp<CompPowerBattery>();
            if (battery == null)
                return;

            battery.SetStoredEnergyPct(1f);
        }
    }

    public class PowerExtension : DefModExtension
    {
        public bool shortCircuitSource = false;
    }
}
