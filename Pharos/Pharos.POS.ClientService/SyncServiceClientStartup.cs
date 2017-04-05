using Common.Logging;
using Pharos.SyncService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pharos.POS.ClientService
{
    static class SyncServiceClientStartup
    {
        static SyncController controller;
        internal static void AutoSync()
        {
            SyncController controller = new SyncController();
            controller.Start();
        }
        //internal static void Close()
        //{
        //    if (controller != null)
        //    {
        //        controller.Close();
        //        controller = null;
        //    }
        //}
    }
}
