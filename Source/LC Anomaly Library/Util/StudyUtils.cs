using RimWorld;
using Verse;

namespace LCAnomalyLibrary.Util
{
    public static class StudyUtil
    {
        public static void DoStudyResultEffect(Pawn studier, Pawn entity, LC_StudyResult result)
        {
            switch (result)
            {
                case LC_StudyResult.Good:
                    FleckMaker.Static(entity.PositionHeld, entity.MapHeld, Defs.FleckDefOf.WorkResult_Good);
                    Log.Message($"工作：{studier.Name} 对异想体 {entity.def.defName.Translate()} 工作成功，质量：良好。");
                    break;

                case LC_StudyResult.Normal:
                    FleckMaker.Static(entity.PositionHeld, entity.MapHeld, Defs.FleckDefOf.WorkResult_Normal);
                    Log.Message($"工作：{studier.Name} 对异想体 {entity.def.defName.Translate()} 工作成功，质量：普通。");
                    break;

                case LC_StudyResult.Bad:
                    FleckMaker.Static(entity.PositionHeld, entity.MapHeld, Defs.FleckDefOf.WorkResult_Bad);
                    Log.Message($"工作：{studier.Name} 对异想体 {entity.def.defName.Translate()} 工作失败，质量：差。");
                    break;

                default:
                    Log.Error("工作：检测到错误的输入。");
                    break;
            }
        }
    }

    public enum LC_StudyResult
    {
        Good,
        Normal,
        Bad
    }
}