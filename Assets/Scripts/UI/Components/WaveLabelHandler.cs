using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Void.Core.Events;
using Zenject;

public class WaveLabelHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI label;
    
    private IEventBus eventBus;
    
    [Inject]
    private void Construct(IEventBus eventBus)
    {
        this.eventBus = eventBus;
        
        eventBus.SubscribeEvent<PlayerEvent.UpdateWave>(OnUpdateWaveEvent);
        eventBus.SubscribeEvent<PlayerEvent.HideWave>(OnHideWaveEvent);
    }

    private void OnHideWaveEvent(PlayerEvent.HideWave evt)
    {
        gameObject.SetActive(false);
    }

    private void OnUpdateWaveEvent(PlayerEvent.UpdateWave evt)
    {
        gameObject.SetActive(true);
        label.text = $"Incoming in {evt.TimeLeft} seconds";
    }

    private void OnDestroy()
    {
        eventBus.UnsubscribeEvent<PlayerEvent.UpdateWave>(OnUpdateWaveEvent);
        eventBus.UnsubscribeEvent<PlayerEvent.HideWave>(OnHideWaveEvent);
    }
}