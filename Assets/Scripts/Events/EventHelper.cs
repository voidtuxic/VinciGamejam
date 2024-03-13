namespace Void.Core.Events
{
    public class EventHelper
    {
        public static string GetEventKey<TEvent, THandler>() => $"{typeof(TEvent).FullName}_{typeof(THandler).FullName}";
    }
}
