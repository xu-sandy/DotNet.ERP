using Pharos.Logic.OMS.BLL;
using Pharos.Logic.OMS.Entity;
using Pharos.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pharos.OMS.Retailing.Controllers
{
    public class SysDictionaryController : BaseController
    {
        [Ninject.Inject]
        SysDictionaryService DictService { get; set; }
        //
        // GET: /SysDictionary/

        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult FindPageList(int? psn)
        {
            int count = 0;
            var list = DictService.GetDataList(psn);
            return ToDataGrid(list,count);
        }
        public ActionResult Save(int? sn,int? psn)
        {
            var obj = new SysDataDictionaryExt();
            if (sn.HasValue)
            {
                var dict= DictService.Get(sn.Value);
                dict.ToCopyProperty(obj);
            }
            if(psn.HasValue)
            {
                var dict = DictService.Get(psn.Value);
                obj.PTitle = dict.Title;
                obj.DicPSN = psn.Value;
            }
            return View(obj);
        }
        [HttpPost]
        public ActionResult Save(SysDataDictionary obj)
        {
            return new OpActionResult(DictService.Save(obj));
        }
        public ActionResult Edit(int? psn,string ptitle)
        {
            return View();
        }
        [HttpPost]
        public void RemoveData(int sn)
        {
            DictService.Remove(sn);
        }
        [HttpPost]
        public void SetState(int sn ,short state)
        {
            DictService.SetState(sn, state);
        }
        [HttpPost]
        public void MoveItem(short mode, int sn)
        {
            DictService.MoveItem(mode, sn);
        }
    }
}
