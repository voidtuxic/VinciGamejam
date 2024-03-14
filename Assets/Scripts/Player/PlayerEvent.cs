using Void.Core.Events;

public static class PlayerEvent
{
    public class UpdateHealth : IEvent
    {
        public int Health { get; set; }
        public int MaxHealth { get; set; }
    }
}