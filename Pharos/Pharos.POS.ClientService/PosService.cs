﻿using Pharos.Api.Retailing;
using Pharos.EmbeddedIISExpress;
using Pharos.Service.Retailing.Marketing;
using Pharos.SocketClient.Retailing.Protocol.Clients;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pharos.POS.ClientService
{
    public partial class POSService : ServiceBase
    {
        internal static IDisposable _server = null;
        internal static PosStoreClient client;
        internal static Thread marketingThread;
        internal static Thread syncServiceThread;
        internal static Thread clientThread;
        internal static Thread storeManagerThread;
        public POSService()
        {
            InitializeComponent();
            this.Disposed += POSService_Disposed;
        }

        void POSService_Disposed(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                Process.GetCurrentProcess().Dispose();
                Process.GetCurrentProcess().Kill();
            });
        }

        protected override void OnStart(string[] args)
        {
            // TODO:  在此处添加代码以启动服务。
            _server = WebApiStartup.RunWebServer();
            Task.Factory.StartNew(() =>
            {
                syncServiceThread = Thread.CurrentThread;

                SyncServiceClientStartup.AutoSync();
            });
            Task.Factory.StartNew(() =>
              {
                  marketingThread = Thread.CurrentThread;
                  MarketingManager.InitStoreMarketing();
              });
            Task.Factory.StartNew(() =>
            {
                clientThread = Thread.CurrentThread;

                client = new PosStoreClient();
            });
            //Task.Factory.StartNew(() =>
            //{
            //    storeManagerThread = Thread.CurrentThread;
            //    IISExpressSeverManager manager = new IISExpressSeverManager();
            //    var config = StoreManageCenterConfig.GetConfig();
            //    var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Store");
            //    manager.Run(config.Port, path, "v4.0", "IIS");
            //});
        }

        protected override void OnStop()
        {
            // TODO:  在此处添加代码以执行停止服务所需的关闭操作。
            if (_server != null)
            {
                _server.Dispose();
                _server = null;
            }
            if (client != null)
            {
                client.Close();
                client.Dispose();
                client = null;
            }
            if (marketingThread != null && marketingThread.ThreadState == System.Threading.ThreadState.Running)
            {
                marketingThread.Abort();
                marketingThread = null;
            }
            if (syncServiceThread != null && syncServiceThread.ThreadState == System.Threading.ThreadState.Running)
            {
                syncServiceThread.Abort();
                syncServiceThread = null;
            }
            if (clientThread != null && clientThread.ThreadState == System.Threading.ThreadState.Running)
            {
                clientThread.Abort();
                clientThread = null;
            }
            if (storeManagerThread != null && storeManagerThread.ThreadState == System.Threading.ThreadState.Running)
            {
                storeManagerThread.Abort();
                storeManagerThread = null;
            }
            base.OnStop();
            base.Dispose();
        }
    }
}
