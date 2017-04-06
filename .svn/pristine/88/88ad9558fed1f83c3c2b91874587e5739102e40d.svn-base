using Pharos.Logic.OMS.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using Pharos.Utility.Helpers;
using Pharos.Utility;

namespace Pharos.Logic.OMS.BLL
{
    public class DepartMentService:BaseService<Entity.SysDepartments>
    {
        [Ninject.Inject]
        IBaseRepository<Entity.SysDepartments> DepartRepository { get; set; }
        [Ninject.Inject]
        IBaseRepository<Entity.SysUser> UserRepository { get; set; }
        [Ninject.Inject]
        IBaseRepository<Entity.SysPositions> PositionRepository { get; set; }
        public IEnumerable<Entity.DepartMentExt> GetPageList(bool all=true)
        {
            var query = DepartRepository.GetQuery();
            var queryUser = UserRepository.GetQuery();
            var queryPosition = PositionRepository.GetQuery();
            var q = from x in query
                    orderby x.SortOrder
                    select new Entity.DepartMentExt()
                    {
                        Id=x.Id,
                        CreateDT=x.CreateDT,
                        CreateUID=x.CreateUID,
                        DeptCode=x.DeptCode,
                        Depth=x.Depth,
                        DeptId=x.DeptId,
                        PDeptId=x.PDeptId,
                        Title=x.Title,
                        SortOrder=x.SortOrder,
                        Status=x.Status,
                        UserCount = queryUser.Count(o => o.DeptId == x.DeptId),
                        PositionCount=queryPosition.Count(o=>(","+o.DeptId+",").Contains(","+ x.DeptId+",")),
                        ManagerTitle = (x.ManagerUId == null || x.ManagerUId == string.Empty) ? "" : queryUser.Where(o=>o.UserId==x.ManagerUId).Select(o=>o.FullName).FirstOrDefault(),
                        DeputyTitle = (x.DeputyUId == null || x.DeputyUId == string.Empty) ? "" : queryUser.Where(o => o.UserId == x.DeputyUId).Select(o => o.FullName).FirstOrDefault()
                    };
            if(!all)
            {
                q=q.Where(o=>o.Status);
            }
            var ms = q.ToList();
            var list = new List<Entity.DepartMentExt>();
            foreach (var dept in ms.Where(o => o.PDeptId <= 0))
            {
                SetChilds(dept, ms);
                dept.Index = list.Count;
                list.Add(dept);
            }
            return list;
        }
        public OpResult SaveOrUpdate(Entity.SysDepartments obj, short parentDepth)
        {
            if (!obj.DeptCode.IsNullOrEmpty() && DepartRepository.IsExists(o => o.DeptCode == obj.DeptCode && o.Id != obj.Id))
                return OpResult.Fail("该代码已存在!");
            if (obj.Id == 0)
            {
                obj.DeptId = DepartRepository.GetMaxInt(o =>(int?)o.DeptId);
                obj.CreateDT = DateTime.Now;
                obj.CreateUID = CurrentUser.UID;
                obj.SortOrder = DepartRepository.GetMaxInt(o => (int?)o.SortOrder);
                obj.Depth = ++parentDepth;
                DepartRepository.Add(obj, false);
            }
            else
            {
                var menu = DepartRepository.Get(obj.Id);
                obj.ToCopyProperty(menu, new List<string>() { "CreateDT", "CreateUID", "DeptId", "SortOrder", "Depth" });
            }
            DepartRepository.SaveChanges();
            return OpResult.Success();
        }
        public OpResult Deletes(int[] ids)
        {
            var idstr = ids.Select(o => "," + o + ",").ToList();
            if (PositionRepository.IsExists(o => idstr.Contains(","+o.DeptId+",")))
                return OpResult.Fail("存在职位关联不能移除！");
            if (UserRepository.IsExists(o => ids.Contains(o.DeptId)))
                return OpResult.Fail("存在人员关联不能移除！");
            var list= DepartRepository.GetQuery(o => ids.Contains(o.DeptId)).ToList();
            DepartRepository.RemoveRange(list);
            return OpResult.Success();
        }
        public Entity.SysDepartments Get(int deptid)
        {
            return DepartRepository.Find(o => o.DeptId == deptid);
        }
        public Dictionary<int, string> GetFullTitle(string deptid)
        {
            var deptids = deptid.Split(',').Select(o=>int.Parse(o)).ToList();
            return GetFullTitle(true).Where(o => deptids.Contains(o.Key)).ToDictionary(o=>o.Key,o=>o.Value);
        }
        //public Dictionary<int, string> GetFullTitle(int deptid)
        //{
        //    var queryDept = DepartRepository.GetQuery();
        //    var query = from x in queryDept
        //                where x.DeptId == deptid
        //                select new
        //                {
        //                    x.DeptId,
        //                    x.Title,
        //                    PDepartMent = x.DeptId > 0 ? queryDept.Where(i => i.DeptId == queryDept.Where(o => o.DeptId == x.DeptId).Select(o => o.PDeptId).FirstOrDefault()).Select(o => o.Title).FirstOrDefault() + "/" : string.Empty,
        //                };
        //    return query.ToDictionary(o => o.DeptId, o => o.PDepartMent + o.Title);
        //}
        public Dictionary<int, string> GetFullTitle(bool all=false)
        {
            var query = DepartRepository.GetQuery();
            if (!all) query = query.Where(o => o.Status);
            var list= query.Select(o => new Entity.DepartMentExt()
            {
                DeptId = o.DeptId,
                PDeptId = o.PDeptId,
                Title = o.Title,
                SortOrder = o.SortOrder
            }).ToList();
            foreach (var dept in list.Where(o => o.PDeptId <= 0))
            {
                SetChilds(dept, list);
            }
            return list.ToDictionary(o => o.DeptId, o => o.FullTitle);
        }
        public void MoveItem(short mode, int deptId)
        {
            var obj = DepartRepository.Find(o => o.DeptId == deptId);
            var list = DepartRepository.GetQuery(o => o.PDeptId == obj.PDeptId).OrderBy(o => o.SortOrder).ToList();
            switch (mode)
            {
                case 2://下移
                    var obj1 = list.LastOrDefault();
                    if (obj.Id != obj1.Id)
                    {
                        Entity.SysDepartments next = null;
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
                            DepartRepository.SaveChanges();
                        }
                    }
                    break;
                default://上移
                     var obj2 = list.FirstOrDefault();
                    if (obj.Id != obj2.Id)
                    {
                        Entity.SysDepartments prev = null;
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
                            DepartRepository.SaveChanges();
                        }
                    }
                    break;
            }
        }
        public void MoveUpItem(short mode, int deptId)
        {
            var obj = DepartRepository.Find(o => o.DeptId == deptId);
            var list = DepartRepository.GetQuery(o => o.PDeptId <= 0).OrderBy(o => o.SortOrder).ToList();
            switch (mode)
            {
                case 4://降级
                    var obj1 = list.LastOrDefault(o =>o.DeptId!=obj.DeptId && o.SortOrder <= obj.SortOrder);//上同节点
                    if (obj1!=null && obj.Id != obj1.Id)
                    {
                        var last = DepartRepository.GetQuery(o => o.PDeptId == obj1.DeptId).OrderByDescending(o => o.SortOrder).FirstOrDefault();
                        obj.PDeptId = obj1.DeptId;
                        obj.Depth =Convert.ToInt16( obj1.Depth +1);
                        if (last != null)
                        {
                            obj.SortOrder =last.SortOrder+1;
                        }
                        DepartRepository.SaveChanges();
                    }
                    break;
                default://升级
                    var obj2 = list.FirstOrDefault(o => o.DeptId == obj.PDeptId);
                    if (obj2 != null && obj.Id != obj2.Id)
                    {
                        var next = list.FirstOrDefault(o => o.DeptId != obj2.DeptId && o.SortOrder >= obj2.SortOrder);//下同节点
                        obj.PDeptId = 0;
                        obj.SortOrder = obj2.SortOrder+1;
                        obj.Depth = obj2.Depth;
                        if (next != null && obj.SortOrder>=next.SortOrder)
                        {
                            next.SortOrder++;//下节点下移
                        }
                        DepartRepository.SaveChanges();
                    }
                    break;
            }
        }
        public void SetState(short mode, int deptId)
        {
            var obj = Get(deptId);
            obj.Status = mode == 1;
            DepartRepository.SaveChanges();
        }
        public IEnumerable<dynamic> GetInput(string searchName)
        {
            var query= DepartRepository.GetQuery(o => o.Title.Contains(searchName) || (o.DeptCode!=null && o.DeptCode.Contains(searchName)));
            var queryDept=DepartRepository.GetQuery();
            query = query.Where(o => o.Status);
            var q = from x in query
                    select new
                    { 
                        x.DeptId,
                        x.Title,
                        x.PDeptId,
                        PDeptTitle = queryDept.Where(i => i.DeptId == queryDept.Where(o => o.DeptId == x.DeptId).Select(o => o.PDeptId).FirstOrDefault()).Select(o => o.Title).FirstOrDefault() + "/"
                    };
            return q.ToList();
        }

        public List<int> GetDeptChildByDeptId(int deptId)
        {
            if (UserRepository == null) UserRepository = NinjectObject.GetFromMVC<DAL.BaseRepository<Entity.SysUser>>();
            if (DepartRepository == null) DepartRepository = NinjectObject.GetFromMVC<DAL.BaseRepository<Entity.SysDepartments>>();
            var all = DepartRepository.GetQuery().ToList();
            var dept = all.FirstOrDefault(o => o.DeptId == deptId);
            var list = new List<Entity.SysDepartments>();
            SetChilds(dept, all, list);
            var deptIds = list.Select(o => o.DeptId).ToList();
            return deptIds;
        }
        public List<string> GetAllCreateUIDByDeptId(int deptId)
        {
            var deptIds = GetDeptChildByDeptId(deptId);
            var users= UserRepository.GetQuery(o => deptIds.Contains(o.DeptId)).Select(o=>o.UserId).Distinct().ToList();
            return users;
        }
        void SetChilds(Entity.DepartMentExt dept,List<Entity.DepartMentExt> alls,string parentTitle="")
        {
            dept.Childrens = alls.Where(o => o.PDeptId == dept.DeptId).ToList();
            dept.FullTitle =parentTitle + dept.Title;
            var prev = alls.LastOrDefault(o => o.DeptId != dept.DeptId && o.SortOrder <= dept.SortOrder);//上同节点
            if (prev != null) dept.PrevId = prev.DeptId;
            int i = 0;
            foreach (var child in dept.Childrens)
            {
                child.Index = i++;
                child.ParentId = dept.Id;
                SetChilds(child, alls, dept.FullTitle+"/");
            }
        }
        void SetChilds(Entity.SysDepartments dept,List<Entity.SysDepartments> alls,List<Entity.SysDepartments> list)
        {
            if (dept == null) return;
            list.Add(dept);
            SetChilds(alls.FirstOrDefault(o => o.PDeptId == dept.DeptId), alls, list);
        }
    }
}
