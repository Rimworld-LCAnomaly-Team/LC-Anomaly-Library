using RimWorld;
using Verse;

namespace LCAnomalyLibrary.Defs
{
    //[DefOf]
    //public static class PawnKindDefOf
    //{
    //    [MayRequireAnomaly]
    //    public static LC_EggedEntityDef LCEggedEntity;
    //}

    /// <summary>
    /// 死亡后会变成蛋的实体PawnKind
    /// </summary>
    public class LC_EggedEntityDef : PawnKindDef;

    [DefOf]
    public static class JobDefOf
    {
        [MayRequireAnomaly]
        public static JobDef LC_CarryToEntityContainer;

        [MayRequireAnomaly]
        public static JobDef LC_TransferBetweenEntityContainers;
    }

    [DefOf]
    public static class ThingDefOf
    {
        [MayRequireAnomaly]
        public static ThingDef LC_ContaningUnit;
    }
}
