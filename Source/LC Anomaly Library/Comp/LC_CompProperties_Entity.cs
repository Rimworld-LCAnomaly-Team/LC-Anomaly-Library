using Verse;

namespace LCAnomalyLibrary.Comp
{
    public class LC_CompProperties_Entity : CompProperties
    {
        public int qliphothCountMax;
        public float studySucessRateBase;
        public float accessoryChance;
        public bool shoundNotifyWhenEscape;

        public LC_CompProperties_Entity()
        {
            compClass = typeof(LC_CompEntity);
        }
    }
}
