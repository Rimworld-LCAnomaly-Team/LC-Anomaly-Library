using RimWorld;
using Verse;

namespace LCAnomalyLibrary.Defs
{
    /// <summary>
    /// 该mod所有的ThingDef
    /// </summary>
    [DefOf]
    public static class ThingDefOf
    {
        /// <summary>
        /// 通用PeBox
        /// </summary>
        public static ThingDef EnkephalinBox;

        /// <summary>
        /// 脑啡肽（成瘾品）
        /// </summary>
        public static ThingDef Enkephalin;

        /// <summary>
        /// Cogito
        /// </summary>
        public static ThingDef Cogito;

        /// <summary>
        /// 脑与脊髓神经
        /// </summary>
        public static ThingDef BrainSpinalNerve;

        /// <summary>
        /// LC收容单元
        /// </summary>
        public static ThingDef LC_HoldingPlatform;

        /// <summary>
        /// 水井（建筑）
        /// </summary>
        public static ThingDef TheWell;

        /// <summary>
        /// 逆卡巴拉计数器（建筑）
        /// </summary>
        public static ThingDef QliphothIndicator;

        /// <summary>
        /// Cogito水桶（建筑）
        /// </summary>
        public static ThingDef CogitoBucket;
    }

    /// <summary>
    /// EGO类型枚举
    /// </summary>
    public enum EGO_TYPE
    {
        /// <summary>
        /// EGO武器
        /// </summary>
        Weapon = 0,
        /// <summary>
        /// EGO装备
        /// </summary>
        Armor
    }
}