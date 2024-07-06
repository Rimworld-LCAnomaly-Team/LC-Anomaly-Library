using RimWorld;

namespace LCAnomalyLibrary.Comp
{
    public abstract class LC_CompProperties_StudyUnlocks : CompProperties_StudyUnlocks
    {
        public LC_CompProperties_StudyUnlocks()
        {
            compClass = typeof(LC_CompStudyUnlocks);
        }
    }
}