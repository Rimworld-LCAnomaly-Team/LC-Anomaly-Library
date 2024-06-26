using LCAnomalyLibrary.Defs;
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
        /// 提取所需PeBox的Def
        /// </summary>
        protected LC_CompPeBoxProduce PeBoxComp => parent.GetComp<LC_CompPeBoxProduce>();

        /// <summary>
        /// 当前已被提取的EGO武器数量
        /// </summary>
        protected int curAmountWeapon;
        /// <summary>
        /// 当前已被提取的EGO护甲数量
        /// </summary>
        protected int curAmountArmor;

        #endregion

        #region 工具方法

        /// <summary>
        /// EGO提取的PeBox消耗
        /// </summary>
        /// <returns></returns>
        protected virtual bool ConsumeExtractEGO(int amount)
        {
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
        /// <returns>EGO物品对象</returns>
        protected virtual bool CheckIfExtractEGO(EGO_TYPE type)
        {
            if (type == EGO_TYPE.Weapon)
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

            if (type == EGO_TYPE.Armor)
            {
                if (curAmountArmor < Props.amountMaxArmor)
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

                switch (type)
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
                GenPlace.TryPlaceThing(thing, parent.PositionHeld, parent.MapHeld, ThingPlaceMode.Near);
            }
        }

        /// <summary>
        /// EGO科技是否研究完成
        /// </summary>
        /// <returns></returns>
        protected virtual bool CheckIfEGOTechFinished()
        {
            bool finished = false;

            if(Props.researchProject != null)
            {
                finished = Props.researchProject.IsFinished;
            }
            return finished;
        }

        #endregion

        #region 生命周期

        /// <summary>
        /// 我不知道这是什么，好像和数据保存有关
        /// </summary>
        public override void PostExposeData()
        {
            Scribe_Values.Look(ref curAmountWeapon, "curAmountWeapon", 0);
            Scribe_Values.Look(ref curAmountArmor, "curAmountArmor", 0);
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
        }

        #endregion UI
    }
}
