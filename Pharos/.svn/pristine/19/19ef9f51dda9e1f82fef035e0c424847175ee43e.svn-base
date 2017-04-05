using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Pharos.Utility
{
    public class MacNoHelper
    {
        public static string GetMACID()
        {
            return GetMacByIPConfig().FirstOrDefault();
        }
        public static List<string> GetMacByIPConfig()
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
    }
}
