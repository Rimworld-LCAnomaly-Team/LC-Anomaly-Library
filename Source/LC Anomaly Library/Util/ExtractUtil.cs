using LCAnomalyLibrary.Defs;
using System.Collections.Generic;
using Verse;

namespace LCAnomalyLibrary.Util
{
    /// <summary>
    /// 提取仪式相关操作的工具类
    /// </summary>
    public static class ExtractUtil
    {
        /// <summary>
        /// 是否已初始化
        /// </summary>
        private static bool hasInited = false;

        public static string[] sAnomalyLevel = new string[5] { "ZAYIN", "TETH", "HE", "WAW", "ALEPH" };

        private static List<ThingDef_LCAnomalyBase> anomalyDefList_ZAYIN = new List<ThingDef_LCAnomalyBase>();
        private static List<ThingDef_LCAnomalyBase> anomalyDefList_TETH = new List<ThingDef_LCAnomalyBase>();
        private static List<ThingDef_LCAnomalyBase> anomalyDefList_HE = new List<ThingDef_LCAnomalyBase>();
        private static List<ThingDef_LCAnomalyBase> anomalyDefList_WAW = new List<ThingDef_LCAnomalyBase>();
        private static List<ThingDef_LCAnomalyBase> anomalyDefList_ALEPH = new List<ThingDef_LCAnomalyBase>();

        /// <summary>
        /// 字典：异想体等级string->所有指定等级异想体的列表
        /// </summary>
        public static Dictionary<string, List<ThingDef_LCAnomalyBase>> AnomlayLvl2DefList;

        public static bool CheckIfLevelLegal(string s)
        {
            foreach (var level in sAnomalyLevel)
            {
                if(s == level) 
                    return true;
            }

            return false;
        }

        public static void CheckHasInitial()
        {
            if(hasInited) return;

            Init();
        }

        private static void Init()
        {
            AnomlayLvl2DefList = new Dictionary<string, List<ThingDef_LCAnomalyBase>>
            {
                {sAnomalyLevel[0],anomalyDefList_ZAYIN},
                {sAnomalyLevel[1],anomalyDefList_TETH},
                {sAnomalyLevel[2],anomalyDefList_HE},
                {sAnomalyLevel[3],anomalyDefList_WAW},
                {sAnomalyLevel[4],anomalyDefList_ALEPH}
            };

            foreach (var level in sAnomalyLevel)
            {
                foreach (var def in DefDatabase<ThingDef_AnomalyEgg>.AllDefsListForReading)
                {
                    if (def.anomalyLevelTag == level)
                    {
                        AnomlayLvl2DefList[level].Add(def);
                    }
                }
                foreach (var def in DefDatabase<ThingDef_AnomalyTool>.AllDefsListForReading)
                {
                    if (def.anomalyLevelTag == level)
                    {
                        AnomlayLvl2DefList[level].Add(def);
                    }
                }
            }

            hasInited = true;
        }
    }
}
