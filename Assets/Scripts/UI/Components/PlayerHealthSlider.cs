using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Void.Core.Events;
using Zenject;

public class PlayerHealthSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;

    private IEventBus eventBus;
    
    [Inject]
    private void Construct(IEventBus eventBus)
    {
        this.eventBus = eventBus;
        
        eventBus.SubscribeEvent<PlayerEvent.UpdateHealth>(OnUpdateHealthEvent);
        slider.value = 1;
    }

    private void OnUpdateHealthEvent(PlayerEvent.UpdateHealth evt)
    {
        slider.value = (float) evt.Health / evt.MaxHealth;
    }

    private void OnDestroy()
    {
        eventBus.UnsubscribeEvent<PlayerEvent.UpdateHealth>(OnUpdateHealthEvent);
    }
}
