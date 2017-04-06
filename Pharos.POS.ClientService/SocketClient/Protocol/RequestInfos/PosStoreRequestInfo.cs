using Newtonsoft.Json;
using SuperSocket.ProtoBase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Pharos.SocketClient.Retailing.Protocol.RequestInfos
{
    public class PosStorePackageInfo : PackageInfo<string, byte[]>
    {
        public PosStorePackageInfo(string key, byte[] body)
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
