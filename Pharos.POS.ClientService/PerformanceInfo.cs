using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Pharos.POS.ClientService
{
    public static class PerformanceInfo
    {
        [DllImport("psapi.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetPerformanceInfo([Out] out PerformanceInformation PerformanceInformation, [In] int Size);

        [StructLayout(LayoutKind.Sequential)]
        public struct PerformanceInformation
        {
            public int Size;
            public IntPtr CommitTotal;
            public IntPtr CommitLimit;
            public IntPtr CommitPeak;
            public IntPtr PhysicalTotal;
            public IntPtr PhysicalAvailable;
            public IntPtr SystemCache;
            public IntPtr KernelTotal;
            public IntPtr KernelPaged;
            public IntPtr KernelNonPaged;
            public IntPtr PageSize;
            public int HandlesCount;
            public int ProcessCount;
            public int ThreadCount;
        }

        public static Int64 GetPhysicalAvailableMemoryInMiB()
        {
            PerformanceInformation pi = new PerformanceInformation();
            if (GetPerformanceInfo(out pi, Marshal.SizeOf(pi)))
            {
                return Convert.ToInt64((pi.PhysicalAvailable.ToInt64() * pi.PageSize.ToInt64() / 1048576));
            }
            else
            {
                return -1;
            }

        }

        public static Int64 GetTotalMemoryInMiB()
        {
            PerformanceInformation pi = new PerformanceInformation();
            if (GetPerformanceInfo(out pi, Marshal.SizeOf(pi)))
            {
                return Convert.ToInt64((pi.PhysicalTotal.ToInt64() * pi.PageSize.ToInt64() / 1048576));
            }
            else
            {
                return -1;
            }

        }

        /// <summary>
        /// 根据盘符获取磁盘信息
        /// </summary>
        /// <param name="diskName"></param>
        /// <returns>一个自定义类对象</returns>
        public static HardDiskInfo GetHardDiskInfoByName(string diskName)
        {
            DriveInfo drive = new DriveInfo(diskName);
            return new HardDiskInfo { FreeSpace = GetDriveData(drive.AvailableFreeSpace), TotalSpace = GetDriveData(drive.TotalSize), Name = drive.Name };
        }

        /// <summary>
        /// 获取所有驱动盘信息
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<HardDiskInfo> GetAllHardDiskInfo()
        {
            List<HardDiskInfo> list = new List<HardDiskInfo>();
            foreach (DriveInfo d in DriveInfo.GetDrives())
            {
                if (d.IsReady)
                {
                    list.Add(new HardDiskInfo { Name = d.Name, FreeSpace = GetDriveData(d.AvailableFreeSpace), TotalSpace = GetDriveData(d.TotalSize) });
                }
            }
            return list;
        }



        private static float GetDriveData(long data)//将磁盘大小的单位由byte转化为G
        {
            return data / 1024f / 1024f / 1024f;
        }





        public class HardDiskInfo//自定义类
        {
            public string Name { get; set; }
            public float FreeSpace { get; set; }
            public float TotalSpace { get; set; }
        }
    }
}
