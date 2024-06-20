using RimWorld;
using Verse;

namespace LCAnomalyLibrary.Defs
{
    [DefOf]
    public static class HediffDefOf
    {
        /// <summary>
        /// 假隐形Hediff，用于做不能被自动索敌的操作
        /// </summary>
        [MayRequireAnomaly]
        public static HediffDef FakeInvisibility;

        [MayRequireAnomaly]
        public static HediffDef CogitoExtracted;

        [MayRequireAnomaly]
        public static HediffDef CogitoInjected;
    }
}