using Verse;

namespace LCAnomalyLibrary.Comp
{
    /// <summary>
    /// LC异想体首饰基础HediffCompProperties
    /// </summary>
    public abstract class LC_HediffCompProperties_Accessory : HediffCompProperties
    {
        /// <summary>
        /// XML：首饰部位
        /// </summary>
        public string accessoryPart;

        /// <summary>
        /// XML：是否允许冲突
        /// </summary>
        public bool ifConflict;

        /// <summary>
        /// Comp
        /// </summary>
        public LC_HediffCompProperties_Accessory()
        {
            compClass = typeof(LC_HediffComp_Accessory);
        }
    }

    /// <summary>
    /// LC异想体首饰基础HediffCompProperties（口部）
    /// </summary>
    public abstract class LC_HediffCompProperties_AccessoryMouth : LC_HediffCompProperties_Accessory
    {
    }
}