﻿using System.Configuration;

namespace Pharos.MessageAgent
{
    public class MessageServerConfiguration : ConfigurationSection
    {
        public static MessageServerConfiguration GetConfig()
        {
            MessageServerConfiguration section = (MessageServerConfiguration)ConfigurationManager.GetSection("MessageServer");
            return section;
        }
        [ConfigurationProperty("Port", DefaultValue = 3033, IsRequired = false)]
        public int Port
        {
            get
            {
                return (int)base["Port"];
            }
            set
            {
                base["Port"] = value;
            }
        }

        [ConfigurationProperty("MessageQueue", DefaultValue = "Redis", IsRequired = false)]
        public string MessageQueue
        {
            get
            {
                return (string)base["MessageQueue"];
            }
            set
            {
                base["MessageQueue"] = value;
            }
        }
        [ConfigurationProperty("MessageQueueConnectionString", IsRequired = false)]
        public string MessageQueueConnectionString
        {
            get
            {
                return (string)base["MessageQueueConnectionString"];
            }
            set
            {
                base["MessageQueueConnectionString"] = value;
            }
        }

        [ConfigurationProperty("MessageStore", DefaultValue = "Redis", IsRequired = false)]
        public string MessageStore
        {
            get
            {
                return (string)base["MessageStore"];
            }
            set
            {
                base["MessageStore"] = value;
            }
        }

        [ConfigurationProperty("MessageStoreConnectionString", IsRequired = false)]
        public string MessageStoreConnectionString
        {
            get
            {
                var messageStore = (string)base["MessageStoreConnectionString"];
                if (MessageStore == "Redis" && string.IsNullOrEmpty(messageStore))
                {
                    return MessageQueueConnectionString;
                }
                return messageStore;
            }
            set
            {
                base["MessageStoreConnectionString"] = value;
            }
        }
    }
}
