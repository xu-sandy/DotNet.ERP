using Pharos.MessageAgent.Data;

namespace Pharos.MessageAgent.MessageQueues
{
    public interface IMessageQueue
    {
        MessageServer Server { get; set; }
        void Subscribe(SubscribeInformaction info);
        void UnSubscribe(SubscribeInformaction info);
        void Pubish(PubishInformaction info);
    }
}
