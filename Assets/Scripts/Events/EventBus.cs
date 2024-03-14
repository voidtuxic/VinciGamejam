using System;
using System.Collections.Generic;

namespace Void.Events
{
    public class EventBus : IEventBus
    {
        private readonly Dictionary<string, object> _events = new Dictionary<string, object>();
        
        public void PublishEvent<T>(T evt) where T : class, IEvent
        {
            PublishEvent<T, EventSubscriptionHandler<T>>((handler) => 
            {
                handler(evt);
            });
        }
        
        public void SubscribeEvent<T>(EventSubscriptionHandler<T> handler) where T : class, IEvent
        {
            SubscribeEvent<T, EventSubscriptionHandler<T>>(handler);
        }

        public void UnsubscribeEvent<T>(EventSubscriptionHandler<T> handler) where T : class, IEvent
        {
            UnsubscribeEvent<T, EventSubscriptionHandler<T>>(handler);
        }

        private void PublishEvent<TEvent, THandler>(Action<THandler> handlerReceived) 
            where TEvent : class, IEvent
        {
            string key = EventHelper.GetEventKey<TEvent, THandler>();
            if (_events.TryGetValue(key, out var handlers))
            {
                var subscriptions = (IList<THandler>)handlers;
                for (int i = 0; i < subscriptions.Count; i++)
                {
                    if (subscriptions[i] == null)
                    {
                        subscriptions.RemoveAt(i);
                        i--;
                        continue;
                    }

                    handlerReceived?.Invoke(subscriptions[i]);
                }
            }
        }

        private void SubscribeEvent<TEvent, THandler>(THandler handler) 
            where TEvent : class, IEvent
        {
            string key = EventHelper.GetEventKey<TEvent, THandler>();
            if (_events.TryGetValue(key, out var handlers))
            {
                var subscriptions = (IList<THandler>)handlers;
                subscriptions.Add(handler);
            }
            else
            {
                var subscriptions = new List<THandler> { handler };
                _events.Add(key, subscriptions);
            }
        }

        private void UnsubscribeEvent<TEvent, THandler>(THandler handler) 
            where TEvent : class, IEvent
        {
            string key = EventHelper.GetEventKey<TEvent, THandler>();
            if (_events.TryGetValue(key, out var handlers))
            {
                var subscriptions = (IList<THandler>)handlers;
                subscriptions.Remove(handler);

                if (subscriptions.Count == 0)
                {
                    _events.Remove(key);
                }
            }
        }
    }
}
