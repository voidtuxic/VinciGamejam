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
        
        eventBus.SubscribeEvent<GameEvent.UpdateWave>(OnUpdateWaveEvent);
        eventBus.SubscribeEvent<GameEvent.HideWave>(OnHideWaveEvent);
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
        eventBus.UnsubscribeEvent<GameEvent.UpdateWave>(OnUpdateWaveEvent);
        eventBus.UnsubscribeEvent<GameEvent.HideWave>(OnHideWaveEvent);
    }
}