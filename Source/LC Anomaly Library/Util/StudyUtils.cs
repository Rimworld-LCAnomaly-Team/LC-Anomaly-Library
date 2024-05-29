using RimWorld;
using Verse;

namespace LCAnomalyLibrary.Util
{
    public class StudyUtil
    {
        public static void DoStudyResultEffect(Pawn studier, Pawn entity, LC_StudyResult result)
        {
            switch (result)
            {
                case LC_StudyResult.Good:
                    FleckMaker.Static(entity.PositionHeld, entity.MapHeld, Defs.FleckDefOf.WorkResult_Good);
                    Log.Message($"{studier.Name} 研究异想体 {entity.def.defName.Translate()} 成功，研究质量：好。");
                    break;
                case LC_StudyResult.Normal:
                    FleckMaker.Static(entity.PositionHeld, entity.MapHeld, Defs.FleckDefOf.WorkResult_Normal);
                    Log.Message($"{studier.Name} 研究异想体 {entity.def.defName.Translate()} 成功，研究质量：普通。");
                    break;

                case LC_StudyResult.Bad:
                    FleckMaker.Static(entity.PositionHeld, entity.MapHeld, Defs.FleckDefOf.WorkResult_Bad);
                    Log.Message($"{studier.Name} 研究异想体 {entity.def.defName.Translate()} 失败，研究质量：差。");
                    break;
                default:
                    Log.Error("检测到错误的输入。");
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
