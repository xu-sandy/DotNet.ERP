using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;

namespace Pharos.Api.Retailing
{
    public static class JsonHelper
    {
        public static string Property(this JObject obj, string name, bool IgnoreCase)
        {
            var result = "";
            if (obj == null) return result;
            foreach (var prop in obj.Properties())
            {
                if (string.Equals(prop.Name, name, StringComparison.CurrentCultureIgnoreCase))
                {
                    result = prop.Value.ToString();
                    break;
                }
            }
            return result;
        }
    }
}