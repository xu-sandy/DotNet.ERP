using System.Linq;
using System.Web.Mvc;
using Pharos.Utility.Helpers;
using Pharos.Utility;
using Pharos.Logic.OMS.Entity;
using Pharos.Logic.OMS.BLL;
using System.Collections.Generic;

namespace QCT.Pay.Admin.Controllers
{
    public class AreaController : BaseController
    {
        //
        // GET: /Brand/
        [Ninject.Inject]
        AreaService AreaService { get; set; }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult FindPageList(string title, int? classfyId)
        {
            var list = AreaService.GetDynamicPageList(Request.Params);
            return Json(list);
        }
        [HttpPost]
        public ActionResult Delete(int[] ids)
        {
            var op = AreaService.Deletes(ids);
            return new OpActionResult(op);
        }
        public ActionResult Save(int? id, int? pid, string ptitle, string pid_0)
        {
            var obj = new Area();
            SelectListItem category = null;
            if (id.HasValue)
            {
                obj = AreaService.GetOne(id.Value);
                category = new SelectListItem { Value = obj.AreaPID.ToString(), Text = obj.PTitle, Selected = true };
            }
            else if (pid.HasValue)
                category = new SelectListItem() { Value = pid.ToString(), Text = ptitle, Selected = true };
            ViewBag.category = category;
            ViewData["SelectPSN"] = pid_0;
            return View(obj.IsNullThrow());
        }
        [HttpPost]
        public ActionResult Save(Area obj)
        {
            var op = AreaService.SaveOrUpdate(obj);
            return new OpActionResult(op);
        }

        #region 省市联动选择
        public ActionResult ProvinceSelect(string emptyTitle = "请选择")
        {
            var parenttypes = AreaService.GetProvinces().Select(o => new DropdownItem(o.AreaID.ToString(), o.Title)).ToList();
            parenttypes.Insert(0, new DropdownItem("", Server.UrlDecode(emptyTitle), true));
            return new JsonNetResult(parenttypes);
        }
        public ActionResult CitySelect(int? pid, string emptyTitle = "请选择")
        {
            var childtypes = new List<DropdownItem>();
            if (pid.HasValue)
                childtypes = AreaService.GetCitys(pid.Value).Select(o => new DropdownItem(o.AreaID.ToString(), o.Title)).ToList();
            if (!emptyTitle.IsNullOrEmpty())
                childtypes.Insert(0, new DropdownItem("", Server.UrlDecode(emptyTitle), true));
            return new JsonNetResult(childtypes);
        }
        #endregion
    }
}
