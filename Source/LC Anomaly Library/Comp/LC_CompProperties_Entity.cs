using Verse;

namespace LCAnomalyLibrary.Comp
{
    /// <summary>
    /// LC基础实体CompProperties
    /// </summary>
    public abstract class LC_CompProperties_Entity : CompProperties
    {
        #region XML字段

        /// <summary>
        /// XML：最大逆卡巴拉值
        /// </summary>
        public int qliphothCountMax;

        /// <summary>
        /// XML：学习基础成功率
        /// </summary>
        public float studySucessRateBase;

        /// <summary>
        /// XML：获取饰品的概率
        /// </summary>
        public float accessoryChance;

        /// <summary>
        /// XML：出逃时是否通知
        /// </summary>
        public bool shoundNotifyWhenEscape;

        #endregion

        /// <summary>
        /// Comp
        /// </summary>
        public LC_CompProperties_Entity()
        {
            compClass = typeof(LC_CompEntity);
        }
    }
}