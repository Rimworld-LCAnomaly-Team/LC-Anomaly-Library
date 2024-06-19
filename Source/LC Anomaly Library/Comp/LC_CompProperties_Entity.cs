using Verse;

namespace LCAnomalyLibrary.Comp
{
    public class LC_CompProperties_Entity : CompProperties
    {
        public int qliphothCountMax;
        public float studySucessRateBase;

        public ThingDef peBoxDef;
        public int amountPeBoxStudyGood;
        public int amountPeBoxStudyNormal;
        public int amountPeBoxStudyBad;

        public float accessoryChance;
        public bool shoundNotifyWhenEscape;

        public LC_CompProperties_Entity()
        {
            compClass = typeof(LC_CompEntity);
        }
    }
}
