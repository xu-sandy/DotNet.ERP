﻿// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：蔡少发
// 创建时间：2015-05-28
// 描述信息：数据字典-业务逻辑层
// --------------------------------------------------

using System.Data;
using System.Collections.Generic;
using Pharos.DBFramework;
using Pharos.Logic.Entity;
using Pharos.Sys.Entity;

namespace Pharos.Logic.BLL
{
    /// <summary>
    /// 数据字典-业务逻辑层
    /// </summary>
    public class SysDataDictionaryBLL
    {
        DAL.SysDataDictionaryDAL dal = new DAL.SysDataDictionaryDAL();

        /// <summary>
        /// 根据该父级类别下的子字典项
        /// </summary>
        /// <param name="psn">父级编号</param>
        public IList<SysDataDictionary> GetDicListByPSN(int psn, bool filter = false)
        {
            DataTable dt = dal.GetDicListByPSN(psn,filter);

            return DBHelper.ToEntity.ToList<SysDataDictionary>(dt);
        }

        public List<Entity.IndentOrder> GetIndentOrderInfo(string id, out int count)
        {
            DataTable dt = dal.GetIndentOrderInfo(id);

            count = System.Convert.ToInt32(dt.Rows[0]["TotalCount"]);

            return DBHelper.ToEntity.ToList<Entity.IndentOrder>(dt);
        }
    }
}
