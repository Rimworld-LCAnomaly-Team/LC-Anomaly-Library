using RimWorld;
using Verse;

namespace LCAnomalyLibrary.Comp
{
    public sealed class LC_HediffCompProperties_FakeInvisibility : HediffCompProperties
    {
        public int fadeDurationTicks;
        public bool visibleToPlayer;

        public LC_HediffCompProperties_FakeInvisibility()
        {
            compClass = typeof(LC_HediffComp_FakeInvisibility);
        }
    }

    public class LC_HediffComp_FakeInvisibility : HediffComp
    {
        [Unsaved(false)]
        private CompHoldingPlatformTarget platformTarget;

        private CompHoldingPlatformTarget PlatformTarget => platformTarget ?? (platformTarget = base.Pawn.TryGetComp<CompHoldingPlatformTarget>());

        public LC_HediffCompProperties_FakeInvisibility Props => (LC_HediffCompProperties_FakeInvisibility)props;

        public bool PsychologicallyVisible
        {
            get
            {
                //判断是否有强制显形的情况
                return checkVisible;
            }
        }

        private bool checkVisible
        {
            get
            {
                if (base.Pawn.Downed)
                {
                    return true;
                }

                CompHoldingPlatformTarget compHoldingPlatformTarget = PlatformTarget;
                if (compHoldingPlatformTarget != null && compHoldingPlatformTarget.CurrentlyHeldOnPlatform)
                {
                    return true;
                }

                return isVisible;
            }
        }

        private bool isVisible;

        public override void CompPostPostAdd(DamageInfo? dinfo)
        {
            base.CompPostPostAdd(dinfo);
            UpdateTarget();
        }

        public override void CompPostPostRemoved()
        {
            base.CompPostPostRemoved();
            UpdateTarget();
        }

        private void UpdateTarget()
        {
            if (ModLister.CheckRoyaltyOrAnomaly("Invisibility hediff"))
            {
                Pawn pawn = parent.pawn;
                if (pawn.Spawned)
                {
                    pawn.Map.attackTargetsCache.UpdateTarget(pawn);
                }

                if (pawn.RaceProps.Humanlike)
                {
                    PortraitsCache.SetDirty(pawn);
                    GlobalTextureAtlasManager.TryMarkPawnFrameSetDirty(pawn);
                }
            }
        }

        public void BecomeVisible()
        {
            isVisible = true;
            base.Pawn.Notify_BecameVisible();
        }

        public void BecomeInvisible()
        {
            isVisible = false;
            base.Pawn.Notify_BecameInvisible();
        }
    }
}