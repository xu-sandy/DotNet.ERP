using Pharos.SocketClient.Retailing.Protocol.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Threading;
using System.Net.Sockets;


namespace Pharos.SocketClient.Retailing
{
    class Program
    {
        static void Main(string[] args)
        {
            PosStoreClient client = new PosStoreClient();
            while (true)
            {
                Console.WriteLine("按Q退出！");
                if (Console.ReadKey().Key.ToString().ToLower() == "q")
                {
                    client.Dispose();
                    break;
                }
            }
        }
    }
    public class StoreInfo
    {
        public string StoreId { get; set; }

        public int CompanyId { get; set; }
    }
}
