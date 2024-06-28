using Verse;

namespace LCAnomalyLibrary.Setting
{
    /// <summary>
    /// 游戏内配置文件
    /// </summary>
    public class Setting_LCAnomalyLibrary : ModSettings
    {
        /// <summary>
        /// 是否启用LC警报系统
        /// </summary>
        public bool If_EnableLCWarning = true;
        /// <summary>
        /// 是否启用LC警报音乐
        /// </summary>
        public bool If_EnableLCWarningMusic = true;

        /// <summary>
        /// 警报点数降低间隔（Tick）
        /// </summary>
        public int PointsOfWarning_FadeDuration = 60;
        /// <summary>
        /// 每次降低的警报点数
        /// </summary>
        public int PointsOfWarning_FadeAmount = 1;

        /// <summary>
        /// 玩家派系死亡提供的警报点数
        /// </summary>
        public int PointsOfWarning_PlayerFactionDie = 4;
        /// <summary>
        /// 友军派系死亡提供的警报点数
        /// </summary>
        public int PointsOfWarning_AllyFactionDie = 3;
        /// <summary>
        /// 中立派系死亡提供的警报点数
        /// </summary>
        public int PointsOfWarning_NeturalFactionDie = 2;



        /// <summary>
        /// 保存相关
        /// </summary>
        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Values.Look<bool>(ref this.If_EnableLCWarning, "If_EnableLCWarning", default, false);
            Scribe_Values.Look<bool>(ref this.If_EnableLCWarningMusic, "If_EnableLCWarningMusic", default, false);

            Scribe_Values.Look<int>(ref this.PointsOfWarning_FadeDuration, "PointsOfWarning_FadeDuration", default, false);
            Scribe_Values.Look<int>(ref this.PointsOfWarning_FadeAmount, "PointsOfWarning_FadeAmount", default, false);

            Scribe_Values.Look<int>(ref this.PointsOfWarning_PlayerFactionDie, "PointsOfWarning_PlayerFactionDie", default, false);
            Scribe_Values.Look<int>(ref this.PointsOfWarning_AllyFactionDie, "PointsOfWarning_AllyFactionDie", default, false);
            Scribe_Values.Look<int>(ref this.PointsOfWarning_NeturalFactionDie, "PointsOfWarning_NeturalFactionDie", default, false);

        }
    }
}
