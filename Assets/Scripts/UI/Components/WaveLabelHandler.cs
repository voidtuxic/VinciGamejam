using Void.Player;
using TMPro;
using UnityEngine;

namespace Void.UI.Components
{
    public class WaveLabelHandler : UIEventComponent
    {
        [SerializeField] private TextMeshProUGUI label;
    
        protected override void Bind()
        {
            EventBus.SubscribeEvent<GameEvent.UpdateWave>(OnUpdateWaveEvent);
            EventBus.SubscribeEvent<GameEvent.HideWave>(OnHideWaveEvent);
        }

        private void OnHideWaveEvent(GameEvent.HideWave evt)
        {
            gameObject.SetActive(false);
        }

        private void OnUpdateWaveEvent(GameEvent.UpdateWave evt)
        {
            gameObject.SetActive(true);
            label.text = $"Incoming in {evt.TimeLeft} seconds";
        }

        private void OnDestroy()
        {
            EventBus.UnsubscribeEvent<GameEvent.UpdateWave>(OnUpdateWaveEvent);
            EventBus.UnsubscribeEvent<GameEvent.HideWave>(OnHideWaveEvent);
        }
    }
}