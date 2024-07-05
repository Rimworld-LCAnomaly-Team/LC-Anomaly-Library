using HarmonyLib;
using LCAnomalyLibrary.Setting;
using LCAnomalyLibrary.Util;
using RimWorld;
using System.Reflection;
using Verse.AI;
using Verse;
using LCAnomalyLibrary.Comp;

namespace LCAnomalyLibrary.Patch
{
    /// <summary>
    /// 关于ConpStudyUnlocks的补丁（为了插入改名方法）
    /// </summary>
    [HarmonyPatch(typeof(CompStudyUnlocks), "RegisterStudyLevel")]
    public class Patch_CompStudyUnlocks
    {
        /// <summary>
        /// Postfix
        /// </summary>
        private static void Postfix(CompStudyUnlocks __instance, Pawn studier, int i)
        {
            //Log.Warning("CompStudyUnlocks.RegisterStudyLevel 注入成功");

            var lcComp = __instance as LC_CompStudyUnlocks;
            lcComp?.UnlockNameCheck();
        }
    }
}
