
namespace Pharos.SocketClient.Retailing.CommandProviders
{
    public interface ICommandNameProvider
    {
        string GetCommandName(byte[] cmdCode);

    }
}
