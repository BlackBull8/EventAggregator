using System;
using System.Collections.Generic;

namespace EventAggregator
{
    public class EventAggregator
    {
        private readonly Dictionary<string, List<EventMessageHandler>> _handlers;

        public EventAggregator()
        {
            _handlers = new Dictionary<string, List<EventMessageHandler>>();
        }

        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="subscribeId"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        public EventMessageHandler Subscribe(string subscribeId, Action<EventMessage> handler)
        {
            var eventMessageHandler = new EventMessageHandler(subscribeId, handler);

            if (_handlers.ContainsKey(subscribeId))
            {
                _handlers[subscribeId].Add(eventMessageHandler);
            }
            else
            {
                _handlers.Add(subscribeId,new List<EventMessageHandler>
                {
                    eventMessageHandler
                });
            }

            return eventMessageHandler;
        }

        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="handler"></param>
        public void Unsubscribe(EventMessageHandler handler)
        {
            if (_handlers.ContainsKey(handler.Id))
            {
                _handlers[handler.Id].Remove(handler);
            }
        }

        /// <summary>
        /// 该Id是否已添加到事件处理列表中
        /// </summary>
        /// <param name="subscribeId"></param>
        /// <returns></returns>
        public bool ContainsHandlers(string subscribeId)
        {
            return _handlers.ContainsKey(subscribeId);
        }

        /// <summary>
        /// 获取该订阅Id下的所有事件
        /// </summary>
        /// <param name="subscribeId"></param>
        /// <returns></returns>
        public IEnumerable<EventMessageHandler> GetEventMessageHandlers(string subscribeId)
        {
            return _handlers[subscribeId];
        }
    }
}