using Newtonsoft.Json;
using System;

namespace Pharos.Infrastructure.Net.Http.RestClient
{
    public class RequestSettingWithJsonContent<TParameter> : RequestSetting
    {
        public RequestSettingWithJsonContent(TParameter parameter) : base()
        {
            if (parameter == null)
                throw new ArgumentNullException("请求参数不能为空！");
            Content = JsonConvert.SerializeObject(parameter);
            ContentType = "application/json";
        }


    }
}
