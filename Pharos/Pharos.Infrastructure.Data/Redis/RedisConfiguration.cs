using System.Configuration;

namespace Pharos.Infrastructure.Data.Redis
{
    public sealed class RedisConfiguration : ConfigurationSection
    {
        public static RedisConfiguration GetConfig()
        {
            RedisConfiguration section = (RedisConfiguration)ConfigurationManager.GetSection("RedisConfig");
            return section;
        }

        public static RedisConfiguration GetConfig(string sectionName)
        {
            RedisConfiguration section = (RedisConfiguration)ConfigurationManager.GetSection("RedisConfig");
            if (section == null)
                throw new ConfigurationErrorsException("Section " + sectionName + " is not found.");
            return section;
        }
        /// <summary>
        /// 可写的Redis链接地址
        /// </summary>
        [ConfigurationProperty("ConnectionString", DefaultValue = "127.0.0.1:6379,password=pharos@2016", IsRequired = false)]
        public string ConnectionString
        {
            get
            {
                return (string)base["ConnectionString"];
            }
            set
            {
                base["ConnectionString"] = value;
            }
        }



    }
}
