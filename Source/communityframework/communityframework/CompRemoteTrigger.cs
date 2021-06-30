using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace CF
{
    class CompRemoteTrigger : ThingComp
    {
        public CompProperties_RemoteTrigger Props
        {
            get
            {
                return (CompProperties_RemoteTrigger)this.props;
            }
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            Command_Action comp = GizmoManager.remoteTriggerGizmos[base.parent.def];
            comp.action = delegate
            {
                Thing toDetonate = base.parent;
                CompProjectileSprayer proj = toDetonate.TryGetComp<CompProjectileSprayer>();
                if (!(proj?.fired ?? true)) proj?.Fire();
                CompExplosive expl = toDetonate.TryGetComp<CompExplosive>();
                if (!(expl?.wickStarted ?? true)) expl.StartWick();
            };
            yield return comp;
        }

    }
}
