using RimWorld;
using Verse.AI.Group;
using Verse;
using LCAnomalyLibrary.Defs;
using Verse.Sound;
using LCAnomalyLibrary.Util;

namespace LCAnomalyLibrary.PsychicRituals
{
    public class PsychicRitualToil_ExtractAnomaly : PsychicRitualToil
    {
        public PsychicRitualRoleDef targetRole;

        public PsychicRitualRoleDef invokerRole;

        public string anomalyLevelTag;

        protected PsychicRitualToil_ExtractAnomaly()
        {
        }

        public PsychicRitualToil_ExtractAnomaly(PsychicRitualRoleDef invokerRole, PsychicRitualRoleDef targetRole, string anomalyLevelTag)
        {
            this.invokerRole = invokerRole;
            this.targetRole = targetRole;
            this.anomalyLevelTag = anomalyLevelTag;
        }

        public override void Start(PsychicRitual psychicRitual, PsychicRitualGraph parent)
        {
            base.Start(psychicRitual, parent);
            PsychicRitualDef_ExtractAnomaly psychicRitualDef_ExtractAnomaly = (PsychicRitualDef_ExtractAnomaly)psychicRitual.def;
            Pawn pawn = psychicRitual.assignments.FirstAssignedPawn(invokerRole);
            Pawn pawn2 = psychicRitual.assignments.FirstAssignedPawn(targetRole);

            //验证受害者发起者是否存在，仪式异常等级是否合法
            if (pawn != null && pawn2 != null)
            {
                ApplyOutcome(psychicRitual, pawn, pawn2);
            }
        }

        private void ApplyOutcome(PsychicRitual psychicRitual, Pawn invoker, Pawn target)
        {
            ThingDef_LCAnomalyBase thingDef = ExtractUtil.Get_AnomlayLvl2DefList_Ritual(anomalyLevelTag).RandomElement();
            Log.Message($"即将提取：{thingDef.defName.Translate()}");

            IntVec3 cell = psychicRitual.assignments.Target.Cell;
            Thing item = GenSpawn.Spawn(thingDef, cell, psychicRitual.Map);
            if (item == null)
            {
                Log.Error("The thing is null");
                return;
            }
            target.Kill(null);

            psychicRitual.Map.effecterMaintainer.AddEffecterToMaintain(EffecterDefOf.Skip_ExitNoDelay.Spawn(cell, psychicRitual.Map), cell, 60);
            RimWorld.SoundDefOf.Psycast_Skip_Exit.PlayOneShot(new TargetInfo(cell, psychicRitual.Map));
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Defs.Look(ref invokerRole, "invokerRole");
            Scribe_Defs.Look(ref targetRole, "targetRole");
            Scribe_Values.Look(ref anomalyLevelTag, "anomalyLevelTag");
        }
    }
}
