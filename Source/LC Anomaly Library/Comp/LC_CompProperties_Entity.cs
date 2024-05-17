using Verse;

namespace LCAnomalyLibrary.Comp
{
    public class LC_CompProperties_Entity : CompProperties
    {
        public float accessoryChance;
        public bool shoundNotifyWhenEscape;

        public LC_CompProperties_Entity()
        {
            compClass = typeof(LC_CompEntity);
        }
    }
}
