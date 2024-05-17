using Verse;

namespace LCAnomalyLibrary.Comp
{
    public abstract class LC_HediffCompProperties_Accessory : HediffCompProperties
    {
        public string accessoryPart;
        public bool ifConflict;

        public LC_HediffCompProperties_Accessory()
        {
            compClass = typeof(LC_HediffComp_Accessory);
        }
    }

    public abstract class LC_HediffCompProperties_AccessoryMouth : LC_HediffCompProperties_Accessory
    {

    }
}
