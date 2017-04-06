using Pharos.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pharos.Utility.Helpers;
using Pharos.Logic;
using Pharos.Logic.Entity;
using Pharos.Logic.BLL;
using System.IO;
namespace Pharos.CRM.Retailing.Controllers
{
    public class VoucherController : BaseController
    {
        //
        // GET: /Voucher/

        public ActionResult Index()
        {
            ViewBag.types = ListToSelect(SysDataDictService.GetReceiptsCategories().Select(o => new SelectListItem() { Value = o.DicSN.ToString(), Text = o.Title }), emptyTitle: "全部");
            ViewBag.users = ListToSelect(UserInfoService.GetList().Select(o => new SelectListItem() { Value = o.UID, Text = o.FullName }), emptyTitle: "全部");
            ViewBag.suppliers = ListToSelect(SupplierService.GetList().Select(o => new SelectListItem() { Value = o.Id, Text = o.Title }), emptyTitle: "全部");
            ViewBag.states = EnumToSelect(typeof(ReceipState), emptyTitle: "全部");
            return View();
        }
        [HttpPost]
        public ActionResult FindPageList(int page = 1, int rows = 30)
        {
            int count = 0;
            var list =ReceiptsBLL.FindPageList(Request.Params, out count);
            return ToDataGrid(list, count);
        }
        public ActionResult Save(string id)
        {
            ViewBag.types = ListToSelect(SysDataDictService.GetReceiptsCategories().Select(o => new SelectListItem() { Value = o.DicSN.ToString(), Text = o.Title }), emptyTitle: "请选择");
            var obj = new Receipts() { CreateTitle = Sys.CurrentUser.FullName, CreateDT = DateTime.Now };
            if (!id.IsNullOrEmpty())
            {
                obj = ReceiptsBLL.FindById(id);
                obj.IsNullThrow();
                if (obj.Source == 1)
                {
                    var user = UserInfoService.Find(o => o.UID == obj.CreateUID);
                    if (user != null)
                        obj.CreateTitle = user.FullName;
                }
                else
                {
                    var supplier= SupplierService.Find(o => o.Id == obj.CreateUID);
                    if (supplier != null)
                        obj.CreateTitle = supplier.Title;
                }
            }
            return View(obj);
        }
        [HttpPost]
        public ActionResult Save(Receipts obj)
        {
            var re = ReceiptsBLL.SaveOrUpdate(obj, Request.Files);
            return Content(re.ToJson());
        }
        public ActionResult Detail(string id)
        {
            ViewBag.types = ListToSelect(SysDataDictService.GetReceiptsCategories().Select(o => new SelectListItem() { Value = o.DicSN.ToString(), Text = o.Title }), emptyTitle: "请选择");
            var obj = ReceiptsBLL.FindById(id);
            obj.IsNullThrow();
            var user=UserInfoService.Find(o=>o.UID==obj.CreateUID);
            if (user != null)
                obj.CreateTitle = user.FullName;
            var type= SysDataDictService.Find(o => o.DicSN == obj.CategoryId);
            if (type != null) ViewData["Category"] = type.Title;
            return View(obj);
        }
        [HttpPost]
        public ActionResult Delete(string[] ids)
        {
            var re = ReceiptsBLL.Delete(ids);
            return new JsonNetResult(re);
        }
        [HttpPost]
        public ActionResult DeleteFile(string id, int fileId)
        {
            var files = AttachService.Find(o => o.Id == fileId && o.SourceClassify == 2);
            var re = AttachService.Delete(files);
            return new JsonNetResult(re);
        }
        public ActionResult Auditor(string ids)
        {
            var sId = ids.Split(',');
            var list = ReceiptsBLL.FindList(o => sId.Contains(o.Id));
            list.Each(o => { o.State = (short)ReceipState.已审核; });
            var re = ReceiptsBLL.Update(list);
            return new JsonNetResult(re);
        }
    }
}
