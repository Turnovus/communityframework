using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace CF
{
    // These extensions are just for holding data, and none of these fields are
    // going to be assigned to in this namespace.
#pragma warning disable CS0649

    /// <summary>
    /// <c>DefModExtension</c> for use with
    /// <see cref="CF.ShouldHaveNeedPatch"/>. Needs listed here will be ignored
    /// by pawns with this <c>DefModExtension</c>.
    /// </summary>
    public class IgnoreNeed : DefModExtension
    {
        /// <summary>
        /// A list of <see cref="NeedDef"/>s to be ignored by the pawn.
        /// </summary>
        public List<NeedDef> needs;
    }

    /// <summary>
    /// <c>DefModExtension</c> which flags the parent plant <c>ThingDef</c> as
    /// using negative fertility. Specifies minimum and maximum fertility
    /// values within which the final fertility is clamped.
    /// </summary>
    /// <remarks>
    /// See <see cref="CF.NegativeFertilityPatch"/> for implementation details.
    /// </remarks>
    public class UseNegativeFertility : DefModExtension
    {
        /// <summary>
        /// The lowest possible perceived fertility value. This is how quickly
        /// the plant will grow when planted on the most fertile natural soil
        /// available.
        /// </summary>
        public float minFertility = 0.05f;
        /// <summary>
        /// The highest possible perceived fertility value. This is how quikcly
        /// the plant will grow when planted on the least fertile natural soil
        /// possible.
        /// </summary>
        public float maxFertility = 1.4f;
    }

    /// <summary>
    /// <c>DefModExtension</c> for use with
    /// <see cref="CF.CompFromStuffPatch"/>. Specifies the <c>ThingComps</c>,
    /// by their <c>CompProperties</c>, which should be added to newly-
    /// generated items made from the specified Stuff.
    /// </summary>
    public class CompsToAddWhenStuff : DefModExtension
    {
        /// <summary>
        /// A list of <see cref="ThingComp"/>s that will be attached to
        /// anything that uses the comp's parent as stuff.
        /// </summary>
        public List<CompProperties> comps;

    }

    /// <summary>
    /// An extension meant for use alongside
    /// <see cref="RimWorld.CompFacility"/>, it is meant to be used on
    /// buildings that link to other buildings.
    /// </summary>
    public class BuildingFacilityExtension : DefModExtension
    {
        /// <summary>
        /// If <c>true</c>, then the facility's link will not be active unless
        /// its <see cref="CompRefuelable"/> has fuel.
        /// </summary>
        public bool facilityRequiresFuel = false;
    }

    /// <summary>
    /// An extension used by <see cref="Verse.RecipeDef"/>. It contains a list
    /// of <see cref="OutputWorker"/>s to run when the parent recipe is
    /// complete.
    /// </summary>
    class UseOutputWorkers : DefModExtension
    {
        /// <summary>
        /// A collection of output workers, whose mthods will be run to modify
        /// the outputs of a crafting recipe.
        /// </summary>
        public List<OutputWorker> outputWorkers;
    }

    /// <summary>
    /// A <see cref="DefModExtension"/> that allows modders to customize the
    /// behaviors of any <see cref="CompProperties_Hatcher"/>s attached to the
    /// same <see cref="ThingDef"/>.
    /// </summary>
    public class HatcherExtension : DefModExtension
    {
        /// <summary>
        /// If <c>true</c>, then whatever <see cref="Pawn"/> hatches out of the
        /// hatcher will automatically be assigned to the player's faction.
        /// </summary>
        public bool hatcheeForcePlayerFaction = false;
    }
#pragma warning restore CS0649
}
