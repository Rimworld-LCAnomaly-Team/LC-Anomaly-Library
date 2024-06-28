using HarmonyLib;
using LCAnomalyLibrary.Comp;
using RimWorld;
using Verse;

namespace LCAnomalyLibrary.Patch
{
    /// <summary>
    /// 关于CompStudiable的补丁（为了提供研究工作结束回调的特性）
    /// </summary>
    [HarmonyPatch(typeof(CompStudiable), nameof(CompStudiable.Study))]
    public class Patch_CompStudiable_Study
    {
        /// <summary>
        /// Postfix方法
        /// </summary>
        /// <param name="__instance">原来的反射对象</param>
        /// <param name="studier">研究者</param>
        /// <param name="studyAmount">研究量？</param>
        /// <param name="anomalyKnowledgeAmount">异常知识数量？</param>
        private static void Postfix(CompStudiable __instance, Pawn studier, float studyAmount, float anomalyKnowledgeAmount = 0f)
        {
            //如果是LC实体派生，则触发回调事件
            LC_CompEntity entity = __instance.Pawn.TryGetComp<LC_CompEntity>();
            entity?.Notify_Studied(studier);
        }
    }
}