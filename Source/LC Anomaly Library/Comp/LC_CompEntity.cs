using LCAnomalyLibrary.Defs;
using LCAnomalyLibrary.Util;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using UnityEngine;
using Verse;

namespace LCAnomalyLibrary.Comp
{
    /// <summary>
    /// LC基础实体Comp
    /// </summary>
    public abstract class LC_CompEntity : ThingComp
    {
        #region 变量

        /// <summary>
        /// CompProperties
        /// </summary>
        public LC_CompProperties_Entity Props => (LC_CompProperties_Entity)props;

        /// <summary>
        /// 生物特征
        /// </summary>
        public int biosignature;

        /// <summary>
        /// 当前已被提取的EGO武器数量
        /// </summary>
        protected int curAmountWeapon;
        /// <summary>
        /// 当前已被提取的EGO护甲数量
        /// </summary>
        protected int curAmountArmor;

        /// <summary>
        /// XML引用：逆卡巴拉计数器最大值
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
                if (qliphothCountCurrent == value)
                    return;

                //小于0强制归零，大于最大值时若当前值已经异常就强制归最大，其他情况正常变化
                if (value <= 0)
                {
                    qliphothCountCurrent = 0;
                    Log.Message($"逆卡巴拉计数：{SelfPawn.def.defName} 的逆卡巴拉计数器变化，变为：0");
                    QliphothMeltdown();
                }
                else if (value > Props.qliphothCountMax)
                {
                    qliphothCountCurrent = Props.qliphothCountMax;
                }
                else
                {
                    qliphothCountCurrent = value;
                    Log.Message($"逆卡巴拉计数：{SelfPawn.def.defName} 的逆卡巴拉计数器变化，变为：{QliphothCountCurrent}");
                }
            }
        }
        private int qliphothCountCurrent;

        /// <summary>
        /// 生物特征名
        /// </summary>
        public string BiosignatureName => biosignatureName ?? (biosignatureName = AnomalyUtility.GetBiosignatureName(biosignature));
        private string biosignatureName;

        /// <summary>
        /// Comp被挂载的Pawn
        /// </summary>
        protected Pawn SelfPawn => (Pawn)parent;

        #endregion 变量

        #region 触发事件

        /// <summary>
        /// 逃脱收容后执行的操作
        /// </summary>
        public abstract void Notify_Escaped();

        /// <summary>
        /// 被研究后执行的操作
        /// </summary>
        public abstract void Notify_Studied(Pawn studier);

        /// <summary>
        /// 绑到收容平台上的操作
        /// </summary>
        public virtual void Notify_Holded()
        {
            //重置逆卡巴拉计数器
            QliphothCountCurrent = Props.qliphothCountMax;
        }

        /// <summary>
        /// 研究质量：非差
        /// </summary>
        /// <param name="studier">研究者</param>
        /// <param name="result">研究质量</param>
        protected virtual void StudyEvent_NotBad(Pawn studier, LC_StudyResult result)
        {
            switch (result)
            {
                case LC_StudyResult.Good:
                    QliphothCountCurrent++;
                    break;

                case LC_StudyResult.Normal:
                    break;
            }
            StudyUtil.DoStudyResultEffect(studier, SelfPawn, result);
        }

        /// <summary>
        /// 研究质量：差
        /// </summary>
        /// <param name="studier">研究者</param>
        protected virtual void StudyEvent_Bad(Pawn studier)
        {
            QliphothCountCurrent--;

            StudyUtil.DoStudyResultEffect(studier, SelfPawn, LC_StudyResult.Bad);
        }

        /// <summary>
        /// 逆卡巴拉熔毁事件
        /// </summary>
        protected virtual void QliphothMeltdown()
        {
            Log.Message($"收容：{SelfPawn.def.defName} 的收容单元发生了熔毁");

            CompHoldingPlatformTarget comp = SelfPawn.TryGetComp<CompHoldingPlatformTarget>();
            if (comp != null)
            {
                Log.Message($"收容：{SelfPawn.def.defName} 因收容单元熔毁而出逃");
                comp.Escape(initiator: true);
            }
        }

        #endregion 触发事件

        #region 生命周期

        /// <summary>
        /// 我不知道这是什么，好像和数据保存有关
        /// </summary>
        public override void PostExposeData()
        {
            Scribe_Values.Look(ref biosignature, "biosignature", 0);
            Scribe_Values.Look(ref qliphothCountCurrent, "qliphothCountCurrent", defaultValue: QliphothCountMax);
            Scribe_Values.Look(ref curAmountWeapon, "curAmountWeapon", 0);
            Scribe_Values.Look(ref curAmountArmor, "curAmountArmor", 0);
        }

        #endregion 生命周期

        #region 工具功能

        /// <summary>
        /// 检查饰品是否冲突
        /// </summary>
        /// <param name="studier">研究者</param>
        /// <param name="hediffDef">饰品hediffDef</param>
        /// <param name="tag">部位标签</param>
        /// <returns>不冲突为true，反之false</returns>
        protected virtual bool CheckIfAccessoryConflict(Pawn studier, HediffDef hediffDef, string tag)
        {
            //没有相关hediff就不冲突，可添加
            var hediffs = studier.health.hediffSet.hediffs;
            List<Hediff> hediffs1 = new List<Hediff>();
            foreach (var hediff in hediffs)
            {
                if ((hediff.def.tags != null) && hediff.def.tags.Contains(tag))
                    hediffs1.Add(hediff);
            }
            if (hediffs1.NullOrEmpty())
            {
                Log.Message("没有检测相同部位的hediff");
                return true;
            }

            //如果有相同的hediff则不进行添加操作，否则清理重复部位的hediff
            foreach (var hediff in hediffs1)
            {
                if (hediff.def == hediffDef)
                {
                    Log.Message("检测到相同Hediff");
                    return false;
                }
                else
                    studier.health.RemoveHediff(hediff);
            }
            return true;
        }

        /// <summary>
        /// 检查研究是否成功
        /// </summary>
        /// <param name="studier">研究者</param>
        /// <returns></returns>
        protected virtual bool CheckIfStudySuccess(Pawn studier)
        {
            if (CheckStudierSkillRequire(studier))
            {
                StudyEvent_NotBad(studier, CheckFinalStudyQuality(studier));
                return true;
            }
            else
            {
                StudyEvent_Bad(studier);
                return false;
            }
        }

        /// <summary>
        /// 计算研究质量
        /// </summary>
        /// <param name="studier">研究者</param>
        /// <returns>研究质量</returns>
        protected abstract LC_StudyResult CheckFinalStudyQuality(Pawn studier);

        /// <summary>
        /// 检查研究者技能是否符合最低要求
        /// </summary>
        /// <param name="studier">研究者</param>
        /// <returns></returns>
        protected abstract bool CheckStudierSkillRequire(Pawn studier);

        /// <summary>
        /// 检查生成Pebox
        /// </summary>
        /// <param name="studier">研究者</param>
        /// <param name="result">工作质量</param>
        protected virtual void CheckSpawnPeBox(Pawn studier, LC_StudyResult result)
        {
            if (!Defs.ResearchProjectDefOf.ExtractEnkephalin.IsFinished)
            {
                Log.Warning($"工作：未完成研究项目：{Defs.ResearchProjectDefOf.ExtractEnkephalin.label.Translate()}，无法生成PeBox");
                return;
            }

            if (studier != null)
            {
                int amount = 0;
                switch (result)
                {
                    case LC_StudyResult.Good:
                        amount = Props.amountPeBoxStudyGood;
                        break;

                    case LC_StudyResult.Normal:
                        amount = Props.amountPeBoxStudyNormal;
                        break;

                    case LC_StudyResult.Bad:
                        amount = Props.amountPeBoxStudyBad;
                        break;
                }
                if (amount <= 0) return;

                if (Props.peBoxDef != null)
                {
                    Thing thing = ThingMaker.MakeThing(Props.peBoxDef);
                    thing.stackCount = amount;
                    GenSpawn.Spawn(thing, studier.Position, studier.Map);
                    Log.Message($"工作：{SelfPawn.def.defName}生成了{amount}单位的{Props.peBoxDef.defName}");
                }
            }
        }

        /// <summary>
        /// 检查是否给予饰品
        /// </summary>
        /// <param name="studier">研究者</param>
        /// <param name="hediffDef">饰品的hediffdef</param>
        /// <param name="tag">饰品部位tag</param>
        protected virtual void CheckGiveAccessory(Pawn studier, HediffDef hediffDef, string tag)
        {
            //概率排前面是为了减少计算量，避免下面的foreach每次都要触发
            if (!Rand.Chance(Props.accessoryChance))
            {
                Log.Message($"工作：{studier.Name} 获取饰品失败，概率判定失败");
                return;
            }

            if (CheckIfAccessoryConflict(studier, hediffDef, tag))
            {
                var bodypart = studier.RaceProps.body.corePart;
                if (bodypart != null)
                {
                    studier.health.AddHediff(hediffDef, bodypart);
                    Log.Message($"工作：{studier.Name} 获取饰品成功");
                }
                else
                {
                    Log.Message($"工作：{studier.Name} 获取饰品失败，RaceProps.body.corePart 不存在");
                }
            }
            else
            {
                Log.Message($"工作：{studier.Name} 获取饰品失败，已经拥有相同饰品");
            }
        }

        /// <summary>
        /// Debug：调试用逆卡巴拉强制熔毁
        /// </summary>
        protected void ForceQliphothMeltdown()
        {
            QliphothCountCurrent = 0;
        }

        /// <summary>
        /// EGO提取的PeBox消耗
        /// </summary>
        /// <returns></returns>
        protected virtual bool ConsumeExtractEGO(int amount)
        {
            List<Thing> stackList = parent.MapHeld.listerThings.AllThings.Where(x => x.def == Props.peBoxDef).ToList();
            int sum = stackList.Sum(x => x.stackCount);

            //如果地图上没有PeBox，就不能提取
            if (stackList.NullOrEmpty())
            {
                Log.Warning($"提取EGO：地图上找不到{Props.peBoxDef.label.Translate()}");
                return false;
            }
            else
            {
                //如果地图上PeBox的总数量不足，就不能提取
                if (sum < amount)
                {
                    Log.Warning($"提取EGO：地图上{Props.peBoxDef.label.Translate()}总数量不足，需要{amount}个，发现{sum}个在{stackList.Count}个堆中");
                    return false;
                }
                //如果地图上PeBox的总数量足够，就进行进一步的数量判断
                else if (sum > amount)
                {
                    foreach(var stack in stackList)
                    {
                        //如果当前堆数量大于等于需要的数量，直接减去
                        if (stack.stackCount >= amount)
                        {
                            stack.stackCount -= amount;

                            Log.Warning($"提取EGO：地图上{Props.peBoxDef.label.Translate()}的堆数量足够，数量操作完成");
                            break;
                        }
                        //如果当前堆数量小于需要的数量，减去后继续对下一个堆进行操作
                        else
                        {
                            amount -= stack.stackCount;
                            stack.Destroy();

                            Log.Warning($"提取EGO：地图上{Props.peBoxDef.label.Translate()}的单个堆数量不足，移除该堆后仍然需要{amount}个");
                        }
                    }
                }
                //如果地图上PeBox的总数量刚好，直接全部移除
                else
                {
                    stackList.ForEach(x => x.Destroy());
                }
            }

            return true;
        }

        /// <summary>
        /// EGO提取检查
        /// </summary>
        /// <param name="type">EGO类型</param>
        /// <returns>EGO物品对象</returns>
        protected virtual bool CheckIfExtractEGO(EGO_TYPE type)
        {
            if(type == EGO_TYPE.Weapon)
            {
                if (curAmountWeapon < Props.amountMaxWeapon)
                {
                    Log.Message($"提取EGO：{Props.weaponExtracted.label.Translate()}武器当前提取数量状态为：{curAmountWeapon}/{Props.amountMaxWeapon}");

                    return ConsumeExtractEGO(Props.weaponExtractedNeed);
                }
                else
                {
                    Log.Message($"提取EGO：{Props.weaponExtracted.label.Translate()}武器无法提取，因为已经达到提取上限");
                }
            }

            if(type == EGO_TYPE.Armor)
            {
                if(curAmountArmor < Props.amountMaxArmor)
                {
                    Log.Message($"提取EGO：{Props.weaponExtracted.label.Translate()}装备当前提取数量状态为：{curAmountArmor}/{Props.amountMaxArmor}");
                    
                    return ConsumeExtractEGO(Props.armorExtractedNeed);
                }
                else
                {
                    Log.Message($"提取EGO：{Props.armorExtracted.label.Translate()}护甲无法提取，因为已经达到提取上限");
                }
            }

            return false;
        }

        /// <summary>
        /// 提取EGO
        /// </summary>
        /// <param name="type">EGO类型</param>
        protected virtual void ExtractEGO(EGO_TYPE type)
        {
            if (CheckIfExtractEGO(type))
            {
                ThingDef ego;

                switch(type)
                {
                    case EGO_TYPE.Weapon:
                        ego = Props.weaponExtracted;
                        curAmountWeapon++;
                        break;

                    case EGO_TYPE.Armor:
                        ego = Props.armorExtracted;
                        curAmountArmor++;
                        break;
                    default:
                        return;
                }   

                Thing thing = ThingMaker.MakeThing(ego);
                thing.stackCount = 1;
                GenPlace.TryPlaceThing(thing, SelfPawn.PositionHeld, SelfPawn.MapHeld, ThingPlaceMode.Near);
            }
        }

        /// <summary>
        /// EGO科技是否研究完成
        /// </summary>
        /// <returns></returns>
        protected virtual bool CheckIfEGOTechFinished()
        {
            return false;
        }

        #endregion 工具功能

        #region UI

        /// <summary>
        /// Gizmos
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<Verse.Gizmo> CompGetGizmosExtra()
        {
            foreach (Verse.Gizmo gizmo in base.CompGetGizmosExtra())
            {
                yield return gizmo;
            }

            //TODO 需要加一个判断是否可以提取的条件
            if (true)
            {
                if (CheckIfEGOTechFinished())
                {
                    yield return new Command_Action
                    {
                        defaultLabel = "Extract EGO Weapon",
                        defaultDesc = $"提取数量：{curAmountWeapon}/{Props.amountMaxWeapon}",
                        action = delegate
                        {
                            ExtractEGO(EGO_TYPE.Weapon);
                        }
                    };

                    yield return new Command_Action
                    {
                        defaultLabel = "Extract EGO Armor",
                        defaultDesc = $"提取数量：{curAmountArmor}/{Props.amountMaxArmor}",
                        action = delegate
                        {
                            ExtractEGO(EGO_TYPE.Armor);
                        }
                    };
                }
            }

            if (DebugSettings.ShowDevGizmos)
            {
                yield return new Command_Action
                {
                    defaultLabel = "kill",
                    action = delegate
                    {
                        SelfPawn.Kill(null);
                    }
                };

                yield return new Command_Action
                {
                    defaultLabel = "Force Meltdown",
                    action = delegate
                    {
                        Log.Warning($"Dev：{SelfPawn.def.defName} 的收容单元发生了强制熔毁");
                        ForceQliphothMeltdown();
                    }
                };

                yield return new Command_Action
                {
                    defaultLabel = "QliphothCount +1",
                    action = delegate
                    {
                        Log.Warning($"Dev：{SelfPawn.def.defName} 的逆卡巴拉计数器上升了1点");
                        QliphothCountCurrent++;
                    }
                };

                yield return new Command_Action
                {
                    defaultLabel = "QliphothCount -1",
                    action = delegate
                    {
                        Log.Warning($"Dev：{SelfPawn.def.defName} 的逆卡巴拉计数器下降了1点");
                        QliphothCountCurrent--;
                    }
                };
            }
        }

        #endregion UI
    }
}