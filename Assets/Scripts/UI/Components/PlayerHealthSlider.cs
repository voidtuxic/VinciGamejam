using Void.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Void.UI.Components
{
    public class PlayerHealthSlider : UIEventComponent
    {
        [SerializeField] private Slider slider;
    
        protected override void Bind()
        {
            EventBus.SubscribeEvent<PlayerEvent.UpdateHealth>(OnUpdateHealthEvent);
            slider.value = 1;
        }

        private void OnUpdateHealthEvent(PlayerEvent.UpdateHealth evt)
        {
            slider.value = (float) evt.Health / evt.MaxHealth;
        }

        private void OnDestroy()
        {
            EventBus.UnsubscribeEvent<PlayerEvent.UpdateHealth>(OnUpdateHealthEvent);
        }
    }
}
