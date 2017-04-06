using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Pharos.Logic.WeighDevice.ScaleEntity;

namespace Pharos.Logic.WeighDevice
{
    /// <summary>
    /// 顶尖电子秤
    /// </summary>
    public class AclasService
    {
        /// <summary>
        /// 传输数据
        /// </summary>
        /// <param name="tplu"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static string TransferData(List<TPlu> tplu, string ip)
        {
            string msg = "";
            int Result = -1;
            char[] Str_Renamed = new char[256];
            var Hotkey = new List<int>();
            if (!Pharos.Utility.NetWorkInfo.NetPing(ip))
            {
                return "网络异常，请联系管理员！";
            }
            ClearPLUData(ip);
            for (int i = 0; i < tplu.Count; i++)
            {
                TPlu PLU = tplu[i];
                PLU.BarCode = 17;
                Hotkey.Add(PLU.LFCode);
                TPlu[] PLUs = new TPlu[4];
                PLUs[0] = PLU;
                try
                {
                    Result = PBusConnectEx(".\\lfzk.dat", ".\\system.cfg", ip);
                }
                catch { msg = "网络异常，请联系管理员！"; break; }
                if (Result >= 0)
                {
                    Result = PBusPLUToStr(ref PLU, Str_Renamed);
                }
                if (Result >= 0)
                {
                    Result = PBusTransferPLUCluster(PLUs);
                }
                if (Result >= 0)
                {
                    if (i == tplu.Count - 1)
                    {
                        Result = PBusTransferHotkey(Hotkey.ToArray(), 0);
                    }
                }
                if (Result >= 0)
                {
                    try
                    {
                        Result = PBusDisConnectEx(ip);
                    }
                    catch (Exception)
                    {
                        //Result = 2;
                    }
                }
            }
            return msg;
        }

        private static void ClearPLUData(string ip)
        {
            try
            {
                int Result = PBusConnectEx(".\\lfzk.dat", ".\\system.cfg", ip);
                Result = PBusClearPLUData();
                Result = PBusDisConnectEx(ip);
            }
            catch (Exception e) { }
        }

        /// <summary>
        /// 传送PLU函数
        /// </summary>
        /// <param name="PLUCluster"></param>
        /// <returns></returns>
        [DllImport("PBusDrv.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        static extern int PBusTransferPLUCluster(TPlu[] PLUCluster);
        /// <summary>
        /// 传送Hotkey函数
        /// </summary>
        /// <param name="HotkeyTable"></param>
        /// <param name="TableIndex"></param>
        /// <returns></returns>
        [DllImport("PBusDrv.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        static extern int PBusTransferHotkey(int[] HotkeyTable, int TableIndex);
        /// <summary>
        /// 将PLU转换为字符串
        /// </summary>
        /// <param name="PLU"></param>
        /// <param name="LPStr"></param>
        /// <returns></returns>
        [DllImport("PBusDrv.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        static extern int PBusPLUToStr(ref TPlu PLU, char[] LPStr);
        /// <summary>
        /// 连接函数
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="configName"></param>
        /// <param name="ipAddr"></param>
        /// <returns></returns>
        [DllImport("PBusDrv.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        static extern int PBusConnectEx(string fileName, string configName, string ipAddr);


        /// <summary>
        /// 断开函数
        /// </summary>
        /// <param name="ipAddr"></param>
        /// <returns></returns>
        [DllImport("PBusDrv.dll", CharSet = CharSet.Ansi)]
        static extern int PBusDisConnectEx(string ipAddr);

        /// <summary>
        /// 清除全部PLU
        /// </summary>
        /// <returns></returns>
        [DllImport("PBusDrv.dll", CharSet = CharSet.Ansi, ExactSpelling = true)]
        static extern int PBusClearPLUData();

    }

}
