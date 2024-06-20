using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace LCAnomalyLibrary.Util
{
    /// <summary>
    /// 贴图工具
    /// </summary>
    public static class GraphicUtil
    {
        private static List<Graphic> CachedTopGraphic = new List<Graphic>();

        /// <summary>
        /// 获取逆卡巴拉计数器的图集
        /// </summary>
        /// <returns>图集</returns>
        public static List<Graphic> QliphothIndicator_GetCachedTopGraphic()
        {
            if(CachedTopGraphic.Empty())
            {
                for (int i = 0; i < 6; i++)
                {
                    Log.Message("贴图缓存：Things/Building/QliphothIndicator/Top" + i);

                    CachedTopGraphic.Add(GraphicDatabase.Get<Graphic_Single>("Things/Building/QliphothIndicator/Top" + i,
                        ShaderDatabase.Transparent, Defs.ThingDefOf.QliphothIndicator.graphicData.drawSize, Color.white));
                }
            }

            return CachedTopGraphic;
        }
    }
}
