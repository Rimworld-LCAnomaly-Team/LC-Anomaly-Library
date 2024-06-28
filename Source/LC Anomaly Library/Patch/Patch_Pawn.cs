using HarmonyLib;
using RimWorld;
using Verse;
using LCAnomalyLibrary.Util;
using LCAnomalyLibrary.Setting;

namespace LCAnomalyLibrary.Patch
{
    /// <summary>
    /// 关于CompHoldingPlatformTarget的补丁（为了提供生物特征可传递的特性）
    /// </summary>
    [HarmonyPatch(typeof(Pawn), nameof(Pawn.Kill))]
    public class Patch_Pawn
    {
        /// <summary>
        /// Prefix方法
        /// </summary>
        /// <param name="__instance">原来的反射对象</param>
        /// <param name="dinfo">伤害信息</param>
        /// <param name="exactCulprit">?</param>
        /// <returns>true 返回原方法</returns>
        private static bool Prefix(Pawn __instance, DamageInfo? dinfo, Hediff exactCulprit = null)
        {
            //Log.Warning("Patch_Pawn.Kill 注入成功");

            //如果未启用警报系统，就返回原方法
            if (!Setting_LCAnomalyLibrary_Main.Settings.If_EnableLCWarning)
                return true;

            //非人类和野人死亡不提供点数
            if(__instance.NonHumanlikeOrWildMan())
            {
                Log.Message("警报点数：非人类和野人的死亡不提供点数");
                return true;
            }

            //如果是玩家派系死亡
            if (__instance.Faction.IsPlayer)
            {
                if (Components.LC != null)
                {
                    Components.LC.CurWarningPoints += Setting_LCAnomalyLibrary_Main.Settings.PointsOfWarning_PlayerFactionDie;
                    Log.Message($"警报点数：玩家派系人类死亡，点数+{Setting_LCAnomalyLibrary_Main.Settings.PointsOfWarning_PlayerFactionDie}");
                }

                else
                {
                    Log.Error("GameComponent_LC is null");
                }

                return true;
            }

            //如果是盟友和中立派系死亡
            if (!__instance.Faction.HostileTo(Faction.OfPlayer))
            {
                if(__instance.Faction.PlayerRelationKind == FactionRelationKind.Ally)
                {
                    Components.LC.CurWarningPoints += Setting_LCAnomalyLibrary_Main.Settings.PointsOfWarning_AllyFactionDie;
                    Log.Message($"警报点数：盟友派系人类死亡，点数+{Setting_LCAnomalyLibrary_Main.Settings.PointsOfWarning_AllyFactionDie}");
                }
                else
                {
                    Components.LC.CurWarningPoints += Setting_LCAnomalyLibrary_Main.Settings.PointsOfWarning_NeturalFactionDie;
                    Log.Message($"警报点数：中立派系人类死亡，点数+{Setting_LCAnomalyLibrary_Main.Settings.PointsOfWarning_NeturalFactionDie}");
                }
            }

            return true;
        }
    }
}
