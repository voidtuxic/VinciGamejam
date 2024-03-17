using Void.Events;

namespace Void.Player
{
    public static class GameEvent
    {
        public class StartGame : IEvent {}

        public class StartWave : IEvent
        {
            public int Wave { get; }
            public StartWave(int wave)
            {
                Wave = wave;
            }
        }
        
        public class EndWave : IEvent {}

        public class GameOver : IEvent
        {
            public string Message { get; }
            
            public GameOver(string message)
            {
                Message = message;
            }
        }
    }
}
