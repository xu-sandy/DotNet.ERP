using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.ApiData.Mobile.Exceptions
{
    public class LoginExecption : MessageException
    {
        public LoginExecption(string msg)
            : base("401", msg)
        {

        }
        public LoginExecption(string errorCode, string msg)
            : base(errorCode, msg)
        {
        }
    }
}
