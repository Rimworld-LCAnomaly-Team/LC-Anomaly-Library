using RimWorld;
using Verse;

namespace LCAnomalyLibrary.Comp
{
    /// <summary>
    /// LC假隐形HediffComp
    /// </summary>
    public class LC_HediffComp_FakeInvisibility : HediffComp
    {
        #region 字段

        [Unsaved(false)]
        private CompHoldingPlatformTarget platformTarget;

        private CompHoldingPlatformTarget PlatformTarget => platformTarget ?? (platformTarget = base.Pawn.TryGetComp<CompHoldingPlatformTarget>());

        /// <summary>
        /// CompProperties
        /// </summary>
        public LC_HediffCompProperties_FakeInvisibility Props => (LC_HediffCompProperties_FakeInvisibility)props;

        /// <summary>
        /// 是否处于隐身状态
        /// </summary>
        public bool PsychologicallyVisible
        {
            get
            {
                //判断是否有强制显形的情况
                return checkVisible;
            }
        }

        private bool checkVisible
        {
            get
            {
                if (base.Pawn.Downed)
                {
                    return true;
                }

                CompHoldingPlatformTarget compHoldingPlatformTarget = PlatformTarget;
                if (compHoldingPlatformTarget != null && compHoldingPlatformTarget.CurrentlyHeldOnPlatform)
                {
                    return true;
                }

                return isVisible;
            }
        }
        private bool isVisible;

        #endregion

        #region 生命周期

        /// <summary>
        /// hediff被添加前时
        /// </summary>
        /// <param name="dinfo"></param>
        public override void CompPostPostAdd(DamageInfo? dinfo)
        {
            base.CompPostPostAdd(dinfo);
            UpdateTarget();
        }

        /// <summary>
        /// hediff被移除前时
        /// </summary>
        public override void CompPostPostRemoved()
        {
            base.CompPostPostRemoved();
            UpdateTarget();
        }

        #endregion

        #region 工具方法

        private void UpdateTarget()
        {
            if (ModLister.CheckRoyaltyOrAnomaly("Invisibility hediff"))
            {
                Pawn pawn = parent.pawn;
                if (pawn.Spawned)
                {
                    pawn.Map.attackTargetsCache.UpdateTarget(pawn);
                }

                if (pawn.RaceProps.Humanlike)
                {
                    PortraitsCache.SetDirty(pawn);
                    GlobalTextureAtlasManager.TryMarkPawnFrameSetDirty(pawn);
                }
            }
        }

        /// <summary>
        /// 显形
        /// </summary>
        public void BecomeVisible()
        {
            isVisible = true;
            base.Pawn.Notify_BecameVisible();
        }

        /// <summary>
        /// 隐身
        /// </summary>
        public void BecomeInvisible()
        {
            isVisible = false;
            base.Pawn.Notify_BecameInvisible();
        }

        #endregion
    }
}
