using Verse;

namespace LCAnomalyLibrary.Comp
{
    /// <summary>
    /// LC基础实体CompProperties
    /// </summary>
    public class LC_CompProperties_Entity : CompProperties
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
        /// XML：工作产生PeBox的类型
        /// </summary>
        public ThingDef peBoxDef;

        /// <summary>
        /// XML：工作产生PeBox的数量（良好）
        /// </summary>
        public int amountPeBoxStudyGood;
        /// <summary>
        /// XML：工作产生PeBox的数量（普通）
        /// </summary>
        public int amountPeBoxStudyNormal;
        /// <summary>
        /// XML：工作产生PeBox的数量（差）
        /// </summary>
        public int amountPeBoxStudyBad;

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