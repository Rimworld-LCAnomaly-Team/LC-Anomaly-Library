using RimWorld;
using Verse;

namespace LCAnomalyLibrary.Comp
{
    public class CompProperties_LC_EntityContainerUnit : CompProperties
    {
        [NoTranslate]
        public string untetheredGraphicTexPath;

        [NoTranslate]
        public string tilingChainTexPath;

        [NoTranslate]
        public string baseChainFastenerTexPath;

        [NoTranslate]
        public string targetChainFastenerTexPath;

        public SoundDef entityLungeSoundHi;

        public SoundDef entityLungeSoundLow;

        public CompProperties_LC_EntityContainerUnit()
        {
            compClass = typeof(Comp_LC_EntityContainerUnit);
        }
    }
}
