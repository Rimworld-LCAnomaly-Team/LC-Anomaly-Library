using Verse;

namespace LCAnomalyLibrary.Comp
{
    public sealed class LC_HediffCompProperties_FakeInvisibility : HediffCompProperties_Invisibility
    {
        public LC_HediffCompProperties_FakeInvisibility()
        {
            compClass = typeof(LC_HediffComp_FakeInvisibility);
        }
    }

    public class LC_HediffComp_FakeInvisibility : HediffComp_Invisibility
    {

    }
}
