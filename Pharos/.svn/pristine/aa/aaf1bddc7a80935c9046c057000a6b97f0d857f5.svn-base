using System;
using System.Collections.Generic;

namespace Pharos.Infrastructure.Net.Http.RestClient
{
    public class RequestSetting
    {
        public RequestSetting()
        {
            Schema = "http";
            Host = string.Empty;
            Port = 80;
            Timeout = 60000;

            Method = "GET";
            Path = string.Empty;
            UriParameters = new Dictionary<string, string>();
            Headers = new List<string>();
            Content = string.Empty;
            ContentType = string.Empty;
        }


        #region commonSetting
        public string Schema { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public int Timeout { get; set; }
        #endregion

        #region requestSetting

        public string Method { get; set; }
        public string Path { get; set; }
        public Dictionary<string, string> UriParameters { get; private set; }
        public List<string> Headers { get; private set; }
        public string Content { get; set; }
        public string ContentType { get; set; }
        #endregion

        public RequestSetting SetUriParameters(Dictionary<string, string> uriParameters)
        {
            if (uriParameters != null)
            {
                foreach (var item in uriParameters)
                {
                    UriParameters.Add(item.Key, item.Value);
                }
            }
            return this;
        }
    }
}