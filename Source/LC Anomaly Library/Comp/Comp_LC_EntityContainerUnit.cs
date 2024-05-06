using Verse;

namespace LCAnomalyLibrary.Comp
{
    public class Comp_LC_EntityContainerUnit : Comp_LC_EntityContainer
    {
        public override bool Available => !base.ContainingUnit.Occupied;

        public override Pawn HeldPawn => base.ContainingUnit.HeldPawn;

        public override ThingOwner Container => base.ContainingUnit.innerContainer;

        public new CompProperties_LC_EntityContainerUnit Props => (CompProperties_LC_EntityContainerUnit)props;

        public override void EjectContents()
        {
            base.ContainingUnit.EjectContents();
        }
    }
}
