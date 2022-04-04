using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.Sound;
using RimWorld;

namespace CF
{
    /// <summary>
    /// Static helper class, contains helpful methods to apply to buildings.
    /// </summary>
    public static class CommunityBuildingUtility
    {
        /// <summary>
        /// The vanilla <c>SoundDef</c> played when a building is
        /// minified/uninstalled. Stored here as a public member for
        /// consistency.
        /// </summary>
        public static readonly SoundDef minifySound =
            SoundDef.Named("ThingUninstalled");

        /// <summary>
        /// Minifies a <c>Thing</c> on the spot if its <c>ThingDef</c> says
        /// that it can be minified, and destroys it if not.
        /// </summary>
        /// <param name="thing">The <c>Thing</c> to minify/destroy.</param>
        /// <param name="destroyMode">
        /// The destroy mode to pass to <c>thing.Destroy</c>. It's not
        /// recommended to set this unless you have a good reason to do so.
        /// </param>
        public static void MinifyOrDestroy(
            this Thing thing,
            DestroyMode destroyMode = DestroyMode.KillFinalize
        )
        {
            if (thing.def.Minifiable)
            {
                Map map = thing.Map;
                MinifiedThing package = MinifyUtility.MakeMinified(thing);
                GenPlace.TryPlaceThing(
                    package, thing.Position, map, ThingPlaceMode.Near);
                minifySound.PlayOneShot(new TargetInfo(thing.Position, map));
            }
            else
            {
                thing.Destroy(destroyMode);
            }
        }
    }
}
