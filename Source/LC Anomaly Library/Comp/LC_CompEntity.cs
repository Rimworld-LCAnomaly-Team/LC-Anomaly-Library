using RimWorld;
using Verse;

namespace LCAnomalyLibrary.Comp
{
    /// <summary>
    /// LC基础实体抽象类
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
        /// 是否在攻击时受伤
        /// </summary>
        public bool injuredWhileAttacking;

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
        /// 是否被看到的操作
        /// </summary>
        protected abstract void CheckIfSeen();
    }
}
