﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.SocketService.Retailing.Protocol
{
    public interface ICommandRuleProvider
    {
        string GetCommandName(CommandRule routeRule);

        int BytesLength { get; }

        bool Verfy(CommandRule routeRule, bool enableThrowException = false); 
    }
}
