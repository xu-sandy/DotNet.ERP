using Newtonsoft.Json;
using SuperSocket.SocketBase.Protocol;
using System.IO;

namespace Pharos.SocketService.Retailing.Protocol.RequestInfos
{
    public class PosStoreRequestInfo : RequestInfo<byte[]>
    {
        public PosStoreRequestInfo(string key, byte[] body)
            : base(key, body)
        {

        }
        public T Read<T>(JsonSerializerSettings settings = null)
        {
            Stream s = new MemoryStream(Body);
            StreamReader sw = new StreamReader(s);
            JsonTextReader writer = new JsonTextReader(sw);
            if (settings == null)
                settings = new JsonSerializerSettings();
            JsonSerializer ser = JsonSerializer.Create(settings);
            var result = ser.Deserialize<T>(writer);
            writer.Close();
            sw.Close();
            s.Close();
            return result;
        }
    }
}
