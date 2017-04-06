
using Pharos.MessageAgent.Data;
namespace Pharos.MessageAgent.MessageQueues.Defualt
{
    public class DefualtMessageQueue : IMessageQueue
    {
        public DefualtMessageQueue()
        {
            Server = MessageServer.Current;

        }
        public MessageServer Server { get; set; }


        public void Pubish(PubishInformaction info)
        {
            Server.MessageAgent.ReceiveMessage(info);
        }


        public void Subscribe(SubscribeInformaction info)
        {
        }

        public void UnSubscribe(SubscribeInformaction info)
        {
        }
    }
}
