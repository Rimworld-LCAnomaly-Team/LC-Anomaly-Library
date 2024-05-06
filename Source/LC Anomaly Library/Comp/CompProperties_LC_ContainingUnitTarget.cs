using LCAnomalyLibrary.Defs;
using Verse;

namespace LCAnomalyLibrary.Comp
{
    public class CompProperties_LC_ContainingUnitTarget : CompProperties
    {
        public PawnKindDef heldPawnKind;

        [MustTranslate]
        public string capturedLetterLabel;

        [MustTranslate]
        public string capturedLetterText;

        public float baseEscapeIntervalMtbDays = 60f;

        public bool lookForTargetOnEscape = true;

        public bool canBeExecuted = true;

        public bool getsColdContainmentBonus;

        public bool hasAnimation = true;

        public CompProperties_LC_ContainingUnitTarget()
        {
            compClass = typeof(Comp_LC_ContainingUnitTarget);
        }
    }
}
