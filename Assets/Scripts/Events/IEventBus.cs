namespace Void.Core.Events
{
    public interface IEventBus
    { 
        void PublishEvent<T>(T evt) where T : class, IEvent;
        void SubscribeEvent<T>(EventSubscriptionHandler<T> handler) where T : class, IEvent;
        void UnsubscribeEvent<T>(EventSubscriptionHandler<T> handler) where T : class, IEvent;
    }
}