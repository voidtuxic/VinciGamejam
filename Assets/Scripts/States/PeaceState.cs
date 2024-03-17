using Void.UI;
using UnityEngine;
using Void.Audio;
using Void.Events;

namespace Void.States
{
    public class PeaceState : BaseState
    {
        private float _timer;
        private int _lastTimerUpdate;
        
        public override StateId StateId => StateId.Peace;

        public PeaceState(IEventBus eventBus, IStateDTO data) : base(eventBus, data)
        {
        }
        
        public override void Enter()
        {
            _timer = 0;
            _lastTimerUpdate = -1; // send immediate update
            EventBus.PublishEvent(new AudioEvent.PlayBGM(BGMType.Peace));
        }

        public override void Update()
        {
            _timer += Time.deltaTime;
            if(_timer >= _lastTimerUpdate + 1)
            {
                _lastTimerUpdate++;
                EventBus.PublishEvent(new UIEvent.UpdateWave(
                    Data.Waves.CurrentWave,
                    Data.Settings.PeaceTime - _lastTimerUpdate));
            }

            if(_timer >= Data.Settings.PeaceTime)
            {
                EventBus.PublishEvent(new StateEvent.Change(StateId.Battle));
            }
        }

        public override void Exit()
        {
            EventBus.PublishEvent(new UIEvent.HideWaveTimer());
        }
    }
}
