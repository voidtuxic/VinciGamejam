namespace Void.Events
{
    public delegate void EventSubscriptionHandler<in T>(T evt) where T : class, IEvent;
    public interface IEvent
    {
        
    }
}
