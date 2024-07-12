using LCAnomalyLibrary.GameComponent;
using LCAnomalyLibrary.Util;
using System;
using System.Collections.Generic;
using Verse;

namespace LCAnomalyLibrary.Comp
{
    /// <summary>
    /// LC可产PeBox的基类Comp
    /// </summary>
    public abstract class LC_CompPeBoxProduce : ThingComp
    {
        /// <summary>
        /// CompProperties
        /// </summary>
        public LC_CompProperties_PeBoxProduce Props => (LC_CompProperties_PeBoxProduce)props;

        protected GameComponent_LC component => Components.LC;

        /// <summary>
        /// 当前独立PeBox数量
        /// </summary>
        public int CurAmountIndiPebox
        {
            get
            {
                component.TryGetAnomalyStatusSaved(parent.def, out AnomalyStatusSaved saved);
                return saved.IndiPeBoxAmount;
            }
            set
            {
                component.TryGetAnomalyStatusSaved(parent.def, out AnomalyStatusSaved saved);
                saved.IndiPeBoxAmount = value;
                component.AnomalyStatusSavedDict[parent.def] = saved;

                OnContentsChanged?.Invoke();
            }
        }

        /// <summary>
        /// 检查生成Pebox
        /// </summary>
        /// <param name="studier">研究者</param>
        /// <param name="result">工作质量</param>
        public virtual void CheckSpawnPeBox(Pawn studier, LC_StudyResult result)
        {
            if (studier != null)
            {
                int amount = 0;
                switch (result)
                {
                    case LC_StudyResult.Good:
                        amount = Props.amountStudyGood;
                        break;

                    case LC_StudyResult.Normal:
                        amount = Props.amountStudyNormal;
                        break;

                    case LC_StudyResult.Bad:
                        amount = Props.amountStudyBad;
                        break;
                }
                if (amount <= 0) return;

                if (Props.peBoxDef != null)
                {
                    CurAmountIndiPebox += amount;
                    Log.Message($"工作：{parent.def.label.Translate()}生成了{amount}单位的独立PeBox");

                    Thing thing = ThingMaker.MakeThing(Props.peBoxDef);
                    thing.stackCount = amount;
                    GenSpawn.Spawn(thing, studier.Position, studier.Map);
                    Log.Message($"工作：{parent.def.label.Translate()}生成了{amount}单位的打包PeBox");
                }
            }
        }

        /// <summary>
        /// 值发生变化时调用
        /// </summary>
        public event Action OnContentsChanged;

        public override IEnumerable<Verse.Gizmo> CompGetGizmosExtra()
        {
            foreach (var gizmo in base.CompGetGizmosExtra())
            {
                yield return gizmo;
            }

            if (DebugSettings.ShowDevGizmos)
            {
                yield return new Command_Action
                {
                    defaultLabel = "IndiPebox + 20",
                    action = delegate
                    {
                        Log.Warning($"Dev：{parent.def.label.Translate()} 的独立PeBox增加了20点");
                        CurAmountIndiPebox += 20;
                    }
                };

                yield return new Command_Action
                {
                    defaultLabel = "IndiPebox - 20",
                    action = delegate
                    {
                        Log.Warning($"Dev：{parent.def.label.Translate()} 的独立PeBox减少了20点");
                        CurAmountIndiPebox -= 20;
                    }
                };
            }
        }
    }
}