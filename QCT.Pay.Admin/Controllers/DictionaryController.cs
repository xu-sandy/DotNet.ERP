using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pharos.Logic.OMS.BLL;
using Pharos.Utility;
using Pharos.Logic.OMS.Entity;
namespace QCT.Pay.Admin.Controllers
{
    public class DictionaryController:BaseController
    {
        [Ninject.Inject]
        DictionaryService DictionaryService { get; set; }
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 数据字典-页面数据加载
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetDict(int page = 1, int rows = 20, string key = "")
        {
            var result = DictionaryService.GetPageList(page, rows, key);
            return ToDataGrid(result, result.Count());
        }
        /// <summary>
        /// 数据字典-添加子级数据字典
        /// </summary>
        /// <param name="id"></param>
        /// <param name="psn"></param>
        /// <returns></returns>
        public ActionResult Show(int id, int psn)
        {
            ViewData["psn"] = psn;
            return View();

        }
        /// <summary>
        /// 数据字典-根据父级字典SN获得子级数据字典集合
        /// </summary>
        /// <param name="psn"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetDictItems(int psn)
        {
            var entity = DictionaryService.GetChildList(psn);
            return ToDataGrid(entity, entity.Count());
        }
        /// <summary>
        /// 数据字典-增加或编辑数据字典
        /// </summary>
        /// <param name="id"></param>
        /// <param name="psn"></param>
        /// <returns></returns>
        public ActionResult Save(int id = -1, int psn = 0)
        {
            var model = DictionaryService.GetExtModel(id, psn);
            return View(model);
        }
        /// <summary>
        /// 数据字典-更改数据字典状态
        /// </summary>
        /// <param name="sn"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SwitchStatus(int sn)
        {
            var result = DictionaryService.ChangeStatus(sn);
            return new OpActionResult(result);
        }
        /// <summary>
        /// 数据字典-数据字典表单-保存
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Save(SysDataDictionary model)
        {
            var result = DictionaryService.SaveOrUpdate(model);
            return new OpActionResult(result);
        }
        [HttpPost]
        public ActionResult MoveDataItem(int mode, int sn)
        {
            var op = DictionaryService.MoveItem(mode, sn);
            return new OpActionResult(op);
        }
    }
}