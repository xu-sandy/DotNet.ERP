﻿using Pharos.Logic.OMS.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using Pharos.Utility.Helpers;
using Pharos.Utility;

namespace Pharos.Logic.OMS.BLL
{
    public class ProductVerService:BaseService<Entity.ProductVer>
    {
        [Ninject.Inject]
        IBaseRepository<Entity.ProductVer> ProductVerRepository { get; set; }
        [Ninject.Inject]
        IBaseRepository<Entity.ProductModuleVer> ProductModuleVerRepository { get; set; }
        [Ninject.Inject]
        IBaseRepository<Entity.ProductDictionaryVer> ProductDictionaryVerRepository { get; set; }
        [Ninject.Inject]
        IBaseRepository<Entity.ProductDataVer> ProductDataVerRepository { get; set; }
        [Ninject.Inject]
        IBaseRepository<Entity.ProductRoleVer> ProductRoleVerRepository { get; set; }
        [Ninject.Inject]
        IBaseRepository<Entity.SysUser> UserRepository { get; set; }
        public IEnumerable<dynamic> GetPageList(System.Collections.Specialized.NameValueCollection nvl, out int recordCount)
        {
            var query = ProductVerRepository.GetQuery();
            var queryUse = ProductModuleVerRepository.GetQuery().Select(o=> o.ProductId)
                .Union(ProductDictionaryVerRepository.GetQuery().Select(o=>o.ProductId))
                .Union(ProductDataVerRepository.GetQuery().Select(o=>o.ProductId))
                .Union(ProductRoleVerRepository.GetQuery().Select(o=>o.ProductId));
            var queryUser = UserRepository.GetQuery();
            var code = nvl["code"].ToType<int?>();
            var state = nvl["state"];
            var q = from x in query
                    select new
                    {
                        x.Id,
                        x.ProductId,
                        x.Name,
                        x.SysName,
                        x.Alias,
                        x.CreateDT,
                        x.CreateUID,
                        x.Status,
                        HasUse = queryUse.Any(o => o == x.ProductId) ? "√" : "--",
                        StatusTitle = x.Status == 1 ? "已生效" : x.Status == 2 ? "已失效" : "未生效",
                        x.Memo,
                        Creater=queryUser.Where(o => o.UserId == x.CreateUID).Select(o => o.FullName).FirstOrDefault()
                    };
            if (code.HasValue)
                q = q.Where(o => o.ProductId == code);
            if (!state.IsNullOrEmpty())
            {
                var st= state.Split(',').Select(o => short.Parse(o)).ToList();
                q = q.Where(o => st.Contains(o.Status));
            }
            recordCount = q.Count();
            return q.ToPageList();
        }
        public OpResult SaveOrUpdate(Entity.ProductVer obj)
        {
            if (ProductVerRepository.IsExists(o => o.SysName == obj.SysName && o.Id != obj.Id))
                return OpResult.Fail("系统名称已存在!");
            if(obj.Id==0)
            {
                obj.ProductId = ProductVerRepository.GetMaxInt(o => o.ProductId);
                obj.CreateDT = DateTime.Now;
                obj.CreateUID = CurrentUser.UID;
                ProductVerRepository.Add(obj, false);
            }
            else
            {
                var product = ProductVerRepository.Find(o => o.Id == obj.Id);
                product.Name = obj.Name;
                product.SysName = obj.SysName;
                product.Alias = obj.Alias;
                product.Memo = obj.Memo;
                obj = product;
            }
            ProductVerRepository.SaveChanges(obj);
            return OpResult.Success();
        }
        
        public OpResult Deletes(int[] ids)
        {
            var list= ProductVerRepository.GetQuery(o => ids.Contains(o.Id)).ToList();
            if (list.Any(o => o.Status > 0))
                return OpResult.Fail("该状态不允许删除！");
            ProductVerRepository.RemoveRange(list);
            return OpResult.Success();
        }
        public Entity.ProductVer Get(int id)
        {
            return ProductVerRepository.GetQuery(o => o.Id==id).FirstOrDefault();
        }
        public OpResult SetState(string id, short state)
        {
            var ids= id.Split(',').Select(o => int.Parse(o)).ToList();
            var list = ProductVerRepository.GetQuery(o => ids.Contains(o.Id)).ToList();
            list.ForEach(o=>o.Status=state);
            ProductVerRepository.SaveChanges();
            return OpResult.Success();
        }

        public List<Entity.ProductVer> GetList(params short[] status)
        {
            var query = ProductVerRepository.GetQuery();
            if (status != null && status.Any())
                query = query.Where(o => status.Contains(o.Status));
            return query.ToList();
        }
    }
}
