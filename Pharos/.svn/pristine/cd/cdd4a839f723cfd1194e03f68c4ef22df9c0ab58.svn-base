// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：蔡少发
// 创建时间：2015-05-27
// 描述信息：公共字典
// --------------------------------------------------

using System.Collections.Generic;
using Pharos.Logic.BLL;
using Pharos.Logic.Entity;
using Pharos.Utility;
using Pharos.Sys.Entity;

namespace Pharos.Logic
{
    /// <summary>
    /// 公共字典（该类仅限于读取）
    /// </summary>
    public class CommonDic
    {
        BLL.SysDataDictionaryBLL dic = new BLL.SysDataDictionaryBLL();

        /// <summary>
        /// 根据该类别下的字典项
        /// </summary>
        /// <returns></returns>
        public IList<SysDataDictionary> GetDicList(DicType type)
        {
            return this.GetDicList((int)type);
        }

        /// <summary>
        /// 根据该类别下的字典项
        /// </summary>
        /// <param name="psn"></param>
        /// <returns></returns>
        public IList<SysDataDictionary> GetDicList(int psn)
        {
            //return DataDictionaryService.GetDictionaryList(psn);

            return dic.GetDicListByPSN(psn);
        }
    }
}
