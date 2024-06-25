using Verse;

namespace LCAnomalyLibrary.Comp
{
    public class LC_CompProperties_Entity : CompProperties
    {
        #region XML字段

        /// <summary>
        /// 最大逆卡巴拉值
        /// </summary>
        public int qliphothCountMax;

        /// <summary>
        /// 学习基础成功率
        /// </summary>
        public float studySucessRateBase;

        /// <summary>
        /// 工作产生PeBox的类型
        /// </summary>
        public ThingDef peBoxDef;

        /// <summary>
        /// 工作产生PeBox的数量（良好）
        /// </summary>
        public int amountPeBoxStudyGood;
        /// <summary>
        /// 工作产生PeBox的数量（普通）
        /// </summary>
        public int amountPeBoxStudyNormal;
        /// <summary>
        /// 工作产生PeBox的数量（差）
        /// </summary>
        public int amountPeBoxStudyBad;

        /// <summary>
        /// 获取饰品的概率
        /// </summary>
        public float accessoryChance;

        /// <summary>
        /// 出逃时是否通知
        /// </summary>
        public bool shoundNotifyWhenEscape;

        #endregion

        public LC_CompProperties_Entity()
        {
            compClass = typeof(LC_CompEntity);
        }
    }
}