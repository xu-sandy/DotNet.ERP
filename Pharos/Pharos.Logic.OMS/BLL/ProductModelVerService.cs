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
    public class ProductModelVerService : BaseService<Entity.ProductModuleVer>
    {
        [Ninject.Inject]
        IProductModuleVerRepository ProductModelVerRepository { get; set; }
        [Ninject.Inject]
        IBaseRepository<Entity.ProductVer> ProductVerRepository { get; set; }
        [Ninject.Inject]
        IBaseRepository<Entity.ProductMenuLimit> ProductMenuLimitRepository { get; set; }
        [Ninject.Inject]
        IBaseRepository<Entity.ProductUpdateLog> ProductUpdateLogRepository { get; set; }
        [Ninject.Inject]
        IBaseRepository<Entity.SysUser> UserRepository { get; set; }
        
        public IEnumerable<dynamic> GetPageList(System.Collections.Specialized.NameValueCollection nvl, out int recordCount)
        {
            var query = ProductModelVerRepository.GetQuery();
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
                        MenuCount = x.ProductMenuLimits.Count(o=>o.Type!=3),
                        LimitCount = x.ProductMenuLimits.Count(o => o.Type == 3),
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
                x.x.ModuleId,
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
                x.MenuCount,
                x.LimitCount,
                x.Updater,
                x.Publisher
            });
        }
        public OpResult SaveOrUpdate(Entity.ProductModuleVer obj)
        {
            if(obj.Id==0)
            {
                obj.ModuleId =CommonService.GUID;
                obj.CreateDT = DateTime.Now;
                obj.UpdateDT = obj.CreateDT;
                obj.UpdateUID = CurrentUser.UID;
                obj.CreateUID = obj.UpdateUID;
                ProductModelVerRepository.Add(obj, false);
            }
            else
            {
                var product = ProductModelVerRepository.Find(o => o.Id == obj.Id);
                product.UpdateUID = CurrentUser.UID;
                product.UpdateDT = DateTime.Now;
            }
            ProductModelVerRepository.SaveChanges();
            return OpResult.Success();
        }
        public OpResult Copy(string modelId)
        {
            var pro = Get(modelId);
            if (pro!=null && pro.Status != 0)
            {
                //if (ProductVerRepository.IsExists(o => o.ProductCode == pro.ProductCode && o.VerCode > pro.VerCode && o.Status == 0))
                    //return OpResult.Fail("已存在未发布的版本！");
                var obj = new Entity.ProductModuleVer();
                pro.ToCopyProperty(obj);
                obj.Id = 0;
                obj.Status = 0;
                obj.VerStatus = 0;
                obj.VerCode=0;
                obj.PublishDT = null;
                obj.PublishUID = "";
                obj.ProductMenuLimits = pro.ProductMenuLimits.ToClone();
                return SaveOrUpdate(obj);
            }
            return OpResult.Fail();
        }
        public OpResult SaveMenu(Entity.ProductMenuLimit obj)
        {
            if (obj.Id == 0)
            {
                if (ProductModelVerRepository.IsExists(o => o.ProductId == obj.ProductId && o.Status == 0 && o.ModuleId != obj.ModuleId))
                    return OpResult.Fail("已存在未发布的版本");
                obj.ModuleId = obj.ModuleId ?? CommonService.GUID;
                obj.MenuId = ProductMenuLimitRepository.GetMaxInt(o => (int?)o.MenuId, whereLambda: o => o.ProductId == obj.ProductId && o.ModuleId == obj.ModuleId);
                obj.CreateDT = DateTime.Now;
                obj.CreateUID = CurrentUser.UID;
                obj.SortOrder = ProductMenuLimitRepository.GetMaxInt(o => (int?)o.SortOrder, whereLambda: o => o.ProductId == obj.ProductId && o.ModuleId == obj.ModuleId);
                ProductMenuLimitRepository.Add(obj, false);
            }
            else
            {
                var menu = ProductMenuLimitRepository.Get(obj.Id);
                obj.ToCopyProperty(menu, new List<string>() { "CreateDT", "CreateUID", "MenuId", "SortOrder" });
                obj.ModuleId = menu.ModuleId;
            }
            var model = ProductModelVerRepository.Find(o => o.ModuleId == obj.ModuleId);
            if (model != null)
            {
                model.UpdateDT = DateTime.Now;
                model.UpdateUID = CurrentUser.UID;
            }
            else
            {
                ProductModelVerRepository.Add(new Entity.ProductModuleVer()
                {
                    ModuleId = obj.ModuleId,
                    ProductId = obj.ProductId,
                    CreateDT = obj.CreateDT,
                    UpdateDT = obj.CreateDT,
                    UpdateUID = obj.CreateUID,
                    CreateUID = obj.CreateUID,
                }, false);
            }
            
            ProductMenuLimitRepository.SaveChanges();
            return OpResult.Success();
        }
        public void RemoveMenu(int menuId, string modelId)
        {
            var obj = GetMenu(menuId, modelId);
            ProductMenuLimitRepository.Remove(obj);
        }
        public OpResult Deletes(int[] ids)
        {
            var list= ProductModelVerRepository.GetQuery(o => ids.Contains(o.Id)).Include(o=>o.ProductMenuLimits).ToList();
            if (list.Any(o => o.VerStatus > 0))
                return OpResult.Fail("该状态不允许删除！");
            ProductMenuLimitRepository.RemoveRange(list.SelectMany(o => o.ProductMenuLimits).ToList(), false);
            //ProductUpdateLogRepository.RemoveRange(list.SelectMany(o => o.ProductUpdateLogs).ToList(), false);
            ProductModelVerRepository.RemoveRange(list);
            return OpResult.Success();
        }
        public Entity.ProductModuleVer Get(string modelId)
        {
            return ProductModelVerRepository.GetQuery(o => o.ModuleId == modelId).Include(o => o.ProductMenuLimits).FirstOrDefault();
        }
        public Entity.ProductModuleVer GetOfficialLast(int productId)
        {
            return ProductModelVerRepository.GetOfficialLast(productId);
        }
        public Entity.ProductMenuLimit GetMenu(int menuid, string modelId)
        {
            return ProductMenuLimitRepository.GetQuery(o=>o.ModuleId==modelId && o.MenuId==menuid).FirstOrDefault();
        }

        public List<Models.ProductMenuModel> MenuList(string modelId)
        {
            var queryMenu = ProductMenuLimitRepository.GetQuery(o => o.ModuleId==modelId);
            var q = from x in queryMenu
                    where x.Type!=3
                    orderby x.SortOrder
                    select new { 
                        x,
                        Haslimit = queryMenu.Any(o =>o.PMenuId== x.MenuId && o.Type == 3)
                    };
            var ms = q.ToList();
            var menus = new List<Models.ProductMenuModel>();
            foreach(var m in ms)
            {
                var pm = new Models.ProductMenuModel();
                m.x.ToCopyProperty(pm);
                pm.HasLimit = m.Haslimit;
                menus.Add(pm);
            }
            var list = new List<Models.ProductMenuModel>();
            foreach (var menu in menus.Where(o => o.PMenuId <= 0))
            {
                SetChilds(menu, menus);
                menu.Index = list.Count;
                list.Add(menu);
            }
            return list;
        }
        public void MoveMenuItem(short mode, int menuId, string modelId)
        {
            var obj = ProductMenuLimitRepository.Find(o => o.ModuleId == modelId && o.MenuId == menuId);
            var list = ProductMenuLimitRepository.GetQuery(o => o.ModuleId == modelId && o.PMenuId == obj.PMenuId).OrderBy(o => o.SortOrder).ToList();
            switch (mode)
            {
                case 2://下移
                    var obj1 = list.LastOrDefault();
                    if (obj.Id != obj1.Id)
                    {
                        Entity.ProductMenuLimit next = null;
                        for (var i = 0; i < list.Count; i++)
                        {
                            if (obj.Id == list[i].Id)
                            {
                                next = list[i + 1]; break;
                            }
                        }
                        if (next != null)
                        {
                            var sort = obj.SortOrder;
                            obj.SortOrder = next.SortOrder;
                            next.SortOrder = sort;
                            ProductMenuLimitRepository.SaveChanges();
                        }
                    }
                    break;
                default://上移
                     var obj2 = list.FirstOrDefault();
                    if (obj.Id != obj2.Id)
                    {
                        Entity.ProductMenuLimit prev = null;
                        for (var i=0;i<list.Count;i++)
                        {
                            if (obj.Id == list[i].Id)
                            {
                                prev = list[i - 1]; break;
                            }
                        }
                        if (prev != null)
                        {
                            var sort = obj.SortOrder;
                            obj.SortOrder = prev.SortOrder;
                            prev.SortOrder = sort;
                            ProductMenuLimitRepository.SaveChanges();
                        }
                    }
                    break;
            }
        }
        public void MoveUpMenuItem(short mode, int menuId, string modelId)
        {
            var obj = ProductMenuLimitRepository.Find(o => o.ModuleId == modelId && o.MenuId == menuId);
            var list = ProductMenuLimitRepository.GetQuery(o => o.ModuleId == modelId && o.PMenuId <= 0).OrderBy(o => o.SortOrder).ToList();
            switch (mode)
            {
                case 4://降级
                    var obj1 = list.LastOrDefault(o =>o.ModuleId==modelId && o.MenuId!=obj.MenuId && o.SortOrder <= obj.SortOrder);//上同节点
                    if (obj1!=null && obj.Id != obj1.Id)
                    {
                        var last = ProductMenuLimitRepository.GetQuery(o => o.ModuleId == modelId && o.PMenuId == obj1.MenuId).OrderByDescending(o => o.SortOrder).FirstOrDefault();
                        obj.PMenuId = obj1.MenuId;
                        if (last != null)
                        {
                            obj.SortOrder =(short)(last.SortOrder+1);
                        }
                        ProductMenuLimitRepository.SaveChanges();
                    }
                    break;
                default://升级
                    var obj2 = list.FirstOrDefault(o => o.ModuleId == modelId && o.MenuId == obj.PMenuId);
                    if (obj2 != null && obj.Id != obj2.Id)
                    {
                        var next = list.FirstOrDefault(o => o.ModuleId == modelId && o.MenuId != obj2.MenuId && o.SortOrder >= obj2.SortOrder);//下同节点
                        obj.PMenuId = 0;
                        obj.SortOrder = (short)(obj2.SortOrder+1);
                        if (next != null && obj.SortOrder>=next.SortOrder)
                        {
                            next.SortOrder++;//下节点下移
                        }
                        ProductMenuLimitRepository.SaveChanges();
                    }
                    break;
            }
        }
        void SetChilds(Models.ProductMenuModel menu,List<Models.ProductMenuModel> alls)
        {
            menu.Childrens = alls.Where(o => o.PMenuId == menu.MenuId).ToList();
            int i = 0;
            foreach (var child in menu.Childrens)
            {
                child.Index = i++;
                child.ParentId = menu.Id;
                SetChilds(child, alls);
            }
        }

        public Entity.ProductMenuLimit GetLimit(int limitid, string modelId)
        {
            return ProductMenuLimitRepository.GetQuery(o => o.ModuleId == modelId && o.MenuId == limitid).FirstOrDefault();
        }
        public void SetLimitState(short mode, int limitId, string modelId)
        {
            var obj = GetLimit(limitId, modelId);
            obj.Status = mode == 1;
            ProductMenuLimitRepository.SaveChanges();
        }
        public List<Models.LimitModels> LimitList(string modelId)
        {
            var query = ProductMenuLimitRepository.GetQuery(o => o.ModuleId == modelId);
            var queryMenu = query.Where(o => o.Type != 3);
            var queryLimit = query.Where(o => o.Type == 3);
            var q = from x in queryMenu
                        join y in queryLimit on x.MenuId equals y.PMenuId into tmp
                        from o in tmp.DefaultIfEmpty()
                        orderby x.SortOrder, o.SortOrder
                        select new {
                            x.MenuId,
                            x.PMenuId,
                            MenuTitle= x.Title,
                            LimitId=(int?)o.MenuId,
                            Title=o.Title,
                            o.Memo,
                            Status=(bool?)o.Status,
                            MenuIdFK = (int?)o.PMenuId
                        };
            var menus = q.ToList().Select(o=>new Models.LimitModels(){
                MenuId=o.MenuId,
                MenuIdFK=o.MenuIdFK,
                ParentId=o.PMenuId,
                MenuTitle=o.MenuTitle,
                LimitId=o.LimitId.GetValueOrDefault(),
                Title=o.Memo.IsNullOrEmpty()?o.Title:o.Title+" ("+o.Memo+")",
                Status=o.Status.GetValueOrDefault()
            }).ToList();
            var list = new List<Models.LimitModels>();
            foreach (var menuid in menus.Where(o => o.MenuIdFK.HasValue).Select(o=>o.MenuIdFK).Distinct())
            {
                int i = 0;
                var count = menus.Where(o => o.MenuIdFK == menuid).Count();
                menus.Where(o => o.MenuIdFK == menuid).Each(o =>
                {
                    o.ChildCount = count;
                    o.Index = i++;
                });
                
            }
            foreach (var menu in menus.Where(o => o.ParentId <= 0))
            {
                SetChilds(menu, menus);
                list.Add(menu);
            }
            return list;
        }

        public void MoveLimitItem(short mode, int limitId, string modelId)
        {
            var obj = ProductMenuLimitRepository.Find(o => o.ModuleId == modelId && o.MenuId == limitId);
            var list = ProductMenuLimitRepository.GetQuery(o => o.ModuleId == modelId && o.Type == 3 && o.PMenuId == obj.PMenuId).OrderBy(o => o.SortOrder).ToList();
            switch (mode)
            {
                case 2://下移
                    var obj1 = list.LastOrDefault();
                    if (obj.Id != obj1.Id)
                    {
                        Entity.ProductMenuLimit next = null;
                        for (var i = 0; i < list.Count; i++)
                        {
                            if (obj.Id == list[i].Id)
                            {
                                next = list[i + 1]; break;
                            }
                        }
                        if (next != null)
                        {
                            var sort = obj.SortOrder;
                            obj.SortOrder = next.SortOrder;
                            next.SortOrder = sort;
                            ProductMenuLimitRepository.SaveChanges();
                        }
                    }
                    break;
                default://上移
                    var obj2 = list.FirstOrDefault();
                    if (obj.Id != obj2.Id)
                    {
                        Entity.ProductMenuLimit prev = null;
                        for (var i = 0; i < list.Count; i++)
                        {
                            if (obj.Id == list[i].Id)
                            {
                                prev = list[i - 1]; break;
                            }
                        }
                        if (prev != null)
                        {
                            var sort = obj.SortOrder;
                            obj.SortOrder = prev.SortOrder;
                            prev.SortOrder = sort;
                            ProductMenuLimitRepository.SaveChanges();
                        }
                    }
                    break;
            }
        }
        public OpResult Publish(string modelId, short state)
        {
            var obj = Get(modelId);
            if (obj != null)
            {
                if (!obj.ProductMenuLimits.Any(o=>o.Type!=3))
                    return OpResult.Fail("请先配置菜单！");
                if (!obj.ProductMenuLimits.Any(o => o.Type == 3))
                    return OpResult.Fail("请先配置权限！");
                obj.VerStatus = state;
                var list = ProductModelVerRepository.GetQuery(o => o.ProductId == obj.ProductId && o.ModuleId != obj.ModuleId).ToList();
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
                ProductVerRepository.SaveChanges();
                return OpResult.Success();
            }
            return OpResult.Fail();
        }
        public List<Entity.ProductVer> GetProductVers()
        {
            var queryProduct = ProductVerRepository.GetQuery();
            var queryModel = ProductModelVerRepository.GetQuery();
            var query = from x in queryProduct
                        where !queryModel.Any(o => o.ProductId == x.ProductId)
                            && x.Status==1
                        select x;
            return query.ToList();
        }
        void SetChilds(Models.LimitModels menu, List<Models.LimitModels> alls)
        {
            menu.Childrens = alls.Where(o => o.ParentId == menu.MenuId).ToList();
            foreach (var child in menu.Childrens)
            {
                SetChilds(child, alls);
            }
        }
    }
}
