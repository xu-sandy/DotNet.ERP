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
    public class ProductDataVerService : BaseService<Entity.ProductDataVer>
    {
        [Ninject.Inject]
        IBaseRepository<Entity.ProductDataVer> ProductDataVerRepository { get; set; }
        [Ninject.Inject]
        IBaseRepository<Entity.ProductVer> ProductVerRepository { get; set; }
        [Ninject.Inject]
        IBaseRepository<Entity.ProductDataSql> ProductDataSqlRepository { get; set; }
        [Ninject.Inject]
        IBaseRepository<Entity.ProductMenuLimit> ProductMenuLimitRepository { get; set; }
        [Ninject.Inject]
        IBaseRepository<Entity.SysUser> UserRepository { get; set; }
        
        public IEnumerable<dynamic> GetPageList(System.Collections.Specialized.NameValueCollection nvl, out int recordCount)
        {
            var query = ProductDataVerRepository.GetQuery();
            var queryProduct = ProductVerRepository.GetQuery();
            var queryUser = UserRepository.GetQuery();
            var productId = nvl["productId"].ToType<int?>();
            var state = nvl["state"];
            var verstate = nvl["verstate"];
            var q = from x in query
                    join y in queryProduct on x.ProductId equals y.ProductId
                    select new
                    {
                        x.Id,
                        x.CreateDT,
                        x.ProductId,
                        x.Status,
                        x.VerStatus,
                        x,
                        ModelCount = x.ProductDataSqls.Count,
                        y.SysName,
                        Updater = queryUser.Where(o => o.UserId == x.UpdateUID).Select(o => o.FullName).FirstOrDefault(),
                        Publisher = queryUser.Where(o => o.UserId == x.PublishUID).Select(o => o.FullName).FirstOrDefault()
                    };
            if (productId.HasValue)
                q = q.Where(o => o.ProductId == productId);
            if (!state.IsNullOrEmpty())
            {
                var st= state.Split(',').Select(o => short.Parse(o)).ToList();
                q = q.Where(o => st.Contains(o.Status));
            }
            if (!verstate.IsNullOrEmpty())
            {
                var st = verstate.Split(',').Select(o => short.Parse(o)).ToList();
                q = q.Where(o => st.Contains(o.VerStatus));
            }
            recordCount = q.Count();
            return q.ToPageList().Select(x=>new{
                x.Id,
                x.x.DataId,
                x.ProductId,
                x.SysName,
                x.x.VerCode,
                x.CreateDT,
                x.x.CreateUID,
                x.Status,
                x.VerStatus,
                x.x.UpdateDT,
                x.x.PublishDT,
                x.x.StatusTitle,
                x.x.VerStatusTitle,
                x.ModelCount,
                x.Updater,
                x.Publisher
            });
        }
        public OpResult SaveOrUpdate(Entity.ProductDataVer obj)
        {
            if(obj.Id==0)
            {
                obj.DataId =CommonService.GUID;
                obj.CreateDT = DateTime.Now;
                obj.UpdateDT = obj.CreateDT;
                obj.UpdateUID = CurrentUser.UID;
                obj.CreateUID = obj.UpdateUID;
                ProductDataVerRepository.Add(obj, false);
            }
            else
            {
                var product = ProductDataVerRepository.Find(o => o.Id == obj.Id);
                product.UpdateUID = CurrentUser.UID;
                product.UpdateDT = DateTime.Now;
            }
            ProductDataVerRepository.SaveChanges();
            return OpResult.Success();
        }
        public OpResult Copy(string verId)
        {
            var pro = Get(verId);
            if (pro!=null && pro.Status != 0)
            {
                //if (ProductVerRepository.IsExists(o => o.ProductCode == pro.ProductCode && o.VerCode > pro.VerCode && o.Status == 0))
                    //return OpResult.Fail("已存在未发布的版本！");
                var obj = new Entity.ProductDataVer();
                pro.ToCopyProperty(obj);
                obj.Id = 0;
                obj.Status = 0;
                obj.VerStatus = 0;
                obj.VerCode=0;
                obj.PublishDT = null;
                obj.PublishUID = "";
                obj.ProductDataSqls = pro.ProductDataSqls.ToClone();
                return SaveOrUpdate(obj);
            }
            return OpResult.Fail();
        }
        public OpResult SaveData(Entity.ProductDataSql obj,int productId)
        {
            if(obj.Id==0)
            {
                if (ProductDataVerRepository.IsExists(o => o.ProductId == productId && o.Status == 0 && o.DataId != obj.DataId))
                    return OpResult.Fail("已存在未发布的版本");
                obj.DataId = obj.DataId ?? CommonService.GUID;
                obj.CreateDT = DateTime.Now;
                obj.CreateUID = CurrentUser.UID;
                obj.RunSort = ProductDataSqlRepository.GetMaxInt(o => (int?)o.RunSort, whereLambda: o => o.DataId == obj.DataId);
                ProductDataSqlRepository.Add(obj, false);
            }
            else
            {
                var menu = ProductDataSqlRepository.Get(obj.Id);
                if (ProductDataSqlRepository.IsExists(o => o.MenuId == obj.MenuId && o.DataId == menu.DataId && o.Id != obj.Id))
                    return OpResult.Fail("该菜单模块已存在");
                menu.RunSql = obj.RunSql;
                obj.DataId = menu.DataId;
            }
            var model = ProductDataVerRepository.Find(o => o.DataId == obj.DataId);
            if (model != null)
            {
                model.UpdateDT = DateTime.Now;
                model.UpdateUID = CurrentUser.UID;
            }
            else
            {
                ProductDataVerRepository.Add(new Entity.ProductDataVer()
                {
                    DataId = obj.DataId,
                    ProductId = productId,
                    ModuleId = System.Web.HttpContext.Current.Request["modelId"],
                    CreateDT = obj.CreateDT,
                    UpdateDT = obj.CreateDT,
                    UpdateUID = obj.CreateUID,
                    CreateUID = obj.CreateUID,
                }, false);
            }
            ProductDataSqlRepository.SaveChanges();
            return OpResult.Success();
        }
        public void RemoveData(int id)
        {
            var obj = ProductDataSqlRepository.Find(o => o.Id == id);
            ProductDataSqlRepository.Remove(obj);
        }

        public OpResult Deletes(int[] ids)
        {
            var list= ProductDataVerRepository.GetQuery(o => ids.Contains(o.Id)).Include(o=>o.ProductDataSqls).ToList();
            if (list.Any(o => o.VerStatus > 0))
                return OpResult.Fail("该状态不允许删除！");
            ProductDataSqlRepository.RemoveRange(list.SelectMany(o => o.ProductDataSqls).ToList(), false);
            ProductDataVerRepository.RemoveRange(list);
            return OpResult.Success();
        }
        public Entity.ProductDataVer Get(string verId)
        {
            return ProductDataVerRepository.GetQuery(o => o.DataId == verId).Include(o => o.ProductDataSqls).FirstOrDefault();
        }

        public List<Models.ProductDataSqlModel> DataList(string verId)
        {
            var query = ProductDataSqlRepository.GetQuery(o => o.DataId == verId);
            var queryVer = ProductDataVerRepository.GetQuery(o => o.DataId == verId);
            var queryMenu= ProductMenuLimitRepository.GetQuery();
            var q = from x in query
                    join y in queryVer on x.DataId equals y.DataId
                    orderby x.RunSort
                    select new Models.ProductDataSqlModel(){ 
                        Id= x.Id,
                        MenuId= x.MenuId,
                        RunSort= x.RunSort,
                        RunSql= x.RunSql,
                        DataId= x.DataId,
                        Title = queryMenu.Where(o=>o.ModuleId==y.ModuleId && o.MenuId==x.MenuId).Select(o=>o.Title).FirstOrDefault()
                    };

            var ms = q.ToList();
            var menus = new List<Models.ProductDataSqlModel>();
            int i = 0;
            foreach(var m in ms)
            {
                m.SqlMore= m.RunSql.TrimMore(200);
                m.Count = ms.Count;
                m.Index=i;
                i++;
                menus.Add(m);
            }
            return menus;
        }
        public void MoveItem(short mode, int sn, string verId)
        {
            var list = ProductDataSqlRepository.GetQuery(o => o.DataId == verId).OrderBy(o => o.RunSort).ToList();
            var obj = list.FirstOrDefault(o => o.MenuId == sn);
            switch (mode)
            {
                case 2://下移
                    var obj1 = list.LastOrDefault();
                    if (obj.Id != obj1.Id)
                    {
                        Entity.ProductDataSql next = null;
                        for (var i = 0; i < list.Count; i++)
                        {
                            if (obj.Id == list[i].Id)
                            {
                                next = list[i + 1]; break;
                            }
                        }
                        if (next != null)
                        {
                            var sort = obj.RunSort;
                            obj.RunSort = next.RunSort;
                            next.RunSort = sort;
                            ProductDataSqlRepository.SaveChanges();
                        }
                    }
                    break;
                default://上移
                     var obj2 = list.FirstOrDefault();
                    if (obj.Id != obj2.Id)
                    {
                        Entity.ProductDataSql prev = null;
                        for (var i=0;i<list.Count;i++)
                        {
                            if (obj.Id == list[i].Id)
                            {
                                prev = list[i - 1]; break;
                            }
                        }
                        if (prev != null)
                        {
                            var sort = obj.RunSort;
                            obj.RunSort = prev.RunSort;
                            prev.RunSort = sort;
                            ProductDataSqlRepository.SaveChanges();
                        }
                    }
                    break;
            }
        }
        
        public OpResult Publish(string verId, short state)
        {
            var obj = Get(verId);
            if (obj != null)
            {
                if (!obj.ProductDataSqls.Any())
                    return OpResult.Fail("请先配置初始数据！");
                obj.VerStatus = state;
                var list = ProductDataVerRepository.GetQuery(o => o.ProductId == obj.ProductId && o.DataId != obj.DataId).ToList();
                list.Where(o => o.VerStatus == obj.VerStatus).Each(o => o.Status = 2);
                if (state == 1)//测试
                {
                    obj.PublishDT = DateTime.Now;
                    obj.PublishUID = CurrentUser.UID;
                    obj.Status = 1;
                    var source = list.OrderByDescending(o => o.VerCode).FirstOrDefault(o => o.VerCode > 0);
                    if (source == null)
                        obj.VerCode = 1;
                    else
                        obj.VerCode = source.VerCode + 0.1m;
                }
                ProductDataSqlRepository.SaveChanges();
                return OpResult.Success();
            }
            return OpResult.Fail();
        }
        public Entity.ProductDataSql SeeData(int id)
        {
            return ProductDataSqlRepository.Find(o => o.Id == id);
        }
        public List<Entity.ProductVer> GetProductVers()
        {
            var queryProduct = ProductVerRepository.GetQuery();
            var queryModel = ProductDataVerRepository.GetQuery();
            var query = from x in queryProduct
                        where !queryModel.Any(o => o.ProductId == x.ProductId)
                            && x.Status == 1
                        select x;
            return query.ToList();
        }
    }
}
