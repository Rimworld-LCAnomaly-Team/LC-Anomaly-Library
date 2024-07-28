﻿using Verse;

namespace LCAnomalyLibrary.Util
{
    /// <summary>
    /// 类型转换工具
    /// </summary>
    public static class Types
    {
        /// <summary>
        /// 检查是否是LC实体
        /// </summary>
        /// <param name="thing">实体</param>
        /// <returns>是则返回true，反之false</returns>
        public static bool CheckIfLCEntity(Thing thing)
        {
            return thing.def.entityCodexEntry is Defs.EntityCodexEntryDef;
        }
    }
}
