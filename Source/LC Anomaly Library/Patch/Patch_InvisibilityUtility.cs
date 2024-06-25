using HarmonyLib;
using LCAnomalyLibrary.Comp;
using RimWorld;
using System.Collections.Generic;
using Verse;

namespace LCAnomalyLibrary.Patch
{
    /// <summary>
    /// 关于InvisibilityUtility的补丁（为了提供假灵能隐身判断的特性）
    /// </summary>
    [HarmonyPatch(typeof(InvisibilityUtility), nameof(InvisibilityUtility.IsPsychologicallyInvisible))]
    public class Patch_InvisibilityUtility_IsPsychologicallyInvisible
    {
        /// <summary>
        /// Prefix方法
        /// </summary>
        /// <param name="pawn">持有hediff的pawn</param>
        /// <param name="__result">原方法返回结果</param>
        /// <returns>false 终止原方法（会和同样的prefix产生可能的不兼容）</returns>
        private static bool Prefix(Pawn pawn, ref bool __result)
        {
            List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
            for (int i = 0; i < hediffs.Count; i++)
            {
                HediffComp hediffComp = hediffs[i].TryGetComp<HediffComp>();

                if (hediffComp != null)
                {
                    if (hediffComp is HediffComp_Invisibility)
                    {
                        if (!((HediffComp_Invisibility)hediffComp).PsychologicallyVisible)
                        {
                            __result = true;
                            return false;
                        }
                    }
                    else if (hediffComp is LC_HediffComp_FakeInvisibility)
                    {
                        if (!((LC_HediffComp_FakeInvisibility)hediffComp).PsychologicallyVisible)
                        {
                            //Log.Warning("deep dark invisibility");
                            __result = true;
                            return false;
                        }
                    }
                }
            }

            __result = false;
            return false;
        }
    }
}