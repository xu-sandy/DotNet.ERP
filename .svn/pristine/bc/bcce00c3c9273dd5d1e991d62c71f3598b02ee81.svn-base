using System.Collections.Generic;
using System.Web.Mvc;
using Pharos.Utility.Helpers;
using Pharos.Logic.OMS.BLL;
using Pharos.Logic.OMS.Entity;
using System.Linq;
namespace QCT.Pay.Admin.Controllers
{
    public class DeviceAuthorController : BaseController
    {
        [Ninject.Inject]
        DeviceAuthorService DeviceAuthorService { get; set; }
        [Ninject.Inject]
        DictionaryService DictionaryService { get; set; }
        [Ninject.Inject]
        BusinessService BusinessService { get; set; }
        [Ninject.Inject]
        UserService UserService { get; set; }
        [Ninject.Inject]
        DevicesService DevicesService { get; set; }
        //
        // GET: /Authorization/
        public ActionResult Index()
        {
            ViewBag.users = ListToSelect(UserService.GetList().Select(o => new SelectListItem() { Text = o.FullName, Value = o.UserId }), emptyTitle: "全部");
            ViewBag.categorys = ListToSelect(DevicesService.getDataList().Select(o => new SelectListItem() { Value = o.DicSN.ToString(), Text = o.Title }), emptyTitle: "全部");
            return View();
        }
        [HttpPost]
        public ActionResult FindPageList()
        {
            int count = 0;
            var list = DeviceAuthorService.GetPageList(Request.Params, out count);
            return ToDataGrid(list, count);
        }
        public ActionResult Save(int? id)
        {
            var obj = new DeviceAuthorize() { Source=1,EffectiveDT=System.DateTime.Now.ToString("yyyy-MM-dd")};
            var devices = new List<SelectListItem>();
            if (id.HasValue)
            {
                obj = DeviceAuthorService.GetOne(id.Value);
                devices.Add(new SelectListItem() { Value=obj.DeviceId,Text=obj.DeviceName});
                var dev= DevicesService.GetOneByuid(obj.DeviceId);
                if(dev!=null)
                {
                    ViewBag.Category = dev.CategoryId;
                    ViewBag.Brand = dev.Brand;
                    ViewBag.Spec = dev.Spec;
                }
            }
            var list= DevicesService.GetList();
            ViewBag.list = list.ToJson();
            ViewBag.devices = devices;
            ViewBag.users = ListToSelect(UserService.GetList(false).Select(o => new SelectListItem() {Text=o.FullName,Value=o.UserId }), emptyTitle: "请选择");
            return View(obj.IsNullThrow());
        }
        [HttpPost]
        public ActionResult Save(DeviceAuthorize obj)
        {
            var op = DeviceAuthorService.SaveOrUpdate(obj,Request["Category"], Request["Brand"], Request["Spec"]);
            return new OpActionResult(op);
        }
        [HttpPost]
        public ActionResult Delete(int[] ids)
        {
            var op = DeviceAuthorService.Deletes(ids);
            return Content(op.ToJson());
        }
        [HttpPost]
        public ActionResult SetState(string ids,short state)
        {
            var op = DeviceAuthorService.SetState(ids, state);
            return Content(op.ToJson());
        }
    }
}
