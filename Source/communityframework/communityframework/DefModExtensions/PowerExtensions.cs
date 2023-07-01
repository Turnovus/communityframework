using Verse;
using RimWorld;

namespace CF
{
    /// <summary>
    /// A base class for <see cref="DefModExtension"/>s, used to define custom behaviors that
    /// affect the maximum amount of energy that a sibling <see cref="CompPowerBattery"/> can
    /// store.
    /// </summary>
    public abstract class DefModExtension_BatteryAmountCanAccept : DefModExtension
    {
        /// <summary>
        /// Augments the amount of energy that can be stored by a <see cref="CompPowerBattery"/>
        /// attached to any <see cref="Thing"/> with the parent <see cref="ThingDef"/>.
        /// </summary>
        /// <param name="comp">
        /// The <see cref="CompPowerBattery"/> whose that
        /// <see cref="CompPowerBattery.AmountCanAccept"/> is being adjusted for.
        /// </param>
        /// <param name="original">
        /// The vanilla value of <see cref="CompPowerBattery.AmountCanAccept"/> from <c>comp</c>.
        /// </param>
        /// <returns>The new amount of additional energy that the battery can accept.</returns>
        public abstract float AmountCanAccept(CompPowerBattery comp, float original);
    }

    /// <summary>
    /// Any <see cref="CompPowerBattery"/> will never be able to recharge if its parent has this
    /// extension in its <see cref="ThingDef"/>.
    /// </summary>
    public class NeverRecharge : DefModExtension_BatteryAmountCanAccept
    {
        /// <inheritdoc/>
        public override float AmountCanAccept(CompPowerBattery comp, float original) => 0f;
    }

    /// <summary>
    /// A <see cref="DefModExtension"/> that provides more customization options for any
    /// <see cref="CompPowerBattery"/> attached to <see cref="Thing"/>s that use the extension's
    /// parent <see cref="ThingDef"/>.
    /// </summary>
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

    /// <summary>
    /// A <see cref="DefModExtension"/> that provides more customization options for
    /// <see cref="ThingDef"/>s that define electrical objects.
    /// </summary>
    public class PowerExtension : DefModExtension
    {
        /// <summary>
        /// If <c>true</c>, this <see cref="Thing"/> can be a culprit of short circuits, like
        /// vanilla power conduits.
        /// </summary>
        public bool shortCircuitSource = false;
    }
}
