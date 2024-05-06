using RimWorld;
using System.Collections.Generic;
using Verse;

namespace LCAnomalyLibrary.Comp
{
    public class CompProperties_LC_Studiable : CompProperties
    {
        public int frequencyTicks = -1;

        public float studyAmountToComplete = -1f;

        public bool showToggleGizmo;

        public bool studyEnabledByDefault = true;

        public bool canBeActivityDeactivated;

        internal float? anomalyKnowledge;

        internal KnowledgeCategoryDef knowledgeCategory;

        public float knowledgeFactorOutdoors = 1f;

        public int minMonolithLevelForStudy;

        internal bool requiresHoldingPlatform;

        internal bool requiresImprisonment;

        [MustTranslate]
        public string completedLetterTitle;

        [MustTranslate]
        public string completedLetterText;

        [MustTranslate]
        public string completedMessage;

        public LetterDef completedLetterDef;

        public bool Completable => studyAmountToComplete > 0f;

        public CompProperties_LC_Studiable()
        {
            compClass = typeof(Comp_LC_Studiable);
        }

        public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
        {
            foreach (string item in base.ConfigErrors(parentDef))
            {
                yield return item;
            }

            if (requiresHoldingPlatform && !typeof(Pawn).IsAssignableFrom(parentDef.thingClass))
            {
                yield return "requiresHoldingPlatform can only be used on Pawns";
            }

            if (requiresImprisonment && !typeof(Pawn).IsAssignableFrom(parentDef.thingClass))
            {
                yield return "requiresImprisonment can only be used on Pawns";
            }

            if (ModsConfig.AnomalyActive && anomalyKnowledge > 0f)
            {
                if (knowledgeCategory == null && parentDef != ThingDefOf.VoidMonolith)
                {
                    yield return "anomalyKnowledge is set but knowledgeCategory is not";
                }

                if (minMonolithLevelForStudy == 0)
                {
                    yield return "anomalyKnowledge is set but minMonolithLevelForStudy is not";
                }
            }
        }
    }
}
