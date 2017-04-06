using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.MessageTransferAgenClient.DomainEvent
{
    public class ActionEventHandler<TEvent> : BaseEventHandler<TEvent>
        where TEvent : IEvent
    {
        public Action<TEvent> CallBack { get; set; }
        public ActionEventHandler(string topic, Action<TEvent> action)
            : base(topic)
        {
            CallBack = action;
        }
        public override void Handle(TEvent evnt)
        {
            CallBack(evnt);
        }

    }
}
