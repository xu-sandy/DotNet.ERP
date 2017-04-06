using Pharos.Logic.OMS.BLL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Mvc;
using System.Linq;
using System.Security.Cryptography;
using Pharos.Logic.OMS.Entity;
using Pharos.Logic.OMS;
using Pharos.Utility;
using Pharos.Utility.Helpers;

namespace QCT.Pay.Admin.Controllers
{
    /// <summary>
    /// 金融接口
    /// </summary>
    public class FinanceController : BaseController
    {
        [Ninject.Inject]
        DictionaryService DicSvc { get; set; }
        [Ninject.Inject]
        PayApiService PayApiSvc { get; set; }
        //
        // GET: /Finance/

        #region 支付接口页面
        /// <summary>
        /// 支付接口页面-页面加载
        /// </summary>
        /// <returns></returns>
        public ActionResult PayApiIndex()
        {
            return View();
        }
        /// <summary>
        /// 支付接口页面-获取分页数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetPayApiPageing()
        {
            var count = 0;
            var list = PayApiSvc.GetPayApiPaging(Request.Params, out count);
            return ToDataGrid(list, count);
        }

        /// <summary>
        /// 支付接口页面-移除所选项
        /// </summary>
        /// <returns></returns>
        public ActionResult RemovePayApi()
        {
            //fishtodo:待确定使用关联判断再完成删除功能
            return View();
        }
        /// <summary>
        /// 支付接口页面-设置启用关闭所选项
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SetStatus(int id, short status)
        {
            return new JsonNetResult(PayApiSvc.SetStatus(id, status));
        }
        #endregion

        #region 支付接口表单
        /// <summary>
        /// 支付接口页面-新增支付接口页面
        /// </summary>
        /// <returns></returns>
        public ActionResult PayApiForm(int? id)
        {
            ViewBag.payTypes = ListToSelect(DicSvc.GetChildList(380).Select(o => new SelectListItem() { Value = o.DicSN.ToString(), Text = o.Title }));
            ViewBag.payModes = ListToSelect(DicSvc.GetChildList(381).Select(o => new SelectListItem() { Value = o.DicSN.ToString(), Text = o.Title }));
            ViewBag.status = EnumToSelect(typeof(ApiStatus), selectValue: 0);
            var obj = new PayApi();
            if (id.HasValue)
            {
                obj = PayApiSvc.GetOne(id);
            }
            return View(obj.IsNullThrow());
        }
        /// <summary>
        /// 支付接口页面-新增或编辑支付接口页面-保存
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SavePayApi(PayApi unitPay)
        {
            var op = PayApiSvc.SaveOrUpdate(unitPay);
            return View("PayApiForm", (PayApi)op.Data);
        }
        #endregion

        

    }
    
}
