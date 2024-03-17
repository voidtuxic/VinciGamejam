using Void.Events;

namespace Void.Player
{
    public static class PlayerEvent
    {
        public class Damage : IEvent
        {
            public int Amount { get; }
            
            public Damage(int amount)
            {
                Amount = amount;
            }
        }
        
        public class Kill : IEvent {}

        public class Fire : IEvent
        {
            public int ProjectileIndex { get; }
            
            public Fire(int projectileIndex)
            {
                ProjectileIndex = projectileIndex;
            }
        }
    }
}