using Void.Core.Events;

public static class PlayerEvent
{
    public class UpdateHealth : IEvent
    {
        public int Health { get; set; }
        public int MaxHealth { get; set; }
    }
    
    public class StartGame : IEvent {}

    public class GameOver : IEvent
    {
        public string Message { get; set; }
    }
    
    public class UpdateScore : IEvent
    {
        public int AddedScore { get; set; }
    }
    
    public class UpdateWave : IEvent
    {
        public int Wave { get; set; }
        public int TimeLeft { get; set; }
    }
    public class HideWave : IEvent { }
}
