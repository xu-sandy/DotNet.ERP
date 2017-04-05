using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.MessageTransferAgenClient
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class SubscriberAttribute : Attribute
    {
        public SubscriberAttribute(string topic)
        {
            Topic = topic;
        }
        public string Topic { get; set; }

    }
}
