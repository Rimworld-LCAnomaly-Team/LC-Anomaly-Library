using HarmonyLib;
using RimWorld;
using Verse;
using LCAnomalyLibrary.Util;
using LCAnomalyLibrary.Setting;

namespace LCAnomalyLibrary.Patch
{
    /// <summary>
    /// 关于Pawn的补丁（为了提供死亡警报点数）
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
        private static void Postfix(Pawn __instance, DamageInfo? dinfo, Hediff exactCulprit = null)
        {
            //Log.Warning("Patch_Pawn.Kill 注入成功");

            //LC组件null直接返回原方法
            if (Components.LC == null)
            {
                Log.Error("警报点数（死亡）：GameComponent_LC is null");
                return;
            }

            //Pawn不能为null
            if (__instance == null)
            {
                Log.Error("警报点数（死亡）：Pawn is null");
                return;
            }

            //如果未启用警报机制，就返回原方法
            if (!Setting_LCAnomalyLibrary_Main.Settings.If_EnableLCWarning)
            {
                Log.Warning("警报点数（死亡）：未启用警报机制");
                return;
            }


            //如果未启用死亡警报机制，就返回原方法
            if (!Setting_LCAnomalyLibrary_Main.Settings.If_EnableLCWarningDeath)
            {
                Log.Warning("警报点数（死亡）：未启用死亡警报机制");
                return;
            }


            //非人类不提供点数
            if(!__instance.RaceProps.Humanlike)
            {
                Log.Message("警报点数（死亡）：非人类不提供点数");
                return;
            }

            //如果是无派系人就和中立派系死亡相同
            if (__instance.Faction == null)
            {
                Log.Message($"警报点数（死亡）：无派系人类，点数 +{Setting_LCAnomalyLibrary_Main.Settings.PointsOfWarning_NeturalFactionDie}");
                Components.LC.CurWarningPoints += Setting_LCAnomalyLibrary_Main.Settings.PointsOfWarning_NeturalFactionDie;
                return;
            }

            //如果是玩家派系死亡
            if (__instance.Faction.IsPlayer)
            {
                Log.Message($"警报点数（死亡）：玩家派系人类，点数 +{Setting_LCAnomalyLibrary_Main.Settings.PointsOfWarning_PlayerFactionDie}");
                Components.LC.CurWarningPoints += Setting_LCAnomalyLibrary_Main.Settings.PointsOfWarning_PlayerFactionDie;
                return;
            }

            //如果是盟友和中立派系死亡
            if (!__instance.Faction.HostileTo(Faction.OfPlayer))
            {
                if(__instance.Faction.PlayerRelationKind == FactionRelationKind.Ally)
                {
                    Log.Message($"警报点数（死亡）：盟友派系人类，点数 +{Setting_LCAnomalyLibrary_Main.Settings.PointsOfWarning_AllyFactionDie}");
                    Components.LC.CurWarningPoints += Setting_LCAnomalyLibrary_Main.Settings.PointsOfWarning_AllyFactionDie;
                }
                else
                {
                    Log.Message($"警报点数（死亡）：中立派系人类，点数 +{Setting_LCAnomalyLibrary_Main.Settings.PointsOfWarning_NeturalFactionDie}");
                    Components.LC.CurWarningPoints += Setting_LCAnomalyLibrary_Main.Settings.PointsOfWarning_NeturalFactionDie;
                }
            }
        }
    }
}
