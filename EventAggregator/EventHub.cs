using System;
using System.Collections.Generic;

namespace EventAggregator
{
    public class EventHub:IDisposable
    {
        private readonly Dictionary<object, EventAggregator> _eventAggregators;
        private bool _breakEventLoop;
        private bool _isBroadcastingEvent;

        public EventHub()
        {
            _eventAggregators = new Dictionary<object, EventAggregator>();
        }

        /// <summary>
        /// 获取EventAggregator
        /// </summary>
        /// <param name="eventSource"></param>
        /// <returns></returns>
        private EventAggregator GetEventAggregator(object eventSource)
        {
            EventAggregator eventAggregator;

            if (_eventAggregators.ContainsKey(eventSource))
            {
                eventAggregator = _eventAggregators[eventSource];
            }
            else
            {
                eventAggregator = new EventAggregator();
                _eventAggregators.Add(eventSource, eventAggregator);
            }

            return eventAggregator;
        }

        internal EventMessageHandler Subscribe(object eventSource,string subscribeId, Action<EventMessage> handler)
        {
            var aggergator = GetEventAggregator(eventSource);
            return aggergator.Subscribe(subscribeId, handler);
        }

        internal void Unsubscribe(EventMessageHandler eventMessageHandler)
        {
            var eventAggergators = new List<EventAggregator>(_eventAggregators.Values);
            foreach (EventAggregator eventAggergator in eventAggergators)
            {
                if (eventAggergator.ContainsHandlers(eventMessageHandler.Id))
                {
                    eventAggergator.Unsubscribe(eventMessageHandler);
                }
            }
        }

        internal void BroadcastEvent(object eventSource,EventMessage eventMessage)
        {
            _isBroadcastingEvent = true;

            var specialAggergator = GetEventAggregator(eventSource);

            var eventAggergators = new List<EventAggregator>(_eventAggregators.Values);

            foreach (EventAggregator eventAggergator in eventAggergators)
            {
                if (specialAggergator != eventAggergator &&
                    eventAggergator.ContainsHandlers(eventMessage.EventMessageId))
                {
                    var eventHandlers =
                        new List<EventMessageHandler>(
                            eventAggergator.GetEventMessageHandlers(eventMessage.EventMessageId));

                    if (eventHandlers.Count <= 0) return;

                    foreach (EventMessageHandler eventMessageHandler in eventHandlers)
                    {
                        if (_breakEventLoop)
                        {
                            break;
                        }

                        eventMessageHandler.HandleEvent(eventMessage);
                    }
                }

                if (_breakEventLoop)
                {
                    _breakEventLoop = false;
                    break;
                }
            }

            _isBroadcastingEvent = false;
        }

        internal void BreakEventLoop()
        {
            if (!_isBroadcastingEvent)
            {
                throw new InvalidOperationException("No event loop to break!");
            }

            _breakEventLoop = true;
        }

        public void Dispose()
        {
            _eventAggregators.Clear();
        }
    }
}