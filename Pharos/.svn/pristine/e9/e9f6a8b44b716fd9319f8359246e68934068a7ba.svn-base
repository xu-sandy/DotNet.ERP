using Pharos.Logic.OMS.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Utility.Helpers;
using Pharos.Utility;
namespace Pharos.Logic.OMS.BLL
{
    public class SysLimitService
    {
        [Ninject.Inject]
        IBaseRepository<Entity.SysLimits> LimitRepository { get; set; }
        [Ninject.Inject]
        IBaseRepository<Entity.SysMenus> MenuRepository { get; set; }
        public List<Models.LimitModels> FindPageList(out int recordCount)
        {
            var queryMenu = MenuRepository.GetQuery();
            var queryLimit = LimitRepository.GetQuery();
            var q = from x in queryMenu
                    join y in queryLimit on x.MenuId equals y.PLimitId into tmp
                    from o in tmp.DefaultIfEmpty()
                    orderby x.SortOrder, o.SortOrder
                    select new
                    {
                        x.Id,
                        x.MenuId,
                        x.PMenuId,
                        MenuTitle = x.Title,
                        LimitId = (int?)o.LimitId,
                        Title = o.Title,
                        Status = (bool?)o.Status,
                        MenuIdFK = (int?)o.PLimitId
                    };
            recordCount = q.Count();
            var menus = q.ToList().Select(o => new Models.LimitModels()
            {
                MenuId = o.MenuId,
                MenuIdFK = o.MenuIdFK,
                ParentId = o.PMenuId,
                MenuTitle = o.MenuTitle,
                LimitId = o.LimitId.GetValueOrDefault(),
                Title =  o.Title,
                Status = o.Status.GetValueOrDefault()
            }).ToList();
            var list = new List<Models.LimitModels>();
            foreach (var menuid in menus.Where(o => o.MenuIdFK.HasValue).Select(o => o.MenuIdFK).Distinct())
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
        public OpResult SaveOrUpdate(Entity.SysLimits obj)
        {
            if (obj.Id == 0)
            {
                obj.LimitId = LimitRepository.GetMaxInt(o => (int?)o.LimitId);
                obj.SortOrder = obj.LimitId;
                LimitRepository.Add(obj, false);
            }else
            {
                var limit = LimitRepository.Get(obj.Id);
                limit.Title = obj.Title;
                limit.Status = obj.Status;
            }
            LimitRepository.SaveChanges();
            return OpResult.Success();
        }
        public OpResult Deletes(int[] ids)
        {
            var list= LimitRepository.FindList(o => ids.Contains(o.Id));
            LimitRepository.RemoveRange(list);
            return OpResult.Success();
        }
        public OpResult Remove(int limitid)
        {
            var obj = GetLimit(limitid);
            LimitRepository.Remove(obj);
            return OpResult.Success();
        }
        public Entity.SysLimits GetLimit(int limitid)
        {
            return LimitRepository.Find(o => o.LimitId == limitid);
        }
        public Entity.SysMenus GetMenu(int menuid)
        {
            return MenuRepository.Find(o => o.MenuId == menuid);
        }
        public void MoveItem(short mode, int limitid)
        {
            var obj = GetLimit(limitid);
            var list = LimitRepository.GetQuery(o => o.PLimitId == obj.PLimitId).OrderBy(o => o.SortOrder).ToList();
            switch (mode)
            {
                case 2://下移
                    var obj1 = list.LastOrDefault();
                    if (obj.Id != obj1.Id)
                    {
                        Entity.SysLimits next = null;
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
                            LimitRepository.SaveChanges();
                        }
                    }
                    break;
                default://上移
                    var obj2 = list.FirstOrDefault();
                    if (obj.Id != obj2.Id)
                    {
                        Entity.SysLimits prev = null;
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
                            LimitRepository.SaveChanges();
                        }
                    }
                    break;
            }
        }
        public void SetState(int limitid, short status)
        {
            var obj = GetLimit(limitid);
            obj.Status = status == 1;
            LimitRepository.SaveChanges();
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
