using Pharos.Logic.OMS.Entity;
using Pharos.Logic.OMS.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using Pharos.Utility.Helpers;
using Pharos.Logic.OMS.Models;
using Pharos.Utility;
using System.Transactions;
using System.Data.Entity;
namespace Pharos.Logic.OMS.BLL
{
    public class MenuService:BaseService<SysMenus>
    {
        [Ninject.Inject]
        IBaseRepository<SysMenus> MenuRepository { get; set; }
        [Ninject.Inject]
        IBaseRepository<SysUser> UserRepository { get; set; }
        [Ninject.Inject]
        IBaseRepository<SysRoleData> RoleDataRepository { get; set; }
        [Ninject.Inject]
        IBaseRepository<SysRoles> RoleRepository { get; set; }
        [Ninject.Inject]
        IBaseRepository<SysLimits> LimitRepository { get; set; }
        [Ninject.Inject]
        IDictRepository DictRepository { get; set; }
        [Ninject.Inject]
        ProductPublishVerService PublishVerService { get; set; }
        public Pharos.Utility.OpResult SaveOrUpdate(SysMenus model)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<dynamic> GetPageList(System.Collections.Specialized.NameValueCollection nvl, out int recordCount)
        {
            var query = MenuRepository.GetQuery(null);
            recordCount = query.Count();
            return query.ToList();
        }

        public Pharos.Utility.OpResult Deletes(object[] ids)
        {
            throw new NotImplementedException();
        }

        public SysMenus GetOne(object id)
        {
            throw new NotImplementedException();
        }

        public List<SysMenus> GetList()
        {
            return MenuRepository.GetQuery(o => o.Status).OrderBy(o => o.SortOrder).ToList();
        }
        public List<SysMenus> GetChildList()
        {
            return MenuRepository.GetQuery(o => o.Status && o.PMenuId!=0).OrderBy(o => o.SortOrder).ToList();
        }
        public List<SysMenuLimitModel> GetUserMenus(string userId)
        {
            var list = new List<SysMenuLimitModel>();
            if(UserRepository==null) UserRepository= NinjectObject.GetFromMVC<DAL.BaseRepository<SysUser>>();
            if (MenuRepository == null) MenuRepository = NinjectObject.GetFromMVC<DAL.BaseRepository<SysMenus>>();
            if (RoleDataRepository == null) RoleDataRepository = NinjectObject.GetFromMVC<DAL.BaseRepository<SysRoleData>>();
            if (RoleRepository == null) RoleRepository = NinjectObject.GetFromMVC<DAL.BaseRepository<SysRoles>>();
            var user = UserRepository.Find(o => o.UserId == userId);
            if(user!=null)
            { 
                if(user.IsSuper)
                    list = MenuRepository.GetQuery(o => o.Status).OrderBy(o => o.SortOrder).Select(y => new SysMenuLimitModel()
                    {
                        Id = y.Id,
                        MenuId = y.MenuId,
                        PMenuId = y.PMenuId,
                        SortOrder = y.SortOrder,
                        Status = y.Status,
                        Title = y.Title,
                        URL = y.URL,
                        LimitIdStr="-1"
                    }).ToList();
                else if(!user.RoleIds.IsNullOrEmpty())
                {
                    var rids= user.RoleIds.ToIntArray();
                    var queryRole= RoleRepository.GetQuery();
                    var queryMenu = MenuRepository.GetQuery();
                    var queryData = RoleDataRepository.GetQuery();
                    var query = from x in queryData
                                join y in queryMenu on x.MenuId equals y.MenuId
                                join z in queryRole on x.RoleId equals z.RoleId
                                where y.Status && z.Status && rids.Contains(x.RoleId) && x.HasSelected
                                orderby x.SortOrder
                                select new SysMenuLimitModel()
                                { 
                                    Id=y.Id,
                                    MenuId=y.MenuId,
                                    PMenuId=y.PMenuId,
                                    Status=y.Status,
                                    Title=y.Title,
                                    URL=y.URL,
                                    SortOrder = x.SortOrder,
                                    LimitIdStr = z.Limitids
                                };
                    list = query.ToList();
                }
            }
            return list;
        }

