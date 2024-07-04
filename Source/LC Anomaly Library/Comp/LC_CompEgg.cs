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
        public LC_CompProperties_InteractableEgg Props => (LC_CompProperties_InteractableEgg)props;

        /// <summary>
        /// 研究知识等级
        /// </summary>
        public int StudyProgress;

        /// <summary>
        /// EGO武器已提取数量
        /// </summary>
        public int CurEgoWeaponExtractAmount;
        /// <summary>
        /// EGO装备已提取数量
        /// </summary>
        public int CurEgoArmorExtractAmount;

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