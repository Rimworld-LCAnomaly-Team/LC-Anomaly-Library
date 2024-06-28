using LCAnomalyLibrary.Setting;
using Verse;

namespace LCAnomalyLibrary.GameComponent
{
    /// <summary>
    /// LC游戏组件
    /// </summary>
    public class GameComponent_LC: Verse.GameComponent
    {
        /// <summary>
        /// 当前的警报点数
        /// </summary>
        public int CurWarningPoints
        {
            get => curWarningPoints;
            set
            {
                //如果未启用警报系统，就不处理
                if (!Setting_LCAnomalyLibrary_Main.Settings.If_EnableLCWarning) return;

                if (value == curWarningPoints) return;
                if (value < 0) return;
                if (value > 100) return;

                curWarningPoints = value;
                Log.Message($"警报点数：警报点数更新，当前警报点数为：{curWarningPoints}");

                Util.MusicUtils.PlayMusic_LC(curWarningPoints);
            }
        }
        private int curWarningPoints;

        private int waringPointsCounter;


        public GameComponent_LC(Game game)
        {

        }

        /// <summary>
        /// Tick
        /// </summary>
        public override void GameComponentTick()
        {
            //如果启用警报系统，就自动更新警报点数
            if (Setting_LCAnomalyLibrary_Main.Settings.If_EnableLCWarning)
            {
                //警报点数大于0时，才能降低点数
                if (CurWarningPoints > 0)
                {
                    //每xTick降低y点警报点数
                    waringPointsCounter++;
                    if (waringPointsCounter >= Setting_LCAnomalyLibrary_Main.Settings.PointsOfWarning_FadeDuration)
                    {
                        CurWarningPoints -= Setting_LCAnomalyLibrary_Main.Settings.PointsOfWarning_FadeAmount;
                        waringPointsCounter = 0;
                    }
                    //计时器值不得小于0
                    else if (waringPointsCounter < 0)
                    {
                        waringPointsCounter = 0;
                    }
                }
            }
        }

        public override void ExposeData()
        {
            Scribe_Values.Look(ref curWarningPoints, "curWarningPoints", 0);
            Scribe_Values.Look(ref waringPointsCounter, "waringPointsCounter", 0);
        }
    }
}
