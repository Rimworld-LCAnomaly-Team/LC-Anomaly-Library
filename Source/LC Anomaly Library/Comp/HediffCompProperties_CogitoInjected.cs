using Verse;

namespace LCAnomalyLibrary.Comp
{
    public class HediffCompProperties_CogitoInjected : HediffCompProperties
    {
        public float turnSevenSinEnitityChance = 1f;

        public HediffCompProperties_CogitoInjected()
        {
            compClass = typeof(HediffComp_CogitoInjected);
        }
    }
}
