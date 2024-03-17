using TMPro;
using UnityEngine;

namespace Void.UI.Components
{
    public class WaveStageLabelHandler : UIEventComponent
    {
        [SerializeField] private TextMeshProUGUI label;
    
        protected override void Bind()
        {
            EventBus.SubscribeEvent<UIEvent.UpdateWave>(OnUpdateWaveEvent);
        }

        private void OnUpdateWaveEvent(UIEvent.UpdateWave evt)
        {
            label.text = $"Wave {evt.Wave}";
        }

        private void OnDestroy()
        {
            EventBus.UnsubscribeEvent<UIEvent.UpdateWave>(OnUpdateWaveEvent);
        }
    }
}
