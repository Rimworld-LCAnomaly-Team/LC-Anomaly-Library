using RimWorld;
using Verse;

namespace LCAnomalyLibrary.Defs
{
    [DefOf]
    public static class ThingDefOf
    {
        //[MayRequireAnomaly]
        //public static ThingDef LC_ContaningUnit;
    }

    [DefOf]
    public static class DamageDefOf
    {
        public static DamageDef LC_Entity_RED;
        public static DamageDef LC_Entity_WHITE;
        public static DamageDef LC_Entity_BLACK;
        public static DamageDef LC_Entity_PALE;
    }

    [DefOf]
    public static class LC_HediffDefOf
    {
        /// <summary>
        /// 假隐形Hediff，用于做不能被自动索敌的操作
        /// </summary>
        [MayRequireAnomaly]
        public static HediffDef FakeInvisibility;
    }

}
