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
        
        eventBus.SubscribeEvent<GameEvent.UpdateWave>(OnUpdateWaveEvent);
    }

    private void OnUpdateWaveEvent(GameEvent.UpdateWave evt)
    {
        label.text = $"Wave {evt.Wave}";
    }

    private void OnDestroy()
    {
        eventBus.UnsubscribeEvent<GameEvent.UpdateWave>(OnUpdateWaveEvent);
    }
}
