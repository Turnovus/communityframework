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
        private float EnergyMax => this.GetStatValue(StatDefOf.EnergyShieldEnergyMax, true);
        private float EnergyGainPerTick => this.GetStatValue(StatDefOf.EnergyShieldRechargeRate, true) / 60f;
        public float Energy => energy;

        public ShieldState ShieldState
        {
            get
            {
                if (ticksToReset > 0)
                {
                    return ShieldState.Resetting;
                }
                return ShieldState.Active;
            }
        }

        private bool ShouldDisplay
        {
            get
            {
                Pawn wearer = base.Wearer;
                return (wearer.Spawned && !wearer.Dead && !wearer.Downed) &&
                       wearer.InAggroMentalState ||
                       wearer.Drafted ||
                       (wearer.Faction.HostileTo(Faction.OfPlayer) && !wearer.IsPrisoner) ||
                       Find.TickManager.TicksGame < lastKeepDisplayTick + KeepDisplayingTicks;
            }
        }
        #endregion properties

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref energy, "energy", 0f, false);
            Scribe_Values.Look(ref ticksToReset, "ticksToReset", -1, false);
            Scribe_Values.Look(ref lastKeepDisplayTick, "lastKeepDisplayTick", 0, false);
        }

        public override IEnumerable<Gizmo> GetWornGizmos()
        {
            if (Find.Selector.SingleSelectedThing != base.Wearer)
            {
                yield break;
            }
            yield return (Gizmo)new Gizmo_RangedShieldStatus
            {
                shield = this
            };
            ;
        }

        public override float GetSpecialApparelScoreOffset()
        {
            return EnergyMax * ApparelScorePerEnergyMax;
        }

        public override void Tick()
        {
            base.Tick();
            if (base.Wearer == null)
            {
                energy = 0f;
            }
            else if (ShieldState == ShieldState.Resetting)
            {
                ticksToReset--;
                if (ticksToReset <= 0)
                {
                    Reset();
                }
            }
            else if (ShieldState == ShieldState.Active)
            {
                energy += EnergyGainPerTick;
                if (energy > EnergyMax)
                {
                    energy = EnergyMax;
                }
            }
        }

        public override bool CheckPreAbsorbDamage(DamageInfo dinfo)
        {
            if (ShieldState != 0)
            {
                return false;
            }
            if (dinfo.Def == DamageDefOf.EMP)
            {
                energy = 0f;
                Break();
                return false;
            }
            if (!dinfo.Def.isRanged && !dinfo.Def.isExplosive)
            {
                return false;
            }
            energy -= dinfo.Amount * EnergyLossPerDamage;
            if (energy < 0f)
            {
                Break();
            }
            else
            {
                AbsorbedDamage(dinfo);
            }
            return true;
        }

        public void KeepDisplaying()
        {
            lastKeepDisplayTick = Find.TickManager.TicksGame;
        }

        private void AbsorbedDamage(DamageInfo dinfo)
        {
            SoundDefOf.EnergyShield_AbsorbDamage.PlayOneShot(new TargetInfo(base.Wearer.Position, base.Wearer.Map, false));
            impactAngleVect = Vector3Utility.HorizontalVectorFromAngle(dinfo.Angle);
            Vector3 loc = base.Wearer.TrueCenter() + impactAngleVect.RotatedBy(180f) * 0.5f;
            float num = Mathf.Min(10f, 2f + dinfo.Amount / 10f);
            MoteMaker.MakeStaticMote(loc, base.Wearer.Map, ThingDefOf.Explosion, num);
            int num2 = (int)num;
            for (int i = 0; i < num2; i++)
            {
                FleckMaker.ThrowDustPuff(loc, base.Wearer.Map, Rand.Range(0.8f, 1.2f));
            }
            lastAbsorbDamageTick = Find.TickManager.TicksGame;
            KeepDisplaying();
        }

        private void Break()
        {
            SoundDefOf.EnergyShield_Broken.PlayOneShot(new TargetInfo(base.Wearer.Position, base.Wearer.Map, false));
            MoteMaker.MakeStaticMote(base.Wearer.TrueCenter(), base.Wearer.Map, ThingDefOf.Explosion, 12f);
            for (int i = 0; i < 6; i++)
            {
                Vector3 loc = base.Wearer.TrueCenter() + Vector3Utility.HorizontalVectorFromAngle((float)Rand.Range(0, 360)) * Rand.Range(0.3f, 0.6f);
                FleckMaker.ThrowDustPuff(loc, base.Wearer.Map, Rand.Range(0.8f, 1.2f));
            }
            energy = 0f;
            ticksToReset = StartingTicksToReset;
        }

        private void Reset()
        {
            if (base.Wearer.Spawned)
            {
                SoundDefOf.EnergyShield_Reset.PlayOneShot(new TargetInfo(base.Wearer.Position, base.Wearer.Map, false));
                FleckMaker.ThrowLightningGlow(base.Wearer.TrueCenter(), base.Wearer.Map, 3f);
            }
            ticksToReset = -1;
            energy = EnergyOnReset;
        }

        public override void DrawWornExtras()
        {
            if (ShieldState == ShieldState.Active && ShouldDisplay)
            {
                float num = Mathf.Lerp(1.2f, 1.55f, energy);
                Vector3 vector = base.Wearer.Drawer.DrawPos;
                vector.y = AltitudeLayer.MoteOverhead.AltitudeFor();
                int num2 = Find.TickManager.TicksGame - lastAbsorbDamageTick;
                if (num2 < 8)
                {
                    float num3 = (float)(8 - num2) / 8f * 0.05f;
                    vector += impactAngleVect * num3;
                    num -= num3;
                }
                float angle = (float)Rand.Range(0, 360);
                Vector3 s = new Vector3(num, 1f, num);
                Matrix4x4 matrix = default(Matrix4x4);
                matrix.SetTRS(vector, Quaternion.AngleAxis(angle, Vector3.up), s);
                Graphics.DrawMesh(MeshPool.plane10, matrix, BubbleMat, 0);
            }
        }

        public override bool AllowVerbCast(Verb v)
        {
            return true;
        }
    }
}