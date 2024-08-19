using LCAnomalyLibrary.Util;
using System.Collections.Generic;
using Verse;

namespace LCAnomalyLibrary.Comp.Pawns
{
    /// <summary>
    /// LC员工基本属性Comp
    /// </summary>
    public class CompPawnStatus : ThingComp
    {
        /// <summary>
        /// Properties
        /// </summary>
        public CompProperties_PawnStatus Props => (CompProperties_PawnStatus)props;

        protected PawnStatus status_Fortitude;
        protected PawnStatus status_Prudence;
        protected PawnStatus status_Temperance;
        protected PawnStatus status_Justice;

        protected bool Inited = false;

        public override void Initialize(CompProperties props)
        {
            base.Initialize(props);
            StatusInit();
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            StatusInit();
        }

        protected void StatusInit(bool force = false)
        {
            //未初始化或强制初始化，就执行初始化
            if (!Inited || force)
            {
                status_Fortitude.Status = Props.initialRange_Fortitude.RandomInRange;
                status_Prudence.Status = Props.initialRange_Prudence.RandomInRange;
                status_Temperance.Status = Props.initialRange_Temperance.RandomInRange;
                status_Justice.Status = Props.initialRange_Justice.RandomInRange;

                LogUtil.Message($"{((Pawn)parent).Name}'s LC_PawnStatus inited.");
                Inited = true;
            }
        }

        /// <summary>
        /// 综合等级
        /// </summary>
        public EPawnLevel SingleLevel
        {
            get
            {
                int sum = 0;
                int result = 0;
                EPawnLevel level;

                //获取总等级
                sum = (int)GetPawnStatusELevel(EPawnStatus.Fortitude) 
                    + (int)GetPawnStatusELevel(EPawnStatus.Temperance) 
                    + (int)GetPawnStatusELevel(EPawnStatus.Prudence) 
                    + (int)GetPawnStatusELevel(EPawnStatus.Justice);

                //计算平均等级
                result = sum / 3;
                if (result > 6)
                    result = 6;
                level = (EPawnLevel)result;

                //预防出现0或者负值
                if (result <= 0)
                    return EPawnLevel.I;
                else
                {
                    //V和EX属性只能在总和大于16的时候出现
                    if(level == EPawnLevel.V || level == EPawnLevel.EX)
                    {
                        if (sum >= 16)
                            return level;
                        else
                            level = EPawnLevel.IV;
                    }
                }

                return level;
            }
        }

        public ref PawnStatus GetPawnStatusLevel(EPawnStatus statusType)
        {
            if (statusType == EPawnStatus.Fortitude)
                return ref status_Fortitude;
            else if (statusType == EPawnStatus.Prudence)
                return ref status_Prudence;
            else if (statusType == EPawnStatus.Temperance)
                return ref status_Temperance;
            else
                return ref status_Justice;
        }

        public EPawnLevel GetPawnStatusELevel(EPawnStatus statusType)
        {
            if (statusType == EPawnStatus.Fortitude)
                return StudyUtil.CalculatePawnLevel(status_Fortitude.Status);
            else if (statusType == EPawnStatus.Prudence)
                return StudyUtil.CalculatePawnLevel(status_Prudence.Status);
            else if(statusType == EPawnStatus.Temperance)
                return StudyUtil.CalculatePawnLevel(status_Temperance.Status);
            else
                return StudyUtil.CalculatePawnLevel(status_Justice.Status);
        }

        public override void PostExposeData()
        {
            Scribe_Values.Look(ref Inited, "inited");
            Scribe_Deep.Look(ref status_Fortitude, "pawnStatus_Fortitude");
            Scribe_Deep.Look(ref status_Prudence, "pawnStatus_Prudence");
            Scribe_Deep.Look(ref status_Temperance, "pawnStatus_Temperance");
            Scribe_Deep.Look(ref status_Justice, "pawnStatus_Justice");
        }

        public override IEnumerable<Verse.Gizmo> CompGetGizmosExtra()
        {
            if (DebugSettings.ShowDevGizmos)
            {
                yield return new Command_Action
                {
                    defaultLabel = "DEV: Reset To LC Level I",
                    action = delegate
                    {
                        StatusInit(true);
                    }
                };

                //yield return new Command_Action
                //{
                //    defaultLabel = "DEV: Add 10 Points of Fortitude",
                //    action = delegate
                //    {
                //        status_Fortitude.Status += 10;
                //    }
                //};
                //yield return new Command_Action
                //{
                //    defaultLabel = "DEV: Add 10 Points of Prudence",
                //    action = delegate
                //    {
                //        status_Prudence.Status += 10;
                //    }
                //};
                //yield return new Command_Action
                //{
                //    defaultLabel = "DEV: Add 10 Points of Temperance",
                //    action = delegate
                //    {
                //        status_Temperance.Status += 10;
                //    }
                //};
                //yield return new Command_Action
                //{
                //    defaultLabel = "DEV: Add 10 Points of Justice",
                //    action = delegate
                //    {
                //        status_Justice.Status += 10;
                //    }
                //};
            }
        }
    }

    /// <summary>
    /// LC员工基本属性类型
    /// </summary>
    public enum EPawnStatus
    {
        /// <summary>
        /// 勇气
        /// </summary>
        Fortitude = 0,
        /// <summary>
        /// 谨慎
        /// </summary>
        Prudence,
        /// <summary>
        /// 自律
        /// </summary>
        Temperance,
        /// <summary>
        /// 正义
        /// </summary>
        Justice
    }

    /// <summary>
    /// LC员工基本属性等级
    /// </summary>
    public enum EPawnLevel
    {
        /// <summary>
        /// 1级
        /// </summary>
        I = 1,
        /// <summary>
        /// 2级
        /// </summary>
        II,
        /// <summary>
        /// 3级
        /// </summary>
        III,
        /// <summary>
        /// 4级
        /// </summary>
        IV,
        /// <summary>
        /// 5级
        /// </summary>
        V,
        /// <summary>
        /// 6级
        /// </summary>
        EX
    }

    public struct PawnStatus : IExposable
    {
        private static readonly int maxLevel = 100;

        public int Status
        {
            get => status;
            set
            {
                if(value >= 0 && value <= maxLevel)
                    status = value;
            }
        }
        private int status;

        public float Exp
        {
            get => exp;
            set
            {
                if (value >= 1f)
                {
                    if (Status < maxLevel)
                    {
                        exp = 0;
                        Status++;
                    }
                }
                else if(value >= 0)
                    exp = value;
            }
        }
        private float exp;

        public PawnStatus(int status, int exp = 0)
        {
            Status = status;
            Exp = exp;
        }

        public void ExposeData()
        {
            Scribe_Values.Look(ref status, "status", 1);
            Scribe_Values.Look(ref exp, "exp", 0);
        }
    }
}
