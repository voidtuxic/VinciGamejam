using Void.Player;
using TMPro;
using UnityEngine;

namespace Void.UI.Components
{
    public class WaveStageLabelHandler : UIEventComponent
    {
        [SerializeField] private TextMeshProUGUI label;
    
        protected override void Bind()
        {
            EventBus.SubscribeEvent<GameEvent.UpdateWave>(OnUpdateWaveEvent);
        }

        private void OnUpdateWaveEvent(GameEvent.UpdateWave evt)
        {
            label.text = $"Wave {evt.Wave}";
        }

        private void OnDestroy()
        {
            EventBus.UnsubscribeEvent<GameEvent.UpdateWave>(OnUpdateWaveEvent);
        }
    }
}
