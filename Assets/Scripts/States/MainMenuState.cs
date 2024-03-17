using Void.Audio;
using Void.Events;
using Void.Player;

namespace Void.States
{
    public class MainMenuState : BaseState
    {
        public override StateId StateId => StateId.MainMenu;

        public MainMenuState(IEventBus eventBus, IStateDTO data) : base(eventBus, data)
        {
        }

        public override void Enter()
        {
            EventBus.SubscribeEvent<GameEvent.StartGame>(OnStartGameEvent);
            EventBus.PublishEvent(new AudioEvent.PlayBGM(BGMType.Menu));
        }

        private void OnStartGameEvent(GameEvent.StartGame evt)
        {
            Data.Player.Health = Data.Settings.PlayerMaxHealth;
            EventBus.PublishEvent(new StateEvent.Change(StateId.Peace));
        }

        public override void Exit()
        {
            EventBus.UnsubscribeEvent<GameEvent.StartGame>(OnStartGameEvent);
        }
    }
}
