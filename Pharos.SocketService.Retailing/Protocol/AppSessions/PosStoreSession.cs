using Newtonsoft.Json;
using Pharos.SocketService.Retailing.Protocol.AppServers;
using Pharos.SocketService.Retailing.Protocol.RequestInfos;
using SuperSocket.SocketBase;
using System;
using System.IO;

namespace Pharos.SocketService.Retailing.Protocol.AppSessions
{
    public class PosStoreSession : AppSession<PosStoreSession, PosStoreRequestInfo>
    {
        public PosStoreSession()
            : base()
        {
            SyncObject = new object();
        }
        public object SyncObject { get; private set; }

        public SessionStatus Status { get; private set; }
        public PosStoreServer PosStoreServer { get { return AppServer as PosStoreServer; } }


        public byte[] Format(byte[] cmdCode, byte[] body)
        {
            var len = BitConverter.GetBytes(body.Length);
            var rawMsg = new byte[8 + body.Length];
            Array.Copy(cmdCode, 0, rawMsg, 0, 4);
            Array.Copy(len, 0, rawMsg, 4, 4);
            Array.Copy(body, 0, rawMsg, 8, body.Length);
            return rawMsg;
        }
        public void SendObject(byte[] cmdCode, object obj)
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.NullValueHandling = NullValueHandling.Ignore;

            MemoryStream s = new MemoryStream();
            StreamWriter sw = new StreamWriter(s);
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, obj);
            }
            var body = s.ToArray();
            sw.Close();
            s.Close();
            // Send data to the server
            var content = this.Format(cmdCode, body);
            this.Send(new ArraySegment<byte>(content));
        }
        public override void Initialize(IAppServer<PosStoreSession, PosStoreRequestInfo> appServer, ISocketSession socketSession)
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
#if DEBUG
        public byte[] LastBuffer = null;
        public int ReceivedCount = 0;
        public int ResolvedCount = 0;
        public int ReceivedRequestCount = 0;
        public int ResolvedRequestCount = 0;
        public int ReceivedDataCount = 0;
        public int ResolvedDataCount = 0;

        protected override void HandleException(Exception e)
        {
            Logger.Error(string.Format("Session({0}) HandleException", SessionID), e);
            base.HandleException(e);
        }
#endif
    }
}
