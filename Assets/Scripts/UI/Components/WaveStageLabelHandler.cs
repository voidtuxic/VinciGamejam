using TMPro;
using UnityEngine;
using Void.Core.Events;
using Zenject;

public class WaveStageLabelHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI label;
    
    private IEventBus eventBus;
    
    [Inject]
    private void Construct(IEventBus eventBus)
    {
        this.eventBus = eventBus;
        
        eventBus.SubscribeEvent<PlayerEvent.UpdateWave>(OnUpdateWaveEvent);
    }

    private void OnUpdateWaveEvent(PlayerEvent.UpdateWave evt)
    {
        label.text = $"Wave {evt.Wave}";
    }

    private void OnDestroy()
    {
        eventBus.UnsubscribeEvent<PlayerEvent.UpdateWave>(OnUpdateWaveEvent);
    }
}
