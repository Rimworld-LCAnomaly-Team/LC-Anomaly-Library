using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace LCAnomalyLibrary.Util
{
    /// <summary>
    /// 图像工具类
    /// </summary>
    public static class GraphicUtil
    {
        #region 逆卡巴拉计数器

        /// <summary>
        /// 缓存的逆卡巴拉计数器的图集
        /// </summary>
        private static List<Graphic> CachedTopGraphic = new List<Graphic>();

        /// <summary>
        /// 获取逆卡巴拉计数器的图集
        /// </summary>
        /// <returns>图集</returns>
        public static List<Graphic> QliphothIndicator_GetCachedTopGraphic()
        {
            if (CachedTopGraphic.Empty())
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

        #endregion
    }
}