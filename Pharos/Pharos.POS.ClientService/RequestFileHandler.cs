using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pharos.POS.ClientService
{
    public class RequestFileHandler : HttpMessageHandler
    {
        public static string WebSitePath = "WebSite";
        IAssetProvider _Provider = new FileAssetProvider();
        public IAssetProvider Provider
        {
            get { return _Provider; }
            set
            {
                _Provider = value;
            }
        }
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var assetPath = request.GetRouteData().Values["assetPath"].ToString();
            var rootUrl = DefaultRootUrlResolver(request);
            try
            {
                var webAsset = _Provider.GetAsset(rootUrl, assetPath);
                var content = ContentFor(webAsset);
                return TaskFor(new HttpResponseMessage { Content = content });
            }
            catch (AssetNotFound ex)
            {
                return TaskFor(request.CreateErrorResponse(HttpStatusCode.NotFound, ex));
            }
        }
        protected virtual string GetHeaderValue(HttpRequestMessage request, string headerName)
        {
            IEnumerable<string> list;
            return request.Headers.TryGetValues(headerName, out list) ? list.FirstOrDefault() : null;
        }
        public virtual string DefaultRootUrlResolver(HttpRequestMessage request)
        {
            var scheme = GetHeaderValue(request, "X-Forwarded-Proto") ?? request.RequestUri.Scheme;
            var host = GetHeaderValue(request, "X-Forwarded-Host") ?? request.RequestUri.Host;
            var port = GetHeaderValue(request, "X-Forwarded-Port") ?? request.RequestUri.Port.ToString(CultureInfo.InvariantCulture);

            var httpConfiguration = request.GetConfiguration();
            var virtualPathRoot = httpConfiguration.VirtualPathRoot.TrimEnd('/');

            return string.Format("{0}://{1}:{2}{3}", scheme, host, port, virtualPathRoot);
        }
        private HttpContent ContentFor(Asset webAsset)
        {
            var content = new StreamContent(webAsset.Stream);
            content.Headers.ContentType = new MediaTypeHeaderValue(webAsset.MediaType);
            return content;
        }

        private Task<HttpResponseMessage> TaskFor(HttpResponseMessage response)
        {
            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
        }
    }


    public class FileAssetProvider : IAssetProvider
    {

        public Asset GetAsset(string rootUrl, string assetPath)
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, RequestFileHandler.WebSitePath, assetPath);
            if (!File.Exists(filePath))
            {
                throw new AssetNotFound(String.Format("Mapping not found - {0}", assetPath));
            }
            var stream = File.OpenRead(filePath);
            return new Asset(
                            stream,
                            InferMediaTypeFrom(assetPath)
                        );
        }
        private static string InferMediaTypeFrom(string path)
        {
            var extension = path.Split('.').Last().ToLower();

            switch (extension)
            {
                case "css":
                    return "text/css";
                case "js":
                    return "text/javascript";
                case "gif":
                    return "image/gif";
                case "png":
                    return "image/png";
                case "eot":
                    return "application/vnd.ms-fontobject";
                case "woff":
                    return "application/font-woff";
                case "woff2":
                    return "application/font-woff2";
                case "otf":
                    return "application/font-sfnt"; // formerly "font/opentype"
                case "ttf":
                    return "application/font-sfnt"; // formerly "font/truetype"
                case "svg":
                    return "image/svg+xml";
                default:
                    return "text/html";
            }
        }
    }

    public class Asset
    {
        public Asset(Stream stream, string mediaType)
        {
            Stream = stream;
            MediaType = mediaType;
        }

        public Stream Stream { get; private set; }

        public string MediaType { get; private set; }
    }
    public interface IAssetProvider
    {
        Asset GetAsset(string rootUrl, string assetPath);
    }
    public class AssetNotFound : Exception
    {
        public AssetNotFound(string message)
            : base(message)
        { }
    }

}
