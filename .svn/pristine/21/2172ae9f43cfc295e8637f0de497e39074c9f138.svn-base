using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Pharos.Utility.Helpers
{
    public static class JsonHelper
    {
        public static T ToObject<T>(this string sJasonData)
        {
            JsonSerializerSettings jsonSs = new JsonSerializerSettings();
            var timeConverter = new IsoDateTimeConverter();
            timeConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            jsonSs.Converters.Add(timeConverter);
            jsonSs.Converters.Add(new DataTableConverter());
            try
            {
                return JsonConvert.DeserializeObject<T>(sJasonData, jsonSs);
            }
            catch { return default(T); }
        }

        public static object ToObject(this string sJasonData, Type type)
        {
            JsonSerializerSettings jsonSs = new JsonSerializerSettings();
            var timeConverter = new IsoDateTimeConverter();
            timeConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            jsonSs.Converters.Add(timeConverter);
            jsonSs.Converters.Add(new DataTableConverter());
            try
            {
                return JsonConvert.DeserializeObject(sJasonData, type, jsonSs);
            }
            catch { return null; }
        }

        public static String ToJson<T>(this T obj)
        {
            JsonSerializerSettings jsonSs = new JsonSerializerSettings();
            var timeConverter = new IsoDateTimeConverter();
            timeConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            jsonSs.Converters.Add(timeConverter);
            jsonSs.Converters.Add(new DataTableConverter());
            //jsonSs.Converters.Add(new JavaScriptDateTimeConverter());
            //jsonSs.Culture = System.Threading.Thread.CurrentThread.CurrentCulture;
            //jsonSs.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            //jsonSs.DateTimeZoneHandling = DateTimeZoneHandling.Local;
            string json = JsonConvert.SerializeObject(obj, Formatting.None, jsonSs);
            return json;
        }

        public static String ToJsonTime<T>(this T obj)
        {
            JsonSerializerSettings jsonSs = new JsonSerializerSettings();
            var timeConverter = new IsoDateTimeConverter();
            timeConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            jsonSs.Converters.Add(timeConverter);
            jsonSs.Converters.Add(new DataTableConverter());
            string json = JsonConvert.SerializeObject(obj, Formatting.None, jsonSs);
            return json;
        }
        public static string Property(this JObject obj, string name, bool IgnoreCase)
        {
            var result="";
            if (obj == null) return result;
            foreach(var prop in obj.Properties())
            {
                if(string.Equals(prop.Name,name,StringComparison.CurrentCultureIgnoreCase))
                   result= prop.Value.ToString();
            }
            return result;
        }
    }
}
