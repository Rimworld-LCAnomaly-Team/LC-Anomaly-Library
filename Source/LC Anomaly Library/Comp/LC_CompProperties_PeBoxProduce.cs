using Verse;

namespace LCAnomalyLibrary.Comp
{
    /// <summary>
    /// LC可产PeBox的CompProperties
    /// </summary>
    public class LC_CompProperties_PeBoxProduce : CompProperties
    {
        #region XML字段

        /// <summary>
        /// XML：工作产生PeBox的类型
        /// </summary>
        public ThingDef peBoxDef;

        /// <summary>
        /// XML：工作产生PeBox的数量（良好）
        /// </summary>
        public int amountStudyGood;
        /// <summary>
        /// XML：工作产生PeBox的数量（普通）
        /// </summary>
        public int amountStudyNormal;
        /// <summary>
        /// XML：工作产生PeBox的数量（差）
        /// </summary>
        public int amountStudyBad;

        #endregion

        /// <summary>
        /// Comp
        /// </summary>
        public LC_CompProperties_PeBoxProduce()
        {
            compClass = typeof(LC_CompPeBoxProduce);
        }
    }
}
