using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.MessageAgent.Data
{
    public class Message
    {
        public static readonly byte[] MessageRouteCode = new byte[] { 0x01, 0x00, 0x00, 0x00 };
        public string Code { get; set; }
        public string RouteCode { get; set; }
        public string Content { get; set; }
        public object OtherInformaction { get; set; }

        public static Message ErrorMessage(string content, string routeCode, string code = "500", object other = null)
        {
            return CreateMessage(content, routeCode, code, other);
        }
        public static Message CreateMessage(string content, string routeCode, string code, object other)
        {
            return new Message() { Code = code, RouteCode = routeCode, Content = content, OtherInformaction = other };
        }
        public static Message DebugMessage(string content, string routeCode, string code = "200", object other = null)
        {
            return CreateMessage(content, routeCode, code, other);
        }
        public static Message InfoMessage(string content, string routeCode, string code = "200", object other = null)
        {
            return CreateMessage(content, routeCode, code, other);
        }
        public static Message SuccessMessage(string content, string routeCode, string code = "200", object other = null)
        {
            return CreateMessage(content, routeCode, code, other);
        }
        public static Message ExceptionMessage(string content, string routeCode, string code = "500", object other = null)
        {
            return CreateMessage(content, routeCode, code, other);
        }
    }
}
