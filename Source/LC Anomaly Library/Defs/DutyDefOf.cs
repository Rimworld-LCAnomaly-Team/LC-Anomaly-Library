using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
