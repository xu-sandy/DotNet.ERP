using System;
using System.Management;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Utility
{
    /// <summary>
    /// 机器信息
    /// </summary>
    public class Machine
    {
        /// <summary>
        /// 获取本机MAC编号
        /// </summary>
        /// <returns></returns>
        public static string GetMAC
        {
            get
            {
                return MacNoHelper.GetMACID();
                //todo:

                //string mac = "";
                //ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                //ManagementObjectCollection moc = mc.GetInstances();
                //foreach (ManagementObject mo in moc)
                //{
                //    if ((bool)mo["IPEnabled"])
                //    {
                //        mac = mo["MacAddress"].ToString();
                //        break;
                //    }
                //}
                //return mac.Replace(":", "");
            }
        }
    }
}
