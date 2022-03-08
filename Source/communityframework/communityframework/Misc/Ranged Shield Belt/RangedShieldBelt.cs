using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using UnityEngine;
using Verse.Sound;

namespace CF
{
    /// <summary>
    /// <c>ThingClass</c> which acts like the vanilla <c>ShieldBelt</c> class but allows the user to fire ranged weapons while worn.
    /// </summary>
    /// <remarks>
    /// Used to be a pretty simple subclass of <c>ShieldBelt</c>, but that ran into compatibility problems:
    /// <list type="bullet">
    /// <item>Some mods check whether ranged shots were allowed by seeing if the thingClass <c>is</c> a ShieldBelt, which returned true in this case, 
    /// causing errors including them being automatically, erroneously, unequipped on ranged pawns.</item>
    /// <item>A growing number of patches were necessary to prevent the vanilla game treating ranged shield belts in the same way.</item>
    /// </list>
    /// </remarks>
    [StaticConstructorOnStartup]
    public class RangedShieldBelt : Apparel
    {
        #region fields
        private float energy;
        private int ticksToReset = -1;
        private int lastKeepDisplayTick = -9999;
        private Vector3 impactAngleVect;
        private int lastAbsorbDamageTick = -9999;
        private const float MinDrawSize = 1.2f;
        private const float MaxDrawSize = 1.55f;
        private const float MaxDamagedJitterDist = 0.05f;
        private const int JitterDurationTicks = 8;
        private int StartingTicksToReset = 3200;
        private float EnergyOnReset = 0.2f;
        private float EnergyLossPerDamage = 0.033f;
        private int KeepDisplayingTicks = 1000;
        private float ApparelScorePerEnergyMax = 0.25f;
        private static readonly Material BubbleMat = MaterialPool.MatFrom("Other/ShieldBubble", ShaderDatabase.Transparent);
        #endregion fields
        #region properties
        private float EnergyMax => this.GetStatValue(StatDefOf.EnergyShieldEnergyMax);
        private float EnergyGainPerTick => this.GetStatValue(StatDefOf.EnergyShieldRechargeRate) / 60f;
        public float Energy => energy;
        public ShieldState ShieldState => ticksToReset > 0 ? ShieldState.Resetting : ShieldState.Active;
        private bool ShouldDisplay
        {
            get
            {
                Pawn wearer = base.Wearer;
                if (!wearer.Spawned || wearer.Dead || wearer.Downed) return false;
                if (wearer.InAggroMentalState || wearer.Drafted || wearer.Faction.HostileTo(Faction.OfPlayer) && !wearer.IsPrisoner) return true;
                if (Find.TickManager.TicksGame < lastKeepDisplayTick + KeepDisplayingTicks) return true;
                return false;
            }
        }
        #endregion properties
        /// <summary>
        /// The only meaningful change here, allowing ranged verb casts (or, more precisely, not disallowing them as the original <c>ShieldBelt</c> does).
        /// </summary>
        /// <param name="v">Disregarded.</param>
        /// <returns>True.</returns>
        public override bool AllowVerbCast(Verb v) => true;

        // All of the following are marked as throwing NotImplementedExceptions because they should:tm: be overridden by loading the methods by reflection in ReflectionEntrypoint.
        public override void ExposeData()
        {
            throw new NotImplementedException();
        }
        public override IEnumerable<Gizmo> GetWornGizmos()
        {
            throw new NotImplementedException();
        }
        public override float GetSpecialApparelScoreOffset()
        {
            throw new NotImplementedException();
        }
        public override void Tick()
        {
            throw new NotImplementedException();
        }
        public override bool CheckPreAbsorbDamage(DamageInfo dinfo)
        {
            throw new NotImplementedException();
        }
        public void KeepDisplaying()
        {
            throw new NotImplementedException();
        }
        private void AbsorbedDamage(DamageInfo dinfo)
        {
            throw new NotImplementedException();
        }
        private void Break()
        {
            throw new NotImplementedException();
        }
        private void Reset()
        {
            throw new NotImplementedException();
        }
        public override void DrawWornExtras()
        {
            throw new NotImplementedException();
        }        
    }
}