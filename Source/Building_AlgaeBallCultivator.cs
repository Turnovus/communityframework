using System.Collections.Generic;
using RimWorld;
using Verse;

namespace MonolynAlgaeBall
{
    // Algae-ball cultivator box (海藻球培育箱).
    // Based on the vanilla hydroponics basin (Building_PlantGrower) so it keeps
    // auto-harvest / re-sow behaviour, but with a kibble fuel system that boosts
    // fertility from 100% to 500%, and a 24-hour power-cut grace period instead
    // of the vanilla instant plant death.
    public class Building_AlgaeBallCultivator : Building_PlantGrower
    {
        public const float BaseFertility = 1f;    // 100% (default, unfueled)
        public const float FueledFertility = 5f;  // 500% (fueled with kibble)

        private const int PowerFailKillTicks = 60000; // 60000 ticks = 24 hours

        private CompRefuelable refuelable;
        private CompPowerTrader powerTrader;
        private int powerFailTicks;

        public CompRefuelable Refuelable
        {
            get { return refuelable ?? (refuelable = GetComp<CompRefuelable>()); }
        }

        public CompPowerTrader PowerTrader
        {
            get { return powerTrader ?? (powerTrader = GetComp<CompPowerTrader>()); }
        }

        public bool Fueled
        {
            get { return Refuelable != null && Refuelable.HasFuel; }
        }

        public bool PowerOn
        {
            get { return PowerTrader == null || PowerTrader.PowerOn; }
        }

        // 0 = no power (growth stops), 1 = unfueled, 5 = fueled.
        public float Fertility
        {
            get
            {
                if (!PowerOn)
                    return 0f;
                return Fueled ? FueledFertility : BaseFertility;
            }
        }

        // Power cut: plants stop growing (fertility 0 via the Harmony patch) and
        // die only after 24 hours without power.
        // tickerType is Normal (the refuelable comp needs per-tick ticking), so the
        // timer counts every tick here instead of TickRare.
        protected override void Tick()
        {
            base.Tick();
            if (PowerTrader != null && !PowerTrader.PowerOn)
            {
                powerFailTicks++;
                if (powerFailTicks >= PowerFailKillTicks)
                {
                    powerFailTicks = 0;
                    KillAllPlants();
                }
            }
            else
            {
                powerFailTicks = 0;
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref powerFailTicks, "powerFailTicks", 0);
        }

        private void KillAllPlants()
        {
            List<Thing> things = Map.thingGrid.ThingsListAt(Position);
            for (int i = 0; i < things.Count; i++)
            {
                Plant plant = things[i] as Plant;
                if (plant != null)
                {
                    plant.Kill(null, null);
                }
            }
        }
    }
}
