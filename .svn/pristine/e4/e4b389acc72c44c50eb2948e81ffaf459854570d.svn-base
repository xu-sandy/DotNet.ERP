using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.ApiData.Mobile.Exceptions
{
    public class MessageException : Exception
    {
        public MessageException(string msg)
            : base(msg)
        {

        }
        public MessageException(string errorCode, string msg)
            : this(msg)
        {
            ErrorCode = errorCode;
        }
        public string ErrorCode { get; set; }

        public string Descript()
        {
            var inex = ToInnerException(this);
            var msg = (inex.Message == Message ? Message : Message + "\r\n描述:" + inex.Message) + "\r\n源:" + inex.Source + "\r\n引发原因:" + inex.TargetSite + "\r\n堆栈跟踪:" + inex.StackTrace;
            return msg;
        }
        Exception ToInnerException(Exception ex)
        {
            if (ex.InnerException == null) return ex;
            return ToInnerException(ex.InnerException);
        }
    }
}
