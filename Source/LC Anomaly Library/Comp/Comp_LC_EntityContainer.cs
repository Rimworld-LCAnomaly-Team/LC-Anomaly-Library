using RimWorld;
using Verse;

namespace LCAnomalyLibrary.Comp
{
    public abstract class Comp_LC_EntityContainer : ThingComp
    {
        public virtual float ContainmentStrength => parent.GetStatValue(StatDefOf.ContainmentStrength);

        public CompProperties_LC_EntityContainer Props => (CompProperties_LC_EntityContainer)props;

        protected Building.Building_LC_ContainingUnit ContainingUnit => (Building.Building_LC_ContainingUnit)parent;

        public abstract bool Available { get; }

        public abstract Pawn HeldPawn { get; }

        public abstract ThingOwner Container { get; }

        public abstract void EjectContents();

        public override string CompInspectStringExtra()
        {
            string text = base.CompInspectStringExtra();
            if (!text.NullOrEmpty())
            {
                text += "\n";
            }

            float statValue = parent.GetStatValue(StatDefOf.ContainmentStrength);
            text += $"{StatDefOf.ContainmentStrength.LabelCap}: {statValue:F0}";
            if (!parent.Spawned)
            {
                return text;
            }

            if (parent.IsOutside())
            {
                text += string.Format(" ({0})", "Outdoors".Translate());
            }
            else if (StatWorker_ContainmentStrength.AnyDoorForcedOpen(parent.GetRoom()))
            {
                text += string.Format(" ({0})", "Stat_ContainmentStrength_DoorForcedOpen".Translate());
            }

            return text;
        }
    }
}
