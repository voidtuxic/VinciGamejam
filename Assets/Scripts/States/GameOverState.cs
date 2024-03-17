using Void.Audio;
using Void.Events;
using Void.Player;

namespace Void.States
{
    public class GameOverState : BaseState
    {
        public override StateId StateId => StateId.GameOver;

        public GameOverState(IEventBus eventBus, IStateDTO data) : base(eventBus, data)
        {
        }

        public override void Enter()
        {
            EventBus.PublishEvent(new AudioEvent.PlayBGM(BGMType.GameOver));
            var message = Data.Player.Health <= 0 ? "YOU DIED" : "YOU SURVIVED";
            EventBus.PublishEvent(new GameEvent.GameOver(message));
        }
    }
}
