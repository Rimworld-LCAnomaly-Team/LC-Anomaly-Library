using RimWorld;

namespace LCAnomalyLibrary.Defs
{
    [DefOf]
    public static class HistoryEventDefOf
    {
        [MayRequireAnomaly]
        public static HistoryEventDef CogitoExtracted;

        [MayRequireAnomaly]
        public static HistoryEventDef CogitoInjected;
    }
}