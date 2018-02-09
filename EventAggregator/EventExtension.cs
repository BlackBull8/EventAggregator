using System;
using System.Collections.Generic;

namespace EventAggregator
{
    public static class EventExtension
    {
        private static readonly EventHub EventHub;

        static EventExtension()
        {
            EventHub = new EventHub();
        }

        public static void Send(this object eventSource, string subscribeId,
            params KeyValuePair<string, object>[] parameters)
        {       
            Send(eventSource,EventHub, subscribeId, parameters);
        }

        public static void Send(this object eventSource, EventHub eventHub, string subscribeId,
            params KeyValuePair<string, object>[] parameters)
        {
            eventHub.BroadcastEvent(eventSource, new EventMessage(subscribeId, parameters));
        }

        public static EventMessageHandler Subscribe(this object eventSource, string subscribeId, Action<EventMessage> handler)
        {
            return Subscribe(eventSource,EventHub,subscribeId, handler);
        }

        public static EventMessageHandler Subscribe(this object eventSource, EventHub eventHub, string subscribeId,
            Action<EventMessage> handler)
        {
            return eventHub.Subscribe(eventSource, subscribeId, handler);
        }

        public static void Unsubscribe(this object eventSource, EventMessageHandler eventMessageHandler)
        {
            Unsubscribe(eventSource, EventHub, eventMessageHandler);
        }

        public static void Unsubscribe(this object eventSource, EventHub eventHub,
            EventMessageHandler eventMessageHandler)
        {
            eventHub.Unsubscribe(eventMessageHandler);
        }

        public static void BreakEventLoop(this object eventSource)
        {
            BreakEventLoop(eventSource,EventHub);
        }

        public static void BreakEventLoop(this object eventSource,EventHub eventHub)
        {
            eventHub.BreakEventLoop();
        }       
    }
}