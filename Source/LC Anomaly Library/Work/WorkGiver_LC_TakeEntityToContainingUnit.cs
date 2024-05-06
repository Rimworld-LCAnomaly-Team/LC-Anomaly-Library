using RimWorld;
using Verse.AI;
using Verse;
using LCAnomalyLibrary.Comp;

namespace LCAnomalyLibrary.Work
{
    public class WorkGiver_LC_TakeEntityToContainingUnit : WorkGiver_Scanner
    {
        public override ThingRequest PotentialWorkThingRequest => ThingRequest.ForGroup(ThingRequestGroup.HoldingPlatformTarget);

        public override bool ShouldSkip(Pawn pawn, bool forced = false)
        {
            return !ModsConfig.AnomalyActive;
        }

        public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            Comp_LC_ContainingUnitTarget compHoldingPlatformTarget = t.TryGetComp<Comp_LC_ContainingUnitTarget>();
            if (compHoldingPlatformTarget?.targetHolder == null || compHoldingPlatformTarget.targetHolder.Destroyed || compHoldingPlatformTarget.targetHolder.MapHeld != t.MapHeld || compHoldingPlatformTarget.EntityHolder.HeldPawn != null)
            {
                Log.Warning("阿米诺斯 0 false");
                return false;
            }

            if (!pawn.CanReserveAndReach(t, PathEndMode.ClosestTouch, Danger.Deadly, 1, -1, null, forced))
            {
                Log.Warning("阿米诺斯 1 false");
                return false;
            }

            if (!pawn.CanReserveAndReach(compHoldingPlatformTarget.targetHolder, PathEndMode.ClosestTouch, Danger.Deadly, 1, -1, null, forced))
            {
                Log.Warning("阿米诺斯 2 false");
                return false;
            }

            if (t is Pawn pawn2 && !pawn2.ThreatDisabled(pawn))
            {
                Log.Warning("阿米诺斯 3 false");
                return false;
            }

            Log.Warning("阿米诺斯 true");
            return true;
        }

        public override Verse.AI.Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            Log.Warning("阿米诺斯 泰南 2");
            Comp_LC_ContainingUnitTarget compHoldingPlatformTarget = t.TryGetComp<Comp_LC_ContainingUnitTarget>();
            if (compHoldingPlatformTarget == null)
            {
                return null;
            }
            Log.Warning("阿米诺斯 泰南 3");
            Verse.AI.Job job = JobMaker.MakeJob(JobDefOf.CarryToEntityHolder, compHoldingPlatformTarget.targetHolder, t);
            job.count = 1;
            return job;
        }
    }
}
