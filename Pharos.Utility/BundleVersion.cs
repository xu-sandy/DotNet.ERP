using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;

namespace Pharos.Utility
{
    /// <summary>
    /// Bundle支持版本号（前端指定不缓存）
    /// </summary>
    public class BundleVersion
    {
        public static IHtmlString ScriptRender(params string[] paths)
        {
            return GenerateVersion("<script src=\"{0}\" ></script>", paths);
        }

        public static IHtmlString StyleRender(params string[] paths)
        {
            return GenerateVersion("<link  href=\"{0}\" rel=\"stylesheet\"></link>", paths);
        }

        private static IHtmlString GenerateVersion(string html, string[] paths)
        {
            BundleResolver bundle = new BundleResolver(BundleTable.Bundles);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            var context = new HttpContextWrapper(HttpContext.Current);

            foreach (var p in paths)
            {
                var list = new List<string>();
                string parm = "", path = p;
                if (p.Contains("?"))
                {
                    parm = p.Substring(p.LastIndexOf("?"));
                    path = p.Replace(parm, "");
                }
                if (BundleTable.EnableOptimizations)
                {
                    var b = bundle.GetBundleUrl(path);//取压缩
                    if (b == null)
                        b = path;
                    else if (!string.IsNullOrWhiteSpace(parm))
                        parm = parm.Replace("?", "&");

                    list.Add(b);
                }
                else
                {
                    var bs = bundle.GetBundleContents(path);//取原路径
                    if (bs == null)
                        list.Add(path);
                    else
                        list.AddRange(bs);
                }
                foreach (var bc in list)
                {
                    var u = UrlHelper.GenerateContentUrl(bc, context);
                    sb.AppendFormat(html, u + parm);
                    sb.Append(Environment.NewLine);
                }
            }
            return new HtmlString(sb.ToString());
        }
    }
}
