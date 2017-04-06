// --------------------------------------------------
// Copyright (C) 2015 版权所有
// 创 建 人：蔡少发
// 创建时间：2015-05-28
// 更新时间：2016-01-20
// 描述信息：常用池
// --------------------------------------------------

using System;

namespace Pharos.Sys
{
    /// <summary>
    /// 常用池
    /// </summary>
    public partial class SysConstPool
    {
        #region 主题皮肤

        /// <summary>
        /// 默认主题皮肤路径
        /// </summary>
        public static string SkinPath(string url)
        {
            return System.IO.Path.Combine(_skinPath, url);
        }
        private static string _skinPath = @"/Content/mythemes/default/";

        #endregion

        #region 版权、名称、ICON相关

        /// <summary>
        /// 顶部LOGO
        /// </summary>
        public static string PC_Logo
        {
            get { return SkinPath(_pc_logo); }
            set { _pc_logo = value; }
        }
        public static string _pc_logo = "images/login/logo.png";
        /// <summary>
        /// 系统ICON
        /// </summary>
        public static string PC_Icon
        {
            get { return _pc_icon; }
            set { _pc_icon = value; }
        }
        public static string _pc_icon = @"/favicon.ico";
        /// <summary>
        /// 底部LOGO
        /// </summary>
        public static string PC_FooterLogo
        {
            get { return SkinPath(_pc_footerLogo); }
            set { _pc_footerLogo = value; }
        }
        public static string _pc_footerLogo = @"images/pharoslogo.png";

        /// <summary>
        /// 页面Title
        /// </summary>
        public static string Page_Title
        {
            get
            {
                var obj= new Pharos.Sys.BLL.SysWebSettingBLL().GetWebSetting();
                if (obj == null) return _page_title;
                return string.IsNullOrWhiteSpace(obj.PageTitle)?_page_title:obj.PageTitle;
            }
        }
        public static string _page_title = "后台管理系统";        //全城淘·商业管理平台

        /// <summary>
        /// 官网地址
        /// </summary>
        public static string Page_WebHttp
        {
            get { return _page_webhttp; }
            set { _page_webhttp = value; }
        }
        public static string _page_webhttp = "www.qzyds.cn";// "www.xmpharos.com";

        /// <summary>
        /// 公司简称
        /// </summary>
        public static string CompanyName
        {
            get { return _companyName; }
            set { _companyName = value; }
        }
        public static string _companyName = "道诚科技";

        /// <summary>
        /// 公司电话
        /// </summary>
        public static string CompanyTel
        {
            get { return _companyTel; }
            set { _companyTel = value; }
        }
        public static string _companyTel;

        #endregion

        #region SMTP

        /// <summary>
        /// 发件箱服务器
        /// </summary>
        public static string SMTP_Server { get; set; }
        /// <summary>
        /// 端口
        /// </summary>
        public static int SMTP_Port
        {
            get { return _smtp_port; }
            set { _smtp_port = value; }
        }
        private static int _smtp_port = 25;
        /// <summary>
        /// SSL端口
        /// </summary>
        public static string SMTP_SSLPort { get; set; }
        private static int _smtp_sslport = 465;
        /// <summary>
        /// 帐号
        /// </summary>
        public static string SMTP_Account { get; set; }
        /// <summary>
        /// 显示名称
        /// </summary>
        public static string SMTP_ShowName { get; set; }


        #endregion

        public static string Page_GoError = "/Error.aspx?msg=";

        /// <summary>
        /// 物理存储路径
        /// </summary>
        /// <param name="RelativePath">返回相对路径</param>
        /// <param name="dir">自定义目录名称</param>
        /// <returns>绝对路径</returns>
        public static string SaveAttachPath(ref string RelativePath, string dir = "")
        {
            var path = "Attachs\\" + DateTime.Now.ToString("yyyyMM") + "\\";
            if (!string.IsNullOrEmpty(dir)) { path += dir + "\\"; }
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
                var root = Pharos.Utility.Config.GetAppSettings("SaveRoot");
                return string.IsNullOrEmpty(root) ? AppDomain.CurrentDomain.BaseDirectory : root;
            }
        }
    }
}
