using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Utility.Helpers
{
    public class FileHelper
    {
        /// <summary>
        /// 物理存储路径
        /// </summary>
        /// <param name="RelativePath">返回相对路径</param>
        /// <param name="dir">自定义目录名称</param>
        /// <returns>绝对路径</returns>
        public static string SaveAttachPath(ref string RelativePath, string dir = "")
        {
            var path = "Attachs\\";
            if (!string.IsNullOrEmpty(dir)) { path += dir + "\\"; }
            path +=DateTime.Now.ToString("yyyyMM") + "\\";
            RelativePath = path;
            path = System.IO.Path.Combine(GetRoot, path);
            if (!System.IO.Directory.Exists(path)) { System.IO.Directory.CreateDirectory(path); }
            return path;
        }

        /// <summary>
        /// 基本配置Logo物理存储路径
        /// </summary>
        /// <param name="RelativePath">返回相对路径</param>
        /// <param name="dir">自定义目录名称</param>
        /// <returns>绝对路径</returns>
        public static string SaveLogoPath(ref string RelativePath, string dir = "")
        {
            var path = "SysImg\\";
            if (!string.IsNullOrEmpty(dir)) { path += dir + "\\"; }
            RelativePath = path;
            path = System.IO.Path.Combine(GetRoot, path);
            if (!System.IO.Directory.Exists(path)) { System.IO.Directory.CreateDirectory(path); }
            return path;
        }

        /// <summary>
        /// 保存根目录
        /// </summary>
        public static string GetRoot
        {
            get
            {
                var root = Pharos.Utility.Config.GetAppSettings("FileRoot");
                return string.IsNullOrEmpty(root) ? AppDomain.CurrentDomain.BaseDirectory : root;
            }
        }
        /// <summary>
        ///附件URL根位置 
        /// </summary>
        public static string GetUrlRoot
        {
            get
            {
                var root = Pharos.Utility.Config.GetAppSettings("FileUrlRoot");
                return root??"";
            }
        }
        /// <summary>
        /// 转换文件大小单位
        /// </summary>
        /// <param name="fileSize"></param>
        /// <returns></returns>
        public static string GetFileSize(int fileSize)
        {
            var title = "";
            if (fileSize > 1024 * 1024 * 1024)
                title = Math.Round(fileSize / (1024 * 1024 * 1024m), 2) + " G";
            else if (fileSize > 1024 * 1024)
                title = Math.Round(fileSize / (1024 * 1024m), 2) + " M";
            else if (fileSize > 1024)
                title = Math.Round(fileSize / 1024m, 2) + " K";
            else
                title = fileSize.ToString();
            return title;
        }
        /// <summary>
        /// 获取字体图标样式
        /// </summary>
        /// <param name="extName"></param>
        /// <returns></returns>
        public static string GetFileFontIcon(string extName)
        {
            var cls="iconfont ";
            if (extName.IsNullOrEmpty())
                cls += "icon-wenjianfile65";//其它
            else if(extName.StartsWith("doc",StringComparison.CurrentCultureIgnoreCase))
                cls += "icon-doc";
            else if (extName.StartsWith("pdf", StringComparison.CurrentCultureIgnoreCase))
                cls += "icon-pdf";
            else if (extName.StartsWith("rar", StringComparison.CurrentCultureIgnoreCase))
                cls += "icon-zip";
            else if (extName.StartsWith("zip", StringComparison.CurrentCultureIgnoreCase))
                cls += "icon-zip";
            else if (extName.StartsWith("txt", StringComparison.CurrentCultureIgnoreCase))
                cls += "icon-txt";
            else if (extName.StartsWith("xls", StringComparison.CurrentCultureIgnoreCase))
                cls += "icon-xls";
            else
                cls += "icon-other";//其它
            return cls;

        }

        /// <summary>
        /// 获取扩展名对应的ContentType
        /// </summary>
        /// <param name="ext">扩展名</param>
        /// <returns></returns>
        public static string GetContentType(string ext)
        {
            var rg = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            object obj = rg.GetValue("Content Type");
            string result = obj != null ? obj.ToString() : "application/octet-stream";
            rg.Close();
            return result;
        }
    }
}
