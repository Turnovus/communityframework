using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public bool neverDischarge = false;
        public bool startCharged = false;
        public bool neverShortCircuit = false;

        public void PostMake(Thing thing) { }

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
