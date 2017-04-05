﻿using Pharos.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Pharos.Utility.Helpers;
using Pharos.ObjectModels.DTOs;
namespace Pharos.Logic.BLL
{
    public class WarehouseService : BaseService<Warehouse>
    {
        public static List<Warehouse> GetList(bool isAll = false)
        {
            if (isAll)
            {
                var all = FindList(o => o.CompanyId == CommonService.CompanyId);
                all.ForEach(a => { if (a.State == 0) a.Title = "*" + a.Title; });
                return all.OrderByDescending(a => a.State).ThenBy(a => a.Title).ToList();
            }
            return FindList(o => o.State == 1 && o.CompanyId == CommonService.CompanyId).OrderBy(o => o.Title).ToList();
        }
        public static List<Warehouse> GetAdminList(bool isAll = false)
        {
            return FindList(o => o.AdminState == 1 && o.CompanyId == CommonService.CompanyId).OrderBy(o => o.Title).ToList();
        }

        public static List<Warehouse> GetAdminList(int cid,string sid)
        {
            return FindList(o => o.AdminState == 1 && o.CompanyId == cid&o.StoreId==sid).OrderBy(o => o.Title).ToList();
        }

        /// <summary>
        /// 用于datagrid列表
        /// </summary>
        /// <param name="nvl">传递条件</param>
        /// <param name="recordCount">返回总行数</param>
        /// <returns>list</returns>
        public static object FindStoragePageList(NameValueCollection nvl, out int recordCount)
        {
            var query = CurrentRepository.QueryEntity.Where(o => o.CompanyId == CommonService.CompanyId);
            var state = nvl["State"].IsNullOrEmpty() ? -1 : short.Parse(nvl["State"]);
            if (state != -1)
                query = query.Where(o => o.State == state);
            recordCount = query.Count();
            var list = query.ToList();
            var parentTypes = ProductCategoryService.GetParentTypes();
            var ls = list.Select(o => new
            {
                o.Id,
                o.State,
                o.Title,
                o.StoreId,
                o.CategorySN,
                CategoryTitle = GetCategoryTitle(parentTypes, o.CategorySN),
                StateTitle = o.State == 0 ? "停业" : "经营",
                AdminTitle = o.AdminState == 0 ? "关闭" : "开放"
            }).ToList();
            return ls;
        }
        public static string MaxSn
        {
            get
            {
                var dal = new Pharos.Logic.DAL.StoreDAL();
                return dal.MaxSn(CommonService.CompanyId).ToString();
            }
        }
        static string GetCategoryTitle(List<ProductCategory> categorys, string sn)
        {
            if (sn.IsNullOrEmpty()) return "";
            var sns = sn.Split(',').Where(o => !o.IsNullOrEmpty()).Select(o => int.Parse(o));
            var titles = categorys.Where(o => sns.Contains(o.CategorySN)).Select(o => o.Title);
            return string.Join(",", titles);
        }
        public static IEnumerable<InventoryResult> CheckedPrice(string storeId, int token, IEnumerable<int> categorySns, decimal from, decimal to)
        {
            try
            {
                string categorySnsStr = null;
                if (categorySns != null)
                    categorySnsStr = string.Join(",", categorySns.Select(o => o.ToString()));
                var result = CurrentRepository._context.Database.SqlQuery<InventoryResult>("exec CheckedPrice @p0,@p1,@p2,@p3,@p4", categorySnsStr, storeId, from, to, token).ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static IEnumerable<Warehouse> GetStores()
        {
            return CurrentRepository.Entities.Where(o => o.State == 1).ToList();
        }
    }

}
