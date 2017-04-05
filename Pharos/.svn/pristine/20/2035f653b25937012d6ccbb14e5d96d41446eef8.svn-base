using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pharos.Utility;
using Pharos.Utility.Helpers;

namespace Pharos.MSPP.Retailing
{
    /// <summary>
    /// 权限验证属性类
    /// </summary>
    public class SysPermissionValidateAttribute : ActionFilterAttribute
    {
        public SysPermissionValidateAttribute() { }
        public SysPermissionValidateAttribute(int code) { this.Code = code; }
        /// <summary>
        /// 权限编码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 执行Action前判断是否有访问权限
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var haspermiss = Pharos.Sys.CurrentUser.HasPermiss(this.Code);
            if (!haspermiss)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest() || 
                    string.Equals(filterContext.HttpContext.Request.HttpMethod, "POST", StringComparison.CurrentCultureIgnoreCase))
                {
                    var op = new OpResult() { Message = "您的访问权限不足，请联系管理员！" };
                    filterContext.Result = new ContentResult() { Content = op.ToJson() };
                }
                else
                {
                    string url = string.Empty;
                    var area = filterContext.RouteData.DataTokens["area"];
                    var controller = filterContext.RouteData.Values["controller"];
                    var action = filterContext.RouteData.DataTokens["action"];
                    //域
                    if (area != null)
                        url += string.Format("/{0}", area);
                    //控制器
                    url += string.Format("/{0}", controller);
                    //操作方法
                    url += string.Format("/{0}", action);

                    filterContext.Controller.TempData["type"] = "权限不足";
                    filterContext.Controller.TempData["message"] = "您的访问权限不足，请联系管理员！";
                    filterContext.Result = new RedirectResult("~/SysAdmin/Error/");
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}