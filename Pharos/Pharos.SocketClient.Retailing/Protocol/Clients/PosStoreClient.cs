﻿using Pharos.SocketClient.Retailing.CommandProviders;
using Pharos.SocketClient.Retailing.Protocol.ReceiveFilters;
using SuperSocket.ClientEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using Pharos.Infrastructure.Data.Redis;
using Pharos.Infrastructure.Data.Normalize;
using Pharos.Logic.ApiData.Pos;
using Pharos.Logic.ApiData.Pos.Sale;
using Pharos.Logic.ApiData.Pos.Sale.Suspend;

namespace Pharos.SocketClient.Retailing.Protocol.Clients
{
    public class PosStoreClient : EasyClient, IDisposable
    {
        private Dictionary<string, Type> cmdRount = new Dictionary<string, Type>();
        private bool isDispose;
        public PosStoreClient()
        {
            ReflectCommand();
            isDispose = false;
            this.Initialize(new PosStoreFixedHeaderReceiveFilter(new PosStoreCommandNameProvider()), (package) =>
            {
                if (cmdRount.ContainsKey(package.Key) && cmdRount[package.Key] != null)
                {
                    var type = cmdRount[package.Key];
                    var cmd = (ICommand)AppDomain.CurrentDomain.CreateInstance(type.Assembly.FullName, type.ToString()).Unwrap();
                    if (cmd.Key == package.Key)
                    {
                        cmd.Execute(this, package);
                    }
                }
            });
            var config = SocketClientConfig.GetConfig();
            var connected = this.ConnectAsync(new IPEndPoint(IPAddress.Parse(config.Ip), config.Port));
            connected.Wait();
            SendHeartbeatPacket();
            SubscribeStoreMesssage();
        }
        public void RegisterCommand(string key, Type type)
        {
            if (cmdRount.ContainsKey(key))
                cmdRount.Remove(key);
            cmdRount.Add(key, type);
        }
        private void ReflectCommand()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(ICommand)))).ToArray();
            foreach (var type in types)
            {
                if (!type.IsAbstract && !type.IsInterface)
                {
                    var cmd = (ICommand)AppDomain.CurrentDomain.CreateInstance(type.Assembly.FullName, type.ToString()).Unwrap();
                    RegisterCommand(cmd.Key, type);
                }
            }
        }
        private void SendHeartbeatPacket()
        {
            Task.Factory.StartNew(() =>
            {
                if (isDispose)
                    return;
                while (true)
                {
                    try
                    {
                        var config = SocketClientConfig.GetConfig();
                        this.SendObject(new byte[4] { 0x00, 0x00, 0x00, 0x01 }, new StoreInfo { CompanyId = config.CompanyId, StoreId = config.StoreId });
                        Thread.Sleep(5000);
                    }
                    catch { }
                }
            });

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
            if (!this.IsConnected)
            {
                var config = SocketClientConfig.GetConfig();

                var connected = this.ConnectAsync(new IPEndPoint(IPAddress.Parse(config.Ip), config.Port));
                connected.Wait();
            }
            this.Send(new ArraySegment<byte>(content));
        }
        public byte[] Format(byte[] cmdCode, byte[] body)
        {
            var len = BitConverter.GetBytes(body.Length);
            var rawMsg = new byte[8 + body.Length];
            Array.Copy(cmdCode, 0, rawMsg, 0, 4);
            Array.Copy(len, 0, rawMsg, 4, 4);
            Array.Copy(body, 0, rawMsg, 8, body.Length);
            return rawMsg;
        }
        private void SubscribeStoreMesssage()
        {
            RedisManager.Subscribe("SyncSerialNumber", (channel, info) =>
            {
                try
                {
                    var paysn = JsonConvert.DeserializeObject<PaySn>(info);
                    this.SendObject(new byte[4] { 0x00, 0x00, 0x00, 0x02 }, paysn);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            });
            RedisManager.Subscribe("SyncOnlineCache", (channel, info) =>
            {
                try
                {
                    var machineInformation = JsonConvert.DeserializeObject<MachineInformation>(info);
                    this.SendObject(new byte[4] { 0x00, 0x00, 0x00, 0x04 }, machineInformation);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            });
            //RedisManager.Subscribe("SyncShoppingCartCache", (channel, info) =>
            //{
            //    try
            //    {
            //        var jsonSerializerSettings = new Newtonsoft.Json.JsonSerializerSettings();
            //        jsonSerializerSettings.Converters.Add(new BarcodeConverter());
            //        var shoppingCart = JsonConvert.DeserializeObject<ShoppingCart>(info, jsonSerializerSettings);
            //        this.SendObject(new byte[4] { 0x00, 0x00, 0x00, 0x03 }, shoppingCart);
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine(ex.Message);
            //    }
            //});
        }
        public void Dispose()
        {
            isDispose = true;
            this.Close();
        }
    }
}