        public OpResult UpdateData()
        {
            ProductPublishVer ver =null;
            try
            {
                using (TransactionScope tran = new TransactionScope())
                {
                    ver = PublishVerService.UpdatePublish(0, 1, CurrentUser.FullName);
                    if (ver.ProductModuleVer != null)
                    {
                        var menus = new List<SysMenus>();
                        var limits = new List<SysLimits>();
                        ver.ProductModuleVer.ProductMenuLimits.Each(o =>
                        {
                            switch (o.Type)
                            {
                                case 1:
                                    menus.Add(new SysMenus() { MenuId = o.MenuId, PMenuId = o.PMenuId, SortOrder = o.SortOrder, Status = o.Status, Title = o.Title, URL = o.Url });
                                    break;
                                case 3:
                                    limits.Add(new SysLimits() { LimitId = o.MenuId, PLimitId = o.PMenuId, SortOrder = o.SortOrder, Status = o.Status, Title = o.Title });
                                    break;
                                default:
                                    break;
                            }
                        });
                        if (menus.Any())
                        {
                            var list = MenuRepository.GetQuery().ToList();
                            MenuRepository.RemoveRange(list);
                            MenuRepository.AddRange(menus);
                        }
                        if (limits.Any())
                        {
                            LimitRepository.RemoveRange(LimitRepository.GetQuery().ToList());
                            LimitRepository.AddRange(limits);
                        }
                    }
                    if (ver.ProductRoleVer != null)
                    {
                        var roles = new List<SysRoles>();
                        ver.ProductRoleVer.ProductRoles.Each(o =>
                        {
                            var role = new SysRoles();
                            role.CreateDT = DateTime.Now;
                            role.CreateUID = CurrentUser.UID;
                            role.RoleId = o.RoleId.GetValueOrDefault();
                            role.Title = o.Title;
                            role.Limitids = o.Limitids;
                            role.Type = 0;
                            role.Status = true;
                            role.UpdateDT = role.CreateDT;
                            role.UpdateUID = role.CreateUID;
                            role.SysRoleDatas = new List<SysRoleData>();
                            o.ProductRoleDatas.Each(i =>
                            {
                                role.SysRoleDatas.Add(new SysRoleData()
                                {
                                    HasSelected = i.HasSelected,
                                    MenuId = i.MenuId,
                                    PMenuId = i.PMenuId,
                                    SortOrder = i.SortOrder
                                });
                            });
                            roles.Add(role);
                        });
                        if (roles.Any())
                        {
                            var list = RoleRepository.GetQuery(o => o.Type == 0).Include(o=>o.SysRoleDatas).ToList();
                            roles.Each(o => {
                                var r= list.FirstOrDefault(i => i.RoleId == o.RoleId);
                                if (r != null)
                                {
                                    o.DeptId = r.DeptId;
                                    if(!r.Memo.IsNullOrEmpty())
                                        o.Memo = r.Memo;
                                    o.Status = r.Status;
                                    o.CreateDT = r.CreateDT;
                                    o.CreateUID = r.CreateUID;
                                }
                            });
                            RoleDataRepository.RemoveRange(list.SelectMany(o => o.SysRoleDatas).ToList());
                            RoleRepository.RemoveRange(list);
                            RoleRepository.AddRange(roles);
                        }
                    }
                    if (ver.ProductDictionaryVer != null)
                    {
                        var dicts = new List<SysDataDictionary>();
                        ver.ProductDictionaryVer.ProductDictionaryDatas.Each(o =>
                        {
                            dicts.Add(new SysDataDictionary()
                            {
                                CreateDT = DateTime.Now,
                                CreateUID = CurrentUser.UID,
                                Depth = o.Depth,
                                DicPSN = o.DicPSN,
                                DicSN = o.DicSN,
                                HasChild = o.HasChild,
                                SortOrder = o.SortOrder,
                                Status = Convert.ToBoolean(o.Status),
                                Title = o.Title
                            });
                        });
                        if (dicts.Any())
                        {
                            var list = DictRepository.GetQuery().ToList();
                            DictRepository.RemoveRange(list);
                            DictRepository.AddRange(dicts);
                        }
                    }
                    var sqls = new Dictionary<int, string>();
                    var db = new DBFramework.DBHelper();
                    if (ver.ProductDataVer != null)
                    {
                        ver.ProductDataVer.ProductDataSqls.OrderBy(o => o.RunSort).Each(o =>
                        {
                            sqls[o.MenuId] = o.RunSql;
                        });
                        foreach (var de in sqls)
                        {
                            try
                            {
                                db.ExecuteNonQueryText(de.Value, null);
                            }
                            catch (Exception ex)
                            {
                                throw new Exception("更新初始化数据失败！MenuId:" + de.Key + "；" + LogEngine.ToInnerException(ex).Message);
                            }
                        }
                        sqls.Clear();
                    }
                    ver.ProductPublishSqls.OrderBy(o => o.RunSort).Each(o =>
                    {
                        sqls[o.MenuId] = o.RunSql;
                    });
                    foreach (var de in sqls)
                    {
                        try
                        {
                            db.ExecuteNonQueryText(de.Value, null);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("历史数据处理失败！MenuId:" + de.Key + "；" + LogEngine.ToInnerException(ex).Message);
                        }
                    }
                    tran.Complete();
                }
            }catch(Exception e)
            {
                LogEngine.WriteError(e);
                if (ver != null)
                    PublishVerService.AddUpdateLog(ver.PublishId, 0, false, LogEngine.ToInnerException(e).Message, CurrentUser.FullName);
                return OpResult.Fail();
            }
            return OpResult.Success();
        }
    }
}
