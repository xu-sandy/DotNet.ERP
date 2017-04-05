﻿using Common.Logging;
using Pharos.Infrastructure.Data.Redis;
using Pharos.Logic.ApiData.Pos;
using Pharos.Logic.ApiData.Pos.Exceptions;
using Pharos.POS.ClientService;
using Pharos.POS.ClientService.Models;
using Pharos.SocketClient.Retailing.Protocol;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Pharos.Api.Retailing.Controllers
{
    /// <summary>
    /// 登陆接口
    /// </summary>
    [RoutePrefix("api/StoreManager")]
    public class StoreManagerController : ApiController
    {
        public const string WEBSERVER = "WebServer";
        public const string SYNCSERVICECLIENT = "SyncServiceClient";
        public const string MARKETINGMANAGER = "MarketingManager";
        public const string SOCKETCLIENT = "SocketClient";
        /// <summary>
        /// 获取服务状态
        /// </summary>
        /// <returns>服务状态列表</returns>
        [Route("GetServicesState")]
        public IEnumerable<ServiceState> GetServicesState()
        {

            List<ServiceState> services = new List<ServiceState>();
            services.Add(new ServiceState() { IsRunning = true, ServiceName = "Web服务", ServiceId = WEBSERVER });
            services.Add(new ServiceState() { IsRunning = true, ServiceName = "通信服务", ServiceId = SOCKETCLIENT });
            services.Add(new ServiceState() { IsRunning = POSService.syncServiceThread.IsAlive, ServiceName = "同步服务", ServiceId = SYNCSERVICECLIENT });
            services.Add(new ServiceState() { IsRunning = true, ServiceName = "促销管理", ServiceId = MARKETINGMANAGER });
            return services;
        }
        /// <summary>
        /// 获取服务器状态
        /// </summary>
        /// <returns></returns>
        [Route("GetServerStates")]
        public IEnumerable<ServerState> GetServerStates()
        {
            //memory
            Int64 phav = PerformanceInfo.GetPhysicalAvailableMemoryInMiB();
            Int64 tot = PerformanceInfo.GetTotalMemoryInMiB();
            decimal percentFree = ((decimal)phav / (decimal)tot) * 100;
            decimal percentOccupied = 100 - percentFree;

            var diskInfo = PerformanceInfo.GetHardDiskInfoByName(AppDomain.CurrentDomain.BaseDirectory.FirstOrDefault().ToString());



            List<ServerState> result = new List<ServerState>();
            result.Add(new ServerState() { InUse = (float)percentOccupied, Project = "内存" });
            result.Add(new ServerState() { InUse = PerformanceCounterFun("Processor", "_Total", "% Processor Time"), Project = "CPU" });
            result.Add(new ServerState() { InUse = diskInfo.FreeSpace / diskInfo.TotalSpace, Project = "硬盘" });
            return result;
        }

        private static float PerformanceCounterFun(string CategoryName, string InstanceName, string CounterName)
        {
            PerformanceCounter pc = new PerformanceCounter(CategoryName, CounterName, InstanceName);
            pc.MachineName = ".";
            pc.NextValue();
            Thread.Sleep(1000);
            var vlu = pc.NextValue();
            if (vlu == 0f)
            {
                vlu = pc.NextValue();
            }
            return vlu;
        }
        [HttpPost]
        [Route("OperateService")]
        public bool OperateService()
        {
            try
            {
                Process.Start(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Pharos.POS.ClientService.exe"), "Restart");
                System.Windows.Forms.Application.Exit();
                Process.GetCurrentProcess().Kill();
                return true;
            }
            catch
            {
                return false;
            }
        }

        [Route("GetSyncLog")]
        public IEnumerable<string> GetSyncLog()
        {
            //var redisClient = RedisManager.Connection;
            //var endpoint = redisClient.GetEndPoints().FirstOrDefault();
            //if (endpoint == null)
            //{
            //    return new string[0];
            //}
            //var keys = redisClient.GetServer(endpoint).Keys(pattern: "*SyncLogCache");
            //var json = redisClient.GetDatabase().StringGet(keys.ToArray());

            //return json.Select(o => o.ToString());
            return StoreManager.GetSyncLogs();
        }

        [Route("GetServiceSettings")]
        public ServiceSettings GetServiceSettings()
        {
            var uri = GetEndpointAddress("WSHttpBinding_PosServerDbSyncService");
            return new ServiceSettings()
            {
                Redis = RedisConfiguration.GetConfig().ConnectionString,
                SocketIp = SocketClientConfig.GetConfig().Ip,
                SocketPort = SocketClientConfig.GetConfig().Port,
                WCFIp = (uri == default(Uri) ? "192.168.10.122" : uri.Host),
                WCFPort = (uri == default(Uri) ? 808 : uri.Port)
            };
        }

        [Route("GetStoreSettings")]
        public StoreSettings GetStoreSettings()
        {
            var cid = 0;
            try
            {
                cid = Convert.ToInt32(ConfigurationManager.AppSettings["CompanyId"]);
            }
            catch
            {
                cid = 0;
            }
            return new StoreSettings()
            {

                CompanyId = cid,
                CompanyName = ConfigurationManager.AppSettings["CompanyName"],
                DeviceSn = GetMacByIPConfig().FirstOrDefault(),
                Phone = ConfigurationManager.AppSettings["Phone"],
                RegNo = ConfigurationManager.AppSettings["RegNo"],
                StoreId = ConfigurationManager.AppSettings["StoreId"],
                StoreName = ConfigurationManager.AppSettings["StoreName"],
                ReplaceDatabase = false
            };
        }
        private List<string> GetMacByIPConfig()
        {
            List<string> macs = new List<string>();
            ProcessStartInfo startInfo = new ProcessStartInfo("ipconfig", "/all");
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.CreateNoWindow = true;
            Process p = Process.Start(startInfo);
            //截取输出流
            StreamReader reader = p.StandardOutput;
            string line = reader.ReadLine();

            while (!reader.EndOfStream)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    line = line.Trim();

                    if (line.StartsWith("Physical Address") || line.StartsWith("物理地址"))
                    {
                        macs.Add(line.Split(':')[1].Replace("-", "").Trim());
                    }
                }

                line = reader.ReadLine();
            }

            //等待程序执行完退出进程
            p.WaitForExit();
            p.Close();
            reader.Close();

            return macs;
        }


        [HttpPost]
        [Route("NewServiceSettings")]
        public bool NewServiceSettings(ServiceSettings requestParams)
        {
            if (requestParams.WCFPort <= 0)
            {
                throw new PosException("WCF 端口配置错误！");
            }
            if (requestParams.SocketPort <= 0)
            {
                throw new PosException("Socket 端口配置错误！");
            }
            if (string.IsNullOrEmpty(requestParams.Redis))
            {
                throw new PosException("Redis 配置不能为空！");
            }
            if (!Regex.IsMatch(requestParams.SocketIp, @"^((25[0-5])|(2[0-4]\d)|(1\d\d)|([1-9]\d)|\d)(\.((25[0-5])|(2[0-4]\d)|(1\d\d)|([1-9]\d)|\d)){3}$|^([a-zA-Z0-9]([a-zA-Z0-9\-]{0,61}[a-zA-Z0-9])?\.)+[a-zA-Z]{2,6}$"))
            {
                throw new PosException("Socket Ip 非法！");
            }
            if (!Regex.IsMatch(requestParams.WCFIp, @"^((25[0-5])|(2[0-4]\d)|(1\d\d)|([1-9]\d)|\d)(\.((25[0-5])|(2[0-4]\d)|(1\d\d)|([1-9]\d)|\d)){3}$|^([a-zA-Z0-9]([a-zA-Z0-9\-]{0,61}[a-zA-Z0-9])?\.)+[a-zA-Z]{2,6}$"))
            {
                throw new PosException("WCF Ip 非法！");
            }
            System.Configuration.Configuration config1 = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            RedisConfiguration redisconfig = new RedisConfiguration();
            redisconfig.ConnectionString = requestParams.Redis;
            config1.Sections.Remove("RedisConfig");
            config1.Sections.Add("RedisConfig", redisconfig);

            SocketClientConfig socketClientConfig = new SocketClientConfig();
            socketClientConfig.Ip = requestParams.SocketIp;
            socketClientConfig.Port = requestParams.SocketPort;
            config1.Sections.Remove("SocketClientConfig");
            config1.Sections.Add("SocketClientConfig", socketClientConfig);
            config1.Save();

            SetEndpointAddress("WSHttpBinding_PosServerDbSyncService", string.Format(@"net.tcp://{0}:{1}/PosServerDbSyncService.svc", requestParams.WCFIp, requestParams.WCFPort));
            return true;
        }


        [HttpPost]
        [Route("NewStoreSettings")]
        public bool NewStoreSettings(StoreSettings requestParams)
        {
            if (requestParams.CompanyId <= 0)
            {
                throw new PosException("CID配置错误！");
            }
            if (string.IsNullOrEmpty(requestParams.CompanyName))
            {
                throw new PosException("公司简称配置错误！");
            }
            if (string.IsNullOrEmpty(requestParams.StoreId))
            {
                throw new PosException("SID配置错误！");
            }
            if (string.IsNullOrEmpty(requestParams.StoreName))
            {
                throw new PosException("分店名称配置错误！");
            }
            //if (string.IsNullOrEmpty(requestParams.RegNo))
            //{
            //    throw new PosException("注册码配置错误！");
            //}
            //if (string.IsNullOrEmpty(requestParams.Phone))
            //{
            //    throw new PosException("小票电话配置错误！");
            //}
            Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            cfa.AppSettings.Settings["CompanyId"].Value = requestParams.CompanyId.ToString();
            cfa.AppSettings.Settings["CompanyName"].Value = requestParams.CompanyName;
            cfa.AppSettings.Settings["StoreId"].Value = requestParams.StoreId;
            cfa.AppSettings.Settings["StoreName"].Value = requestParams.StoreName;
            cfa.AppSettings.Settings["RegNo"].Value = requestParams.RegNo;
            cfa.AppSettings.Settings["Phone"].Value = requestParams.Phone;
            cfa.Save();

            if (requestParams.ReplaceDatabase)
            {
                try
                {
                    var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    var dbfile = Path.Combine(path, "LocalDB.sdf");
                    File.Delete(dbfile);
                    File.Copy(Path.Combine(path, "backfile"), dbfile, true);
                    path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SyncTemp");
                    if (Directory.Exists(path))
                        Directory.Delete(path, true);
                }
                catch (Exception ex)
                {
                    ILog logger = LogManager.GetLogger(Assembly.GetExecutingAssembly().GetName().Name);
                    logger.Error(ex.Message, ex);
                    return false;
                }
            }

            return true;

        }
        [HttpPost]
        [Route("ResetStore")]
        public bool ResetStore()
        {
            Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            cfa.AppSettings.Settings["CompanyId"].Value = string.Empty;
            cfa.AppSettings.Settings["CompanyName"].Value = string.Empty;
            cfa.AppSettings.Settings["StoreId"].Value = string.Empty;
            cfa.AppSettings.Settings["StoreName"].Value = string.Empty;
            cfa.AppSettings.Settings["RegNo"].Value = string.Empty;
            cfa.AppSettings.Settings["Phone"].Value = string.Empty;
            cfa.Save();
            return true;
        }


        /// <summary>
        /// 读取EndpointAddress
        /// </summary>
        /// <param name="endpointName"></param>
        /// <returns></returns>
        private Uri GetEndpointAddress(string endpointName)
        {

            ClientSection clientSection = ConfigurationManager.GetSection("system.serviceModel/client") as ClientSection;

            foreach (ChannelEndpointElement item in clientSection.Endpoints)
            {
                if (item.Name == endpointName)
                    return item.Address;
            }
            return default(Uri);
        }


        /// <summary>
        /// 设置EndpointAddress
        /// </summary>
        /// <param name="endpointName"></param>
        /// <param name="address"></param>
        private void SetEndpointAddress(string endpointName, string address)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ClientSection clientSection = config.GetSection("system.serviceModel/client") as ClientSection;
            foreach (ChannelEndpointElement item in clientSection.Endpoints)
            {
                if (item.Name == endpointName)
                {
                    item.Address = new Uri(address);
                    break;
                }
            }
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("system.serviceModel");
        }
    }
}
