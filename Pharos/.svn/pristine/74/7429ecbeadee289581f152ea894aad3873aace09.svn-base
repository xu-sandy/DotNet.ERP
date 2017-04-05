using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pharos.Infrastructure.Net.Http.RestClient;

namespace Pharos.JiGuangPush
{
    public class JPushRestClient
    {
        //public JPushRestClient(string appkey, string masterSecret)
        //{
        //    AppKey = appkey;
        //    MasterSecret = masterSecret;
        //}
        //private static string SubmitUrl { get { return "api.jpush.cn/v3/push"; } }
        //private static string AppKey { get { return ""; } }
        //private static string MasterSecret { get { return ""; } }

        private const string AppKey = "988292a14ca98d614ade06f5";
        private const string MasterSecret = "c0c9ee2bed1536f12732b0d5";
        private const string SubmitUrl = "v3/push";

        public static TResult Post<TParameter, TResult>(TParameter parameter, Dictionary<string, string> uriParameters = null, string apiUrl = SubmitUrl)
           where TResult : class
        {
            RequestSetting setting = LoadDefaultSetting(
                new RequestSettingWithJsonContent<TParameter>(parameter)
                {
                    Method = "POST"
                },
                apiUrl, AppKey, MasterSecret);
            if (uriParameters == null)
            {
                uriParameters = new Dictionary<string, string>();

            }
            setting = setting.SetUriParameters(uriParameters);
            return GetResult<TResult>(setting);
        }

        public static TResult Post<TResult>(string apiUrl, Dictionary<string, string> parameters = null)
    where TResult : class
        {
            var setting = LoadDefaultSetting(
                new RequestSetting()
                {
                    Method = "POST"
                },
                apiUrl, AppKey, MasterSecret);
            if (parameters == null)
            {
                parameters = new Dictionary<string, string>();
            }
            setting = setting.SetUriParameters(parameters);
            return GetResult<TResult>(setting);
        }

        private static TResult GetResult<TResult>(RequestSetting setting)
    where TResult : class
        {
            using (var request = new RestRequest<TResult>(setting))
            {
                return request.Execute();
            }
        }



        private static RequestSetting LoadDefaultSetting(RequestSetting setting, string aipUrl, string appkey, string masterSecret)
        {
            if (setting == null)
                setting = new RequestSetting();
            //设置uri 信息等默认配置
            var keyBytes = Encoding.UTF8.GetBytes(string.Format("{0}:{1}", appkey, masterSecret));
            var base64_auth_string = Convert.ToBase64String(keyBytes);
            setting.Headers.Add(string.Format("Authorization: Basic {0}", base64_auth_string));
            setting.Host = "api.jpush.cn";
            setting.Port = 443;
            setting.Schema = "https";
            setting.Path = aipUrl;
            return setting;
        }
    }
}
