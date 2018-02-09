using System;
using System.Collections.Generic;
using System.Linq;

namespace AnotherEventAggregator
{
    public class EventAggregator:IEventAggregator
    {
        private readonly Dictionary<Type,List<WeakReference>> _eventSubscribers = new Dictionary<Type, List<WeakReference>>();

        private static readonly object Lock = new object();

        public void PublishEvent<TEventType>(TEventType eventToPublish)
        {
            var subscriberType = typeof(ISubscriber<>).MakeGenericType(typeof(TEventType));
            var subscribers = GetSubscriberList(subscriberType);

            List<WeakReference> subscribersToBeRemoved = new List<WeakReference>();

            foreach (WeakReference weakReference in subscribers)
            {
                if (weakReference.IsAlive)
                {
                    var subscriber = (ISubscriber<TEventType>) weakReference.Target;
                    subscriber.OnEventHandler(eventToPublish);
                }
                else
                {
                    subscribersToBeRemoved.Add(weakReference);
                }
            }

            if (subscribersToBeRemoved.Any())
            {
                lock (Lock)
                {
                    foreach (WeakReference weakReference in subscribersToBeRemoved)
                    {
                        subscribers.Remove(weakReference);
                    }
                }
            }
        }

        public void SubsribeEvent(object subscriber)
        {
            lock (Lock)
            {
                // 获取所有接口，这些接口必须是泛型且类型为ISubscriber<>类型的
                var subscriberTypes = subscriber.GetType().GetInterfaces()
                    .Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ISubscriber<>));

                // 将传进来的对象放到弱引用中
                WeakReference weakReference = new WeakReference(subscriber);

                // 遍历所有接口类型
                foreach (Type subscriberType in subscriberTypes)
                {
                    // 获取key为subscriberType的列表，将弱引用放进去
                    List<WeakReference> subscribers = GetSubscriberList(subscriberType);

                    subscribers.Add(weakReference);
                }
            }
        }

        private List<WeakReference> GetSubscriberList(Type subscriberType)
        {
            List<WeakReference> subscribersList;

            bool found = _eventSubscribers.TryGetValue(subscriberType, out subscribersList);

            if (!found)
            {
                subscribersList = new List<WeakReference>();
                _eventSubscribers.Add(subscriberType, subscribersList);
            }

            return subscribersList;
        }
    }
}