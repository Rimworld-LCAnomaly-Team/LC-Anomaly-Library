using Verse;

namespace LCAnomalyLibrary.Comp
{
    /// <summary>
    /// Cogito被注射状态的HediffCompProperties
    /// </summary>
    public class HediffCompProperties_CogitoInjected : HediffCompProperties
    {
        /// <summary>
        /// XML：注射后大罪生物生成概率
        /// </summary>
        public float turnSevenSinEnitityChance = 1f;

        /// <summary>
        /// Comp
        /// </summary>
        public HediffCompProperties_CogitoInjected()
        {
            compClass = typeof(HediffComp_CogitoInjected);
        }
    }
}