using LCAnomalyLibrary.Defs;
using LCAnomalyLibrary.Util;
using RimWorld;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace LCAnomalyLibrary.PsychicRituals
{
    /// <summary>
    /// 提取异想体仪式的Toil
    /// </summary>
    public class PsychicRitualToil_ExtractAnomaly : PsychicRitualToil
    {
        /// <summary>
        /// 目标角色
        /// </summary>
        public PsychicRitualRoleDef targetRole;

        /// <summary>
        /// 触发角色
        /// </summary>
        public PsychicRitualRoleDef invokerRole;

        /// <summary>
        /// 异常等级标签
        /// </summary>
        public string anomalyLevelTag;


        /// <summary>
        /// 提取仪式
        /// </summary>
        /// <param name="invokerRole">触发角色</param>
        /// <param name="targetRole">目标角色</param>
        /// <param name="anomalyLevelTag">异常等级标签</param>
        public PsychicRitualToil_ExtractAnomaly(PsychicRitualRoleDef invokerRole, PsychicRitualRoleDef targetRole, string anomalyLevelTag)
        {
            this.invokerRole = invokerRole;
            this.targetRole = targetRole;
            this.anomalyLevelTag = anomalyLevelTag;
        }

        /// <summary>
        /// 开始仪式
        /// </summary>
        /// <param name="psychicRitual">仪式</param>
        /// <param name="parent"></param>
        public override void Start(PsychicRitual psychicRitual, PsychicRitualGraph parent)
        {
            base.Start(psychicRitual, parent);

            Pawn pawn = psychicRitual.assignments.FirstAssignedPawn(invokerRole);
            Pawn pawn2 = psychicRitual.assignments.FirstAssignedPawn(targetRole);

            //验证受害者发起者是否存在，仪式异常等级是否合法
            if (pawn != null && pawn2 != null)
            {
                ApplyOutcome(psychicRitual, pawn, pawn2);
            }
        }

        /// <summary>
        /// 仪式结果
        /// </summary>
        /// <param name="psychicRitual">仪式</param>
        /// <param name="invoker">触发者</param>
        /// <param name="target">目标</param>
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
            SoundDefOf.Psycast_Skip_Exit.PlayOneShot(new TargetInfo(cell, psychicRitual.Map));
        }

        /// <summary>
        /// 和保存有关
        /// </summary>
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Defs.Look(ref invokerRole, "invokerRole");
            Scribe_Defs.Look(ref targetRole, "targetRole");
            Scribe_Values.Look(ref anomalyLevelTag, "anomalyLevelTag");
        }
    }
}