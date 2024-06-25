using RimWorld;
using Verse;

namespace LCAnomalyLibrary.Defs
{
    /// <summary>
    /// 该mod的所有HediffDef
    /// </summary>
    [DefOf]
    public static class HediffDefOf
    {
        /// <summary>
        /// 假隐形Hediff，用于做不能被自动索敌的操作
        /// </summary>
        [MayRequireAnomaly]
        public static HediffDef FakeInvisibility;

        /// <summary>
        /// 被提取了Cogito
        /// </summary>
        [MayRequireAnomaly]
        public static HediffDef CogitoExtracted;

        /// <summary>
        /// 被注射了Cogito
        /// </summary>
        [MayRequireAnomaly]
        public static HediffDef CogitoInjected;
    }
}