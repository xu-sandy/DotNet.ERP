using System.ServiceProcess;

namespace Pharos.MessageAgent
{
    partial class MessagingAgentServerService : ServiceBase
    {
        private MessageServer appServer;
        public MessagingAgentServerService()
        {
            InitializeComponent();
        }
        internal void Start()
        {
            appServer = MessageServer.InitServer();
        }
        protected override void OnStart(string[] args)
        {
            Start();
        }
        internal void Close()
        {
            if (appServer != null)
            {
                appServer.Stop();
                appServer = null;
            }
        }
        protected override void OnStop()
        {
            Close();
        }
    }
}
