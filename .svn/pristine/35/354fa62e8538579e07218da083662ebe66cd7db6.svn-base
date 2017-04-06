using Pharos.Logic;
using Pharos.Logic.BLL;
using Pharos.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pharos.Utility.Helpers;
using Pharos.Utility;
using Pharos.POS.StoreServer;
using Pharos.POS.StoreServer.Models;

namespace Pharos.POS.StoreServer.Controllers
{
    public class StoreController : BaseController
    {
        //
        // GET: /Store/
        private readonly Pharos.Sys.BLL.SysMenuBLL _menuBLL = new Pharos.Sys.BLL.SysMenuBLL();

        #region 首页
        public ActionResult Index()
        {
            #region 如未登录 或 非门店登录，则跳转到 门店登录页
            if (!Sys.CurrentStoreUser.IsLogin)
            {
                Sys.CurrentStoreUser.Exit();
                return RedirectToAction("Login");
                //Response.Redirect("/Store/Login");
            }
            #endregion
            /*
            csID csid2 = ipLocalhost();

            //url中cid
            string s_cid = "";

            if (!RouteData.Values["cid"].IsNullOrEmpty())
            {
                s_cid = RouteData.Values["cid"].ToString();
            }
            //url中sid
            string s_sid = "";

            if (!RouteData.Values["sid"].IsNullOrEmpty())
            {
                s_sid = RouteData.Values["sid"].ToString();
            }

            if (csid2.message == "禁止访问")
            {
                Response.Redirect("/Account/noBusiness");
                return null;
            }

            if (!s_cid.IsNullOrEmpty() && !s_sid.IsNullOrEmpty())
            {
                Authorize authorize = new Authorize();
                csID csid = authorize.getCidSid(s_cid, s_sid);
                if (csid.message == "格式错误")
                {
                    Response.Redirect("/Account/noBusiness");
                    return null;
                }
                else if (csid.message == "域名的store后面必须是数字")
                {
                    Response.Redirect("/Account/noBusiness");
                    return null;
                }
                else if (csid.message == "success")
                {
                    //如未登录 或 非门店登录，则跳转到 门店登录页
                    if (!Sys.CurrentUser.IsLogin || !Sys.CurrentUser.IsStore)
                    {
                        Sys.CurrentUser.Exit();

                        //ip、localhost门店登录
                        if (csid2.message == "localhost" || csid2.message == "ip")
                        {
                            Response.Redirect("/store" + csid2.cid + "-" + csid2.sid);
                            return null;
                        }

                        Response.Redirect("/Store/Login");
                        return null;
                    }
                    else
                    {
                        if (Cookies.IsExist("remuc"))
                        {
                            //cookie的CID
                            string cid = Cookies.Get("remuc", "_cid").Trim();
                            //cookie的门店ID
                            string sid = Server.UrlDecode(Cookies.Get("remuc", "_storeId"));
                            sid = sid.Split('~')[0];

                            string ss = sid.Split('~')[0];

                            if (cid.IsNullOrEmpty())
                            {
                                cid = "0";
                            }
                            if (sid.IsNullOrEmpty())
                            {
                                sid = "0";
                            }

                            if (csid.cid.Trim() != cid || csid.sid.Trim() != sid)
                            {
                                Sys.CurrentUser.Exit();

                                //ip、localhost门店登录
                                if (csid2.message == "localhost" || csid2.message == "ip")
                                {
                                    Response.Redirect("/store" + csid2.cid + "-" + csid2.sid);
                                    return null;
                                }

                                Response.Redirect("/Store/Login");
                                return null;
                            }
                        }
                    }

                }
            }
            else
            {
                Response.Redirect("/Account/noBusiness");
                return null;
            }
            //是否localhost、ip门店登录
            string isLocalhostIp = "0";
            if (csid2.message == "localhost" || csid2.message == "ip")
            {
                isLocalhostIp = "1";
            }
            */
            ViewBag.isLocalhostIp = "1";
            ViewBag.cid = Pharos.Utility.Config.GetAppSettings("CompanyId");
            ViewBag.sid = Sys.SysCommonRules.CurrentStore;


            ViewBag.WelcomeText = "欢迎光临";
            ViewBag.CurUserName = Sys.CurrentStoreUser.FullName;
            ViewBag.CurLoginName = Sys.CurrentStoreUser.UserName;
            ViewBag.comptitle = Sys.CurrentStoreUser.StoreName;
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
                        notice.State == 1 ? "已发布" : "未发布", notice.CreateDT, 2));
                }
            }
            activityNoticeList = activityNoticeList.OrderByDescending(o => o.CreateDT).Take(3).ToList();
            if (activityNoticeList == null)
                activityNoticeList = new List<ActivityNoticeModel>();
            ViewBag.activityNoticeList = activityNoticeList;//活动公告

            //近3天数据
            var beginTime = DateTime.Parse(DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd"));
            var endTime = DateTime.Parse(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"));
            ViewBag.newMemberNumber = MembersService.GetNewMemberNumber(beginTime, endTime, Sys.CurrentStoreUser.StoreId);//新增会员数量
            ViewBag.newSalesVolume = ReportBLL.GetSalesVolume(beginTime, endTime, Sys.CurrentStoreUser.StoreId);//新增销售量
            var saleOrderList3Day = SaleOrdersService.GetIndexSaleOrder(beginTime, endTime, Sys.CurrentStoreUser.StoreId);//3天内的销售订单
            ViewBag.newSaleOrderNumber = saleOrderList3Day.Count();//新增客单量
            decimal newSaleTotal = 0;
            newSaleTotal = saleOrderList3Day.Sum(o => o.TotalAmount);
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
                var saleOrderList = SaleOrdersService.GetIndexSaleOrder(time1, time2, Sys.CurrentStoreUser.StoreId);

                dayTitleList.Add(int.Parse(DateTime.Now.AddDays(0 - i).ToString("dd")) + "日");
                saleTotalList.Add(saleOrderList.Sum(o => o.TotalAmount));
                saleOederNumberList.Add(saleOrderList.Count());
            }

            var hotProductBeginTime = DateTime.Parse(DateTime.Now.AddDays(-6).ToString("yyyy-MM-dd"));
            var hotProductEndTime = DateTime.Parse(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"));

            ReportBLL.GetHotProduct(hotProductBeginTime, hotProductEndTime, out hotProductNameList, out hotProductSaleNumList, Sys.CurrentStoreUser.StoreId);

            //近7天热销商品
            ViewBag.hotProductNameList = hotProductNameList.ToJson();
            ViewBag.hotProductSaleNumList = hotProductSaleNumList.ToJson();
            ViewBag.hotProductNameListNotJson = hotProductNameList;

            ViewBag.dayTitleList = dayTitleList.ToJson();
            ViewBag.saleTotalList = saleTotalList.ToJson();//近7天销售额
            ViewBag.saleOederNumberList = saleOederNumberList.ToJson();//近7天客单量

            var list = new List<Pharos.Sys.Models.MenuModel>();
            //list = _menuBLL.GetHomeMenusByUID(Sys.CurrentStoreUser.UID, 1);
            //var pids = new string[] { "1", "3", "15", "25", "68" };
            //传秤门店固定菜单
            list.Add(new Sys.Models.MenuModel()
            {
                Id = "165",
                Level = 0,
                Name = "商品传秤",
                ParentId = "0",
                Url = null,
                Value = "165",
                Children = new List<Sys.Models.MenuModel>() { 
                    new Sys.Models.MenuModel(){
                    Id = "166", 
                    Level = 1, 
                    Name = "下发到电子秤", 
                    ParentId = "165", 
                    Url = "/Product/ProductWeight", 
                    Value = "166", 
                    Children=null
                    },
                    new Sys.Models.MenuModel(){
                    Id = "167", 
                    Level = 1, 
                    Name = "电子秤设备配置", 
                    ParentId = "165", 
                    Url = "/Product/ProductWeightSetting", 
                    Value = "167", 
                    Children=null
                    }
                }
            });
            ViewBag.Menus = list;
            var set = new Pharos.Sys.BLL.SysWebSettingBLL().GetWebSetting();
            return View(set);
        }
        #endregion



        #region 登陆
        public ActionResult Login(string id)
        {
            /*var user = new UserLogin();
            csID csid2 = ipLocalhost();

            //url中cid
            string s_cid = "";

            if (!RouteData.Values["cid"].IsNullOrEmpty())
            {
                s_cid = RouteData.Values["cid"].ToString();
            }
            //url中sid
            string s_sid = "";

            if (!RouteData.Values["sid"].IsNullOrEmpty())
            {
                s_sid = RouteData.Values["sid"].ToString();
            }

            if (csid2.message == "禁止访问")
            {
                Response.Redirect("/Account/noBusiness");
                return null;
            }

            csID csid = new csID();
            if (!s_cid.IsNullOrEmpty() && !s_sid.IsNullOrEmpty())
            {
                Authorize authorize = new Authorize();
                csid = authorize.getCidSid(s_cid, s_sid);
                if (csid.message == "格式错误")
                {
                    Response.Redirect("/Account/noBusiness");
                    return null;
                }
                else if (csid.message == "域名的store后面必须是数字")
                {
                    Response.Redirect("/Account/noBusiness");
                    return null;
                }
                else if (csid.message == "success")
                {
                    user.CID = Convert.ToInt32(csid.cid);
                    if (Cookies.IsExist("remuc"))
                    {
                        //cookie的CID
                        string cid = Cookies.Get("remuc", "_cid").Trim();
                        //cookie的门店ID
                        string sid = Server.UrlDecode(Cookies.Get("remuc", "_storeId"));
                        sid = sid.Split('~')[0];

                        if (cid.IsNullOrEmpty())
                        {
                            cid = "0";
                        }
                        if (sid.IsNullOrEmpty())
                        {
                            sid = "0";
                        }

                        if (csid.cid.Trim() == cid || csid.sid.Trim() == sid)
                        {
                            user.UserName = Cookies.Get("remuc", "_uname");
                            user.UserPwd = Cookies.Get("remuc", "_pwd");
                            user.StoreId = Server.UrlDecode(Cookies.Get("remuc", "_storeId"));
                            user.RememberMe = true;
                        }
                    }
                }
            }
            else
            {
                Response.Redirect("/Account/noBusiness");
                return null;
            }
            

            List<SelectListItem> list = ListToSelect(WarehouseService.GetAdminList(Convert.ToInt32(csid.cid), csid.sid).Select(o => new SelectListItem() { Text = o.Title, Value = o.StoreId + "~" + o.Title, Selected = o.StoreId == csid.sid })).ToList();
            if (list.Count == 0)
            {
                Response.Redirect("/Account/error?msg=" + Pharos.Utility.DESEncrypt.Encrypt("无效门店，请联系管理员检查该门店是否存在或开放！"));
                return null;
            }
            

            ViewBag.stores = list;
            return View(user);
            */

            var user = new UserLogin();
            user.CID = Pharos.Utility.Config.GetAppSettings("CompanyId").ToType<int>();
            user.StoreId = Sys.SysCommonRules.CurrentStore;
            ViewBag.stores = ListToSelect(WarehouseService.GetAdminList().Select(o => new SelectListItem() { Text = o.Title, Value = o.StoreId + "~" + o.Title, Selected = o.StoreId == user.StoreId }));
            if (Cookies.IsExist("storeremuc"))
            {
                user.UserName = Cookies.Get("storeremuc", "_uname");
                user.StoreId = Server.UrlDecode(Cookies.Get("storeremuc", "_storeId"));
                user.RememberMe = true;
            }
            return View(user);

        }
        [HttpPost]
        public ActionResult Login(UserLogin user)
        {
            if (!ModelState.IsValid) return View(user);
            var obj = UserInfoService.GetStoreUserBy(user.UserName, Pharos.Utility.Security.MD5_Encrypt(user.UserPwd), user.StoreId.Split('~')[0]);
            if (obj == null)
            {
                ViewBag.msg = "帐户或密码输入不正确!";
                //ViewBag.stores = ListToSelect(WarehouseService.GetAdminList().Select(o => new SelectListItem() { Text = o.Title, Value = o.StoreId + "~" + o.Title }), emptyTitle: "请选择门店");
                ViewBag.stores = ListToSelect(WarehouseService.GetAdminList(user.CID, user.StoreId.Split('~')[0]).Select(o => new SelectListItem() { Text = o.Title, Value = o.StoreId + "~" + o.Title, Selected = o.StoreId == user.StoreId.Split('~')[0] }));
                return View(user);
            }
            //obj.StoreId = user.StoreId;
            obj.RoleIds = "10";
            new Sys.CurrentStoreUser().StoreLogin(obj, user.StoreId, user.RememberMe);

            //csID csid = ipLocalhost();
            //if (csid.message == "禁止访问")
            //{
            //    Response.Redirect("/Account/noBusiness");
            //    return null;
            //}
            ////ip、localhost门店登录
            //else if (csid.message == "localhost" || csid.message == "ip")
            //{
            //    Response.Redirect("/store"+csid.cid+"-"+csid.sid+"/Store/Index");
            //    return null;
            //}

            return Redirect(Url.Action("Index"));
        }
        public ActionResult Logout()
        {
            Sys.CurrentStoreUser.Exit();
            /*
            if (Request["isLocalhostIp"] != null && Request["cid"] != null && Request["sid"] != null)
            {
                //ip、localhost门店登录
                if (Request["isLocalhostIp"].Trim() == "1")
                {
                    Response.Redirect("/store" + Request["cid"].Trim() + "-" + Request["sid"].Trim());
                    return null;
                }
            }
            Response.Redirect("/");
            return null;*/

            return RedirectToAction("Login");
        }

        /// <summary>
        /// ip门店登录、localhost门店登录
        /// </summary>
        /// <returns></returns>
        public csID ipLocalhost()
        {
            //url中localhost
            string localhost = "";
            if (!RouteData.Values["localhost"].IsNullOrEmpty())
            {
                localhost = RouteData.Values["localhost"].ToString();
            }

            //url中cid
            string cid = "";
            if (!RouteData.Values["cid"].IsNullOrEmpty())
            {
                cid = RouteData.Values["cid"].ToString();
            }

            //url中sid
            string sid = "";
            if (!RouteData.Values["sid"].IsNullOrEmpty())
            {
                sid = RouteData.Values["sid"].ToString();
            }

            //url中ip1
            string ip1 = "";
            if (!RouteData.Values["ip1"].IsNullOrEmpty())
            {
                ip1 = RouteData.Values["ip1"].ToString();
            }

            //url中ip2
            string ip2 = "";
            if (!RouteData.Values["ip2"].IsNullOrEmpty())
            {
                ip2 = RouteData.Values["ip2"].ToString();
            }

            //url中ip3
            string ip3 = "";
            if (!RouteData.Values["ip3"].IsNullOrEmpty())
            {
                ip3 = RouteData.Values["ip3"].ToString();
            }

            //url中ip4
            string ip4 = "";
            if (!RouteData.Values["ip4"].IsNullOrEmpty())
            {
                ip4 = RouteData.Values["ip4"].ToString();
            }

            //Authorize authorize = new Authorize();
            //return authorize.ipLocalhostStore(localhost, ip1, ip2, ip3, ip4, cid, sid);
            return null;
        }

        #endregion

        #region 门店自动完成
        /// <summary>
        /// 输入自动完成商品
        /// </summary>
        /// <param name="searchName"></param>
        /// <param name="supplierID"></param>
        /// <returns></returns>
        public ActionResult GetStoreInput(string searchName, int? bigId)
        {
            var category = bigId.HasValue ? bigId.ToString() : "";
            var express = DynamicallyLinqHelper.Empty<Warehouse>()
                .And(o => (o.StoreId.StartsWith(searchName)) || o.Title.Contains(searchName), searchName.IsNullOrEmpty())
                .And(o => ("," + o.CategorySN + ",").Contains("," + category + ","), !bigId.HasValue);

            var list = BaseService<Warehouse>.FindList(express);
            return ToDataGrid(list, 0);
        }
        [HttpPost]
        public ActionResult GetStoreList(bool? showAll, string emptyTitle)
        {
            var list = WarehouseService.GetList(showAll.GetValueOrDefault()).Select(o => new DropdownItem() { Value = o.StoreId, Text = o.Title, IsSelected = Sys.CurrentStoreUser.StoreId == o.StoreId }).ToList();
            if (!emptyTitle.IsNullOrEmpty())
            {
                var isSel = list.Any(o => o.IsSelected);
                list.Insert(0, new DropdownItem("", emptyTitle, !isSel));
            }
            return new JsonNetResult(list);
        }

        #endregion
    }
}
