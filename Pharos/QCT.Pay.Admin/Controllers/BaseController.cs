using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections;
using System.Collections.Generic;
using Pharos.Utility;
using Pharos.Utility.Helpers;
using Pharos.Logic.OMS;

namespace QCT.Pay.Admin.Controllers
{
    public class BaseController : AsyncController
    {
        /// <summary>
        /// 转成easyui格式
        /// </summary>
        /// <param name="list"></param>
        /// <param name="rowCount">总记录数</param>
        /// <param name="footers">带页脚信息</param>
        /// <returns></returns>
        protected JsonResult ToDataGrid(object list, int rowCount,object footers=null)
        {
            list = list ?? new ArrayList();
            object obj = null;
            if (footers == null)
                obj= new { total = rowCount, rows = list };
            else
                obj = new{ total = rowCount, rows = list, footer=footers };
            return new JsonNetResult(obj);
        }
        /// <summary>
        /// 枚举转成下拉框数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="emptyTitle"></param>
        /// <param name="selectValue"></param>
        /// <returns></returns>
        protected IList<SelectListItem> EnumToSelect(Type type, string emptyTitle=null, byte? selectValue=null)
        {
            return EnumToSelect(type, "", emptyTitle, selectValue);
        }
        protected IList<SelectListItem> EnumToSelect(Type type, string emptyValue, string emptyTitle = null, byte? selectValue = null)
        {
            IList<SelectListItem> list = new List<SelectListItem>();
            if (emptyTitle != null) list.Insert(0, new SelectListItem() { Text = emptyTitle, Value = emptyValue, Selected = true });
            string[] t = Enum.GetNames(type);
            foreach (string f in t)
            {
                var obj = Enum.Parse(type, f);
                var d = Convert.ToInt32(Convert.ChangeType(obj, type));
                var item = new SelectListItem() { Text = f, Value = d.ToString() };
                if (selectValue.HasValue)
                    item.Selected = d == selectValue.Value;
                list.Add(item);
            }
            return list;
        }
        /// <summary>
        /// 对存在的列表处理
        /// </summary>
        /// <param name="list"></param>
        /// <param name="emptyTitle"></param>
        /// <param name="selectValue"></param>
        /// <returns></returns>
        protected IList<SelectListItem> ListToSelect(IEnumerable<SelectListItem> ies, string emptyTitle = null, byte? selectValue = null)
        {
            if (ies == null) return null;
            var list = ies.ToList();
            if (emptyTitle != null) list.Insert(0, new SelectListItem() { Text = emptyTitle, Value = "",Selected=true });
            if(selectValue.HasValue)
            {
                var obj= list.FirstOrDefault(o => o.Value == selectValue.Value.ToString());
                if (obj != null) obj.Selected = true;
            }
            return list;
        }
        protected override void OnException(ExceptionContext filterContext)
        {
            var err= DataCache.Get<string>("err");
            if (err != filterContext.Exception.Message)//不重复添加异常信息
            {
                LogEngine log = new LogEngine();
                log.WriteError(filterContext.Exception);
                DataCache.Set("err", filterContext.Exception.Message, 1);

                if (filterContext.HttpContext.Request.IsAjaxRequest() || 
                    filterContext.HttpContext.Request.HttpMethod.Equals("POST",StringComparison.CurrentCultureIgnoreCase))
                {
                    filterContext.ExceptionHandled = true;
                    var op = new OpResult() { Message = filterContext.Exception.Message };
                    filterContext.Result = new ContentResult() { Content = op.ToJson() };
                }
            }
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //if (!Pharos.Logic.CurrentUser.IsLogin)
            //{
            //    filterContext.HttpContext.Response.Write("<script>top.window.location.href='" + Url.Action("Login", "Account") + "'</script>");
            //    filterContext.HttpContext.Response.End();
            //}
        }
        /// <summary>
        /// 跳转页带提示信息(异步)
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="msg">提示信息</param>
        /// <param name="isTop">是否顶级跳转页</param>
        /// <returns></returns>
        protected ActionResult RedirectAlert(string actionName,string msg,object routerValues=null,string controlName=null, bool isTop=false)
        {
            return Content(string.Format("<script>var msg='{0}';if(msg) alert(msg); {1}window.location.href='", msg, isTop ? "top." : "") + Url.Action(actionName, controlName, routerValues) + "'</script>");
        }
        /// <summary>
        /// 跳转页带提示信息(同步)
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="msg"></param>
        /// <param name="controlName"></param>
        /// <param name="isTop"></param>
        /// <returns></returns>
        protected void RedirectAlertSync(string actionName, string msg, string controlName = null, bool isTop = false)
        {
             Response.Write(string.Format("<script>alert('{0}');{1}window.location.href='", msg, isTop ? "top." : "") + Url.Action(actionName, controlName) + "'</script>");
        }
 
    }
}
