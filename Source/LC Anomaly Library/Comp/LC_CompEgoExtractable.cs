using LCAnomalyLibrary.Defs;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace LCAnomalyLibrary.Comp
{
    public class LC_CompEgoExtractable : ThingComp
    {
        #region 字段

        /// <summary>
        /// CompProperties
        /// </summary>
        public LC_CompProperties_EgoExtractable Props => (LC_CompProperties_EgoExtractable)props;

        /// <summary>
        /// CompEntity
        /// </summary>
        public LC_CompEntity EntityComp => parent.GetComp<LC_CompEntity>();

        /// <summary>
        /// 提取所需PeBox的Def
        /// </summary>
        protected LC_CompPeBoxProduce PeBoxComp => parent.GetComp<LC_CompPeBoxProduce>();

        /// <summary>
        /// CompHoldingPlatformTarget
        /// </summary>
        protected CompHoldingPlatformTarget HoldingTargetComp => parent.GetComp<CompHoldingPlatformTarget>();

        /// <summary>
        /// 当前已被提取的EGO武器数量
        /// </summary>
        public int CurAmountWeapon;
        /// <summary>
        /// 当前已被提取的EGO护甲数量
        /// </summary>
        public int CurAmountArmor;

        #endregion

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="curAmountWeapon">当前提取武器数量</param>
        /// <param name="curAmountArmor">当前提取护甲数量</param>
        public void TransferEgoExtractAmount(int curAmountWeapon, int curAmountArmor)
        {
            this.CurAmountWeapon = curAmountWeapon;
            this.CurAmountArmor = curAmountArmor;
        }

        #region 工具方法

        /// <summary>
        /// EGO提取的PeBox消耗
        /// </summary>
        /// <returns></returns>
        protected virtual bool ConsumePebox(EGO_TYPE type)
        {
            int amount = type == EGO_TYPE.Weapon ? Props.weaponExtractedNeed : Props.armorExtractedNeed;

            //如果没有PeBoxComp，就不能提取
            if (PeBoxComp == null)
            {
                Log.Warning($"提取EGO：{parent.def.label.Translate()}没有PeBoxComp，无法提取");
                return false;
            }

            //如果PeBoxComp没有PeBoxDef，就不能提取
            var peBoxDef = PeBoxComp.Props.peBoxDef;
            if( peBoxDef == null )
            {
                Log.Warning($"提取EGO：{PeBoxComp.ToString()}没有PeBoxDef，无法提取");
                return false;
            }

            List<Thing> stackList = parent.MapHeld.listerThings.AllThings.Where(x => x.def == peBoxDef).ToList();
            int sum = stackList.Sum(x => x.stackCount);

            //如果地图上没有PeBox，就不能提取
            if (stackList.NullOrEmpty())
            {
                Log.Warning($"提取EGO：地图上找不到{peBoxDef.label.Translate()}");
                return false;
            }
            else
            {
                //如果地图上PeBox的总数量不足，就不能提取
                if (sum < amount)
                {
                    Log.Warning($"提取EGO：地图上{peBoxDef.label.Translate()}总数量不足，需要{amount}个，发现{sum}个在{stackList.Count}个堆中");
                    return false;
                }
                //如果地图上PeBox的总数量足够，就进行进一步的数量判断
                else if (sum > amount)
                {
                    foreach (var stack in stackList)
                    {
                        //如果当前堆数量大于等于需要的数量，直接减去
                        if (stack.stackCount >= amount)
                        {
                            stack.stackCount -= amount;
                            Log.Warning($"提取EGO：地图上{peBoxDef.label.Translate()}的堆数量足够，数量操作完成");
                            break;
                        }
                        //如果当前堆数量小于需要的数量，减去后继续对下一个堆进行操作
                        else
                        {
                            amount -= stack.stackCount;
                            stack.Destroy();
                            Log.Warning($"提取EGO：地图上{peBoxDef.label.Translate()}的单个堆数量不足，移除该堆后仍然需要{amount}个");
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
        /// <returns></returns>
        protected virtual bool CheckIfExtractEGO(EGO_TYPE type)
        {
            if (!HasReachedMaxExtractAmount(type))
            {
                return ConsumePebox(type);
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

                switch (type)
                {
                    case EGO_TYPE.Weapon:
                        ego = Props.weaponExtracted;
                        CurAmountWeapon++;
                        break;

                    case EGO_TYPE.Armor:
                        ego = Props.armorExtracted;
                        CurAmountArmor++;
                        break;
                    default:
                        return;
                }

                Thing thing = ThingMaker.MakeThing(ego);
                thing.stackCount = 1;
                GenPlace.TryPlaceThing(thing, parent.PositionHeld, parent.MapHeld, ThingPlaceMode.Near);
                Messages.Message("EGOExtractSucceedText".Translate() + thing.def.label.Translate(), MessageTypeDefOf.TaskCompletion, false);
            }
            else
            {
                Messages.Message("EGOExtractFailedByPeBoxConsumeText".Translate(), MessageTypeDefOf.RejectInput, false);
            }
        }

        /// <summary>
        /// EGO提取是否已解锁
        /// </summary>
        /// <returns></returns>
        protected virtual bool CheckIfEGOExtractUnlocked()
        {

            //没有收容和研究组件固定不允许提取
            if (HoldingTargetComp == null || EntityComp.StudyUnlocksComp == null)
                return false;

            //逃离中不允许提取
            if (HoldingTargetComp.isEscaping)
                return false;

            //到达设定的解锁等级后才允许提取
            return EntityComp.StudyUnlocksComp.Progress >= Props.unlockLevel;
        }

        /// <summary>
        /// E.G.O是否已经达到提取最大量
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        protected virtual bool HasReachedMaxExtractAmount(EGO_TYPE type)
        {
            switch (type)
            {
                case EGO_TYPE.Weapon:
                    return CurAmountWeapon >= Props.amountMaxWeapon;
                case EGO_TYPE.Armor:
                    return CurAmountArmor >= Props.amountMaxArmor;
                default:
                    Log.Error("提取EGO:EGO类型不存在");
                    return true;
            }

        }

        #endregion

        #region 生命周期

        /// <summary>
        /// 数据保存
        /// </summary>
        public override void PostExposeData()
        {
            Scribe_Values.Look(ref CurAmountWeapon, "curAmountWeapon", 0);
            Scribe_Values.Look(ref CurAmountArmor, "curAmountArmor", 0);
        }

        #endregion

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

            //在收容平台上才能提取
            if (HoldingTargetComp != null && HoldingTargetComp.HeldPlatform != null)
            {
                //科技研究完成后才能提取
                if (CheckIfEGOExtractUnlocked())
                {
                    //提取EGO武器
                    yield return new Command_Action
                    {
                        icon = ContentFinder<UnityEngine.Texture2D>.Get(Props.weaponIconPath),
                        defaultLabel = "ExtractEGOWeaponCommandText".Translate(),
                        defaultDesc = $"{"ExtractEGOCommandText".Translate()}{CurAmountWeapon}/{Props.amountMaxWeapon}"
                        + $"\n{"ExtractEGONeedPeboxDefCommandText".Translate()}{PeBoxComp.Props.peBoxDef.label.Translate()}"
                        + $"\n{"ExtractEGONeedPeboxAmountCommandText".Translate()}{Props.weaponExtractedNeed}",
                        Disabled = HasReachedMaxExtractAmount(EGO_TYPE.Weapon),
                        disabledReason = "ExtractEGODisabledReasonText".Translate(),
                        action = delegate
                        {
                            ExtractEGO(EGO_TYPE.Weapon);
                        }
                    };

                    //提取EGO装备
                    yield return new Command_Action
                    {
                        icon = ContentFinder<UnityEngine.Texture2D>.Get(Props.armorIconPath),
                        defaultLabel = "ExtractEGOArmorCommandText".Translate(),
                        defaultDesc = $"{"ExtractEGOCommandText".Translate()}{CurAmountArmor}/{Props.amountMaxArmor}"
                        + $"\n{"ExtractEGONeedPeboxDefCommandText".Translate()}{PeBoxComp.Props.peBoxDef.label.Translate()}"
                        + $"\n{"ExtractEGONeedPeboxAmountCommandText".Translate()}{Props.armorExtractedNeed}",
                        Disabled = HasReachedMaxExtractAmount(EGO_TYPE.Armor),
                        disabledReason = "ExtractEGODisabledReasonText".Translate(),
                        action = delegate
                        {
                            ExtractEGO(EGO_TYPE.Armor);
                        }
                    };
                }
            }
        }

        #endregion UI
    }
}
