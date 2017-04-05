// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：蔡少发
// 创建时间：2015-05-27
// 描述信息：公共字典
// --------------------------------------------------

using System.Collections.Generic;
using Pharos.Utility;
using Pharos.Sys.Entity;
using Pharos.Sys.BLL;

namespace Pharos.Sys
{
    /// <summary>
    /// 公共字典（该类仅限于读取）
    /// </summary>
    public class SysCommonDic
    {
        SysDataDictionaryBLL _dic = new SysDataDictionaryBLL();

        /// <summary>
        /// 根据该类别下的字典项
        /// </summary>
        /// <param name="psn"></param>
        /// <returns></returns>
        public IList<SysDataDictionary> GetDicList(int psn)
        {
            return _dic.GetDicListByPSN(psn);
        }
    }
}
