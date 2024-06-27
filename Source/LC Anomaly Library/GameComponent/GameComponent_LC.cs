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
            //警报点数大于0时，才能降低点数
            if (CurWarningPoints > 0)
            {
                //每60Tick（1秒）降低1点警报点数
                waringPointsCounter++;
                if (waringPointsCounter >= 60)
                {
                    CurWarningPoints--;
                    waringPointsCounter = 0;
                }
                //计时器值不得小于0
                else if (waringPointsCounter < 0)
                {
                    waringPointsCounter = 0;
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
