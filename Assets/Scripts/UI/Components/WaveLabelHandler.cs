using TMPro;
using UnityEngine;

namespace Void.UI.Components
{
    public class WaveLabelHandler : UIEventComponent
    {
        [SerializeField] private TextMeshProUGUI label;
    
        protected override void Bind()
        {
            EventBus.SubscribeEvent<UIEvent.UpdateWave>(OnUpdateWaveEvent);
            EventBus.SubscribeEvent<UIEvent.HideWaveTimer>(OnHideWaveEvent);
        }

        private void OnHideWaveEvent(UIEvent.HideWaveTimer evt)
        {
            gameObject.SetActive(false);
        }

        private void OnUpdateWaveEvent(UIEvent.UpdateWave evt)
        {
            gameObject.SetActive(true);
            label.text = $"Incoming in {evt.TimeLeft} seconds";
        }

        private void OnDestroy()
        {
            EventBus.UnsubscribeEvent<UIEvent.UpdateWave>(OnUpdateWaveEvent);
            EventBus.UnsubscribeEvent<UIEvent.HideWaveTimer>(OnHideWaveEvent);
        }
    }
}