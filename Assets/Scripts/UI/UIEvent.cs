using Void.Events;

namespace Void.UI
{
    public static class UIEvent
    {
        public class UpdateHealth : IEvent
        {
            public float Factor { get; }
            public UpdateHealth(float factor)
            {
                Factor = factor;
            }
        }
        
        public class UpdateWave : IEvent
        {
            public int Wave { get; }
            public int TimeLeft { get; }
            
            public UpdateWave(int wave, int timeLeft)
            {
                Wave = wave;
                TimeLeft = timeLeft;
            }
        }

        public class HideWaveTimer : IEvent { }
    }
}
