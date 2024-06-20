using Verse;

namespace LCAnomalyLibrary.Comp
{
    public abstract class LC_HediffComp_Accessory : HediffComp
    {
        public LC_HediffCompProperties_Accessory Props => (LC_HediffCompProperties_Accessory)props;
    }

    public abstract class LC_HediffComp_AccessoryMouth : LC_HediffComp_Accessory
    {
    }
}