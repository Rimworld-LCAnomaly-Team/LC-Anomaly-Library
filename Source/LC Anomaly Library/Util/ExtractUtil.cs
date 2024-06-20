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

        private static string[] sAnomalyLevel = ["ZAYIN", "TETH", "HE", "WAW", "ALEPH"];

        private static List<ThingDef_LCAnomalyBase> anomalyDefList_Ritual_ZAYIN = [];
        private static List<ThingDef_LCAnomalyBase> anomalyDefList_Ritual_TETH = [];
        private static List<ThingDef_LCAnomalyBase> anomalyDefList_Ritual_HE = [];
        private static List<ThingDef_LCAnomalyBase> anomalyDefList_Ritual_WAW = [];
        private static List<ThingDef_LCAnomalyBase> anomalyDefList_Ritual_ALEPH = [];

        private static List<ThingDef_LCAnomalyBase> anomalyDefList_Cogito_ZAYIN = [];
        private static List<ThingDef_LCAnomalyBase> anomalyDefList_Cogito_TETH = [];
        private static List<ThingDef_LCAnomalyBase> anomalyDefList_Cogito_HE = [];
        private static List<ThingDef_LCAnomalyBase> anomalyDefList_Cogito_WAW = [];
        private static List<ThingDef_LCAnomalyBase> anomalyDefList_Cogito_ALEPH = [];

        private static List<PawnKindDef_LCAnomalyBase> anomalyDefList_SevenSin_ZAYIN = [];
        private static List<PawnKindDef_LCAnomalyBase> anomalyDefList_SevenSin_TETH = [];
        private static List<PawnKindDef_LCAnomalyBase> anomalyDefList_SevenSin_HE = [];
        private static List<PawnKindDef_LCAnomalyBase> anomalyDefList_SevenSin_WAW = [];
        private static List<PawnKindDef_LCAnomalyBase> anomalyDefList_SevenSin_ALEPH = [];

        /// <summary>
        /// 字典：异想体等级string->所有指定等级异想体的列表（仅限仪式召唤）
        /// </summary>
        private static Dictionary<string, List<ThingDef_LCAnomalyBase>> anomlayLvl2DefList_Ritual;

        /// <summary>
        /// 字典：异想体等级string->所有指定等级异想体的列表（仅限Cogito注射）
        /// </summary>
        private static Dictionary<string, List<ThingDef_LCAnomalyBase>> anomlayLvl2DefList_Cogito;

        /// <summary>
        /// 字典：异想体等级string->所有指定等级异想体的列表（仅限大罪生物）
        /// </summary>
        private static Dictionary<string, List<PawnKindDef_LCAnomalyBase>> anomlayLvl2DefList_SevenSin;

        public static Dictionary<string, List<ThingDef_LCAnomalyBase>> Get_AnomlayLvl2DefDict_Ritual()
        {
            CheckHasInitial();
            return anomlayLvl2DefList_Ritual;
        }

        public static List<ThingDef_LCAnomalyBase> Get_AnomlayLvl2DefList_Ritual(string level)
        {
            if (CheckIfLevelLegal(level))
            {
                CheckHasInitial();
                return anomlayLvl2DefList_Ritual[level];
            }

            return null;
        }

        public static Dictionary<string, List<ThingDef_LCAnomalyBase>> Get_AnomlayLvl2DefDict_Cogito()
        {
            CheckHasInitial();
            return anomlayLvl2DefList_Cogito;
        }

        public static List<ThingDef_LCAnomalyBase> Get_AnomlayLvl2DefList_Cogito(string level)
        {
            if (CheckIfLevelLegal(level))
            {
                CheckHasInitial();
                return anomlayLvl2DefList_Cogito[level];
            }

            return null;
        }

        public static Dictionary<string, List<PawnKindDef_LCAnomalyBase>> Get_AnomlayLvl2DefDict_SevenSin()
        {
            CheckHasInitial();
            return anomlayLvl2DefList_SevenSin;
        }

        public static List<PawnKindDef_LCAnomalyBase> Get_AnomlayLvl2DefList_SevenSin(string level)
        {
            if (CheckIfLevelLegal(level))
            {
                CheckHasInitial();
                return anomlayLvl2DefList_SevenSin[level];
            }

            return null;
        }

        private static bool CheckIfLevelLegal(string s)
        {
            foreach (var level in sAnomalyLevel)
            {
                if (s == level)
                    return true;
            }

            return false;
        }

        private static void CheckHasInitial()
        {
            if (hasInited) return;

            Init();
        }

        private static void Init()
        {
            anomlayLvl2DefList_Ritual = new Dictionary<string, List<ThingDef_LCAnomalyBase>>
            {
                {sAnomalyLevel[0],anomalyDefList_Ritual_ZAYIN},
                {sAnomalyLevel[1],anomalyDefList_Ritual_TETH},
                {sAnomalyLevel[2],anomalyDefList_Ritual_HE},
                {sAnomalyLevel[3],anomalyDefList_Ritual_WAW},
                {sAnomalyLevel[4],anomalyDefList_Ritual_ALEPH}
            };

            anomlayLvl2DefList_Cogito = new Dictionary<string, List<ThingDef_LCAnomalyBase>>
            {
                {sAnomalyLevel[0],anomalyDefList_Cogito_ZAYIN},
                {sAnomalyLevel[1],anomalyDefList_Cogito_TETH},
                {sAnomalyLevel[2],anomalyDefList_Cogito_HE},
                {sAnomalyLevel[3],anomalyDefList_Cogito_WAW},
                {sAnomalyLevel[4],anomalyDefList_Cogito_ALEPH}
            };

            anomlayLvl2DefList_SevenSin = new Dictionary<string, List<PawnKindDef_LCAnomalyBase>>
            {
                {sAnomalyLevel[0],anomalyDefList_SevenSin_ZAYIN},
                {sAnomalyLevel[1],anomalyDefList_SevenSin_TETH},
                {sAnomalyLevel[2],anomalyDefList_SevenSin_HE},
                {sAnomalyLevel[3],anomalyDefList_SevenSin_WAW},
                {sAnomalyLevel[4],anomalyDefList_SevenSin_ALEPH}
            };

            foreach (var level in sAnomalyLevel)
            {
                //Ritual字典初始化
                foreach (var def in DefDatabase<ThingDef_AnomalyEgg>.AllDefsListForReading)
                {
                    if (def.anomalyLevelTag == level)
                    {
                        anomlayLvl2DefList_Ritual[level].Add(def);
                    }
                }
                foreach (var def in DefDatabase<ThingDef_AnomalyTool>.AllDefsListForReading)
                {
                    if (def.anomalyLevelTag == level)
                    {
                        anomlayLvl2DefList_Ritual[level].Add(def);
                    }
                }

                //Cogito字典初始化
                foreach (var def in DefDatabase<ThingDef_AnomalyEntity_Spawn>.AllDefsListForReading)
                {
                    if (def.anomalyLevelTag == level)
                    {
                        anomlayLvl2DefList_Cogito[level].Add(def);
                    }
                }

                //SevenSin字典初始化
                foreach (var def in DefDatabase<PawnKindDef_AnomalyEntity_SevenSin>.AllDefsListForReading)
                {
                    if (def.anomalyLevelTag == level)
                    {
                        anomlayLvl2DefList_SevenSin[level].Add(def);
                    }
                }
            }

            hasInited = true;
        }
    }
}