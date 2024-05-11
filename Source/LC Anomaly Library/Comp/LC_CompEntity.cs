using RimWorld;
using Verse;

namespace LCAnomalyLibrary.Comp
{
    /// <summary>
    /// LC基础实体抽象Comp
    /// </summary>
    public abstract class LC_CompEntity : ThingComp
    {
        public const int LongerTracksUnlockIndex = 1;

        public bool everRevealed;

        /// <summary>
        /// 生物特征
        /// </summary>
        public int biosignature;

        /// <summary>
        /// 在攻击时受伤
        /// </summary>
        public bool injuredWhileAttacking;

        /// <summary>
        /// XML输入：是否在逃脱收容后提醒
        /// </summary>
        public bool shoundNotifyWhenEscape = true;

        /// <summary>
        /// 生物特征名
        /// </summary>
        protected string biosignatureName;

        /// <summary>
        /// 生物特征名
        /// </summary>
        public string BiosignatureName => biosignatureName ?? (biosignatureName = AnomalyUtility.GetBiosignatureName(biosignature));

        /// <summary>
        /// Comp被挂载的Pawn
        /// </summary>
        protected Pawn SelfPawn => (Pawn)parent;

        /// <summary>
        /// 被看到的操作
        /// </summary>
        protected abstract void CheckIfSeen();

        /// <summary>
        /// 逃脱收容后执行的操作执行的操作
        /// </summary>
        public abstract void Escape();

        /// <summary>
        /// 绑到收容平台上的操作
        /// </summary>
        public abstract void AfterHoldToPlatform();
    }
}
