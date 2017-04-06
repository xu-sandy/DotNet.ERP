
namespace Pharos.SocketClient.Retailing.CommandProviders
{
    public class PosStoreCommandNameProvider : ICommandNameProvider
    {
        public string GetCommandName(byte[] cmdCode)
        {
            return string.Format(@"v{0}\{1}{2}{3}", cmdCode[0], cmdCode[1], cmdCode[2], cmdCode[3]);
        }
    }
}
