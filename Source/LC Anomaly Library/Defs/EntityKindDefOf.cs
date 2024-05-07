using RimWorld;
using Verse;

namespace LCAnomalyLibrary.Defs
{
    public class CompEggedEntity : ThingComp
    {

    }

    [DefOf]
    public static class ThingDefOf
    {
        [MayRequireAnomaly]
        public static ThingDef LC_ContaningUnit;
    }
}
