using Newtonsoft.Json;
using SuperSocket.SocketBase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Pharos.SuperSocketProtocol
{
    public class SocketSession : AppSession<SocketSession, SockectRequestMessage>
    {
        public SocketServer SocketServer { get { return AppServer as SocketServer; } }
        public SessionStatus Status { get; set; }
        public byte[] Format(byte[] route, byte[] msg)
        {
            var len = BitConverter.GetBytes(msg.Length);
            var rawMsg = new byte[route.Length + len.Length + msg.Length];

            Array.Copy(route, 0, rawMsg, 0, route.Length);
            Array.Copy(len, 0, rawMsg, route.Length, len.Length);
            if (msg.LongLength > 0)
                Array.Copy(msg, 0, rawMsg, route.Length + len.Length, msg.Length);

            return rawMsg;
        }
        public byte[] TextToBytes(string text, Encoding encoding = default(Encoding))
        {
            if (encoding == default(Encoding))
            {
                encoding = Encoding.Default;
            }
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException("发送内容不能为空！");
            }
            var bytes = encoding.GetBytes(text);
            return bytes;
        }
        public string ObjectToJsonString(object obj, JsonSerializerSettings settings = null)
        {
            if (obj == null)
                throw new ArgumentNullException("发送内容不能为null！");
            if (settings == null)
            {
                settings = new JsonSerializerSettings();
            }
            return JsonConvert.SerializeObject(obj, settings);
        }
        public void SendObjectToJsonStream(byte[] cmdCode, object obj, JsonSerializerSettings settings = null)
        {
            if (obj == null)
                throw new ArgumentNullException("发送内容不能为null！");
            if (settings == null)
            {
                settings = new JsonSerializerSettings();
            }
            JsonSerializer serializer = JsonSerializer.Create(settings);
            serializer.NullValueHandling = NullValueHandling.Ignore;

            MemoryStream ms = new MemoryStream();
            StreamWriter sw = new StreamWriter(ms);
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, obj);
            }
            SendMemoryStream(cmdCode, ms);
            sw.Close();
        }
        public void SendObjectToXMLStream(byte[] cmdCode, object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("发送内容不能为null！");
            MemoryStream ms = new MemoryStream();
            XmlSerializer formatter = new XmlSerializer(obj.GetType());
            formatter.Serialize(ms, obj);
            SendMemoryStream(cmdCode, ms);
        }
        public void SendTextToStream(byte[] cmdCode, string text, Encoding encoding = default(Encoding))
        {
            if (encoding == default(Encoding))
            {
                encoding = Encoding.Default;
            }
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException("发送内容不能为空！");
            }
            MemoryStream ms = new MemoryStream();
            StreamWriter sw = new StreamWriter(ms, encoding);
            sw.Write(text);
            SendMemoryStream(cmdCode, ms);
            sw.Close();
        }
        public void SendObjectToBinaryStream(byte[] cmdCode, object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("发送内容不能为null！");
            }
            MemoryStream ms = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(ms, obj);
            SendMemoryStream(cmdCode, ms);
        }
        public void SendBytes(byte[] cmdCode, byte[] body = null)
        {
            if (cmdCode == null || cmdCode.Length != SocketServer.RouteProvider.RouteLength)
            {
                throw new ArgumentNullException("路由码与路由不匹配，请确认路由码长度！");
            }
            if (body == null)
            {
                body = new byte[0];
            }
            var content = this.Format(cmdCode, body);
            this.Send(new ArraySegment<byte>(content));
        }
        public void SendMemoryStream(byte[] cmdCode, MemoryStream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("数据流不能为null！");

            var body = stream.ToArray();
            stream.Close();
            // Send data to the server
            SendBytes(cmdCode, body);
        }

        public override void Initialize(IAppServer<SocketSession, SockectRequestMessage> appServer, ISocketSession socketSession)
        {
            Status = SessionStatus.Initializing;
            base.Initialize(appServer, socketSession);
        }

        public override void Close(CloseReason reason)
        {
            Status = SessionStatus.Closing;
            base.Close(reason);
        }

        protected override void OnSessionStarted()
        {
            base.OnSessionStarted();
            Status = SessionStatus.Started;
        }

        protected override void OnSessionClosed(CloseReason reason)
        {
            base.OnSessionClosed(reason);
            Status = SessionStatus.Closed;
        }
        protected override void HandleException(Exception e)
        {
            Logger.Error(string.Format("Session({0}) HandleException", SessionID), e);
            base.HandleException(e);
        }
    }
}
