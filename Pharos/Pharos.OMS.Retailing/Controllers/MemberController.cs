﻿using Pharos.Logic.OMS;
using Pharos.Logic.OMS.Entity;
using Pharos.Logic.OMS.Entity.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pharos.Utility.Helpers;
using Pharos.Utility;
using Pharos.Logic.OMS.BLL;
using System.Data;

namespace Pharos.OMS.Retailing.Controllers
{
    public class MemberController : BaseController
    {

        [Ninject.Inject]
        TradersService tradersService { get; set; }

        [Ninject.Inject]
        VisitTrackService visitTrackService { get; set; }
        [Ninject.Inject]
        ImportSetService ImportSetService { get; set; }
        TradersBLL tBLL = new TradersBLL();

        [SysPermissionValidate]
        public ActionResult Index(int all=0)
        {
            //0是全部显示，1是部分显示
            ViewBag.all = all;
            //跟踪状态
            ViewBag.TrackStautsId = ListToSelect(tradersService.getDataList(205).Select(o => new SelectListItem() { Value = o.DicSN.ToString(), Text = o.Title }), emptyTitle: "全部");

            List<SysUser> list = new List<SysUser>();
            if (all == 0)
            {
                list = tradersService.getAllUserList();
            }
            else
            {
                list = tradersService.getUserList();
            }
            //业务员
            ViewBag.user = ListToSelect(list.Select(o => new SelectListItem() { Value = o.UserId, Text = o.FullName, Selected = (o.UserId == CurrentUser.UID&&all>0) }), emptyTitle: "全部");
            //客户分类
            ViewBag.TraderType = ListToSelect(tradersService.getTraderTypeList().Select(o => new SelectListItem() { Value = o.TraderTypeId.ToString(), Text = o.Title }), emptyTitle: "将所选项归类到指定客户分类");
            //经营类目
            ViewBag.BusinessScopeId = ListToSelect(tradersService.getBusinessList().Select(o => new SelectListItem() { Value = o.ById, Text = o.Title }), emptyTitle: "全部");
            //经营模式
            ViewBag.ModeId = ListToSelect(tradersService.getDataList().Select(o => new SelectListItem() { Value = o.DicSN.ToString(), Text = o.Title }), emptyTitle: "全部");
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="all">0是全部显示，1是部分显示</param>
        /// <returns></returns>
        public ActionResult FindPageList(int all = 0)
        {
            var count = 0;
            var list = tradersService.GetPageList(Request.Params, out count,all);
            return ToDataGrid(list, count);
        }

        /// <summary>
        /// 获取省
        /// </summary>
        /// <returns></returns>
        public ActionResult getProvince(string defaultTitle = "请选择")
        {
            var province = tradersService.getProvince(defaultTitle);
            return new JsonNetResult(province);
        }

        /// <summary>
        /// 获取城市
        /// </summary>
        /// <returns></returns>
        public ActionResult getCity(int ProvinceID, string defaultTitle = "请选择")
        {
            var city = tradersService.getCity(ProvinceID, defaultTitle);
            return new JsonNetResult(city);
        }

        /// <summary>
        /// 获取区县
        /// </summary>
        /// <returns></returns>
        public ActionResult getDistrict(int CityID)
        {
            var district = tradersService.getDistrict(CityID);
            return new JsonNetResult(district);
        }

        //[SysPermissionValidate(67)]
        public ActionResult Save(int? id,int all=0)
        {
            //0是全部显示，1是部分显示
            ViewBag.all = all;

            //商户分类
            ViewBag.TraderType = ListToSelect(tradersService.getTraderTypeList().Select(o => new SelectListItem() { Value = o.TraderTypeId.ToString(), Text = o.Title }), emptyTitle: "请选择");

            //经营模式
            ViewBag.ModeId = ListToSelect(tradersService.getDataList().Select(o => new SelectListItem() { Value = o.DicSN.ToString(), Text = o.Title }), emptyTitle: "请选择");

            //经营类目
            ViewBag.BusinessScopeId = tradersService.getBusinessList();

            //跟踪状态
            ViewBag.StautsId = ListToSelect(tradersService.getDataList(205).Select(o => new SelectListItem() { Value = o.DicSN.ToString(), Text = o.Title }), emptyTitle: "请选择");

            var obj = new Traders{ 
            BusinessScopeId="",
            Pay="",
            CurrentProvinceId=15
           };
            if (id.HasValue)
            {
                obj = tradersService.GetOne(id.Value);
                obj.BusinessScopeId = obj.BusinessScopeId.IsNullOrEmpty() ? "" : obj.BusinessScopeId;
                obj.Pay = obj.Pay.IsNullOrEmpty() ? "" : obj.Pay;
            }
            //登记人ID
            obj.CreateUID = obj.CreateUID.IsNullOrEmpty() ? CurrentUser.UID : obj.CreateUID;
            //登记人
            ViewBag.Create = tradersService.getFullName(obj.CreateUID);
            //采购意向清单
            ViewBag.OrderList = tradersService.getOrderList(obj.CID);

            //测试
            List<ViewOrderList> llwefe = tradersService.getOrderList(obj.CID);

            //回访小结
            ViewBag.VisitTrack = visitTrackService.getVisitTrackList(obj.CID);
            //设备分类
            ViewBag.DeviceClass = ListToSelect(tradersService.getDataList(197).Select(o => new SelectListItem() { Value = o.DicSN.ToString(), Text = o.Title }), emptyTitle: "请选择");
            //业务员
            ViewBag.user = ListToSelect(tradersService.getUserList().Select(o => new SelectListItem() { Value = o.UserId, Text = o.FullName, Selected = (o.UserId == CurrentUser.UID && all > 0) }), emptyTitle: "请选择");
            //支付方式
            ViewBag.pay = tradersService.getDataList(300);
            //现有门店数量
            ViewBag.storeNum = ListToSelect(tradersService.getDataList(320).Select(o => new SelectListItem() { Value = o.DicSN.ToString(), Text = o.Title }), emptyTitle: "请选择");
            //每门店人均数
            ViewBag.averageNum = ListToSelect(tradersService.getDataList(340).Select(o => new SelectListItem() { Value = o.DicSN.ToString(), Text = o.Title }), emptyTitle: "请选择");
            //采购意向清单单位
            ViewBag.OrderUnit = ListToSelect(tradersService.getDataList(360).Select(o => new SelectListItem() { Value = o.DicSN.ToString(), Text = o.Title }), emptyTitle: "请选择");
            return View(obj.IsNullThrow());
        }

        /// <summary>
        /// 获取时间
        /// </summary>
        /// <returns></returns>
        public string getDT()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 保存、修改
        /// </summary>
        /// <param name="traders">商户资料</param>
        /// <param name="h_OrderList">采购意向清单</param>
        /// <param name="h_VisitTrack">回访小结</param>
        /// <param name="BusinessScopeId">经营类别</param>
        /// <param name="Pay">支付方式</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Save(Traders traders, string h_OrderList, string h_VisitTrack, string[] BusinessScopeId, string[] Pay)
        {
            var op = tradersService.Save(traders, h_OrderList, h_VisitTrack, BusinessScopeId,Pay);
            return new OpActionResult(op);
        }

        /// <summary>
        /// 商户审核通过、设为无效商户
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult setStatus(string ids, short status)
        {
            return new JsonNetResult(tradersService.setStatus(ids, status));
        }

        /// <summary>
        /// 更新客户分类
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [HttpPost]
        [SysPermissionValidate(72)]
        public ActionResult upType(string ids, string TraderTypeId)
        {
            return new JsonNetResult(tradersService.upType(ids, TraderTypeId));
        }

        [HttpPost]
        [SysPermissionValidate(68)]
        public ActionResult Delete(int[] ids)
        {
            return new JsonNetResult(tradersService.Deletes(ids));
        }


        #region 导入
        public ActionResult ImportIndex()
        {
            //跟踪状态
            ViewBag.TrackStautsId = ListToSelect(tradersService.getDataList(205).Select(o => new SelectListItem() { Value = o.Title, Text = o.Title }), emptyTitle: "全部");
            //业务员
            ViewBag.user = ListToSelect(tradersService.getUserList().Select(o => new SelectListItem() { Value = o.FullName, Text = o.FullName }), emptyTitle: "全部");
            //经营类目
            ViewBag.BusinessScopeId = ListToSelect(tradersService.getBusinessList().Select(o => new SelectListItem() { Value = o.Title, Text = o.Title }), emptyTitle: "全部");
            //客户类型
            ViewBag.ModeId = ListToSelect(tradersService.getDataList().Select(o => new SelectListItem() { Value = o.Title, Text = o.Title }), emptyTitle: "全部");
            return View();
        }
        public ActionResult Import()
        {
            var obj = ImportSetService.GetOne("Traders");
            return View(obj ?? new ImportSet() { MinRow = 1 });
        }
        [HttpPost]
        public ActionResult Import(ImportSet imp)
        {
            imp.TableName = "Traders";
            var op = tradersService.Import(imp, Request.Files, Request["FieldName"], Request["ColumnName"]);
            return Content(op.ToJson());
        }
        public ActionResult SureImport()
        {
            var op = tradersService.SureImport();
            return new JsonNetResult(op);
        }
        [HttpPost]
        public ActionResult ImportList(string type, string apiTitle, string searchField, string searchText)
        {
            int count=0;
            List<RedisTraders> list = tradersService.ImportList(Request.Params,out count);
            return ToDataGrid(list,count);
        }
        [HttpPost]
        public ActionResult DeleteAllImport()
        {
            return new JsonNetResult(tradersService.DeleteAllImport());
        }

        [HttpPost]
        public ActionResult DeleteImport(int[] ids)
        {
            return new JsonNetResult(tradersService.DeleteImport(ids));
        }

        #endregion
        #region 客户汇总
        
        public ActionResult Summary()
        {
            //跟踪状态
            ViewBag.TrackStautsId = ListToSelect(tradersService.getDataList(205).Select(o => new SelectListItem() { Value = o.DicSN.ToString(), Text = o.Title }), emptyTitle: "全部");
            //业务员
            ViewBag.user = ListToSelect(tradersService.getSummaryUserList().Select(o => new SelectListItem() { Value = o.UserId, Text = o.FullName }), emptyTitle: "全部");
            //客户分类
            ViewBag.TraderType = ListToSelect(tradersService.getTraderTypeList().Select(o => new SelectListItem() { Value = o.TraderTypeId.ToString(), Text = o.Title }), emptyTitle: "将所选项归类到指定客户分类");
            //经营类目
            ViewBag.BusinessScopeId = ListToSelect(tradersService.getBusinessList().Select(o => new SelectListItem() { Value = o.ById, Text = o.Title }), emptyTitle: "全部");
            //经营模式
            ViewBag.ModeId = ListToSelect(tradersService.getDataList().Select(o => new SelectListItem() { Value = o.DicSN.ToString(), Text = o.Title }), emptyTitle: "全部");

            object footer = null;
            var count = 0;
            List<ViewSysData> l1 = new List<ViewSysData>();
            List<ViewSysData> l2 = new List<ViewSysData>();
            List<ViewSysData> l3 = new List<ViewSysData>();
            DataTable dt = tBLL.GetList(Request.Params, out footer, out count, out l1, out l2, out l3);

            //客户类型
            ViewBag.d1 = l1;
            //跟进状态
            ViewBag.d2 = l2;
            //支付方式
            ViewBag.d3 = l3;

            return View();
        }

        /// <summary>
        /// 获取客户汇总
        /// </summary>
        /// <returns></returns>
        public ActionResult getSummaryList()
        {
            object footer = null;
            var count = 0;

            //客户类型
            List<ViewSysData> l1 = new List<ViewSysData>();
            //跟进状态
            List<ViewSysData> l2 = new List<ViewSysData>();
            //支付方式
            List<ViewSysData> l3 = new List<ViewSysData>();

            DataTable dt = tBLL.GetList(Request.Params, out footer, out count,out l1,out l2,out l3);
            return ToDataGrid(dt, count, footer);
        }

        /// <summary>
        /// 导出客户汇总
        /// </summary>
        /// <returns></returns>
        public ActionResult SummaryExport(string CreateDT_begin, string CreateDT_end)
        {
            if (CreateDT_end.IsNullOrEmpty())
            {
                CreateDT_end = DateTime.Now.ToString();
            }
            object footer = null;
            var count = 0;
            //客户类型
            List<ViewSysData> l1 = new List<ViewSysData>();
            //跟进状态
            List<ViewSysData> l2 = new List<ViewSysData>();
            //支付方式
            List<ViewSysData> l3 = new List<ViewSysData>();
            DataTable dt = tBLL.GetList(Request.Params, out footer, out count, out l1, out l2, out l3);
            if (dt.Rows.Count > 0)
            {
                string fileName = "客户汇总";
                string title = DateTime.Parse(CreateDT_begin).ToString("yyyy年MM月dd日") + ((CreateDT_end.IsNullOrEmpty() || CreateDT_begin == CreateDT_end) ? "" : "至" + DateTime.Parse(CreateDT_end).ToString("yyyy年MM月dd日")) + " 客户汇总";
                List<string> fields = Tool.getColumnName(dt,new int[]{0,1});
                List<string> names = new List<string>();
                List<int> totalCols = getTotalCols(l1,l2,l3);
                names.Add("日期");
                names.Add("业务员");
                names.Add("区域");
                if (l1.Count > 0)
                {
                    foreach(var v in l1)
                    {
                        names.Add(v.Title);
                    }
                }
                if (l2.Count > 0)
                {
                    foreach (var v in l2)
                    {
                        names.Add(v.Title);
                    }
                }
                if (l3.Count > 0)
                {
                    foreach (var v in l3)
                    {
                        names.Add(v.Title);
                    }
                }
                ExportExcel ex = new ExportExcel() { IsBufferOutput = true, HeaderText = title };
                ex.ToExcel(fileName, dt, fields.ToArray(), names.ToArray(), null, totalCols.ToArray());
                return new EmptyResult();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 客户汇总是否有数据
        /// </summary>
        /// <returns></returns>
        public string HaveSummary()
        {
            object footer = null;
            var count = 0;
            //客户类型
            List<ViewSysData> l1 = new List<ViewSysData>();
            //跟进状态
            List<ViewSysData> l2 = new List<ViewSysData>();
            //支付方式
            List<ViewSysData> l3 = new List<ViewSysData>();

            DataTable dt = tBLL.GetList(Request.Params, out footer, out count, out l1, out l2, out l3);
            if (dt != null)
            {
                if (dt.Rows.Count == 0)
                {
                    return "暂无数据可导出!";
                }
                else
                {
                    return "success";
                }
            }
            else
            {
                return "暂无数据可导出!";
            }
        }

        /// <summary>
        /// 获取合计列
        /// </summary>
        /// <param name="l1"></param>
        /// <param name="l2"></param>
        /// <param name="l3"></param>
        /// <returns></returns>
        public List<int> getTotalCols(List<ViewSysData> l1, List<ViewSysData> l2, List<ViewSysData> l3)
        {
            int i = 3;
            List<int> list = new List<int>();
            if (l1.Count > 0)
            {
                foreach (var v in l1)
                {
                    list.Add(i);
                    i = i + 1;
                }
            }
            if (l2.Count > 0)
            {
                foreach (var v in l2)
                {
                    list.Add(i);
                    i = i + 1;
                }
            }
            if (l3.Count > 0)
            {
                foreach (var v in l3)
                {
                    list.Add(i);
                    i = i + 1;
                }
            }
            return list;
        }

        #endregion

        /// <summary>
        /// 查看
        /// </summary>
        /// <param name="type">客户类型、跟进状态、支付方式</param>
        /// <param name="dataId">字典编号</param>
        /// <param name="UserId">业务员ID</param>
        /// <returns></returns>
        public ActionResult See(string type, int dataId, string UserId)
        {
            System.Collections.Specialized.NameValueCollection nvl = Request.Params;
            string kh = "";
            string gj = "";
            string zf = "";
            if (!type.IsNullOrEmpty())
            {
                if (type == "kh")
                {
                    kh = dataId.ToString();
                }
                else if (type == "gj")
                {
                    gj = dataId.ToString();
                }
                else if (type == "zf")
                {
                    zf = dataId.ToString();
                }
            }

            var CurrentProvinceId = (nvl["CurrentProvinceId"] ?? "").Trim();
            var CurrentCityId = (nvl["CurrentCityId"] ?? "").Trim();
            var CreateDT_begin = (nvl["CreateDT_begin"] ?? "").Trim();
            var CreateDT_end = (nvl["CreateDT_end"] ?? "").Trim();
            var BusinessScopeId = (nvl["BusinessScopeId"] ?? "").Trim();

            //客户类型
            ViewBag.kh = kh;
            //跟进状态
            ViewBag.gj = gj;
            //支付类型
            ViewBag.zf = zf;
            //业务员
            ViewBag.UserId = UserId;
            //省份
            ViewBag.CurrentProvinceId = CurrentProvinceId;
            //城市
            ViewBag.CurrentCityId = CurrentCityId;
            //登记日期（开始）
            ViewBag.CreateDT_begin = CreateDT_begin;
            //登记日期（结束）
            ViewBag.CreateDT_end = CreateDT_end;
            //经营范围
            ViewBag.BusinessScopeId = BusinessScopeId;
            return View();
        }

        public ActionResult Assigner(string ids)
        {
            ViewBag.ids = ids;
            ViewBag.user = ListToSelect(tradersService.getAssignerUserList().Select(o => new SelectListItem() { Value = o.UserId, Text = o.FullName }), emptyTitle: "请选择");
            return View();
        }

        /// <summary>
        /// 指派业务员
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="AssignerUID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Assigner(string ids, string AssignerUID)
        {
            return new JsonNetResult(tradersService.UpAssignerUID(ids, AssignerUID));
        }

        /// <summary>
        /// 指派业务员
        /// </summary>
        /// <returns></returns>
        public ActionResult GetListAssigner(string ids)
        {
            var count = 0;
            var list = tradersService.GetListAssigner(Request.Params, out count);
            return ToDataGrid(list, count);
        }


    }
}