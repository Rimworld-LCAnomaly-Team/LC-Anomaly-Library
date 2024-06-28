using UnityEngine;
using Verse;

namespace LCAnomalyLibrary.Setting
{
    public class Setting_LCAnomalyLibrary_Main : Mod
    {
        /// <summary>
        /// 设置文件对象
        /// </summary>
        public static Setting_LCAnomalyLibrary Settings;

        private string PointsOfWarning_PlayerFactionDieEditBuffer;
        private string PointsOfWarning_AllyFactionDieEditBuffer;
        private string PointsOfWarning_NeturalFactionDieEditBuffer;
        private string PointsOfWarning_FadeDurationEditBuffer;
        private string PointsOfWarning_FadeAmountEditBuffer;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="content"></param>
        public Setting_LCAnomalyLibrary_Main(ModContentPack content) : base(content)
        {
            Setting_LCAnomalyLibrary_Main.Settings = base.GetSettings<Setting_LCAnomalyLibrary>();
        }

        /// <summary>
        /// mod配置文件列表名称
        /// </summary>
        /// <returns></returns>
        public override string SettingsCategory()
        {
            return Translator.Translate("Setting_LCAnomalyLibrary_Label");
        }

        /// <summary>
        /// 界面绘制
        /// </summary>
        /// <param name="inRect"></param>
        public override void DoSettingsWindowContents(Rect inRect)
        {
            Widgets.Checkbox(0f, 40f, ref Settings.If_EnableLCWarning, 25f, false, false, null, null);
            Widgets.Label(new Rect(35f, 41f, inRect.width - 50f, 25f), Translator.Translate("If_EnableLCWarningText"));

            if (Settings.If_EnableLCWarning)
            {
                Widgets.Checkbox(35f, 81f, ref Settings.If_EnableLCWarningMusic, 25f, false, false, null, null);
                Widgets.Label(new Rect(70f, 82f, inRect.width - 50f, 25f), Translator.Translate("If_EnableLCWarningMusicText"));

                Widgets.TextFieldNumericLabeled(new Rect(0f, 121f, inRect.width * 0.5f, 25f)
                    , Translator.Translate("PointsOfWarning_FadeDurationText")
                    , ref Settings.PointsOfWarning_FadeDuration
                    , ref this.PointsOfWarning_FadeDurationEditBuffer);

                Widgets.TextFieldNumericLabeled(new Rect(0f, 161f, inRect.width * 0.5f, 25f)
                    , Translator.Translate("PointsOfWarning_FadeAmountText")
                    , ref Settings.PointsOfWarning_FadeAmount
                    , ref this.PointsOfWarning_FadeAmountEditBuffer);

                Widgets.TextFieldNumericLabeled(new Rect(0f, 201f, inRect.width * 0.5f, 25f)
                    , Translator.Translate("PointsOfWarning_PlayerFactionDieText")
                    , ref Settings.PointsOfWarning_PlayerFactionDie
                    , ref this.PointsOfWarning_PlayerFactionDieEditBuffer);

                Widgets.TextFieldNumericLabeled(new Rect(0f, 241f, inRect.width * 0.5f, 25f)
                    , Translator.Translate("PointsOfWarning_AllyFactionDieText")
                    , ref Settings.PointsOfWarning_AllyFactionDie
                    , ref this.PointsOfWarning_AllyFactionDieEditBuffer);

                Widgets.TextFieldNumericLabeled(new Rect(0f, 281f, inRect.width * 0.5f, 25f)
                    , Translator.Translate("PointsOfWarning_NeturalFactionDieText")
                    , ref Settings.PointsOfWarning_NeturalFactionDie
                    , ref this.PointsOfWarning_NeturalFactionDieEditBuffer);
            }

            base.DoSettingsWindowContents(inRect);
        }
    }
}
