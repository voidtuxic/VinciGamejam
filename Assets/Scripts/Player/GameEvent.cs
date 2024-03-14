using Void.Core.Events;

public static class GameEvent
{
    public class StartGame : IEvent {}

    public class GameOver : IEvent
    {
        public string Message { get; set; }
    }
    
    public class UpdateWave : IEvent
    {
        public int Wave { get; set; }
        public int TimeLeft { get; set; }
    }
    public class HideWave : IEvent { }
}
