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

        private static List<ThingDef_LCAnomalyBase> anomalyDefList_Ritual_ZAYIN = new List<ThingDef_LCAnomalyBase>();
        private static List<ThingDef_LCAnomalyBase> anomalyDefList_Ritual_TETH = new List<ThingDef_LCAnomalyBase>();
        private static List<ThingDef_LCAnomalyBase> anomalyDefList_Ritual_HE = new List<ThingDef_LCAnomalyBase>();
        private static List<ThingDef_LCAnomalyBase> anomalyDefList_Ritual_WAW = new List<ThingDef_LCAnomalyBase>();
        private static List<ThingDef_LCAnomalyBase> anomalyDefList_Ritual_ALEPH = new List<ThingDef_LCAnomalyBase>();

        private static List<ThingDef_LCAnomalyBase> anomalyDefList_Cogito_ZAYIN = new List<ThingDef_LCAnomalyBase>();
        private static List<ThingDef_LCAnomalyBase> anomalyDefList_Cogito_TETH = new List<ThingDef_LCAnomalyBase>();
        private static List<ThingDef_LCAnomalyBase> anomalyDefList_Cogito_HE = new List<ThingDef_LCAnomalyBase>();
        private static List<ThingDef_LCAnomalyBase> anomalyDefList_Cogito_WAW = new List<ThingDef_LCAnomalyBase>();
        private static List<ThingDef_LCAnomalyBase> anomalyDefList_Cogito_ALEPH = new List<ThingDef_LCAnomalyBase>();

        /// <summary>
        /// 字典：异想体等级string->所有指定等级异想体的列表（仅限仪式召唤）
        /// </summary>
        public static Dictionary<string, List<ThingDef_LCAnomalyBase>> AnomlayLvl2DefList_Ritual;

        /// <summary>
        /// 字典：异想体等级string->所有指定等级异想体的列表（仅限Cogito注射）
        /// </summary>
        public static Dictionary<string, List<ThingDef_LCAnomalyBase>> AnomlayLvl2DefList_Cogito;

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
            AnomlayLvl2DefList_Ritual = new Dictionary<string, List<ThingDef_LCAnomalyBase>>
            {
                {sAnomalyLevel[0],anomalyDefList_Ritual_ZAYIN},
                {sAnomalyLevel[1],anomalyDefList_Ritual_TETH},
                {sAnomalyLevel[2],anomalyDefList_Ritual_HE},
                {sAnomalyLevel[3],anomalyDefList_Ritual_WAW},
                {sAnomalyLevel[4],anomalyDefList_Ritual_ALEPH}
            };

            AnomlayLvl2DefList_Cogito = new Dictionary<string, List<ThingDef_LCAnomalyBase>>
            {
                {sAnomalyLevel[0],anomalyDefList_Cogito_ZAYIN},
                {sAnomalyLevel[1],anomalyDefList_Cogito_TETH},
                {sAnomalyLevel[2],anomalyDefList_Cogito_HE},
                {sAnomalyLevel[3],anomalyDefList_Cogito_WAW},
                {sAnomalyLevel[4],anomalyDefList_Cogito_ALEPH}
            };

            foreach (var level in sAnomalyLevel)
            {
                //Ritual字典初始化
                foreach (var def in DefDatabase<ThingDef_AnomalyEgg>.AllDefsListForReading)
                {
                    if (def.anomalyLevelTag == level)
                    {
                        AnomlayLvl2DefList_Ritual[level].Add(def);
                    }
                }
                foreach (var def in DefDatabase<ThingDef_AnomalyTool>.AllDefsListForReading)
                {
                    if (def.anomalyLevelTag == level)
                    {
                        AnomlayLvl2DefList_Ritual[level].Add(def);
                    }
                }

                //Cogito字典初始化
                foreach(var def in DefDatabase<ThingDef_AnomalyEntity_Spawn>.AllDefsListForReading)
                {
                    if (def.anomalyLevelTag == level)
                    {
                        AnomlayLvl2DefList_Cogito[level].Add(def);
                    }
                }
            }

            hasInited = true;
        }
    }
}
