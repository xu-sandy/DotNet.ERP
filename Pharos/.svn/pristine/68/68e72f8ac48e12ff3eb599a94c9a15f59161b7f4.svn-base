using System.Net.NetworkInformation;
namespace Pharos.Utility
{
    public class NetWorkInfo
    {
        /// <summary>
        /// Ping网络设备
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool NetPing(string ip)
        {
            Ping ping = new Ping();
            var reply= ping.Send(ip, 3000);
            return reply.Status == IPStatus.Success;
        }
    }
}
