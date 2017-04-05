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
    public class ProductPublishVerService
    {
        [Ninject.Inject]
        IBaseRepository<Entity.ProductPublishVer> ProductPublishVerRepository { get; set; }
        [Ninject.Inject]
        IBaseRepository<Entity.ProductVer> ProductVerRepository { get; set; }
        [Ninject.Inject]
        IBaseRepository<Entity.ProductPublishSql> ProductPublishSqlRepository { get; set; }
        [Ninject.Inject]
        IBaseRepository<Entity.ProductMenuLimit> ProductMenuLimitRepository { get; set; }
        [Ninject.Inject]
        IBaseRepository<Entity.CompanyAuthorize> CompanyAuthorizeRepository { get; set; }
        [Ninject.Inject]
        IBaseRepository<Entity.ProductUpdateLog> ProductUpdateLogRepository { get; set; }
        [Ninject.Inject]
        IBaseRepository<Entity.SysUser> UserRepository { get; set; }
        [Ninject.Inject]
        IProductModuleVerRepository ProductModuleVerRepository { get; set; }
        [Ninject.Inject]
        IProductRoleVerRepository ProductRoleVerRepository { get; set; }
        [Ninject.Inject]
        IProductDictionaryVerRepository ProductDictionaryVerRepository { get; set; }
        [Ninject.Inject]
        IProductDataVerRepository ProductDataVerRepository { get; set; }
        public IEnumerable<dynamic> GetPageList(System.Collections.Specialized.NameValueCollection nvl, out int recordCount)
        {
            var query = ProductPublishVerRepository.GetQuery();
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
                        UpdateCount = x.ProductUpdateLogs.Count(o => o.Status == 0),
                        ModuleVer= x.ProductModuleVer==null?0:x.ProductModuleVer.VerCode,
                        RoleVer = x.ProductRoleVer == null ? 0 : x.ProductRoleVer.VerCode,
                        DictVer = x.ProductDictionaryVer == null ? 0 : x.ProductDictionaryVer.VerCode,
                        DataVer = x.ProductDataVer == null ? 0 : x.ProductDataVer.VerCode,
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
                x.x.PublishId,
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
                x.UpdateCount,
                x.ModuleVer,
                x.RoleVer,
                x.DictVer,
                x.DataVer,
                x.x.CompanyCount,
                x.x.IsRunSql,
                x.x.Forced,
                x.Updater,
                x.Publisher
            });
        }
        public OpResult SaveOrUpdate(Entity.ProductPublishVer obj)
        {
            if(obj.Id==0)
            {
                obj.PublishId = ProductPublishVerRepository.GetMaxInt(o => (int?)o.PublishId);
                obj.CreateDT = DateTime.Now;
                obj.UpdateDT = obj.CreateDT;
                obj.UpdateUID = CurrentUser.UID;
                obj.CreateUID = obj.UpdateUID;
                ProductPublishVerRepository.Add(obj, false);
            }
            else
            {
                var product = ProductPublishVerRepository.Find(o => o.Id == obj.Id);
                obj.ToCopyProperty(product, new List<string>() { "CreateDT", "CreateUID", "PublishId" });
                product.UpdateUID = CurrentUser.UID;
                product.UpdateDT = DateTime.Now;
            }
            ProductPublishVerRepository.SaveChanges();
            return OpResult.Success();
        }
        public OpResult SaveVer(Entity.ProductPublishVer obj)
        {
            if (ProductPublishVerRepository.IsExists(o => o.ProductId == obj.ProductId && o.VerStatus == 0 && o.Id != obj.Id))
                return OpResult.Fail("已存在未发布的版本");
            if (obj.HasModelId == "1")
            {
                if (obj.ModuleId.IsNullOrEmpty())
                {
                    var model = ProductModuleVerRepository.GetOfficialLast(obj.ProductId);
                    if (model == null) return OpResult.Fail("功能升级未发布正式版！");
                    obj.ModuleId = model.ModuleId;
                }
            }
            else
                obj.ModuleId = "";
            if (obj.HasRoleId == "1")
            {
                if (obj.RoleId.IsNullOrEmpty())
                {
                    var model = ProductRoleVerRepository.GetOfficialLast(obj.ProductId);
                    if (model == null) return OpResult.Fail("角色升级未发布正式版！");
                    obj.RoleId = model.RoleVerId;
                }
            }
            else
                obj.RoleId = "";
            if (obj.HasDictId == "1")
            {
                if (obj.DictId.IsNullOrEmpty())
                {
                    var model = ProductDictionaryVerRepository.GetOfficialLast(obj.ProductId);
                    if (model == null) return OpResult.Fail("字典升级未发布正式版！");
                    obj.DictId = model.DictId;
                }
            }
            else
                obj.DictId = "";
            if (obj.HasDataId == "1")
            {
                if (obj.DataId.IsNullOrEmpty())
                {
                    var model = ProductDataVerRepository.GetOfficialLast(obj.ProductId);
                    if (model == null) return OpResult.Fail("初始数据升级未发布正式版！");
                    obj.DataId = model.DataId;
                }
            }
            else
                obj.DataId = "";
            return SaveOrUpdate(obj);
        }
        public OpResult Copy(int verId)
        {
            var pro = Get(verId);
            if (pro!=null && pro.Status != 0)
            {
                //if (ProductVerRepository.IsExists(o => o.ProductCode == pro.ProductCode && o.VerCode > pro.VerCode && o.Status == 0))
                    //return OpResult.Fail("已存在未发布的版本！");
                var obj = new Entity.ProductPublishVer();
                pro.ToCopyProperty(obj);
                obj.Id = 0;
                obj.Status = 0;
                obj.VerStatus = 0;
                obj.VerCode=0;
                obj.PublishDT = null;
                obj.PublishUID = "";
                obj.ProductPublishSqls = pro.ProductPublishSqls.ToClone();
                if (!obj.ModuleId.IsNullOrEmpty())
                {
                    obj.HasModelId = "1";
                    obj.ModuleId = "";
                }
                if (!obj.RoleId.IsNullOrEmpty())
                {
                    obj.HasRoleId = "1";
                    obj.RoleId = "";
                }
                if (!obj.DictId.IsNullOrEmpty())
                {
                    obj.HasDictId = "1";
                    obj.DictId = "";
                }
                if (!obj.DataId.IsNullOrEmpty())
                {
                    obj.HasDataId = "1";
                    obj.DataId = "";
                }
                return SaveVer(obj);
            }
            return OpResult.Fail();
        }
        public OpResult SaveData(Entity.ProductPublishSql obj)
        {
            if(obj.Id==0)
            {
                obj.CreateDT = DateTime.Now;
                obj.CreateUID = CurrentUser.UID;
                obj.RunSort = ProductPublishSqlRepository.GetMaxInt(o => (int?)o.RunSort, whereLambda: o => o.PublishId == obj.PublishId);
                ProductPublishSqlRepository.Add(obj, false);
                
            }
            else
            {
                var menu = ProductPublishSqlRepository.Get(obj.Id);
                if (ProductPublishSqlRepository.IsExists(o => o.MenuId == obj.MenuId && o.PublishId == menu.PublishId && o.Id != obj.Id))
                    return OpResult.Fail("该菜单模块已存在");
                menu.RunSql = obj.RunSql;
            }
            var menuModelId = System.Web.HttpContext.Current.Request["menuModelId"];
            if (obj.PublishId > 0 && !menuModelId.IsNullOrEmpty())
            {
                var ver = Get(obj.PublishId);
                if (ver != null)
                {
                    ver.MenuModuleId = menuModelId;
                    ver.RunSqlWay = System.Web.HttpContext.Current.Request["runSqlWay"].ToType<short>();
                    ver.UpdateDT = DateTime.Now;
                    ver.UpdateUID = CurrentUser.UID;
                }
            }
            ProductPublishSqlRepository.SaveChanges();
            return OpResult.Success();
        }
        public void RemoveData(int id)
        {
            var obj = ProductPublishSqlRepository.Find(o => o.Id == id);
            ProductPublishSqlRepository.Remove(obj);
        }

        public OpResult Deletes(int[] ids)
        {
            var list = ProductPublishVerRepository.GetQuery(o => ids.Contains(o.Id)).Include(o => o.ProductPublishSqls).ToList();
            if (list.Any(o => o.VerStatus > 0))
                return OpResult.Fail("该状态不允许删除");
            ProductPublishSqlRepository.RemoveRange(list.SelectMany(o => o.ProductPublishSqls).ToList(), false);
            ProductPublishVerRepository.RemoveRange(list);
            return OpResult.Success();
        }
        public Entity.ProductPublishVer Get(int verId)
        {
            return ProductPublishVerRepository.GetQuery(o => o.PublishId == verId).Include(o => o.ProductPublishSqls).FirstOrDefault();
        }

        public List<Models.ProductDataSqlModel> DataList(int verId)
        {
            var query = ProductPublishSqlRepository.GetQuery(o => o.PublishId == verId);
            var queryVer = ProductPublishVerRepository.GetQuery(o => o.PublishId == verId);
            var queryMenu= ProductMenuLimitRepository.GetQuery();
            var q = from x in query
                    join y in queryVer on x.PublishId equals y.PublishId
                    orderby x.RunSort
                    select new Models.ProductDataSqlModel(){ 
                        Id= x.Id,
                        MenuId= x.MenuId,
                        RunSort= x.RunSort,
                        RunSql= x.RunSql,
                        Index= x.PublishId,
                        Title = queryMenu.Where(o=>o.ModuleId==y.MenuModuleId && o.MenuId==x.MenuId).Select(o=>o.Title).FirstOrDefault()
                    };

            var ms = q.ToList();
            var menus = new List<Models.ProductDataSqlModel>();
            int i = 0;
            foreach(var m in ms)
            {
                m.DataId = m.Index.ToString();
                m.SqlMore= m.RunSql.TrimMore(200);
                m.Count = ms.Count;
                m.Index=i;
                i++;
                menus.Add(m);
            }
            return menus;
        }
        public void MoveItem(short mode, int sn, int verId)
        {
            var list = ProductPublishSqlRepository.GetQuery(o => o.PublishId == verId).OrderBy(o => o.RunSort).ToList();
            var obj = list.FirstOrDefault(o => o.MenuId == sn);
            switch (mode)
            {
                case 2://下移
                    var obj1 = list.LastOrDefault();
                    if (obj.Id != obj1.Id)
                    {
                        Entity.ProductPublishSql next = null;
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
                            ProductPublishSqlRepository.SaveChanges();
                        }
                    }
                    break;
                default://上移
                     var obj2 = list.FirstOrDefault();
                    if (obj.Id != obj2.Id)
                    {
                        Entity.ProductPublishSql prev = null;
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
                            ProductPublishSqlRepository.SaveChanges();
                        }
                    }
                    break;
            }
        }

        public OpResult Publish(int verId, short state, bool forced)
        {
            var obj = Get(verId);
            if (obj != null)
            {
                obj.VerStatus = state;
                var list = ProductPublishVerRepository.GetQuery(o => o.ProductId == obj.ProductId && o.PublishId != obj.PublishId).ToList();
                list.Where(o => o.VerStatus == obj.VerStatus).Each(o => o.Status = 2);
                if (state == 1)//测试
                {
                    obj.PublishDT = DateTime.Now;
                    obj.PublishUID = CurrentUser.UID;
                    obj.Status = 1;
                    obj.Forced = forced;
                    var source = list.OrderByDescending(o => o.VerCode).FirstOrDefault(o => o.VerCode > 0);
                    if (source == null)
                        obj.VerCode = 1;
                    else
                        obj.VerCode = source.VerCode + 0.1m;
                }
                obj.CompanyCount = CompanyAuthorizeRepository.GetQuery(o=>o.OpenVersionId==obj.ProductId && o.Status==1).Count();
                ProductPublishSqlRepository.SaveChanges();
                return OpResult.Success();
            }
            return OpResult.Fail();
        }
        public Entity.ProductPublishSql SeeData(int id)
        {
            return ProductPublishSqlRepository.Find(o => o.Id == id);
        }

        public List<Entity.ProductVer> GetProductVers()
        {
            var queryProduct = ProductVerRepository.GetQuery();
            var queryModel = ProductPublishVerRepository.GetQuery();
            var query = from x in queryProduct
                        where !queryModel.Any(o => o.ProductId == x.ProductId)
                            && x.Status == 1
                        select x;
            return query.ToList();
        }

        public List<Entity.ProductVer> GetProductVerEnables()
        {
            var queryProduct = ProductVerRepository.GetQuery();
            var queryModel = ProductPublishVerRepository.GetQuery();
            var query = from x in queryProduct
                        where queryModel.Any(o => o.ProductId == x.ProductId && o.VerStatus==2 && o.Status==1)
                            && x.Status == 1
                        select x;
            return query.ToList();
        }
        public string GetHasPublish(int cid, int code)
        {
            var query = GetPublishData(cid, code);
            var pv = query.FirstOrDefault();
            if (pv != null) return pv.Forced?"1":"2";
            return "0";
        }
        public Entity.ProductPublishVer UpdatePublish(int cid, int code,string creater)
        {
            var query = GetPublishData(cid, code);
            var pv = query.Include(o=>o.ProductModuleVer.ProductMenuLimits).Include(o=>o.ProductRoleVer.ProductRoles)
                .Include("ProductRoleVer.ProductRoles.ProductRoleDatas")
                .Include(o => o.ProductDictionaryVer.ProductDictionaryDatas)
                .Include(o => o.ProductDataVer.ProductDataSqls)
                .Include(o=>o.ProductPublishSqls)
                .FirstOrDefault();
            if (pv != null)
            {
                //ProductUpdateLogRepository.GetQuery(o => o.CID == cid && o.PublishId < pv.PublishId).ToList().Each(o =>
                //{
                //    o.Status = 0;
                //});
                AddUpdateLog(pv.PublishId, cid, true, "", creater);
            }
            return pv;
        }

        public void AddUpdateLog(int publishId, int cid, bool success, string descript, string creater)
        {
            ProductUpdateLogRepository.Add(new Entity.ProductUpdateLog() { PublishId = publishId, Status =Convert.ToInt16(success?0:1), Description = descript, CID = cid, CreateDT = DateTime.Now,CreateUID="", Creater = creater });
        }

        private IQueryable<Entity.ProductPublishVer> GetPublishData(int cid, int code)
        {
            var queryPublish = ProductPublishVerRepository.GetQuery(o => o.ProductId == code && o.Status == 1 && o.VerStatus > 0);
            var queryLog = ProductUpdateLogRepository.GetQuery(o => o.CID == cid && o.Status == 0);
            var query = from x in queryPublish
                        where x.PublishId==queryPublish.Max(o=>(int?)o.PublishId) && !queryLog.Any(o => o.PublishId == x.PublishId)
                        select x;
            return query;
        }
    }
}
