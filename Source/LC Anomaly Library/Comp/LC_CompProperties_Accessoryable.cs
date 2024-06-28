using Verse;

namespace LCAnomalyLibrary.Comp
{
    /// <summary>
    /// 可获取饰品的CompProperties
    /// </summary>
    public class LC_CompProperties_Accessoryable : CompProperties
    {
        /// <summary>
        /// XML：饰品Def
        /// </summary>
        public ThingDef accessoryDef;
        /// <summary>
        /// XML：获取饰品的概率
        /// </summary>
        public float accessoryChance;

        /// <summary>
        /// Comp
        /// </summary>
        public LC_CompProperties_Accessoryable()
        {
            compClass = typeof(LC_CompAccessoryable);
        }
    }
}
