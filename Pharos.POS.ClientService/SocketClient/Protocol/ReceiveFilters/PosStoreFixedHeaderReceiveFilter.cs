using Pharos.SocketClient.Retailing.CommandProviders;
using Pharos.SocketClient.Retailing.Protocol.RequestInfos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.SocketClient.Retailing.Protocol.ReceiveFilters
{
    public class PosStoreFixedHeaderReceiveFilter : CustomFixedHeaderReceiveFilter<PosStorePackageInfo>
    {
        ICommandNameProvider commandNameProvider;
        public PosStoreFixedHeaderReceiveFilter(ICommandNameProvider commandNameProvider)
        {
            this.commandNameProvider = commandNameProvider;
        }
        public override PosStorePackageInfo LoadRequestInfo(byte[] cmdCode, byte[] bodyBuffer)
        {
            var result = new PosStorePackageInfo(commandNameProvider.GetCommandName(cmdCode), bodyBuffer);
            return result;
        }
    }
}
