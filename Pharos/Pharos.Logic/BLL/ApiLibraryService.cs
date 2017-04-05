﻿using System;
using System.Linq;
using System.Collections.Generic;
using Pharos.Utility.Helpers;
using Pharos.Utility;
using Pharos.Logic.Entity;
namespace Pharos.Logic.BLL
{
    public class ApiLibraryService : BaseService<Entity.ApiLibrary>
    {
        public static IEnumerable<dynamic> ApiLibraryPageList(short? apiType, string keyword, short? state, out int recordCount)
        {

            var query = from x in CurrentRepository.QueryEntity
                        where x.CompanyId == CommonService.CompanyId
                        select new
                        {
                            ApiTypeTitle = (from y in SysDataDictService.CurrentRepository.QueryEntity
                                            where y.DicPSN == 10 && y.DicSN == x.ApiType && y.CompanyId == x.CompanyId
                                            select y.Title).FirstOrDefault(),
                            x.Id,
                            x.ApiType,
                            x.ApiCode,
                            x.Title,
                            x.ApiUrl,
                            x.State,
                            x.ApiOrder
                        };

            if (apiType.HasValue)
                query = query.Where(o => o.ApiType == apiType.Value);
            if (state.HasValue)
                query = query.Where(o => o.State == state.Value);
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                int code = 0;
                int.TryParse(keyword, out code);
                query = query.Where(o => o.Title.Contains(keyword) || o.ApiCode == code);
            }
            recordCount = query.Count();
            var list = query.ToPageList();
            return list;
        }

        public static OpResult SaveOrUpdate(Entity.ApiLibrary obj)
        {
            var op = new OpResult();
            if (obj.Id == 0)
            {
                obj.CompanyId = CommonService.CompanyId;
                obj.ApiCode = CurrentRepository.QueryEntity.Max(o => (int?)o.ApiCode).GetValueOrDefault() + 1;
                obj.ApiOrder = CurrentRepository.QueryEntity.Max(o => (int?)o.ApiOrder).GetValueOrDefault() + 1;
                op = Add(obj);
            }
            else
            {
                var source = FindById(obj.Id);
                obj.ToCopyProperty(source, new List<string>() { "ApiCode", "CompanyId", "ApiPwd" });
                if (!obj.ApiPwd.IsNullOrEmpty()) source.ApiPwd = obj.ApiPwd;
                op = Update(source);
            }
            if (op.Successed)
            {
                var stores = string.Join(",", WarehouseService.GetList().Select(o => o.StoreId));
                Pharos.Infrastructure.Data.Redis.RedisManager.Publish("SyncDatabase", new Pharos.ObjectModels.DTOs.DatabaseChanged() { CompanyId = obj.CompanyId, StoreId = stores, Target = "ApiLibrary" });
            }
            return op;
        }
        public static OpResult MoveItem(int mode, int id)
        {
            var op = OpResult.Fail("顺序移动失败!");
            var obj = FindById(id);
            switch (mode)
            {
                case 2://下移
                    var next = CurrentRepository.QueryEntity.Where(o => o.ApiOrder > obj.ApiOrder).OrderBy(o => o.ApiOrder).FirstOrDefault();
                    if (next != null)
                    {
                        var sort = obj.ApiOrder;
                        obj.ApiOrder = next.ApiOrder;
                        next.ApiOrder = sort;
                        op = Update(obj);
                    }
                    break;
                default:
                    var prev = CurrentRepository.QueryEntity.Where(o => o.ApiOrder < obj.ApiOrder).OrderByDescending(o => o.ApiOrder).FirstOrDefault();
                    if (prev != null)
                    {
                        var sort = obj.ApiOrder;
                        obj.ApiOrder = prev.ApiOrder;
                        prev.ApiOrder = sort;
                        op = Update(obj);
                    }
                    break;
            }
            return op;
        }
        /// <summary>
        /// 获取所有启用的支付方式
        /// </summary>
        /// <returns></returns>
        public static List<ApiLibrary> GetAllApiLibrary()
        {
            return CurrentRepository.QueryEntity.Where(o => o.State == 1 && o.CompanyId == CommonService.CompanyId).ToList();
        }
    }
}
