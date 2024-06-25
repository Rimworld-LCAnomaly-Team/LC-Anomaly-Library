using RimWorld;
using Verse;

namespace LCAnomalyLibrary.Util
{
    /// <summary>
    /// 研究工作工具类
    /// </summary>
    public static class StudyUtil
    {
        /// <summary>
        /// 播放工作质量特效
        /// </summary>
        /// <param name="studier">研究者</param>
        /// <param name="entity">被研究者</param>
        /// <param name="result">研究质量</param>
        public static void DoStudyResultEffect(Pawn studier, Pawn entity, LC_StudyResult result)
        {
            switch (result)
            {
                case LC_StudyResult.Good:
                    FleckMaker.Static(entity.PositionHeld, entity.MapHeld, Defs.FleckDefOf.WorkResult_Good);
                    Log.Message($"工作：{studier.Name} 对异想体 {entity.def.label.Translate()} 工作成功，质量：良好。");
                    break;

                case LC_StudyResult.Normal:
                    FleckMaker.Static(entity.PositionHeld, entity.MapHeld, Defs.FleckDefOf.WorkResult_Normal);
                    Log.Message($"工作：{studier.Name} 对异想体 {entity.def.label.Translate()} 工作成功，质量：普通。");
                    break;

                case LC_StudyResult.Bad:
                    FleckMaker.Static(entity.PositionHeld, entity.MapHeld, Defs.FleckDefOf.WorkResult_Bad);
                    Log.Message($"工作：{studier.Name} 对异想体 {entity.def.label.Translate()} 工作失败，质量：差。");
                    break;

                default:
                    Log.Error("工作：检测到错误的输入。");
                    break;
            }
        }
    }

    /// <summary>
    /// 研究质量枚举
    /// </summary>
    public enum LC_StudyResult
    {
        /// <summary>
        /// 良好
        /// </summary>
        Good,
        /// <summary>
        /// 一般
        /// </summary>
        Normal,
        /// <summary>
        /// 差
        /// </summary>
        Bad
    }
}