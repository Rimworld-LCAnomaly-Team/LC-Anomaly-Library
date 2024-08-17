using Verse;

namespace LCAnomalyLibrary.Comp.Pawns
{
    /// <summary>
    /// LC员工基本属性CompProperties
    /// </summary>
    public class CompProperties_PawnStatus : CompProperties
    {
        public int statusNumMax = 100;

        public CompProperties_PawnStatus()
        {
            compClass = typeof(CompPawnStatus);
        }
    }
}
