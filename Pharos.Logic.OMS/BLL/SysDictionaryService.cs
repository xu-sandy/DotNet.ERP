using Pharos.Logic.OMS.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.Utility.Helpers;
using Pharos.Utility;
namespace Pharos.Logic.OMS.BLL
{
    public class SysDictionaryService
    {
        [Ninject.Inject]
        IBaseRepository<Entity.SysDataDictionary> DictRepository { get; set; }
        public List<Models.DictionaryModel> GetDataList(int? psn)
        {
            var queryMenu = DictRepository.GetQuery();
            var q = from x in queryMenu
                    select x;

            var childs = new List<Entity.SysDataDictionary>();
            if (psn.HasValue)
            {
                q = q.Where(o => o.DicPSN == psn.Value);
            }
            else
            {
                q = q.Where(o => o.HasChild || o.DicPSN <= 0);
                childs = queryMenu.Where(o => !(o.HasChild || o.DicPSN <= 0)).ToList();
            }
            var ms = q.OrderBy(o => o.SortOrder).ToList();
            var menus = new List<Models.DictionaryModel>();
            int i = 0;
            foreach (var m in ms)
            {
                var pm = new Models.DictionaryModel();
                m.ToCopyProperty(pm);
                pm.Status =Convert.ToInt16(  m.Status);
                pm.ChildTitle = string.Join("、", childs.Where(o => o.DicPSN == m.DicSN).OrderBy(o => o.SortOrder).Select(o => o.Title));
                pm.Index = i;
                i++;
                pm.Count = ms.Count;
                menus.Add(pm);
            }
            if (psn.HasValue) return menus;
            var list = new List<Models.DictionaryModel>();
            foreach (var menu in menus.Where(o => o.DicPSN <= 0))
            {
                SetChilds(menu, menus);
                menu.Index = list.Count;
                list.Add(menu);
            }
            return list;
        }
        public Entity.SysDataDictionary Get(int sn)
        {
            return DictRepository.Find(o => o.DicSN == sn);
        }
        public OpResult Save(Entity.SysDataDictionary obj)
        {
            if(obj.Id==0)
            {
                obj.DicSN = DictRepository.GetMaxInt(o => o.DicSN);
                obj.SortOrder = obj.DicSN;
                obj.CreateDT = DateTime.Now;
                obj.CreateUID = CurrentUser.UID;
                DictRepository.Add(obj, false);
            }
            else
            {
                var dict = DictRepository.Get(obj.Id);
                dict.Title = obj.Title;
                dict.Status = obj.Status;
                if (dict.HasChild != obj.HasChild && DictRepository.IsExists(o => o.DicPSN == obj.DicSN))
                    return OpResult.Fail("存在下级不允许修改！"); 
                dict.HasChild = obj.HasChild;
            }
            DictRepository.SaveChanges();
            return OpResult.Success();
        }
        public OpResult Remove(int sn)
        {
            var obj = Get(sn);
            DictRepository.Remove(obj);
            return OpResult.Success();
        }
        public void SetState(int sn ,short state)
        {
            var obj = Get(sn);
            obj.Status = state == 1;
            DictRepository.SaveChanges();
        }
        public void MoveItem(short mode, int sn)
        {
            var obj = DictRepository.Find(o =>o.DicSN == sn);
            var list = DictRepository.GetQuery(o => o.DicPSN == obj.DicPSN).OrderBy(o => o.SortOrder).ToList();
            switch (mode)
            {
                case 2://下移
                    var obj1 = list.LastOrDefault();
                    if (obj.Id != obj1.Id)
                    {
                        Entity.SysDataDictionary next = null;
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
                            DictRepository.SaveChanges();
                        }
                    }
                    break;
                default://上移
                    var obj2 = list.FirstOrDefault();
                    if (obj.Id != obj2.Id)
                    {
                        Entity.SysDataDictionary prev = null;
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
                            DictRepository.SaveChanges();
                        }
                    }
                    break;
            }
        }
        void SetChilds(Models.DictionaryModel menu, List<Models.DictionaryModel> alls)
        {
            menu.Childrens = alls.Where(o => o.DicPSN == menu.DicSN).ToList();
            int i = 0;
            foreach (var child in menu.Childrens)
            {
                child.Index = i++;
                child.ParentId = menu.Id;
                SetChilds(child, alls);
            }
        }
    }
}
