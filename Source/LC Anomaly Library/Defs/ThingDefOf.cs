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
        /// ZAYIN级别的Pebox
        /// </summary>
        public static ThingDef PeBoxZAYIN;

        /// <summary>
        /// TETH级别的Pebox
        /// </summary>
        public static ThingDef PeBoxTETH;

        /// <summary>
        /// HE级别的Pebox
        /// </summary>
        public static ThingDef PeBoxHE;

        /// <summary>
        /// WAW级别的Pebox
        /// </summary>
        public static ThingDef PeBoxWAW;

        /// <summary>
        /// ALEPH级别的Pebox
        /// </summary>
        public static ThingDef PeBoxALEPH;

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
        /// 提取仪式点（建筑）
        /// </summary>
        public static ThingDef ExtractRitualSpot;

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