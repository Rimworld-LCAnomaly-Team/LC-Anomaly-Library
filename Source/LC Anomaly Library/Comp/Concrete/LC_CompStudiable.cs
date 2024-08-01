using RimWorld;
using System.Collections.Generic;
using Verse;

namespace LCAnomalyLibrary.Comp
{
    public class LC_CompStudiable : CompStudiable
    {

        new public LC_CompProperties_Studiable Props => (LC_CompProperties_Studiable)props;

        public int StudyTimesPeriod => Props.studyTimesPeriod;

        public override void Study(Pawn studier, float studyAmount, float anomalyKnowledgeAmount = 0)
        {
            bool completed = Completed;

            studyAmount *= Find.Storyteller.difficulty.researchSpeedFactor;
            studyAmount *= studier.GetStatValue(StatDefOf.ResearchSpeed);
            anomalyKnowledgeGained += anomalyKnowledgeAmount;
            Find.StudyManager.Study(parent, studier, studyAmount);

            if (ModsConfig.AnomalyActive && anomalyKnowledgeAmount > 0f)
            {
                Find.StudyManager.StudyAnomaly(parent, studier, anomalyKnowledgeAmount, KnowledgeCategory);
            }

            if (!completed && Completed)
            {
                QuestUtility.SendQuestTargetSignals(parent.questTags, "Researched", parent.Named("SUBJECT"), studier.Named("STUDIER"));
                if (!Props.completedMessage.NullOrEmpty())
                {
                    Messages.Message(Props.completedMessage, parent, MessageTypeDefOf.NeutralEvent);
                }

                if (studier != null && !Props.completedLetterText.NullOrEmpty() && !Props.completedLetterTitle.NullOrEmpty())
                {
                    Find.LetterStack.ReceiveLetter(Props.completedLetterTitle.Formatted(studier.Named("STUDIER"), parent.Named("PARENT")), Props.completedLetterText.Formatted(studier.Named("STUDIER"), parent.Named("PARENT")), Props.completedLetterDef ?? LetterDefOf.NeutralEvent, new List<Thing> { parent, studier });
                }
            }

            //如果是LC实体派生，则触发回调事件
            LC_CompEntity entity = Pawn.TryGetComp<LC_CompEntity>();
            entity?.Notify_Studied(studier);
        }
    }
}
