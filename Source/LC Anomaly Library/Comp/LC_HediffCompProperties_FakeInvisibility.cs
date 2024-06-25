using RimWorld;
using Verse;

namespace LCAnomalyLibrary.Comp
{
    /// <summary>
    /// LC假隐形HediffCompProperties
    /// </summary>
    public sealed class LC_HediffCompProperties_FakeInvisibility : HediffCompProperties
    {
        public int fadeDurationTicks;
        public bool visibleToPlayer;

        public LC_HediffCompProperties_FakeInvisibility()
        {
            compClass = typeof(LC_HediffComp_FakeInvisibility);
        }
    }
}