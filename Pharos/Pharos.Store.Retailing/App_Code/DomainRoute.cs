using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;
using System.Text.RegularExpressions;

namespace Pharos.Store.Retailing
{
    public class DomainRoute : Route
    {
        private Regex domainRegex;
        private Regex pathRegex;

        public string Domain { get; set; }

        public DomainRoute(string domain, string url, RouteValueDictionary defaults)
            : base(url, defaults, new MvcRouteHandler())
        {
            Domain = domain;
        }

        public DomainRoute(string domain, string url, RouteValueDictionary defaults, IRouteHandler routeHandler)
            : base(url, defaults, routeHandler)
        {
            Domain = domain;
        }

        public DomainRoute(string domain, string url, object defaults)
            : base(url, new RouteValueDictionary(defaults), new MvcRouteHandler())
        {
            Domain = domain;
        }

        public DomainRoute(string domain, string url, object defaults, IRouteHandler routeHandler)
            : base(url, new RouteValueDictionary(defaults), routeHandler)
        {
            Domain = domain;
        }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            // 构造 regex
            domainRegex = CreateRegex(Domain);
            pathRegex = CreateRegex(Url);

            // 请求信息
            string requestDomain = httpContext.Request.Headers["host"];
            if (!string.IsNullOrEmpty(requestDomain))
            {
                if (requestDomain.IndexOf(":") > 0)
                {
                    requestDomain = requestDomain.Substring(0, requestDomain.IndexOf(":"));
                }
            }
            else
            {
                requestDomain = httpContext.Request.Url.Host;
            }
            string requestPath = httpContext.Request.AppRelativeCurrentExecutionFilePath.Substring(2) + httpContext.Request.PathInfo;

            // 匹配域名和路由
            Match domainMatch = domainRegex.Match(requestDomain);
            Match pathMatch = pathRegex.Match(requestPath);

            // 路由数据
            RouteData data = null;
            if (domainMatch.Success && pathMatch.Success)
            {
                data = new RouteData(this, RouteHandler);

                // 添加默认选项
                if (Defaults != null)
                {
                    foreach (KeyValuePair<string, object> item in Defaults)
                    {
                        data.Values[item.Key] = item.Value;
                        if (item.Key.Equals("area") || item.Key.Equals("Namespaces"))
                        {
                            data.DataTokens[item.Key] = item.Value;
                        }
                    }
                }

                // 匹配域名
                for (int i = 1; i < domainMatch.Groups.Count; i++)
                {
                    Group group = domainMatch.Groups[i];
                    if (group.Success)
                    {
                        string key = domainRegex.GroupNameFromNumber(i);

                        if (!string.IsNullOrEmpty(key) && !char.IsNumber(key, 0))
                        {
                            if (!string.IsNullOrEmpty(group.Value))
                            {
                                data.Values[key] = group.Value;
                                if (key.Equals("area"))
                                {
                                    data.DataTokens[key] = group.Value;
                                }
                            }
                        }
                    }
                }

                // 匹配路径
                for (int i = 1; i < pathMatch.Groups.Count; i++)
                {
                    Group group = pathMatch.Groups[i];
                    if (group.Success)
                    {
                        string key = pathRegex.GroupNameFromNumber(i);

                        if (!string.IsNullOrEmpty(key) && !char.IsNumber(key, 0))
                        {
                            if (!string.IsNullOrEmpty(group.Value))
                            {
                                data.Values[key] = group.Value;
                                if (key.Equals("area"))
                                {
                                    data.DataTokens[key] = group.Value;
                                }
                            }
                        }
                    }
                }
            }

            return data;
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            return base.GetVirtualPath(requestContext, RemoveDomainTokens(values));
        }

        public DomainData GetDomainData(RequestContext requestContext, RouteValueDictionary values)
        {
            // 获得主机名
            string hostname = Domain;
            foreach (KeyValuePair<string, object> pair in values)
            {
                if (pair.Key == "area" && string.IsNullOrEmpty(pair.Value.ToString()))
                {
                    hostname = hostname.Replace("{" + pair.Key + "}", "www");
                }
                else
                {
                    hostname = hostname.Replace("{" + pair.Key + "}", pair.Value.ToString());
                }
            }

            //如果域名的area还没有被替换，说明路由数据里没有area，恢复为顶级域名
            if (hostname.Contains("{area}"))
            {
                hostname = hostname.Replace("{area}", "www");
            }

            RemoveDomainTokens(values);

            // Return 域名数据
            return new DomainData
            {
                Protocol = "http",
                HostName = hostname,
                Fragment = ""
            };
        }

        private Regex CreateRegex(string source)
        {
            // 替换
            source = source.Replace("/", @"\/?");
            source = source.Replace(".", @"\.?");
            source = source.Replace("-", @"\-?");
            source = source.Replace("{", @"(?<");
            source = source.Replace("}", @">([a-zA-Z0-9_]*))");

            return new Regex("^" + source + "$");
        }

        private RouteValueDictionary RemoveDomainTokens(RouteValueDictionary values)
        {
            Regex tokenRegex = new Regex(@"({[a-zA-Z0-9_]*})*-?\.?\/?({[a-zA-Z0-9_]*})*-?\.?\/?({[a-zA-Z0-9_]*})*-?\.?\/?({[a-zA-Z0-9_]*})*-?\.?\/?({[a-zA-Z0-9_]*})*-?\.?\/?({[a-zA-Z0-9_]*})*-?\.?\/?({[a-zA-Z0-9_]*})*-?\.?\/?({[a-zA-Z0-9_]*})*-?\.?\/?({[a-zA-Z0-9_]*})*-?\.?\/?({[a-zA-Z0-9_]*})*-?\.?\/?({[a-zA-Z0-9_]*})*-?\.?\/?({[a-zA-Z0-9_]*})*-?\.?\/?");
            Match tokenMatch = tokenRegex.Match(Domain);
            for (int i = 0; i < tokenMatch.Groups.Count; i++)
            {
                Group group = tokenMatch.Groups[i];
                if (group.Success)
                {
                    string key = group.Value.Replace("{", "").Replace("}", "");
                    if (values.ContainsKey(key))
                        values.Remove(key);
                }
            }

            return values;
        }
    }
}
