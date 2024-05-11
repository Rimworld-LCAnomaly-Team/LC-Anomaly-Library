using RimWorld;

namespace LCAnomalyLibrary.Defs
{
    [DefOf]
    public static class PsychicRitualRoleDefOf
    {
        static PsychicRitualRoleDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(PsychicRitualRoleDefOf));
        }

        [MayRequireAnomaly]
        public static PsychicRitualRoleDef ExtractLCAnomalyZAYIN;

        [MayRequireAnomaly]
        public static PsychicRitualRoleDef ExtractLCAnomalyTETH;

        [MayRequireAnomaly]
        public static PsychicRitualRoleDef ExtractLCAnomalyHE;

        [MayRequireAnomaly]
        public static PsychicRitualRoleDef ExtractLCAnomalyWAW;

        [MayRequireAnomaly]
        public static PsychicRitualRoleDef ExtractLCAnomalyALEPH;
    }
}
