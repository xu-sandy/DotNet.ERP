using Pharos.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pharos.Utility.Helpers;
using Pharos.Logic.OMS.BLL;
using Pharos.Logic.OMS.Entity;
namespace Pharos.OMS.Retailing.Controllers
{
    public class ProductVerController : BaseController
    {
        #region 私有对象
        [Ninject.Inject]
        ProductVerService ProductVerService { get; set; }
        
        #endregion
        #region 首页
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FindPageList()
        {
            int count = 0;
            var list = ProductVerService.GetPageList(Request.Params, out count);
            return ToDataGrid(list, count);
        }

        [HttpPost]
        public ActionResult Delete(int[] ids)
        {
            return new JsonNetResult(ProductVerService.Deletes(ids));
        }
        #endregion
        #region 新建版本
        public ActionResult Save(int? id)
        {
            var obj = new ProductVer();
            if (id.HasValue)
            {
                obj = ProductVerService.Get(id.Value);
            }
            return View(obj.IsNullThrow());
        }
        [HttpPost]
        public ActionResult Save(ProductVer obj)
        {
            var re = ProductVerService.SaveOrUpdate(obj);
            return new OpActionResult(re);
        }
        #endregion
        [HttpPost]
        public ActionResult SetState(string id, short state)
        {
            var re = ProductVerService.SetState(id,state);
            return new OpActionResult(re);
        }
    }
}
