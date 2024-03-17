using Input;
using UnityEngine;
using Void.Audio;
using Void.Events;
using Void.Player;
using Void.Projectiles;
using Void.UI;

namespace Void.States
{
    public class BattleState : BaseState
    {
        private readonly IInputController _inputController;

        protected ProjectileData Projectile => Data.Settings.ProjectileData[Data.Player.ProjectileIndex];
        public override StateId StateId => StateId.Battle;

        public BattleState(IEventBus eventBus, IStateDTO data, IInputController inputController) : base(eventBus, data)
        {
            _inputController = inputController;
        }

        public override void Enter()
        {
            EventBus.SubscribeEvent<PlayerEvent.Damage>(OnPlayerDamageEvent);
            EventBus.SubscribeEvent<GameEvent.EndWave>(OnEndWaveEvent);
            
            EventBus.PublishEvent(new AudioEvent.PlayBGM(BGMType.Battle));
            EventBus.PublishEvent(new AudioEvent.PlaySFX(SFXType.WaveStart));
            EventBus.PublishEvent(new GameEvent.StartWave(Data.Waves.CurrentWave));
        }

        public override void Update()
        {
            if(Data.Player.Cooldown <= 0 && _inputController.IsFiring)
            {
                Data.Player.Cooldown = Projectile.Cooldown;
                EventBus.PublishEvent(new AudioEvent.PlaySFX(Projectile.SFX));
                EventBus.PublishEvent(new PlayerEvent.Fire(Data.Player.ProjectileIndex));
            }
            else if(Data.Player.Cooldown > 0)
            {
                Data.Player.Cooldown -= Time.deltaTime;
            }
        }

        private void OnPlayerDamageEvent(PlayerEvent.Damage evt)
        {
            Data.Player.Health -= evt.Amount;
            EventBus.PublishEvent(new AudioEvent.PlaySFX(SFXType.Hit));
            EventBus.PublishEvent(new UIEvent.UpdateHealth((float)Data.Player.Health / Data.Settings.PlayerMaxHealth));
            if(Data.Player.Health <= 0)
            {
                EventBus.PublishEvent(new PlayerEvent.Kill());
                EventBus.PublishEvent(new StateEvent.Change(StateId.GameOver));
            }
        }

        private void OnEndWaveEvent(GameEvent.EndWave evt)
        {
            if(Data.Waves.CurrentWave < Data.Settings.Waves.Length - 1)
            {
                // player.SetProjectile(waves[_wave - 1].UnlockedProjectile);
                EventBus.PublishEvent(new StateEvent.Change(StateId.Peace));
            } 
            else
            {
                EventBus.PublishEvent(new StateEvent.Change(StateId.GameOver));
            }
        }

        public override void Exit()
        {
            Data.Waves.CurrentWave++;
            EventBus.UnsubscribeEvent<PlayerEvent.Damage>(OnPlayerDamageEvent);
            EventBus.UnsubscribeEvent<GameEvent.EndWave>(OnEndWaveEvent);
        }
    }
}
