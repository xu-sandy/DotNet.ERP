using Pharos.Logic.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pharos.Logic.Entity;
using Pharos.Utility.Helpers;
using Pharos.Utility;
using Pharos.Logic;
namespace Pharos.CRM.Retailing.Controllers
{
    public class MemberController : BaseController
    {
        //
        // GET: /Brand/

        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult FindPageList(string date1, string date2, string title)
        {
            var count = 0;
            var list = MembersService.FindPageList(date1, date2, title, ref count, true);
            return ToDataGrid(list, count);
        }
        public ActionResult Save(int? id)
        {
            ViewBag.stores = ListToSelect(WarehouseService.GetList().Select(o => new SelectListItem() { Text = o.Title, Value = o.StoreId }), emptyTitle: "请选择");
            var obj = new Members();
            SelectListItem city = null;
            if (id.HasValue)
            {
                obj = MembersService.FindById(id.Value);
                if (obj.CurrentCityId > 0)
                {
                    var area = BaseService<Area>.FindById(obj.CurrentCityId);
                    if (area != null)
                    {
                        var parentArea = BaseService<Area>.FindById(area.AreaPID);
                        var text = area.Title;
                        if (parentArea != null && parentArea.Type != 1) text = parentArea.Title + "/" + text;
                        city = new SelectListItem();
                        city.Value = obj.CurrentCityId.ToString();
                        city.Text = text;
                    }
                }
            }
            ViewBag.city = city;
            return View(obj.IsNullThrow());
        }
        [HttpPost]
        public ActionResult Save(Members obj)
        {
            var re = new OpResult();
            string msg = "";
            if (!MembersService.CheckMsg(obj, ref msg))
                re.Message = msg;
            else if (obj.Id == 0)
            {
                obj.MemberId = CommonRules.GUID;
                obj.Status = 1;
                obj.CreateDT = DateTime.Now;
                obj.CreateUID = Sys.CurrentUser.UID;
                obj.CompanyId = CommonService.CompanyId;
                re = MembersService.Add(obj);
            }
            else
            {
                var supp = MembersService.FindById(obj.Id);
                obj.ToCopyProperty(supp, new List<string>() { "CompanyId", "UsableIntegral", "UsedIntegral", "ConsumerCredit", "CreateDT", "CreateUID", "MemberId", "Status" });
                re = MembersService.Update(supp);
            }
            #region 操作日志
            var _msg = Pharos.Sys.LogEngine.CompareModelToLog<Members>(Sys.LogModule.会员管理, obj);
            new Pharos.Sys.LogEngine().WriteInsert(_msg, Sys.LogModule.会员管理);
            #endregion
            if (re.Successed)
            {
                Pharos.Infrastructure.Data.Redis.RedisManager.Publish("SyncDatabase", new Pharos.ObjectModels.DTOs.DatabaseChanged() { CompanyId = CommonService.CompanyId, StoreId = obj.StoreId, Target = "Member" });
            }
            return Content(re.ToJson());
        }
        public ActionResult Detail(int? id)
        {
            var obj = new Members();
            if (id.HasValue)
            {
                obj = MembersService.FindById(id.Value);
                if (obj.CurrentCityId > 0)
                {
                    var area = BaseService<Area>.FindById(obj.CurrentCityId);
                    if (area != null)
                    {
                        var parentArea = BaseService<Area>.FindById(area.AreaPID);
                        var text = area.Title;
                        if (parentArea != null && parentArea.Type != 1) text = parentArea.Title + "/" + text;
                        ViewBag.city = text;
                    }
                }
                if (!obj.StoreId.IsNullOrEmpty())
                {
                    var store = WarehouseService.Find(o => o.StoreId == obj.StoreId);
                    if (store != null) ViewBag.Store = store.Title;
                }
            }
            return View(obj.IsNullThrow());
        }
        [HttpPost]
        public ActionResult SetState(string ids, short state)
        {
            var sId = ids.Split(',').Select(o => int.Parse(o));
            var list = MembersService.FindList(o => sId.Contains(o.Id));
            #region 操作日志状态记录
            var _oList = new List<Members>();
            foreach (var item in list)
            {
                var _member = new Members();
                ExtendHelper.CopyProperty<Members>(_member, item);
                _oList.Add(_member);
            }
            #endregion

            list.ForEach(o => { o.Status = state; });
            var re = MembersService.Update(list);
            #region 操作日志
            if (re.Successed)
            {
                for (int i = 0; i < list.Count(); i++)
                {
                    var msg = Sys.LogEngine.CompareModelToLog<Members>(Sys.LogModule.会员管理, list[i], _oList[i]);
                    new Sys.LogEngine().WriteUpdate(msg, Sys.LogModule.会员管理);
                }
            }
            #endregion
            return new JsonNetResult(re);
        }

        public ActionResult Export(string date1, string date2, string title)
        {
            int count = 0;
            var list = MembersService.FindPageList(date1, date2, title, ref count, false);
            if (!list.Any()) return RedirectAlert("Index", "暂无数据,请先确认是否已审核!");
            var dt = list.ToDataTable();
            string[] fields = { "Store", "MemberCardNum", "RealName", "Sex", "MobilePhone", "Weixin", "Email", "QQ", "ConsumerCredit", "UsableIntegral", "City", "Address", "CreateDT" };
            string[] names = { "来源", "卡号", "姓名", "性别", "手机号", "微信号", "Email", "QQ号", "消费额度", "可用积分", "地区", "地址", "加入时间" };
            var header = "";
            new ExportExcel() { IsBufferOutput = true, HeaderText = header }.ToExcel("会员", dt, fields, names, null, null);
            return new EmptyResult();
        }
        public string Validator(string type, string value)
        {
            return "true";
        }
    }
}
