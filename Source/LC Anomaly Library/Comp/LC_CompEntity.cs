using RimWorld;
using System;
using Verse;

namespace LCAnomalyLibrary.Comp
{
    /// <summary>
    /// LC基础实体抽象Comp
    /// </summary>
    public abstract class LC_CompEntity : ThingComp
    {
        public LC_CompProperties_Entity Props => (LC_CompProperties_Entity)props;

        public const int LongerTracksUnlockIndex = 1;

        public bool everRevealed;

        /// <summary>
        /// 生物特征
        /// </summary>
        public int biosignature;

        /// <summary>
        /// 在攻击时受伤
        /// </summary>
        public bool injuredWhileAttacking;

        /// <summary>
        /// XML输入：逆卡巴拉计数器最大值
        /// </summary>
        public int QliphothCountMax => Props.qliphothCountMax;

        /// <summary>
        /// 逆卡巴拉计数器当前值
        /// </summary>
        public int QliphothCountCurrent
        {
            get => qliphothCountCurrent;
            set
            {
                if (value > QliphothCountMax)
                {
                    qliphothCountCurrent = QliphothCountMax;
                }
                else if(value <= 0)
                {
                    qliphothCountCurrent = 0;
                    QliphothMeltdown();
                }
                else
                {
                    qliphothCountCurrent = value;
                }
            }
        }

        /// <summary>
        /// 逆卡巴拉计数器当前值
        /// </summary>
        private int qliphothCountCurrent;

        /// <summary>
        /// XML输入：基础研究成功率
        /// </summary>
        public float StudySucessRateBase => Props.studySucessRateBase;

        /// <summary>
        /// XML输入：工作时获得饰品的概率
        /// </summary>
        public float AccessoryChance => Props.accessoryChance;

        /// <summary>
        /// XML输入：是否在逃脱收容后提醒
        /// </summary>
        public bool ShoundNotifyWhenEscape => Props.shoundNotifyWhenEscape;

        /// <summary>
        /// 生物特征名
        /// </summary>
        protected string biosignatureName;

        /// <summary>
        /// 生物特征名
        /// </summary>
        public string BiosignatureName => biosignatureName ?? (biosignatureName = AnomalyUtility.GetBiosignatureName(biosignature));

        /// <summary>
        /// Comp被挂载的Pawn
        /// </summary>
        protected Pawn SelfPawn => (Pawn)parent;

        /// <summary>
        /// 被看到的操作
        /// </summary>
        protected abstract void CheckIfSeen();

        /// <summary>
        /// 逃脱收容后执行的操作
        /// </summary>
        public abstract void Escape();

        /// <summary>
        /// 被研究后执行的操作
        /// </summary>
        public abstract void AfterStudy(Pawn studier);

        /// <summary>
        /// 绑到收容平台上的操作
        /// </summary>
        public abstract void AfterHoldToPlatform();

        public override void PostExposeData()
        {
            Scribe_Values.Look(ref everRevealed, "everRevealed", defaultValue: false);
            Scribe_Values.Look(ref biosignature, "biosignature", 0);
            Scribe_Values.Look(ref injuredWhileAttacking, "injuredWhileAttacking", defaultValue: false);
            Scribe_Values.Look(ref qliphothCountCurrent, "qliphothCountCurrent", defaultValue: QliphothCountMax);
        }

        /// <summary>
        /// 检查饰品是否冲突的方法
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public abstract bool CheckIfAccessoryConflict(Pawn studier);

        protected abstract bool CheckIfStudySuccess(Pawn studier);

        protected abstract bool CheckIfFinalStudySuccess(Pawn studier);

        protected abstract void StudyEvent_Failure(Pawn studier);

        protected abstract void StudyEvent_Success(Pawn studier);

        protected abstract void QliphothMeltdown();

        public virtual void ForceQliphothMeltdown()
        {
            QliphothCountCurrent = 0;
        }
    }
}
