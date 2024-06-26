using LCAnomalyLibrary.Util;
using Verse;

namespace LCAnomalyLibrary.Comp
{
    /// <summary>
    /// LC可产PeBox的Comp
    /// </summary>
    public class LC_CompPeBoxProduce : ThingComp
    {
        /// <summary>
        /// CompProperties
        /// </summary>
        public LC_CompProperties_PeBoxProduce Props => (LC_CompProperties_PeBoxProduce)props;

        /// <summary>
        /// 检查生成Pebox
        /// </summary>
        /// <param name="studier">研究者</param>
        /// <param name="result">工作质量</param>
        public virtual void CheckSpawnPeBox(Pawn studier, LC_StudyResult result)
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
                    Thing thing = ThingMaker.MakeThing(Props.peBoxDef);
                    thing.stackCount = amount;
                    GenSpawn.Spawn(thing, studier.Position, studier.Map);
                    Log.Message($"工作：{parent.def.label.Translate()}生成了{amount}单位的{Props.peBoxDef.label.Translate()}");
                }
            }
        }
    }
}
