using UnityEngine;
using UnityEngine.UI;

namespace Void.UI.Components
{
    public class PlayerHealthSlider : UIEventComponent
    {
        [SerializeField] private Slider slider;
    
        protected override void Bind()
        {
            EventBus.SubscribeEvent<UIEvent.UpdateHealth>(OnUpdateHealthEvent);
            slider.value = 1;
        }

        private void OnUpdateHealthEvent(UIEvent.UpdateHealth evt)
        {
            slider.value = evt.Factor;
        }

        private void OnDestroy()
        {
            EventBus.UnsubscribeEvent<UIEvent.UpdateHealth>(OnUpdateHealthEvent);
        }
    }
}
