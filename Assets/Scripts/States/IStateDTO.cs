using Void.Enemies;
using Void.Player;

namespace Void.States
{
    public interface IStateDTO
    {
        StateSettings Settings { get; }
        WaveDTO Waves { get; }
        PlayerDTO Player { get; }
    }

    public class StateDTO : IStateDTO
    {
        public StateSettings Settings { get; }
        public WaveDTO Waves { get; } = new WaveDTO();
        public PlayerDTO Player { get; } = new PlayerDTO();

        public StateDTO(StateSettings settings)
        {
            Settings = settings;
        }
    }
}
