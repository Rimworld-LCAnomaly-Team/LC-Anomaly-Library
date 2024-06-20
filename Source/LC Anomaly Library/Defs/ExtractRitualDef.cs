using LCAnomalyLibrary.Jobs;
using RimWorld;
using Verse;
using Verse.AI.Group;

namespace LCAnomalyLibrary.Defs
{
    public class ExtractRitualDef : PsychicRitualDef
    {
        public override Lord MakeNewLord(PsychicRitualRoleAssignments assignments)
        {
            Lord lord = LordMaker.MakeNewLord(Faction.OfPlayer, new LordJob_ExtractRitual(this, assignments), Find.CurrentMap, assignments.AllAssignedPawns);
            if (assignments.Target.Thing is Verse.Building b)
            {
                lord.AddBuilding(b);
            }

            return lord;
        }
    }
}