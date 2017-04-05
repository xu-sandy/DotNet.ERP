using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.MessageAgent.Data
{
    public class MessageAgentException : Exception
    {
        public MessageAgentException(string msg) : base(msg) { }
    }
}
