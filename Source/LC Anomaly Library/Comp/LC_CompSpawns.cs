using Verse;

namespace LCAnomalyLibrary.Comp
{
    public abstract class LC_CompSpawns : ThingComp
    {
        public int spawnTick = -1;

        public override void PostExposeData()
        {
            Scribe_Values.Look(ref spawnTick, "spawnTick", 0);
        }
    }
}