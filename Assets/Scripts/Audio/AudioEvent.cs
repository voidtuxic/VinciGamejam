using Void.Core.Events;

public static class AudioEvent
{
    public class PlayBGM : IEvent
    {
        public PlayBGM(BGMType type)
        {
            Type = type;
        }

        public BGMType Type { get; private set; }
    }
    public class PlaySFX : IEvent
    {
        public PlaySFX(SFXType type)
        {
            Type = type;
        }

        public SFXType Type { get; private set; }
    }
}
