using System;

namespace EventAggregator
{
    /// <summary>
    /// 事件处理器
    /// </summary>
    public class EventMessageHandler
    {
        private readonly Action<EventMessage> _handler;

        public string Id { get; }
        
        public EventMessageHandler(string id, Action<EventMessage> handler)
        {
            Id = id;
            _handler = handler;
        }

        public void HandleEvent(EventMessage eventMessage)
        {
            _handler?.Invoke(eventMessage);
        }
    }
}