using RimWorld;
using System.Collections.Generic;
using Verse.AI.Group;
using Verse;
using LCAnomalyLibrary.Defs;
using LCAnomalyLibrary.Util;

namespace LCAnomalyLibrary.PsychicRituals
{
    public class PsychicRitualDef_ExtractAnomaly : ExtractRitualDef_InvocationCircle
    {
        string anomalyLevelTag;

        public override List<PsychicRitualToil> CreateToils(PsychicRitual psychicRitual, PsychicRitualGraph parent)
        {
            List<PsychicRitualToil> list = base.CreateToils(psychicRitual, parent);
            list.Add(new PsychicRitualToil_ExtractAnomaly(InvokerRole, TargetRole, anomalyLevelTag));
            return list;
        }

        public override IEnumerable<string> GetPawnTooltipExtras(Pawn pawn)
        {
            if (pawn.guest != null && !pawn.guest.Recruitable)
            {
                yield return "NonRecruitable".Translate();
            }
        }
        public override void CheckPsychicRitualCancelConditions(PsychicRitual psychicRitual)
        {
            base.CheckPsychicRitualCancelConditions(psychicRitual);

            //验证Tag是否正确，Tag对应异想体是否存在，若不满足任意条件则终止仪式
            var list = ExtractUtil.Get_AnomlayLvl2DefList_Ritual(anomalyLevelTag);
            if (list == null)
            {
                psychicRitual.CancelPsychicRitual("PsychicRitualDef_InvocationCircle_InvokerLost".Translate());
                Log.Warning("提取仪式因Tag错误而失败");
            }
            if (list.Count <= 0)
            {
                psychicRitual.CancelPsychicRitual("PsychicRitualDef_InvocationCircle_InvokerLost".Translate());
                Log.Warning("提取仪式因列表为空失败");
            }
        }
    }
}
