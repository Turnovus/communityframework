using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;
using System.Xml;

namespace CF
{
    /// <summary>
    /// <c>PatchOperation</c> which applies <c>match</c> if Royalty is owned by the player, or <c>nomatch</c> otherwise.
    /// </summary>
    /// <remarks>
    /// The naming discrepancy is inherited from the base game, and appears to be intended by Tynan.
    /// </remarks>
    class PatchOperationRoyaltyInstalled : PatchOperation
    {
# pragma warning disable CS0649
        private PatchOperation match, nomatch;
#pragma warning restore CS0649

        protected override bool ApplyWorker(XmlDocument xml)
        {
            if (ModLister.RoyaltyInstalled)
            {
                if (match != null) return match.Apply(xml);
            }                
            else
            {
                if (nomatch != null) return nomatch.Apply(xml);
            }                
            return true;
        }
    }
}
