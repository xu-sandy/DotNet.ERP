﻿using Pharos.Logic.BLL;
using Pharos.Utility;
using Pharos.Utility.Helpers;
using System.Collections.Generic;
using System.Web.Mvc;
using Pharos.CRM.Retailing.Models;
using System;
using System.Linq;
using Pharos.Logic;
using System.Transactions;
using System.ServiceModel;
using System.EnterpriseServices;
using Pharos.CRM.Retailing.ServiceReference1;
using Pharos.Sys.Entity;

namespace Pharos.CRM.Retailing.Controllers
{
    public class HomeController : Controller
    {

        private readonly Pharos.Sys.BLL.SysMenuBLL _menuBLL = new Pharos.Sys.BLL.SysMenuBLL();
        #region 首页
        public ActionResult Index()
        {
            //if (!Sys.CurrentUser.IsLogin || Sys.CurrentUser.IsStore)
            //{
            //    return RedirectToAction("Login", "Account");
            //}    
 
            #region sync
            //ServiceReference1.Service1Client client = new ServiceReference1.Service1Client();
            //ChannelFactory<Sync.IService1> myFactory = new ChannelFactory<Sync.IService1>("endpointConfig");
            //Sync.IService1 client = myFactory.CreateChannel();
            //ServiceReference2.AccountClient client = new ServiceReference2.AccountClient();
            //using (TransactionScope tran = new TransactionScope())
            //{
            //    var pv = client.GetPublish(CommonService.CompanyId, 1);
            //    if (pv != null)
            //    {
            //        var menus = new List<Sys.Entity.SysMenus>();
                    //if (pv.ProductMenusk__BackingField != null)
                    //{
                    //    Pharos.Logic.BLL.SysMenuBLL menuBll = new SysMenuBLL();
                    //    menus = pv.ProductMenusk__BackingField.Select(o => new Sys.Entity.SysMenus()
                    //    {
                    //        CompanyId = CommonService.CompanyId,
                    //        MenuId = o.MenuIdk__BackingField,
                    //        PMenuId = o.PMenuIdk__BackingField,
                    //        SortOrder = o.SortOrderk__BackingField,
                    //        Status = o.Statusk__BackingField,
                    //        Title = o.Titlek__BackingField,
                    //        Type = 0,
                    //        URL = o.Urlk__BackingField
                    //    }).ToList();
                    //    menuBll.SyncMenu(menus);
                    //}
                    //if (pv.ProductLimitsk__BackingField != null)
                    //{
                    //    Pharos.Logic.BLL.SysLimitsBLL limitBll = new SysLimitsBLL();
                    //    var limits = menus.Select(o => new Sys.Entity.SysLimits()
                    //    {
                    //        CompanyId = o.CompanyId,
                    //        LimitId = o.MenuId,
                    //        PLimitId = 0,
                    //        Depth = 1,
                    //        Status = Convert.ToInt16(o.Status),
                    //        Title = o.Title
                    //    }).ToList();

                    //    limits.AddRange(pv.ProductLimitsk__BackingField.Select(o => new Sys.Entity.SysLimits()
                    //    {
                    //        CompanyId = CommonService.CompanyId,
                    //        LimitId = o.LimitIdk__BackingField.GetValueOrDefault(),
                    //        PLimitId = o.MenuIdk__BackingField.GetValueOrDefault(),
                    //        Depth = 2,
                    //        Status = Convert.ToInt16(o.Statusk__BackingField.GetValueOrDefault()),
                    //        Title = o.Titlek__BackingField
                    //    }));

                    //    limitBll.SyncLimit(limits);
                    //}
            //    }
            //    tran.Complete();
            //}
            //client.Close();
            #endregion

            #region 验证
            //1为单商户版本，其他值为多商户版本
            string ver = Pharos.Utility.Config.GetAppSettings("ver");
            //单商户版本
            if (ver == "1")
            {
                if (!Sys.CurrentUser.IsLogin || Sys.CurrentUser.IsStore)
                {
                    Response.Redirect("/Account/Login");
                }
            }
            //多商户版本
            else
            {
                //二级域名
                string d = "";

                //二级域名
                string dom = "";
                if (!RouteData.Values["dom"].IsNullOrEmpty())
                {
                    dom = RouteData.Values["dom"].ToString();
                }
                //一级域名
                string d1 = "";
                if (!RouteData.Values["d1"].IsNullOrEmpty())
                {
                    d1 = RouteData.Values["d1"].ToString();
                }
                //顶级域名
                string d0 = "";
                if (!RouteData.Values["d0"].IsNullOrEmpty())
                {
                    d0 = RouteData.Values["d0"].ToString();
                }

                if (!d0.IsNullOrEmpty())
                {
                    if (!dom.IsNullOrEmpty())
                    {
                        d = dom;
                    }
                }

                //输入保留二级域名:store
                if (!RouteData.Values["cid"].IsNullOrEmpty())
                {
                    d = "store";
                }
                
                //localhost访问、ip访问
                if ((dom.ToLower().Trim() == "localhost") || (dom.IsNullOrEmpty() && d1.IsNullOrEmpty() && d0.IsNullOrEmpty()))
                {
                    if (!Sys.CurrentUser.IsLogin || Sys.CurrentUser.IsStore)
                    {
                        Response.Redirect("/Account/Login");
                        return null;
                    }
                }
                //域名访问
                else
                {
                    //API的CID
                    int cID = Authorize.getCID(d);

                    //输入保留二级域名:store
                    if (d.ToLower().Trim().Contains("store") && cID == -1)
                    {
                        //门店
                        Response.Redirect("/Store/Index");
                        return null;
                    }
                    //请求API发生错误
                    else if (cID == -2)
                    {
                        Response.Redirect("/Account/error");
                        return null;
                    }
                    //输入的二级域名是空
                    else if (cID == -1)
                    {
                        Response.Redirect("/Account/noBusiness");
                        return null;
                    }
                    //输入的域名不存在商户
                    else if (cID == 0)
                    {
                        Response.Redirect("/Account/noBusiness");
                        return null;
                    }
                    //输入的域名是保留二级域名
                    else if (cID == -3)
                    {
                        //在crm里面
                        if (d.ToLower() == "erp")
                        {
                            if (!Sys.CurrentUser.IsLogin || Sys.CurrentUser.IsStore)
                            {
                                Response.Redirect("/Account/Login");
                                return null;
                            }
                        }
                        //不在crm里面
                        else
                        {
                            Response.Redirect("/Account/noBusiness");
                            return null;
                        }
                    }
                    //输入的域名存在商户
                    else if(cID>0)
                    {
                        var obj = UserInfoService.Find(o => o.CompanyId == cID);
                        //CID在目前项目不存在
                        if (obj == null)
                        {
                            Response.Redirect("/Account/noUser?cid=" + cID);
                            return null;
                        }
                        else
                        {
                            if (!Sys.CurrentUser.IsLogin || Sys.CurrentUser.IsStore)
                            {
                                Response.Redirect("/Account/Login");
                                return null;
                            }
                            else
                            {
                                if (Cookies.IsExist("remuc"))
                                {
                                    //cookie的CID
                                    string cid = Cookies.Get("remuc", "_cid");
                                    if (cid.IsNullOrEmpty())
                                    {
                                        cid = "0";
                                    }

                                    if (cID != Convert.ToInt32(cid))
                                    {
                                        Response.Redirect("/Account/Login");
                                        return null;
                                    }
                                }
                            }
                        }

                    }
                }
            }
            #endregion

            //获取活动列表 
            var activityList = CommodityPromotionService.GetNewestActivity(3);
            //获取公告列表
            var noticeList = NoticeService.GetNewestNotice(3);
            //采购订单列表
            ViewBag.OrderList = OrderService.GetNewOrder(3);

            List<ActivityNoticeModel> activityNoticeList = new List<ActivityNoticeModel>();
            if (activityList != null)
            {
                foreach (var activity in activityList)
                {
                    activityNoticeList.Add(new ActivityNoticeModel(activity.Id, Enum.GetName(typeof(PromotionType), activity.PromotionType),
                        DateTime.Parse(activity.StartDate.ToString()).ToString("yyyy-MM-dd") + "至" + DateTime.Parse(activity.EndDate.ToString()).ToString("yyyy-MM-dd"),
                        Enum.GetName(typeof(SaleState), activity.State), activity.CreateDT, 1));
                }
            }
            if (noticeList != null)
            {
                foreach (var notice in noticeList)
                {
                    activityNoticeList.Add(new ActivityNoticeModel(notice.Id.ToString(), notice.Theme, notice.BeginDate.ToString("yyyy-MM-dd") + "至" + notice.ExpirationDate.ToString("yyyy-MM-dd"),
                        notice.State == 1 ? "已发布":"未发布",notice.CreateDT,2));
                }
            }
            activityNoticeList = activityNoticeList.OrderByDescending(o => o.CreateDT).Take(3).ToList();
            if (activityNoticeList == null)
                activityNoticeList = new List<ActivityNoticeModel>();
            ViewBag.activityNoticeList = activityNoticeList;//活动公告


            //todo: 模拟数据
            string mode = Request["mode"];
            ViewBag.accessCount = 0;

            ViewBag.WelcomeText = "欢迎光临";
            ViewBag.CurUserName = Sys.CurrentUser.FullName;
            ViewBag.CurLoginName = Sys.CurrentUser.UserName;

            //近3天数据
            var beginTime = DateTime.Parse(DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd"));
            var endTime = DateTime.Parse(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"));
            ViewBag.newMemberNumber = MembersService.GetNewMemberNumber(beginTime, endTime);//新增会员数量
            ViewBag.newSalesVolume = ReportBLL.GetSalesVolume(beginTime, endTime);//新增销售量
            var saleOrderList3Day = SaleOrdersService.GetIndexSaleOrder(beginTime, endTime);//3天内的销售订单
            ViewBag.newSaleOrderNumber = saleOrderList3Day.Count();//新增客单量
            decimal newSaleTotal = 0;
            newSaleTotal = saleOrderList3Day.Sum(o => o.Receive);
            ViewBag.newSaleTotal = newSaleTotal;//新增销售额

            //近7天数据
            var dayTitleList = new List<string>();
            var saleTotalList = new List<decimal>();
            var saleOederNumberList = new List<int>();
            var hotProductNameList = new List<string>();
            var hotProductSaleNumList = new List<int>();
            for (int i = 6; i >= 0; i--)
            {
                var time1 = DateTime.Parse(DateTime.Now.AddDays(0 - i).ToString("yyyy-MM-dd"));
                var time2 = DateTime.Parse(DateTime.Now.AddDays(0 - i + 1).ToString("yyyy-MM-dd"));
                var saleOrderList = SaleOrdersService.GetIndexSaleOrder(time1, time2);

                dayTitleList.Add(int.Parse(DateTime.Now.AddDays(0 - i).ToString("dd")) + "日");
                saleTotalList.Add(saleOrderList.Sum(o => o.Receive));
                saleOederNumberList.Add(saleOrderList.Count());
            }

            var hotProductBeginTime = DateTime.Parse(DateTime.Now.AddDays(-6).ToString("yyyy-MM-dd"));
            var hotProductEndTime = DateTime.Parse(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"));

            ReportBLL.GetHotProduct(hotProductBeginTime, hotProductEndTime, out hotProductNameList, out hotProductSaleNumList);

            //近7天热销商品
            ViewBag.hotProductNameList = hotProductNameList.ToJson();
            ViewBag.hotProductSaleNumList = hotProductSaleNumList.ToJson();
            ViewBag.hotProductNameListNotJson = hotProductNameList;

            ViewBag.dayTitleList = dayTitleList.ToJson();
            ViewBag.saleTotalList = saleTotalList.ToJson();//近7天销售额
            ViewBag.saleOederNumberList = saleOederNumberList.ToJson();//近7天客单量

            var list = new List<Pharos.Sys.Models.MenuModel>();
            list = _menuBLL.GetHomeMenusByUID(Sys.CurrentUser.UID);
            ViewBag.Menus = list;
            var set= new Pharos.Sys.BLL.SysWebSettingBLL().GetWebSetting();
            //var comp = Authorize.GetCompanyByConnect(null);
            ViewBag.comptitle = set == null ? "ERP管理平台" : set.SysName;
            return View(set??new Sys.Entity.SysWebSetting());
        }
        #endregion

        #region 旧首页
        public ActionResult Old_Index()
        {
            if (!Sys.CurrentUser.IsLogin || Sys.CurrentUser.IsStore)
            {
                return RedirectToAction("Login", "Account");
            }
            //获取公告列表
            ViewBag.NoticeList = NoticeService.GetNewestNotice(3);
            //获取活动列表 
            ViewBag.ActivityList = CommodityPromotionService.GetNewestActivity(3);
            ViewBag.OrderList = OrderService.GetNewOrder(7);
            //todo: 模拟数据
            string mode = Request["mode"];
            ViewBag.accessCount = 0;

            ViewBag.WelcomeText = "欢迎光临";
            ViewBag.CurUserName = Sys.CurrentUser.FullName;
            ViewBag.CurLoginName = Sys.CurrentUser.UserName;

            var categories = new List<string>();
            var stores = new List<string>();
            var hours = new TimeSpan[] { new TimeSpan(8, 0, 0), new TimeSpan(10, 0, 0), new TimeSpan(12, 0, 0), new TimeSpan(14, 0, 0), new TimeSpan(16, 0, 0), new TimeSpan(18, 0, 0), new TimeSpan(20, 0, 0) };
            var list = new List<Pharos.Sys.Models.MenuModel>();
            list = _menuBLL.GetHomeMenusByUID(Sys.CurrentUser.UID);
            ViewBag.Menus = list;
            ViewBag.categoryData = ReportBLL.QueryIndexSaleCategorys(ref categories).ToJson();
            ViewBag.chart1Data = ReportBLL.QueryIndexSaleCategorys(ref categories).FirstOrDefault() == null ? new List<object>() : ReportBLL.QueryIndexSaleCategorys(ref categories).FirstOrDefault().data;
            ViewBag.categoryJSON = categories.ToJson();
            ViewBag.hourData = ReportBLL.QueryIndexSaleHour(categories, hours).ToJson();
            ViewBag.chart2Data = ReportBLL.QueryIndexSaleHour(categories, hours).FirstOrDefault() == null ? new List<object>() : ReportBLL.QueryIndexSaleHour(categories, hours).FirstOrDefault().data;
            ViewBag.hoursJSON = hours.Select(o => o.Hours.ToString("00") + ":" + o.Minutes.ToString("00")).ToJson();
            ViewBag.storeData = ReportBLL.QueryIndexSaleStore(categories, ref stores).ToJson();
            ViewBag.chart3Data = ReportBLL.QueryIndexSaleStore(categories, ref stores).FirstOrDefault() == null ? new List<object>() : ReportBLL.QueryIndexSaleStore(categories, ref stores).FirstOrDefault().data;
            ViewBag.storeJSON = stores.ToJson();
            return View();
        }
        #endregion

        public ActionResult Logout()
        {
            Sys.CurrentUser.Exit();
            //if(CurrentUser.StoreId=="sup")//供应平台
            //    return RedirectToAction("Login", "Supplier");
            Session.Clear();
            Session.Abandon();
            Cookies.Remove("ASP.NET_SessionId");//重新会话ID
            return RedirectToAction("Login", "Account");
        }
        /// <summary>
        /// 获得首页弹窗提醒
        /// </summary>
        /// <param name="type">提醒类型</param>
        /// <returns>提醒列表（最多6条）</returns>
        [HttpPost]
        public ActionResult GetRemind(string type)
        {
            List<RemindModel> rmList = new List<RemindModel>();
            switch (type.ToLower())
            {
                case "stockout"://缺货提醒
                    var datas = CommodityService.GetStockout().GroupBy(o => o.Key);
                    foreach (var item in datas)
                    {
                        rmList.Add(new RemindModel(item.Key + "缺货提醒", item.Key + "以下商品缺货：<br/>" + string.Join(",", item.Select(o => o.Value))));
                    }
                    break;
                case "activity"://活动提醒
                    Dictionary<short, string> promotionTypeDict = new Dictionary<short, string>();
                    //1:单品折扣、 2:捆绑促销、 3:组合促销、4:买赠促销、 5:满元促销
                    promotionTypeDict.Add(1, "单品折扣");
                    promotionTypeDict.Add(2, "捆绑促销");
                    promotionTypeDict.Add(3, "组合促销");
                    promotionTypeDict.Add(4, "买赠促销");
                    promotionTypeDict.Add(5, "满元促销");
                    var promotions = CommodityPromotionService.GetNewestActivity(10);
                    foreach (var item in promotions)
                    {
                        var storeids = item.StoreId.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        var stores = WarehouseService.FindList(o => storeids.Contains(o.StoreId)).Select(o => o.Title);
                        rmList.Add(
                            new RemindModel(
                           string.Format(
                                 "{1}~{2} {0}",
                                 promotionTypeDict[item.PromotionType], (item.StartDate ?? new DateTime()).ToString("yyyy-MM-dd"),
                                (item.EndDate ?? new DateTime()).ToString("yyyy-MM-dd")),
                         item.Id));
                    }
                    break;
                case "receive"://收货提醒
                    var orderDatas = OrderDistributionService.GetReceivedOrder();
                    foreach (var item in orderDatas)
                    {
                        rmList.Add(new RemindModel(string.Format("{0}有订单发货，请注意查收！", item.Store), string.Format("<br/>门店：{0}<br/>配送批次号：{1}<br/>订单编号：{2}<br/>", item.Store, item.DistributionBatch, item.IndentOrderId)));
                    }
                    break;
                case "expiration"://保质期到期提醒
                    var commodities = CommodityService.GetExpiresProduct();
                    foreach (var item in commodities)
                    {
                        rmList.Add(new RemindModel(string.Format("{0}已过期或将要过期", item.Key), string.Format("{0}将要过期<br/>过期时间：{1}", item.Key, item.Value.ExpirationDate)));
                    }
                    break;
                case "contract"://合同提醒
                    var contracts = ContractSerivce.GetContractRemind();
                    foreach (var item in contracts)
                    {
                        rmList.Add(new RemindModel(string.Format("<span style=\"width:120px;display:inline-block;\">{0}</span><span style=\"width:110px;display:inline-block;\">{1}</span><span style=\"width:110px;display:inline-block;\">{2}</span>", item.ContractSN, item.SupplierTitle, item.EndDate), 
                                  string.Format("合同编号：{0}<br/>供应商：{1}<br/>结束日期：{2}",item.ContractSN,item.SupplierTitle,item.EndDate)));
                    }
                    break;
            }

            return new JsonNetResult(rmList);
        }

        /// <summary>
        /// 各角色首页
        /// </summary>
        /// <returns></returns>
        public ActionResult AdminIndex()
        {
            return View();
        }
        #region 升级
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> GetHasPublish()
        {
            using(var client = new ServiceReference1.Service1Client())
            {
                var ver =await client.GetHasPublishAsync(CommonService.CompanyId, 2);
                client.Close();
                return Content(ver);
            }
        }
        [HttpPost]
        public ActionResult UpdateData()
        {
            ProductPublishVer ver = null;
            using(var client = new ServiceReference1.Service1Client())
            {
                try
                {
                    //TransactionOptions options=new TransactionOptions();
                    //options.IsolationLevel=IsolationLevel.Serializable;
                    //options.Timeout = new TimeSpan(0, 0, 60);
                    using (TransactionScope tran = new TransactionScope(TransactionScopeOption.Required))
                    {
                        ver = client.UpdatePublish(CommonService.CompanyId, 2, Sys.CurrentUser.FullName);
                        #region 菜单和权限
                        if (ver.ProductModuleVerk__BackingField != null)
                        {
                            var menus = new List<SysMenus>();
                            var limits = new List<SysLimits>();
                            ver.ProductModuleVerk__BackingField.ProductMenuLimitsk__BackingField.Each(o =>
                            {
                                switch (o.Typek__BackingField)
                                {
                                    case 1:
                                        menus.Add(new SysMenus() { MenuId = o.MenuIdk__BackingField, PMenuId = o.PMenuIdk__BackingField, SortOrder = o.SortOrderk__BackingField, Status = o.Statusk__BackingField, Title = o.Titlek__BackingField, URL = o.Urlk__BackingField,CompanyId=CommonService.CompanyId });
                                        break;
                                    case 3:
                                        limits.Add(new SysLimits() { LimitId = o.MenuIdk__BackingField, PLimitId = o.PMenuIdk__BackingField, SortOrder = o.SortOrderk__BackingField, Status = Convert.ToInt16(o.Statusk__BackingField), Title = o.Titlek__BackingField, CompanyId = CommonService.CompanyId });
                                        break;
                                    default:
                                        break;
                                }
                            });
                            if (menus.Any())
                            {
                                var list = SysMenuBLL.FindList(o => o.CompanyId == CommonService.CompanyId);
                                SysMenuBLL.Delete(list);
                                SysMenuBLL.AddRange(menus);
                            }
                            if (limits.Any())
                            {
                                var list = SysLimitsBLL.FindList(o => o.CompanyId == CommonService.CompanyId);
                                SysLimitsBLL.Delete(list);
                                SysLimitsBLL.AddRange(limits);
                            }
                        }
                        #endregion
                        #region 角色
                        if (ver.ProductRoleVerk__BackingField != null)
                        {
                            var roles = new List<SysRoles>();
                            var customMenus = new List<SysCustomMenus>();
                            ver.ProductRoleVerk__BackingField.ProductRolesk__BackingField.Each(o =>
                            {
                                var role = new SysRoles();
                                role.CompanyId = CommonService.CompanyId;
                                role.RoleId = o.RoleIdk__BackingField.GetValueOrDefault();
                                role.Title = o.Titlek__BackingField;
                                role.LimitsIds = o.Limitidsk__BackingField??"";
                                //role.Type = 0;
                                role.Status = true;
                                var cm = new SysCustomMenus()
                                {
                                    CompanyId = role.CompanyId,
                                    Type = 2,
                                    ObjId = role.RoleId,
                                    MenuIds = string.Join(",", o.ProductRoleDatask__BackingField.Where(i => i.HasSelectedk__BackingField).OrderBy(i=>i.SortOrderk__BackingField).Select(i => i.MenuIdk__BackingField))
                                };
                                roles.Add(role);
                                customMenus.Add(cm);
                            });
                            if (roles.Any())
                            {
                                var list = SysRoleBLL.FindList(o=>o.CompanyId==CommonService.CompanyId);
                                roles.Each(o =>
                                {
                                    var r = list.FirstOrDefault(i => i.RoleId == o.RoleId);
                                    if (r != null)
                                    {
                                        //o.DeptId = r.DeptId;
                                        if(!r.Memo.IsNullOrEmpty())
                                            o.Memo = r.Memo;
                                        o.Status = r.Status;
                                    }
                                });
                                SysRoleBLL.RemoveCustomMenus(SysRoleBLL.GetCustomMenus());
                                SysRoleBLL.Delete(list);
                                SysRoleBLL.AddRange(roles);
                                SysRoleBLL.AddCustomMenus(customMenus);
                            }
                        }
                        #endregion
                        #region 字典
                        if (ver.ProductDictionaryVerk__BackingField != null)
                        {
                            var dicts = new List<SysDataDictionary>();
                            ver.ProductDictionaryVerk__BackingField.ProductDictionaryDatask__BackingField.Each(o =>
                            {
                                dicts.Add(new SysDataDictionary()
                                {
                                    Depth = o.Depthk__BackingField,
                                    DicPSN = o.DicPSNk__BackingField,
                                    DicSN = o.DicSNk__BackingField,
                                    //HasChild = o.HasChildk__BackingField,
                                    SortOrder = o.SortOrderk__BackingField,
                                    Status = Convert.ToBoolean(o.Statusk__BackingField),
                                    Title = o.Titlek__BackingField
                                });
                            });
                            if (dicts.Any())
                            {
                                var list = SysDataDictService.FindList(o => o.CompanyId == CommonService.CompanyId);
                                SysDataDictService.Delete(list);
                                SysDataDictService.AddRange(dicts);
                            }
                        }
                        #endregion
                        #region 初始化数据
                        var sqls = new Dictionary<int, string>();
                        var db = new Pharos.DBFramework.DBHelper();
                        if (ver.ProductDataVerk__BackingField != null)
                        {
                            ver.ProductDataVerk__BackingField.ProductDataSqlsk__BackingField.OrderBy(o => o.RunSortk__BackingField).Each(o =>
                            {
                                sqls[o.MenuIdk__BackingField] = o.RunSqlk__BackingField;
                            });
                            foreach (var de in sqls)
                            {
                                try
                                {
                                    var sql = de.Value;
                                    if (sql.Contains("@"))
                                    {
                                        System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"@\b[a-zA-Z0-9]*");
                                        sql=sql.Replace(regex.Match(sql).ToString(),CommonService.CompanyId.ToString());
                                    }
                                    db.ExecuteNonQueryText(sql, null);
                                }
                                catch (Exception ex)
                                {
                                    throw new Exception("更新初始化数据失败！MenuId:" + de.Key + "；" + Pharos.Sys.LogEngine.ToInnerException(ex).Message);
                                }
                            }
                            sqls.Clear();
                        }
                        #endregion
                        #region 历史数据处理
                        ver.ProductPublishSqlsk__BackingField.OrderBy(o => o.RunSortk__BackingField).Each(o =>
                        {
                            sqls[o.MenuIdk__BackingField] = o.RunSqlk__BackingField;
                        });
                        foreach (var de in sqls)
                        {
                            try
                            {
                                db.ExecuteNonQueryText(de.Value, null);
                            }
                            catch (Exception ex)
                            {
                                throw new Exception("历史数据处理失败！MenuId:" + de.Key + "；" + Pharos.Sys.LogEngine.ToInnerException(ex).Message);
                            }
                        }
                        #endregion
                        tran.Complete();
                    }
                }
                catch (Exception e)
                {
                    new Pharos.Sys.LogEngine().WriteError(e);
                    if (ver != null)
                        client.AddUpdateLog(ver.PublishIdk__BackingField, CommonService.CompanyId, false, Pharos.Sys.LogEngine.ToInnerException(e).Message, Sys.CurrentUser.FullName);
                    return Content(OpResult.Fail().ToJson());
                }
                client.Close();
                return Content(OpResult.Success().ToJson());
            }
        }
        #endregion
    }
}