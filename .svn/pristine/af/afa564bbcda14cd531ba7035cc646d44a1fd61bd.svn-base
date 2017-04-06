/*----------------------------------------------------------------
 * 功能描述：Web.Config 引擎
 * 创 建 人：蔡少发
 * 创建时间：2015-05-11
//----------------------------------------------------------------*/

using System.Configuration;
using System.Web.Configuration;
using System.Xml.Linq;
using System.Linq;
namespace Pharos.Utility
{
    /// <summary>
    /// Web.Config 引擎
    /// </summary>
    public class Config
    {
        #region 获取Web.Config配置中的 ConnectionString 节点

        /// <summary>
        /// 获取Web.Config配置中的 ConnectionString 节点
        /// </summary>
        public static string DefaultConnectionString
        {
            get
            {
                if (ConfigurationManager.ConnectionStrings["ConnectionString"] != null)
                {
                    return ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                }
                if (ConfigurationManager.AppSettings["ConnectionString"] != null)
                {
                    return ConfigurationManager.AppSettings["ConnectionString"];
                }
                return string.Empty;
            }
        }

        #endregion

        #region 获取Web.Config配置指定的节点名称

        /// <summary>
        /// 获取Web.Config配置指定的节点名称
        /// </summary>
        /// <param name="elementName">AppSettings中的配置节名称</param>
        /// <param name="propName">属性名称</param>
        public static string GetAppSettings(string elementName,string attributeName=null)
        {
            if (ConfigurationManager.AppSettings[elementName] != null)
            {
                return ConfigurationManager.AppSettings[elementName].ToString();
            }
            var val = "";
            try
            {
                var filepath = new Config().WebConfig.FilePath;
                XDocument doc = XDocument.Load(filepath);
                XElement ele = null;
                GetElement(doc.Root, elementName, ref ele);
                if (ele != null) val = ele.Attribute(attributeName).Value;
            }
            catch { }
            return val;
        }
        static void GetElement(XElement ele,string elementName,ref XElement rtnEl)
        {
            foreach(var el in ele.Elements())
            {
                if (string.Equals(el.Name.LocalName, elementName, System.StringComparison.CurrentCultureIgnoreCase))
                {
                    rtnEl = el;
                    break;
                }
                else
                    GetElement(el, elementName,ref rtnEl);
            }
        }
        /// <summary>
        /// 获取Web.Config配置指定的节点名称
        /// </summary>
        /// <param name="elementName">ConnectionStrings中的配置节名称</param>
        public static string GetConnectionStrings(string elementName)
        {
            if (ConfigurationManager.ConnectionStrings[elementName] != null)
            {
                return ConfigurationManager.ConnectionStrings[elementName].ConnectionString;
            }
            return string.Empty;
        }

        #endregion

        private Configuration WebConfig { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Config()
            : this(System.Web.HttpContext.Current.Request.ApplicationPath)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configFilePath">Config 配置文件路径</param>
        public Config(string configFilePath)
        {
            this.WebConfig = WebConfigurationManager.OpenWebConfiguration(configFilePath);
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {
            this.WebConfig.Save();
            this.WebConfig = null;
        }

        #region appSettings节点

        /// <summary>
        /// 设置appSettings节点
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public bool SetAppSettings(string key, string value)
        {
            try
            {
                //AppSettingsSection setting = (AppSettingsSection)this.WebConfig.GetSection("appSettings");
                var file = System.AppDomain.CurrentDomain.BaseDirectory + "settings.config";
                XDocument doc = XDocument.Load(file);
                var ele = (from x in doc.Element("appSettings").Elements("add") where x.Attribute("key").Value == key select x).FirstOrDefault();
                if (ele == null)
                {
                    var e = new XElement("add");
                    e.SetAttributeValue("key", key);
                    e.SetAttributeValue("value", value);
                    doc.Element("appSettings").Add(e);
                }
                else
                {
                    ele.SetAttributeValue("value", value);
                }
                //this.WebConfig.Save();注释会被清空
                doc.Save(file);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 移除appSettings节点
        /// </summary>
        /// <param name="key">键</param>
        public void RemoveAppSettings(string key)
        {
            AppSettingsSection setting = (AppSettingsSection)this.WebConfig.GetSection("appSettings");

            if (setting.Settings[key] != null)
            {
                setting.Settings.Remove(key);
            }
        }

        #endregion

        #region connectionStrings节点

        /// <summary>
        /// 设置connectionStrings节点
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="connectionString">连接字符串</param>
        public void SetConnectionStrings(string key, string connectionString)
        {
            ConnectionStringsSection setting = (ConnectionStringsSection)this.WebConfig.GetSection("connectionStrings");

            if (setting.ConnectionStrings[key] == null)
            {
                ConnectionStringSettings connectionStringSettings = new ConnectionStringSettings(key, connectionString);
                setting.ConnectionStrings.Add(connectionStringSettings);
            }
            else
            {
                setting.ConnectionStrings[key].ConnectionString = connectionString;
            }
        }

        /// <summary>
        /// 移除connectionStrings节点
        /// </summary>
        /// <param name="key">键</param>
        public void RemoveConnectionStrings(string key)
        {
            ConnectionStringsSection setting = (ConnectionStringsSection)this.WebConfig.GetSection("connectionStrings");

            if (setting.ConnectionStrings[key] != null)
            {
                setting.ConnectionStrings.Remove(key);
            }
        }

        #endregion
    }
}
