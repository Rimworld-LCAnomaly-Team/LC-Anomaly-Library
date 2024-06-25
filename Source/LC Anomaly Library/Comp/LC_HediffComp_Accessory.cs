using Verse;

namespace LCAnomalyLibrary.Comp
{
    /// <summary>
    /// LC异想体首饰基础HediffComp
    /// </summary>
    public abstract class LC_HediffComp_Accessory : HediffComp
    {
        /// <summary>
        /// HediffCompProperties
        /// </summary>
        public LC_HediffCompProperties_Accessory Props => (LC_HediffCompProperties_Accessory)props;
    }

    /// <summary>
    /// LC异想体首饰基础HediffComp（口部）
    /// </summary>
    public abstract class LC_HediffComp_AccessoryMouth : LC_HediffComp_Accessory
    {
    }
}