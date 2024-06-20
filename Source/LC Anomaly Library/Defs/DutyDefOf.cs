using RimWorld;
using Verse.AI;

namespace LCAnomalyLibrary.Defs
{
    [DefOf]
    public static class DutyDefOf
    {
        static DutyDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(DutyDefOf));
        }

        [MayRequireAnomaly]
        public static DutyDef GatherOfferingsForExtractRitual;
    }
}