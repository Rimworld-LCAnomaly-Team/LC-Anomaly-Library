using RimWorld;
using System.Collections.Generic;
using Verse;

namespace LCAnomalyLibrary.Comp
{
    public abstract class LC_CompEgg : CompInteractable
    {
        public bool ShouldTransferBiosignature => ((LC_CompProperties_InteractableEgg)Props).shouldTransferBioSignature;

        protected override void OnInteracted(Pawn caster)
        {
            CompUsable comp = parent.GetComp<CompUsable>();
            comp.TryStartUseJob(caster, comp.GetExtraTarget(caster));
        }

        public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
        {
            yield break;
        }
    }
}