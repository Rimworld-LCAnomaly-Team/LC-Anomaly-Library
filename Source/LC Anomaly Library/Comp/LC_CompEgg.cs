using RimWorld;
using System.Collections.Generic;
using Verse;

namespace LCAnomalyLibrary.Comp
{
    /// <summary>
    /// LC异想体蛋基础Comp
    /// </summary>
    public abstract class LC_CompEgg : CompInteractable
    {
        /// <summary>
        /// 是否保持生物特征
        /// </summary>
        public bool ShouldTransferBiosignature => ((LC_CompProperties_InteractableEgg)Props).shouldTransferBioSignature;

        /// <summary>
        /// 交互时触发
        /// </summary>
        /// <param name="caster"></param>
        protected override void OnInteracted(Pawn caster)
        {
            CompUsable comp = parent.GetComp<CompUsable>();
            comp.TryStartUseJob(caster, comp.GetExtraTarget(caster));
        }
        
        /// <summary>
        /// 浮动菜单选项
        /// </summary>
        /// <param name="selPawn"></param>
        /// <returns></returns>
        public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
        {
            yield break;
        }
    }
}